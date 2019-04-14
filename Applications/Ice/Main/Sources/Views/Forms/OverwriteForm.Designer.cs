namespace Cube.FileSystem.SevenZip.Ice
{
    partial class OverwriteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverwriteForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AlwaysRenameButton = new System.Windows.Forms.Button();
            this.AlwaysYesButton = new System.Windows.Forms.Button();
            this.YesButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.AlwaysNoButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).BeginInit();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 2;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Controls.Add(this.IconPictureBox, 0, 0);
            this.RootPanel.Controls.Add(this.DescriptionLabel, 1, 0);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 1);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 2;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.RootPanel.Size = new System.Drawing.Size(484, 311);
            this.RootPanel.TabIndex = 0;
            //
            // IconPictureBox
            //
            this.IconPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconPictureBox.Location = new System.Drawing.Point(3, 3);
            this.IconPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 120);
            this.IconPictureBox.Name = "IconPictureBox";
            this.IconPictureBox.Size = new System.Drawing.Size(74, 108);
            this.IconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.IconPictureBox.TabIndex = 0;
            this.IconPictureBox.TabStop = false;
            //
            // DescriptionLabel
            //
            this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionLabel.Location = new System.Drawing.Point(84, 20);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 20, 4, 0);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(396, 211);
            this.DescriptionLabel.TabIndex = 1;
            this.DescriptionLabel.Text = "この場所には同じ名前のファイルが既に存在します。上書きしますか？\r\n\r\n現在のファイル\r\nD:\\Foo\\Bar\\Bas.txt\r\nサイズ : 12.3KB\r\n更新" +
    "日時 : 2017/07/10 23:55:55\r\n\r\n新しいファイル\r\nBas.txt\r\nサイズ : 34.5KB\r\n更新日時 : 2017/07/10 12" +
    ":12:12";
            //
            // ButtonsPanel
            //
            this.RootPanel.SetColumnSpan(this.ButtonsPanel, 2);
            this.ButtonsPanel.Controls.Add(this.AlwaysRenameButton);
            this.ButtonsPanel.Controls.Add(this.AlwaysYesButton);
            this.ButtonsPanel.Controls.Add(this.YesButton);
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.AlwaysNoButton);
            this.ButtonsPanel.Controls.Add(this.NoButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(3, 234);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ButtonsPanel.Size = new System.Drawing.Size(478, 74);
            this.ButtonsPanel.TabIndex = 2;
            //
            // AlwaysRenameButton
            //
            this.AlwaysRenameButton.Location = new System.Drawing.Point(362, 4);
            this.AlwaysRenameButton.Margin = new System.Windows.Forms.Padding(16, 4, 4, 4);
            this.AlwaysRenameButton.Name = "AlwaysRenameButton";
            this.AlwaysRenameButton.Size = new System.Drawing.Size(100, 25);
            this.AlwaysRenameButton.TabIndex = 4;
            this.AlwaysRenameButton.Text = "すべてリネーム";
            this.AlwaysRenameButton.UseVisualStyleBackColor = true;
            //
            // AlwaysYesButton
            //
            this.AlwaysYesButton.Location = new System.Drawing.Point(254, 4);
            this.AlwaysYesButton.Margin = new System.Windows.Forms.Padding(4);
            this.AlwaysYesButton.Name = "AlwaysYesButton";
            this.AlwaysYesButton.Size = new System.Drawing.Size(100, 25);
            this.AlwaysYesButton.TabIndex = 2;
            this.AlwaysYesButton.Text = "すべてはい";
            this.AlwaysYesButton.UseVisualStyleBackColor = true;
            //
            // YesButton
            //
            this.ButtonsPanel.SetFlowBreak(this.YesButton, true);
            this.YesButton.Location = new System.Drawing.Point(146, 4);
            this.YesButton.Margin = new System.Windows.Forms.Padding(4);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(100, 25);
            this.YesButton.TabIndex = 0;
            this.YesButton.Text = "はい";
            this.YesButton.UseVisualStyleBackColor = true;
            //
            // ExitButton
            //
            this.ExitButton.Location = new System.Drawing.Point(362, 37);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(16, 4, 4, 4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 25);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // AlwaysNoButton
            //
            this.AlwaysNoButton.Location = new System.Drawing.Point(254, 37);
            this.AlwaysNoButton.Margin = new System.Windows.Forms.Padding(4);
            this.AlwaysNoButton.Name = "AlwaysNoButton";
            this.AlwaysNoButton.Size = new System.Drawing.Size(100, 25);
            this.AlwaysNoButton.TabIndex = 3;
            this.AlwaysNoButton.Text = "すべていいえ";
            this.AlwaysNoButton.UseVisualStyleBackColor = true;
            //
            // NoButton
            //
            this.NoButton.Location = new System.Drawing.Point(146, 37);
            this.NoButton.Margin = new System.Windows.Forms.Padding(4);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(100, 25);
            this.NoButton.TabIndex = 1;
            this.NoButton.Text = "いいえ";
            this.NoButton.UseVisualStyleBackColor = true;
            //
            // OverwriteForm
            //
            this.AcceptButton = this.YesButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverwriteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "上書きの確認";
            this.RootPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.PictureBox IconPictureBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button AlwaysRenameButton;
        private System.Windows.Forms.Button AlwaysYesButton;
        private System.Windows.Forms.Button YesButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button AlwaysNoButton;
        private System.Windows.Forms.Button NoButton;
    }
}