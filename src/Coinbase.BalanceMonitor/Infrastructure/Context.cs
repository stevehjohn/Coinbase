﻿using System;
using System.IO;
using System.Windows.Forms;
using Coinbase.BalanceMonitor.Resources;
using Coinbase.BalanceMonitor.Service;
using OfficeOpenXml;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public class Context : ApplicationContext
    {
        private readonly NotifyIcon _icon;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - don't want to go out of scope.
        private readonly CryptoApiPoller _poller;

        private int _previousBalance;

        public Context()
        {
            var contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => Exit()));

            _icon = new NotifyIcon
                    {
                        ContextMenuStrip = contextMenu,
                        Visible = true
                    };

            _poller = new CryptoApiPoller
                      {
                          Up = Up,
                          Down = Down
                      };

            _poller.StartPolling();
        }

        private void Up(int balance)
        {
            _icon.Icon = balance > AppSettings.Instance.BalanceHigh
                ? Icons.up_green 
                : Icons.up;

            PopulateTooltip(balance);
        }

        private void Down(int balance)
        {
            _icon.Icon = balance < AppSettings.Instance.BalanceLow
                ? Icons.down_red
                : Icons.down;

            PopulateTooltip(balance);
        }

        private void PopulateTooltip(int balance)
        {
            var symbol = AppSettings.Instance.CurrencySymbol;

            // ReSharper disable once LocalizableElement
            _icon.Text = $"{DateTime.Now:HH:mm}\r\n\r\n🡅 {symbol}{AppSettings.Instance.BalanceHigh / 100m:N2}\r\n🡆 {symbol}{balance / 100m:N2}{Difference(balance)}\r\n🡇 {symbol}{AppSettings.Instance.BalanceLow / 100m:N2}";

            UpdateExcel(balance);
        }

        private string Difference(int balance)
        {
            if (_previousBalance == 0)
            {
                _previousBalance = balance;

                return string.Empty;
            }

            var difference = balance - _previousBalance;

            _previousBalance = balance;

            return $" {(difference < 0 ? string.Empty : '+')}{difference / 100m:N2}";
        }

        private static void UpdateExcel(int balance)
        {
            if (string.IsNullOrWhiteSpace(AppSettings.Instance.ExcelFilePath) || string.IsNullOrWhiteSpace(AppSettings.Instance.ExcelCell))
            {
                return;
            }

            try
            {
                using var package = new ExcelPackage(new FileInfo(AppSettings.Instance.ExcelFilePath));

                var sheet = package.Workbook.Worksheets[0];

                var cell = sheet.Cells[AppSettings.Instance.ExcelCell];

                cell.Style.Numberformat.Format = "£#,###,##0.00";

                cell.Value = balance / 100m;

                package.Save();
            }
            catch (Exception exception)
            {
                Logger.LogError($"An error occurred updating Excel spreadsheet {AppSettings.Instance.ExcelFilePath}", exception);
            } 
        }

        private void Exit()
        {
            _icon.Visible = false;

            Application.Exit();
        }
    }
}