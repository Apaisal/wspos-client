using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;

namespace WSPOS.Client
{
    [PlugDisplayName("POSPrinter")]
    public class POSPrinter : PosCommon, IPlugin
    {
        private POSPrinterService.POSPrinterClient client;
        
        public POSPrinter()
        {
            this.client = new POSPrinterService.POSPrinterClient("BasicHttpBinding_POSPrinter");
           
        }

        public override PowerReporting CapPowerReporting
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CapStatisticsReporting
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CapUpdateStatistics
        {
            get { throw new NotImplementedException(); }
        }

        public override string CheckHealth(HealthCheckLevel level)
        {
            throw new NotImplementedException();
        }

        public override string CheckHealthText
        {
            get { throw new NotImplementedException(); }
        }

        public override void Claim(int timeout)
        {
            this.client.ClaimAsync(timeout);
        }

        public override bool Claimed
        {
            get { return this.client.GetClaimed(); }
        }

        public override void Close()
        {
            this.client.Close();
        }

        public override string DeviceDescription
        {
            get { throw new NotImplementedException(); }
        }

        public override bool DeviceEnabled
        {
            get
            {
                return this.client.GetDeviceEnabled();
            }
            set
            {
                this.client.SetDeviceEnabled(value);
            }
        }

        public override string DeviceName
        {
            get { throw new NotImplementedException(); }
        }

        public override DirectIOData DirectIO(int command, int data, object obj)
        {
            throw new NotImplementedException();
        }

        public override event DirectIOEventHandler DirectIOEvent;

        public override bool FreezeEvents
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override bool IsExclusiveUseDevice
        {
            get { throw new NotImplementedException(); }
        }

        public override void Open()
        {
            this.client.Open();
        }

        public override PowerNotification PowerNotify
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PowerState PowerState
        {
            get { throw new NotImplementedException(); }
        }

        public override void Release()
        {
            this.client.Release();
        }

        public override void ResetStatistic(string statistic)
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics(string[] statistics)
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics(StatisticCategories statistics)
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics()
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistic(string statistic)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics(string[] statistics)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics(StatisticCategories statistics)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics()
        {
            throw new NotImplementedException();
        }

        public override string ServiceObjectDescription
        {
            get { throw new NotImplementedException(); }
        }

        public override ControlState State
        {
            get { return (Microsoft.PointOfService.ControlState) this.client.GetState(); }
        }

        public override event StatusUpdateEventHandler StatusUpdateEvent;

        public override void UpdateStatistic(string name, object value)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStatistics(StatisticCategories statistics, object value)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStatistics(Statistic[] statistics)
        {
            throw new NotImplementedException();
        }


        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            this.client.Abort();
            this.client = null;
        }
    }
}
