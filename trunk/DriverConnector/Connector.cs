using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO.Pipes;

namespace WSPOS.Client.Driver
{
    public class Connector : IPlugin
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

        public Connector(Manager manager)
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
                //this.manager.MessageQueueOUT.Enqueue(sjisEncoding.GetString(buffer));
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
                //this.manager.WSPOS.Send(sjisEncoding.GetString(buffer));
            }
        }

        public void Stop()
        {
            this.serialPortIN.Close();
            this.serialPortOUT.Close();
        }


        public void Start()
        {
            this.serialPortIN.Open();
            this.serialPortOUT.Open();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
