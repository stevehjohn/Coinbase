﻿using System;
using System.Windows.Forms;
using Coinbase.BalanceMonitor.Resources;
using Coinbase.BalanceMonitor.Service;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public class Context : ApplicationContext
    {
        private readonly NotifyIcon _icon;

        private readonly CoinbasePoller _poller;

        private int _previousBalance = 0;

        public Context()
        {
            var contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => Exit()));

            _icon = new NotifyIcon
                    {
                        ContextMenuStrip = contextMenu,
                        Visible = true
                    };

            _poller = new CoinbasePoller
                      {
                          Up = Up,
                          Down = Down
                      };

            _poller.StartPolling();
        }

        private void Up(int balance)
        {
            _icon.Icon = Icons.up;

            PopulateTooltip(balance);
        }

        private void Down(int balance)
        {
            _icon.Icon = Icons.down;

            PopulateTooltip(balance);
        }

        private void PopulateTooltip(int balance)
        {
            // ReSharper disable once LocalizableElement
            _icon.Text = $"{DateTime.Now:HH:mm}\r\n\r\n🡅 £{AppSettings.Instance.BalanceHigh / 100m:N2}\r\n🡆 £{balance / 100m:N2}{Difference(balance)}\r\n🡇 £{AppSettings.Instance.BalanceLow / 100m:N2}";
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

        private void Exit()
        {
            _icon.Visible = false;

            Application.Exit();
        }
    }
}