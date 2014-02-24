using System;
using System.Windows.Forms;

namespace ExileClipboardListener.Classes
{
    static class Program
    {
        ///<summary>
        ///The main entry point for the application.
        ///</summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (var pi = new ProcessIcon())
                {
                    GlobalMethods.LoadCache();
                    pi.Display();
                    Application.Run();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
