﻿
namespace Coinbase.BalanceMonitor.Forms
{
    partial class History
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(500, 200);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "History";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "History";
            this.Deactivate += new System.EventHandler(this.History_Deactivate);
            this.Shown += new System.EventHandler(this.History_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.History_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.History_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.History_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}