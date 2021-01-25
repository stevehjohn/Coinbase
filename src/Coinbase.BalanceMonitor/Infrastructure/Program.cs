using System;
using System.Windows.Forms;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Context());
        }
    }
}
