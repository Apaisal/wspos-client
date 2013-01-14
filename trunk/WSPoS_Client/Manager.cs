using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WSPoS_Client
{

    public enum Status
    {
        OK,
        Busy,
        Close,
    }

    public class Manager
    {
        private EventLog eventLog;
        private wsposConnector wsposConn;
        private driverConnector driverConn;
        private Queue<string> messageQueueIN;
        private Queue<string> messageQueueOUT;
        private Status managerStatus = Status.Close;

        public event EntryWrittenEventHandler WrittenLog;

        public Queue<string> MessageQueueIN
        {
            get
            {
                return messageQueueIN;
            }
        }
        
        public Queue<string> MessageQueueOUT
        {
            get
            {
                return messageQueueOUT;
            }
        }

        public EventLog Log {
            get
            {
                return eventLog;
            }
        }

        public wsposConnector WSPoS 
        { get { return wsposConn; } }

        public driverConnector Driver 
        { get { return driverConn; } }

        public Status ManagerStatus {
            get { return managerStatus; }            
        }

        public Manager()
        {
            messageQueueIN = new Queue<string>();
            messageQueueOUT = new Queue<string>();
            eventLog = new EventLog("WSPoS Client", System.Environment.MachineName, this.ToString());
            eventLog.EnableRaisingEvents = true;
            eventLog.EntryWritten += new EntryWrittenEventHandler(eventLog_EntryWritten);
            wsposConn = new wsposConnector(this);
            driverConn = new driverConnector(this);         
        }

        void eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (WrittenLog != null)
            {
                WrittenLog(this,e);
            }
        }

        internal void Start()
        {
            this.wsposConn.Connect();
            this.driverConn.Connect();
            this.managerStatus = Status.OK;
        }

        internal void Stop()
        {
            wsposConn.Stop();
            driverConn.Stop();
            this.managerStatus = Status.Close;
        }

        internal void Pause()
        {
            throw new NotImplementedException();
        }
    }
}
