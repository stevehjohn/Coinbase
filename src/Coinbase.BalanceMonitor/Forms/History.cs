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

        private Graphics _graphics;

        public History()
        {
            InitializeComponent();
        }

        private void History_Deactivate(object sender, EventArgs e)
        {
            _graphics?.Dispose();

            Close();
        }

        public void SetData(List<int> data)
        {
            _data = data;
        }

        public void UpdateHistory()
        {
            if (_data.Count == 0)
            {
                return;
            }

            _graphics ??= CreateGraphics();

            _graphics.Clear(Color.Black);

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

            float? currentY = null;

            for (var x = Width - 2; x > -Constants.BarWidth; x -= Constants.BarWidth + Constants.BarSpace)
            {
                var barHeight = (_data[d] - min) * yScale;

                if (barHeight < 2)
                {
                    barHeight = 2;
                }

                _graphics.FillRectangle(brush, x - Constants.BarWidth, Constants.TextHeight + (Height - Constants.TextHeight * 2 - barHeight), Constants.BarWidth, barHeight);

                if (currentY == null)
                {
                    currentY = Constants.TextHeight + (Height - Constants.TextHeight * 2 - barHeight);
                }

                d--;

                if (d < 0)
                {
                    break;
                }
            }

            var pen = new Pen(Color.White, 1);

            _graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

            var font = new Font("Lucida Console", 8);

            brush = new SolidBrush(Color.White);

            var title = $"{AppSettings.Instance.CurrencySymbol}{max / 100m:N2}";

            var size = _graphics.MeasureString(title, font);

            _graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, 2);

            title = $"{AppSettings.Instance.CurrencySymbol}{min / 100m:N2}";

            size = _graphics.MeasureString(title, font);

            _graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, Height - size.Height);

            title = $"{AppSettings.Instance.CurrencySymbol}{_data.Last() / 100m:N2}";

            size = _graphics.MeasureString(title, font);

            // TODO: Sort magic constant +2
            // ReSharper disable once PossibleInvalidOperationException
            _graphics.DrawString(title, font, brush, Width - size.Width, (float) currentY - size.Height / 2f + 2);

            pen = new Pen(Color.DimGray, 1);

            _graphics.DrawLine(pen, 1, (float) currentY, Width - size.Width, (float) currentY);

        }

        private void History_Shown(object sender, EventArgs e)
        {
            UpdateHistory();
        }
    }
}
