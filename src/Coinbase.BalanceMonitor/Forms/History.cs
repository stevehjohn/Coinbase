using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var delta = _data.Max() - min;

            if (delta == 0)
            {
                return;
            }

            var yScale = (float) (Height - Constants.TextHeight * 2) / delta;

            var brush = new SolidBrush(Color.DarkSlateBlue);

            var d = _data.Count - 1;

            for (var x = Width; x > 0; x -= Constants.BarWidth + Constants.BarSpace)
            {
                graphics.FillRectangle(brush, x - Constants.BarWidth, Constants.TextHeight + ((Height - Constants.TextHeight * 2) - (_data[d] - min) * yScale), Constants.BarWidth, (_data[d] - min) * yScale);

                d--;

                if (d < 0)
                {
                    break;
                }
            }
        }
    }
}
