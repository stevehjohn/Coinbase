using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            if (_data.Count == 0)
            {
                return;
            }
            
            var graphics = CreateGraphics();

            var min = _data.Min();

            var max = _data.Max();

            var delta = max - min;

            if (delta == 0)
            {
                return;
            }

            var yScale = (float) (Height - Constants.TextHeight * 2) / delta;

            var brush = new SolidBrush(Color.DarkSlateBlue);

            var d = _data.Count - 1;

            for (var x = Width - 2; x > -Constants.BarWidth; x -= Constants.BarWidth + Constants.BarSpace)
            {
                var barHeight = (_data[d] - min) * yScale;

                if (barHeight < 2)
                {
                    barHeight = 2;
                }

                graphics.FillRectangle(brush, x - Constants.BarWidth, Constants.TextHeight + (Height - Constants.TextHeight * 2 - barHeight), Constants.BarWidth, barHeight);

                d--;

                if (d < 0)
                {
                    break;
                }
            }

            var pen = new Pen(Color.White, 1);

            graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

            var font = new Font("Lucida Console", 8);

            brush = new SolidBrush(Color.White);

            var title = $"{AppSettings.Instance.CurrencySymbol}{max / 100m:N2}";

            var size = graphics.MeasureString(title, font);

            graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, 2);

            title = $"{AppSettings.Instance.CurrencySymbol}{min / 100m:N2}";

            size = graphics.MeasureString(title, font);

            graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, Height - size.Height - 1);
        }
    }
}
