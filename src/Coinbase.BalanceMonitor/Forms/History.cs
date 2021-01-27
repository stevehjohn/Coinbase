using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Coinbase.BalanceMonitor.Infrastructure;

namespace Coinbase.BalanceMonitor.Forms
{
    public partial class History : Form
    {
        private List<int> _data;

        public History()
        {
            InitializeComponent();
        }

        private void History_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        public void SetData(List<int> data)
        {
            _data = data;
        }

        private void History_Shown(object sender, EventArgs e)
        {
            var graphics = CreateGraphics();

            using var pen = new Pen(Color.White);

            var previousY = _data[0] - AppSettings.Instance.BalanceLow;

            var delta = AppSettings.Instance.BalanceHigh - AppSettings.Instance.BalanceLow;

            if (delta == 0)
            {
                return;
            }

            var xScale = (float) Width / _data.Count;

            var yScale = (float) Height / delta;

            for (var x = 0; x < _data.Count; x++)
            {
                var y = _data[x] - AppSettings.Instance.BalanceLow;

                graphics.DrawLine(pen, x * xScale, Height - previousY * yScale, (x + 1) * xScale, Height - y * yScale);

                previousY = y;
            }
        }
    }
}
