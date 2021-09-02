namespace Cube.FileSystem.SevenZip.Ice
{
    partial class ProgressWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RemainLabel = new System.Windows.Forms.Label();
            this.MainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CountLabel = new System.Windows.Forms.Label();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.ElapseLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.HeaderPictureBox = new System.Windows.Forms.PictureBox();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPictureBox)).BeginInit();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 2;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.Controls.Add(this.RemainLabel, 1, 3);
            this.RootPanel.Controls.Add(this.CountLabel, 0, 1);
            this.RootPanel.Controls.Add(this.MainProgressBar, 0, 2);
            this.RootPanel.Controls.Add(this.ElapseLabel, 0, 3);
            this.RootPanel.Controls.Add(this.StatusLabel, 0, 4);
            this.RootPanel.Controls.Add(this.HeaderPictureBox, 0, 0);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 5);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 6;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.RootPanel.Size = new System.Drawing.Size(434, 214);
            this.RootPanel.TabIndex = 0;
            //
            // RemainLabel
            //
            this.RemainLabel.AutoSize = true;
            this.RemainLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MainBindingSource, "Remaining", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RemainLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemainLabel.Location = new System.Drawing.Point(229, 91);
            this.RemainLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.RemainLabel.Name = "RemainLabel";
            this.RemainLabel.Size = new System.Drawing.Size(193, 18);
            this.RemainLabel.TabIndex = 4;
            this.RemainLabel.Text = "残り時間 : 23:55:55";
            this.RemainLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.RemainLabel.Visible = false;
            //
            // MainBindingSource
            //
            this.MainBindingSource.DataSource = typeof(Cube.FileSystem.SevenZip.Ice.ProgressViewModel);
            //
            // CountLabel
            //
            this.CountLabel.AutoSize = true;
            this.RootPanel.SetColumnSpan(this.CountLabel, 2);
            this.CountLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MainBindingSource, "Count", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CountLabel.Location = new System.Drawing.Point(12, 43);
            this.CountLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(410, 18);
            this.CountLabel.TabIndex = 1;
            this.CountLabel.Text = "ファイル数 : 669 / 796";
            this.CountLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            //
            // MainProgressBar
            //
            this.RootPanel.SetColumnSpan(this.MainProgressBar, 2);
            this.MainProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainProgressBar.Location = new System.Drawing.Point(14, 67);
            this.MainProgressBar.Margin = new System.Windows.Forms.Padding(14, 3, 14, 3);
            this.MainProgressBar.MarqueeAnimationSpeed = 50;
            this.MainProgressBar.Maximum = 1000;
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(406, 18);
            this.MainProgressBar.Step = 1;
            this.MainProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.MainProgressBar.TabIndex = 2;
            //
            // ElapseLabel
            //
            this.ElapseLabel.AutoSize = true;
            this.ElapseLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MainBindingSource, "Elapsed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ElapseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElapseLabel.Location = new System.Drawing.Point(12, 91);
            this.ElapseLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.ElapseLabel.Name = "ElapseLabel";
            this.ElapseLabel.Size = new System.Drawing.Size(193, 18);
            this.ElapseLabel.TabIndex = 3;
            this.ElapseLabel.Text = "経過時間 : 23:55:55";
            //
            // StatusLabel
            //
            this.StatusLabel.AutoEllipsis = true;
            this.StatusLabel.AutoSize = true;
            this.RootPanel.SetColumnSpan(this.StatusLabel, 2);
            this.StatusLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MainBindingSource, "Text", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusLabel.Location = new System.Drawing.Point(12, 115);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(410, 42);
            this.StatusLabel.TabIndex = 5;
            this.StatusLabel.Text = "ファイル圧縮・解凍処理の準備中です...";
            //
            // HeaderPictureBox
            //
            this.RootPanel.SetColumnSpan(this.HeaderPictureBox, 2);
            this.HeaderPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPictureBox.Image = global::Cube.FileSystem.SevenZip.Ice.Properties.Resources.Hero;
            this.HeaderPictureBox.Location = new System.Drawing.Point(0, 0);
            this.HeaderPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.HeaderPictureBox.Name = "HeaderPictureBox";
            this.HeaderPictureBox.Size = new System.Drawing.Size(434, 40);
            this.HeaderPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.HeaderPictureBox.TabIndex = 5;
            this.HeaderPictureBox.TabStop = false;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.RootPanel.SetColumnSpan(this.ButtonsPanel, 2);
            this.ButtonsPanel.Controls.Add(this.SuspendButton);
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 160);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.ButtonsPanel.Size = new System.Drawing.Size(434, 54);
            this.ButtonsPanel.TabIndex = 0;
            //
            // SuspendButton
            //
            this.SuspendButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SuspendButton.Enabled = false;
            this.SuspendButton.Location = new System.Drawing.Point(60, 12);
            this.SuspendButton.Margin = new System.Windows.Forms.Padding(60, 3, 3, 3);
            this.SuspendButton.Name = "SuspendButton";
            this.SuspendButton.Size = new System.Drawing.Size(125, 30);
            this.SuspendButton.TabIndex = 1;
            this.SuspendButton.Text = "一時停止";
            this.SuspendButton.UseVisualStyleBackColor = true;
            //
            // ExitButton
            //
            this.ExitButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExitButton.Location = new System.Drawing.Point(248, 12);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(60, 3, 3, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(125, 30);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // ProgressWindow
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(434, 214);
            this.Controls.Add(this.RootPanel);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.MainBindingSource, "Title", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProgressWindow";
            this.Text = "CubeICE";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeaderPictureBox)).EndInit();
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button SuspendButton;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.PictureBox HeaderPictureBox;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Label ElapseLabel;
        private System.Windows.Forms.Label RemainLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.BindingSource MainBindingSource;
    }
}

