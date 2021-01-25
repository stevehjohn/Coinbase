using System;
using System.Windows.Forms;
using Coinbase.BalanceMonitor.Resources;
using Coinbase.BalanceMonitor.Service;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public class Context : ApplicationContext
    {
        private readonly NotifyIcon _icon;

        private readonly CoinbasePoller _poller;

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

            // ReSharper disable once LocalizableElement
            _icon.Text = $"£{balance / 100m:N2} at {DateTime.Now:HH:mm}";
        }

        private void Down(int balance)
        {
            _icon.Icon = Icons.down;

            // ReSharper disable once LocalizableElement
            _icon.Text = $"£{balance / 100m:N2} at {DateTime.Now:HH:mm}";
        }

        private void Exit()
        {
            _icon.Visible = false;

            Application.Exit();
        }
    }
}