using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Reflection;
using System.IO;
using Microsoft.PointOfService;

namespace WSPOS.Client
{
    public class Manager
    {
        private EventLog eventLog;
        private List<PosDevice> deviceCollection;

        public List<PosDevice> Devices
        {
            get { return deviceCollection; }
            set { deviceCollection = value; }
        }
        
        public EventLog Log
        {
            get { return eventLog; }
            set { eventLog = value; }
        }
        
        

        public Manager()
        {
            this.eventLog = new EventLog("WSPOS Client", Environment.MachineName);
            this.deviceCollection = new List<PosDevice>();
            // TODO: Adding and deleting POS devices from plugins. 
            this.LoadPlugs();
        }
   


    // TODO: Listing POS devices and/or Service Objects for particular computers. 


    // TODO: Configuring a WEPOS Service Object (SO) to run for a particular device port (non-Plug and Play devices only). 


    // TODO: Setting a default device for a POS device class. 


    // TODO: Preventing a Service Object from running for a device. 


    // TODO: Assigning a logical name by which a POS application can access the Service Object for a device. 
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        private void LoadPlugs()
        {
            string[] files = Directory.GetFiles("Plugins", "*.dll");

            foreach (string f in files)
            {

                try
                {
                    Assembly a = Assembly.LoadFrom(f);
                    System.Type[] types = a.GetTypes();
                    foreach (System.Type type in types)
                    {
                        if (type.BaseType.Name.Equals("PosCommon"))
                        {
                            PosDevice obj = Activator.CreateInstance(type) as PosDevice;
                            this.deviceCollection.Add(obj);
                            
                    //        if (type.GetCustomAttributes(typeof(PlugDisplayNameAttribute),
                    //false).Length != 1)
                    //            throw new PlugNotValidException(type,
                    //              "PlugDisplayNameAttribute is not supported");
                    //        if (type.GetCustomAttributes(typeof(PlugDescriptionAttribute),
                    //false).Length != 1)
                    //            throw new PlugNotValidException(type,
                    //              "PlugDescriptionAttribute is not supported");

                    //        _tree.Nodes.Add(new PlugTreeNode(type));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(e.Message);
                }
            }

            return;
        }
    }
}
