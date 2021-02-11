using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Coinbase.BalanceMonitor.Infrastructure;
using Microsoft.VisualBasic.Devices;

namespace Coinbase.BalanceMonitor.Forms
{
    public partial class History : Form
    {
        private List<int> _data;

        public History()
        {
            InitializeComponent();
        }

        private Point? _previousMouse;

        private void History_Deactivate(object sender, EventArgs e)
        {
            if (! TopMost)
            {
                Close();
            }
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

            using var graphics = CreateGraphics();

            graphics.Clear(Color.Black);

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

                graphics.FillRectangle(brush, x - Constants.BarWidth, Constants.TextHeight + (Height - Constants.TextHeight * 2 - barHeight), Constants.BarWidth, barHeight);

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

            graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

            var font = new Font("Lucida Console", 8);

            brush = new SolidBrush(Color.White);

            var title = $"{AppSettings.Instance.CurrencySymbol}{max / 100m:N2}";

            var size = graphics.MeasureString(title, font);

            graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, 2);

            title = $"{AppSettings.Instance.CurrencySymbol}{min / 100m:N2}";

            size = graphics.MeasureString(title, font);

            graphics.DrawString(title, font, brush, Width / 2f - size.Width / 2, Height - size.Height);

            title = $"{AppSettings.Instance.CurrencySymbol}{_data.Last() / 100m:N2}";

            size = graphics.MeasureString(title, font);

            // TODO: Sort magic constant +2
            // ReSharper disable once PossibleInvalidOperationException
            graphics.DrawString(title, font, brush, Width - size.Width, (float) currentY - size.Height / 2f + 2);

            pen = new Pen(Color.DimGray, 1);

            graphics.DrawLine(pen, 1, (float) currentY, Width - size.Width, (float) currentY);
        }

        private void History_Shown(object sender, EventArgs e)
        {
            Cursor = TopMost 
                ? Cursors.SizeAll 
                : Cursors.Default;

            UpdateHistory();
        }

        private void History_MouseDown(object sender, MouseEventArgs e)
        {
            _previousMouse = Cursor.Position;
        }

        private void History_MouseUp(object sender, MouseEventArgs e)
        {
            _previousMouse = null;
        }

        private void History_MouseMove(object sender, MouseEventArgs e)
        {
            if (! _previousMouse.HasValue)
            {
                return;
            }

            Left += Cursor.Position.X - _previousMouse.Value.X;

            Top += Cursor.Position.Y - _previousMouse.Value.Y;

            _previousMouse = Cursor.Position;
        }
    }
}
