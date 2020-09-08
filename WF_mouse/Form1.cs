using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using teams_en_verde;

namespace WF_mouse
{
    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
    }

    public partial class Form1 : Form
    {
        Keyboard keyboard; 

        public Form1()
        {
            InitializeComponent();
            this.Text = "Ejecutalo como admin...Teams en verde";
            tmrMain.Interval = 60000;
            tmrMain.Enabled = true;
            keyboard = new Keyboard();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        void PreventSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Esta acción volverá a permitir que Teams se vuelva a poner ausente luego de 5 minutos. Confirma?", "Cerrar aplicacón", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AllowSleep();
            }
            else
            {
                e.Cancel = true;
                this.Activate();
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            PreventSleep();
            keyboard.Send(Keyboard.ScanCodeShort.OEM_1);
        }
    }
}
