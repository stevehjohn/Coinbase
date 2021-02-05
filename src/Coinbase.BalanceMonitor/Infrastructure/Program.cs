using System;
using System.Windows.Forms;
using OfficeOpenXml;

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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Application.Run(new Context());
        }
    }
}
