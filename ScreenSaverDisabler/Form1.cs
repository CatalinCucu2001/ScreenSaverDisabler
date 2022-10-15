using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaverDisabler
{
    public partial class Form1 : ApplicationContext
    {
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);


        public Form1()
        {
            InitializeComponent();

            notifyIcon.Visible = true;
            notifyIcon.Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName +
                                       "/icons/closed_eye.ico");

            TrayMenuContext();
        }

        private void TrayMenuContext()
        {
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("On", null, Eye_Click);
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (sender, args) =>
            {
                Eye(false);
                Application.Exit();
                
            });
        }

        void Eye_Click(object sender, EventArgs e)
        {
            var button = notifyIcon.ContextMenuStrip.Items[0];
            if (button.Text == "On")
            {
                Eye(true);
                button.Text = "Off";
            }
            else
            {
                Eye(false);
                button.Text = "On";
            }
            
        }

        private void Eye(bool b)
        {
            ((ToolStripMenuItem)notifyIcon.ContextMenuStrip.Items[0]).Checked = b;
            if (b)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
                notifyIcon.Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName +
                                           "/icons/opened_eye.ico");
                notifyIcon.Text = "THE WORLD SHALL KNOW PAIN!";
                return;
            }
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            notifyIcon.Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName +
                                       "/icons/closed_eye.ico");
            notifyIcon.Text = "Im sleeping";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

    }
}
