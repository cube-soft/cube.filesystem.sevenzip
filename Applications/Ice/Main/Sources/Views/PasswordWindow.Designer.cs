namespace Cube.FileSystem.SevenZip.Ice
{
    partial class PasswordWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IconPictureBox = new System.Windows.Forms.PictureBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.VisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExecButton = new System.Windows.Forms.Button();
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
            this.RootPanel.Controls.Add(this.PasswordTextBox, 1, 1);
            this.RootPanel.Controls.Add(this.VisibleCheckBox, 1, 2);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 4);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 5;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.RootPanel.Size = new System.Drawing.Size(384, 171);
            this.RootPanel.TabIndex = 0;
            //
            // IconPictureBox
            //
            this.IconPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconPictureBox.Location = new System.Drawing.Point(3, 3);
            this.IconPictureBox.Name = "IconPictureBox";
            this.RootPanel.SetRowSpan(this.IconPictureBox, 3);
            this.IconPictureBox.Size = new System.Drawing.Size(74, 89);
            this.IconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.IconPictureBox.TabIndex = 0;
            this.IconPictureBox.TabStop = false;
            //
            // DescriptionLabel
            //
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionLabel.Location = new System.Drawing.Point(83, 0);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(298, 36);
            this.DescriptionLabel.TabIndex = 1;
            this.DescriptionLabel.Text = "パスワードを入力して下さい。";
            this.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // PasswordTextBox
            //
            this.PasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordTextBox.Location = new System.Drawing.Point(83, 43);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(281, 23);
            this.PasswordTextBox.TabIndex = 2;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            //
            // VisibleCheckBox
            //
            this.VisibleCheckBox.AutoSize = true;
            this.VisibleCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisibleCheckBox.Location = new System.Drawing.Point(83, 72);
            this.VisibleCheckBox.Name = "VisibleCheckBox";
            this.VisibleCheckBox.Size = new System.Drawing.Size(298, 20);
            this.VisibleCheckBox.TabIndex = 3;
            this.VisibleCheckBox.Text = "パスワードを表示する";
            this.VisibleCheckBox.UseVisualStyleBackColor = true;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.RootPanel.SetColumnSpan(this.ButtonsPanel, 2);
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.ExecButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 115);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Padding = new System.Windows.Forms.Padding(0, 9, 9, 0);
            this.ButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ButtonsPanel.Size = new System.Drawing.Size(384, 56);
            this.ButtonsPanel.TabIndex = 4;
            //
            // ExitButton
            //
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(247, 12);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(125, 30);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // ExecButton
            //
            this.ExecButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecButton.Enabled = false;
            this.ExecButton.Location = new System.Drawing.Point(116, 12);
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.Size = new System.Drawing.Size(125, 30);
            this.ExecButton.TabIndex = 1;
            this.ExecButton.Text = "OK";
            this.ExecButton.UseVisualStyleBackColor = true;
            //
            // PasswordWindow
            //
            this.AcceptButton = this.ExecButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(384, 171);
            this.Controls.Add(this.RootPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordWindow";
            this.ShowInTaskbar = false;
            this.Text = "パスワードの入力 - CubeICE";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconPictureBox)).EndInit();
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.PictureBox IconPictureBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.CheckBox VisibleCheckBox;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExecButton;
    }
}