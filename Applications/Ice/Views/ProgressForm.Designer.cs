namespace Cube.FileSystem.App.Ice
{
    partial class ProgressForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RemainLabel = new System.Windows.Forms.Label();
            this.FileCountLabel = new System.Windows.Forms.Label();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.ElapseLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RootPanel
            // 
            this.RootPanel.ColumnCount = 2;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.Controls.Add(this.ExitButton, 1, 4);
            this.RootPanel.Controls.Add(this.RemainLabel, 1, 2);
            this.RootPanel.Controls.Add(this.FileCountLabel, 0, 0);
            this.RootPanel.Controls.Add(this.MainProgressBar, 0, 1);
            this.RootPanel.Controls.Add(this.ElapseLabel, 0, 2);
            this.RootPanel.Controls.Add(this.StatusLabel, 0, 3);
            this.RootPanel.Controls.Add(this.SuspendButton, 0, 4);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 5;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Size = new System.Drawing.Size(434, 171);
            this.RootPanel.TabIndex = 0;
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExitButton.Location = new System.Drawing.Point(275, 129);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 25);
            this.ExitButton.TabIndex = 6;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // RemainLabel
            // 
            this.RemainLabel.AutoSize = true;
            this.RemainLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemainLabel.Location = new System.Drawing.Point(229, 57);
            this.RemainLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.RemainLabel.Name = "RemainLabel";
            this.RemainLabel.Size = new System.Drawing.Size(193, 18);
            this.RemainLabel.TabIndex = 3;
            this.RemainLabel.Text = "残り時間 : 約 23:55:55";
            this.RemainLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.RemainLabel.Visible = false;
            // 
            // FileCountLabel
            // 
            this.FileCountLabel.AutoSize = true;
            this.RootPanel.SetColumnSpan(this.FileCountLabel, 2);
            this.FileCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileCountLabel.Location = new System.Drawing.Point(12, 3);
            this.FileCountLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.FileCountLabel.Name = "FileCountLabel";
            this.FileCountLabel.Size = new System.Drawing.Size(410, 24);
            this.FileCountLabel.TabIndex = 0;
            this.FileCountLabel.Text = "ファイル数 : 669 / 796";
            this.FileCountLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // MainProgressBar
            // 
            this.RootPanel.SetColumnSpan(this.MainProgressBar, 2);
            this.MainProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainProgressBar.Location = new System.Drawing.Point(14, 33);
            this.MainProgressBar.Margin = new System.Windows.Forms.Padding(14, 3, 14, 3);
            this.MainProgressBar.MarqueeAnimationSpeed = 50;
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(406, 18);
            this.MainProgressBar.Step = 1;
            this.MainProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.MainProgressBar.TabIndex = 1;
            // 
            // ElapseLabel
            // 
            this.ElapseLabel.AutoSize = true;
            this.ElapseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElapseLabel.Location = new System.Drawing.Point(12, 57);
            this.ElapseLabel.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.ElapseLabel.Name = "ElapseLabel";
            this.ElapseLabel.Size = new System.Drawing.Size(193, 18);
            this.ElapseLabel.TabIndex = 2;
            this.ElapseLabel.Text = "経過時間 : 23:55:55";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoEllipsis = true;
            this.StatusLabel.AutoSize = true;
            this.RootPanel.SetColumnSpan(this.StatusLabel, 2);
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusLabel.Location = new System.Drawing.Point(12, 84);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(410, 36);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "ファイル圧縮・解凍処理の準備中です...";
            // 
            // SuspendButton
            // 
            this.SuspendButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SuspendButton.Location = new System.Drawing.Point(58, 129);
            this.SuspendButton.Name = "SuspendButton";
            this.SuspendButton.Size = new System.Drawing.Size(100, 25);
            this.SuspendButton.TabIndex = 5;
            this.SuspendButton.Text = "中断";
            this.SuspendButton.UseVisualStyleBackColor = true;
            // 
            // ProgressForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(434, 171);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.Text = "CubeICE";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.Label FileCountLabel;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.Label ElapseLabel;
        private System.Windows.Forms.Label RemainLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button SuspendButton;
        private System.Windows.Forms.Button ExitButton;
    }
}

