using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO.Pipes;

namespace WSPoS_Client
{
    public class driverConnector
    {
        private SerialPort serialPortIN;
        private SerialPort serialPortOUT;
        private Manager manager;
        private int iBaudrate = 921600; //115200;
        private Encoding sjisEncoding = Encoding.GetEncoding(932);
        private const string sAckOUT = "00001";

        public int Baudrate
        {
            get { return iBaudrate; }
            set { iBaudrate = value; }
        }

        public driverConnector(Manager manager)
        {
            // TODO: Complete member initialization
            this.manager = manager;

            this.serialPortIN = new SerialPort("COM15", iBaudrate, Parity.None, 8, StopBits.One);
            this.serialPortIN.DataReceived += new SerialDataReceivedEventHandler(serialPortIN_DataReceived);
            this.serialPortIN.ErrorReceived += new SerialErrorReceivedEventHandler(serialPortIN_ErrorReceived);

            this.serialPortOUT = new SerialPort("COM17", iBaudrate, Parity.None, 8, StopBits.One);
            this.serialPortOUT.DataReceived += new SerialDataReceivedEventHandler(serialPortOUT_DataReceived);
            this.serialPortOUT.ErrorReceived += new SerialErrorReceivedEventHandler(serialPortOUT_ErrorReceived);
        }

        void serialPortOUT_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void serialPortIN_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void serialPortOUT_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;

            if (serialPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[serialPort.BytesToRead];
                serialPort.Read(buffer, 0, serialPort.BytesToRead);
                this.manager.MessageQueueOUT.Enqueue(sjisEncoding.GetString(buffer));
                serialPort.Write(sAckOUT);
            }
        }

        void serialPortIN_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            
            if (serialPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[serialPort.BytesToRead];
                serialPort.Read(buffer, 0, serialPort.BytesToRead);
                this.manager.WSPoS.Send(sjisEncoding.GetString(buffer));
            }
        }

        internal void Connect()
        {
            this.serialPortIN.Open();
            this.serialPortOUT.Open();
        }

        internal void Stop()
        {
            this.serialPortIN.Close();
            this.serialPortOUT.Close();
        }
   
    }
}
