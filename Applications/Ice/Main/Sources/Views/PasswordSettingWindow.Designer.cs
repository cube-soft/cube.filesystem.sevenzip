namespace Cube.FileSystem.SevenZip.Ice
{
    partial class PasswordSettingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordSettingWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.ShowPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.ConfirmTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            this.RootPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).BeginInit();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 2;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 5);
            this.RootPanel.Controls.Add(this.ShowPasswordCheckBox, 1, 3);
            this.RootPanel.Controls.Add(this.ConfirmTextBox, 1, 2);
            this.RootPanel.Controls.Add(this.PasswordTextBox, 1, 1);
            this.RootPanel.Controls.Add(this.DescriptionLabel, 1, 0);
            this.RootPanel.Controls.Add(this.IconPictureBox, 0, 0);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 6;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.RootPanel.Size = new System.Drawing.Size(384, 201);
            this.RootPanel.TabIndex = 0;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.RootPanel.SetColumnSpan(this.ButtonsPanel, 2);
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.ExecuteButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 145);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 9, 9, 0);
            this.ButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ButtonsPanel.Size = new System.Drawing.Size(384, 56);
            this.ButtonsPanel.TabIndex = 6;
            //
            // ExitButton
            //
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(247, 12);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(125, 30);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // ExecuteButton
            //
            this.ExecuteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecuteButton.Enabled = false;
            this.ExecuteButton.Location = new System.Drawing.Point(116, 12);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(125, 30);
            this.ExecuteButton.TabIndex = 0;
            this.ExecuteButton.Text = "OK";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            //
            // ShowPasswordCheckBox
            //
            this.ShowPasswordCheckBox.AutoSize = true;
            this.ShowPasswordCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowPasswordCheckBox.Location = new System.Drawing.Point(83, 101);
            this.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox";
            this.ShowPasswordCheckBox.Size = new System.Drawing.Size(298, 20);
            this.ShowPasswordCheckBox.TabIndex = 4;
            this.ShowPasswordCheckBox.Text = "パスワードを表示する";
            this.ShowPasswordCheckBox.UseVisualStyleBackColor = true;
            //
            // ConfirmTextBox
            //
            this.ConfirmTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmTextBox.Location = new System.Drawing.Point(83, 73);
            this.ConfirmTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 23, 4);
            this.ConfirmTextBox.Name = "ConfirmTextBox";
            this.ConfirmTextBox.Size = new System.Drawing.Size(278, 23);
            this.ConfirmTextBox.TabIndex = 3;
            this.ConfirmTextBox.UseSystemPasswordChar = true;
            //
            // PasswordTextBox
            //
            this.PasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordTextBox.Location = new System.Drawing.Point(83, 44);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 23, 4);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(278, 23);
            this.PasswordTextBox.TabIndex = 2;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            //
            // DescriptionLabel
            //
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionLabel.Location = new System.Drawing.Point(83, 0);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(298, 35);
            this.DescriptionLabel.TabIndex = 100;
            this.DescriptionLabel.Text = "パスワードを設定して下さい。";
            this.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // IconPictureBox
            //
            this.IconPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconPictureBox.Location = new System.Drawing.Point(3, 4);
            this.IconPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 30);
            this.IconPictureBox.Name = "IconPictureBox";
            this.RootPanel.SetRowSpan(this.IconPictureBox, 4);
            this.IconPictureBox.Size = new System.Drawing.Size(74, 90);
            this.IconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.IconPictureBox.TabIndex = 1;
            this.IconPictureBox.TabStop = false;
            //
            // PasswordSettingWindow
            //
            this.AcceptButton = this.ExecuteButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(384, 201);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordSettingWindow";
            this.ShowInTaskbar = false;
            this.Text = "パスワードの設定 - CubeICE";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.PictureBox IconPictureBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox ConfirmTextBox;
        private System.Windows.Forms.CheckBox ShowPasswordCheckBox;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExecuteButton;
    }
}