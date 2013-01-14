using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace WSPoS_Client
{
    public class wsposConnector
    {
        private WSPoS.POSPrinterServiceReference.POSPrinterClient client;
        private const string sConfigName = "BasicHttpBinding_POSPrinter";
        private BasicHttpBinding httpBinding;
        private Manager manager;
        private int iTimeout = 10000;
        private Thread outThread, checkThread;
        private Status eStatus = Status.Close;
        private Encoding isoEncoding = Encoding.GetEncoding(1252);
        public Status WSPoSStatus
        {
            get { return eStatus = Status.Close; }
            set { eStatus = value; }
        }

        public wsposConnector(Manager manager)
        {
            // TODO: Complete member initialization
            this.manager = manager;

            this.httpBinding = new BasicHttpBinding(sConfigName);

            this.client = new WSPoS.POSPrinterServiceReference.POSPrinterClient(sConfigName);
            this.client.OpenCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_OpenCompleted);
            this.client.CloseCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_CloseCompleted);
        }

        void client_CloseCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
            this.eStatus = Status.Close;
            this.manager.Log.WriteEntry("WSPoS Close Completed", System.Diagnostics.EventLogEntryType.SuccessAudit);
        }

        void client_OpenCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
            this.outThread = new Thread(new ThreadStart(Start));
            this.outThread.Name = "WSPoS Connect";
            this.checkThread = new Thread(new ThreadStart(CheckHealth));
            this.checkThread.Name = "WSPoS CheckHealth";

            this.outThread.Start();
            this.checkThread.Start();

            this.eStatus = Status.OK;
            this.manager.Log.WriteEntry("WSPoS Open Completed", System.Diagnostics.EventLogEntryType.SuccessAudit);

        }

        internal void Connect()
        {
            this.client.OpenAsync(this.client.Endpoint.Address.ToString(), this.manager);
        }

        internal void Stop()
        {
            this.checkThread.Abort();
            this.checkThread = null;
            this.outThread.Abort();
            this.outThread = null;
            this.client.CloseAsync(this.client.Endpoint.Address.ToString(), this.manager);
        }

        void Start()
        {
            bool bPrint = false;
            string sData = string.Empty;

            while (true)
            {
                if (this.eStatus == Status.OK && this.manager.MessageQueueOUT.Count > 0)
                {
                    sData = this.manager.MessageQueueOUT.Dequeue();

                    // TODO: Check format

                    // TODO: parsing packet

                    // TODO: collect payload

                    // TODO: detect full cut command and send to print
                    bPrint = true;

                    if (bPrint)
                    {
                        this.Send(sData);
                    }
                }
                Thread.Sleep(0);
            }
        }

        public void Send(string sData)
        {
            try
            {               
                this.client.PrintNormal(WSPoS.POSPrinterServiceReference.PrinterStation.Receipt, sData);
            }
            catch (Exception ex)
            {
                this.manager.Log.WriteEntry(ex.Message, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
        }

        void CheckHealth()
        {
            while (true)
            {
                if (!this.client.GetClaimed())
                    this.client.Claim(iTimeout);
                if (!this.client.GetDeviceEnabled())
                    this.client.SetDeviceEnabled(true);
                if (this.client.GetCoverOpen())
                    this.eStatus = Status.Busy;
                else if (this.client.GetSlpEmpty())
                    this.eStatus = Status.Busy;
                else if (this.client.GetRecEmpty())
                    this.eStatus = Status.Busy;
                else if (this.client.GetPowerState() == WSPoS.POSPrinterServiceReference.PowerState.Off)
                    this.eStatus = Status.Busy;
                else
                    this.eStatus = Status.OK;

                Thread.Sleep(0);
            }
        }
    }
}
