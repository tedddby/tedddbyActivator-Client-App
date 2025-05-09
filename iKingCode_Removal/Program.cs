using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iKingCode_BypassHello
{
    static class Program
    {
        static System.Threading.Mutex singleton = new Mutex(true, "1Millonunlock");

        [STAThread]
        static void Main()
        {
            AntiDebuggers antiDebuggers = new AntiDebuggers();
            Task.Run(() => antiDebuggers.RegisteredWaitHandleAssemblyProductAttribute());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!singleton.WaitOne(TimeSpan.Zero, true))
            {
                //there is already another instance running!
                MessageBox.Show("This program is running", "[ERROR]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}