using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSPoS_Client
{
    
    public partial class WSPoS_Client : Form
    {
        private Manager manager;
        private delegate void WriteMessage(string message);

        public WSPoS_Client()
        {
            InitializeComponent();

            manager = new Manager();
            manager.WrittenLog += new System.Diagnostics.EntryWrittenEventHandler(manager_WrittenLog);
        }

        public void manager_WrittenLog(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {
            WriteMessage write = new WriteMessage(txtLog.AppendText);
            this.Invoke(write, e.Entry.Message + Environment.NewLine);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (manager.ManagerStatus == Status.OK)
                {
                    manager.Pause();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (manager.ManagerStatus == Status.Close)
                {
                    manager.Start();    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (manager.ManagerStatus == Status.OK)
                {
                    manager.Stop();    
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
