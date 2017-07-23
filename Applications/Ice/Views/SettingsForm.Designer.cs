﻿namespace Cube.FileSystem.App.Ice
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OutputPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.OutputButton = new System.Windows.Forms.Button();
            this.GeneralGroupBox = new System.Windows.Forms.GroupBox();
            this.GeneralPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ThreadLabel = new System.Windows.Forms.Label();
            this.MethodComboBox = new System.Windows.Forms.ComboBox();
            this.MethodLabel = new System.Windows.Forms.Label();
            this.LevelComboBox = new System.Windows.Forms.ComboBox();
            this.LevelLabel = new System.Windows.Forms.Label();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.FormatComboBox = new System.Windows.Forms.ComboBox();
            this.ThreadNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.EncryptionGroupBox = new System.Windows.Forms.GroupBox();
            this.EncryptionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.EncryptionComboBox = new System.Windows.Forms.ComboBox();
            this.EncryptionLabel = new System.Windows.Forms.Label();
            this.ConfirmTextBox = new System.Windows.Forms.TextBox();
            this.ConfirmLabel = new System.Windows.Forms.Label();
            this.EncryptionCheckBox = new System.Windows.Forms.CheckBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ShowPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.OutputPanel.SuspendLayout();
            this.GeneralGroupBox.SuspendLayout();
            this.GeneralPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadNumericUpDown)).BeginInit();
            this.EncryptionGroupBox.SuspendLayout();
            this.EncryptionPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RootPanel
            // 
            this.RootPanel.ColumnCount = 1;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Controls.Add(this.OutputPanel, 0, 0);
            this.RootPanel.Controls.Add(this.GeneralGroupBox, 0, 1);
            this.RootPanel.Controls.Add(this.EncryptionGroupBox, 0, 2);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 3);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 4;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Size = new System.Drawing.Size(364, 441);
            this.RootPanel.TabIndex = 0;
            // 
            // OutputPanel
            // 
            this.OutputPanel.Controls.Add(this.OutputLabel);
            this.OutputPanel.Controls.Add(this.OutputTextBox);
            this.OutputPanel.Controls.Add(this.OutputButton);
            this.OutputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputPanel.Location = new System.Drawing.Point(3, 3);
            this.OutputPanel.Name = "OutputPanel";
            this.OutputPanel.Size = new System.Drawing.Size(358, 44);
            this.OutputPanel.TabIndex = 0;
            // 
            // OutputLabel
            // 
            this.OutputLabel.Location = new System.Drawing.Point(12, 3);
            this.OutputLabel.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(55, 38);
            this.OutputLabel.TabIndex = 0;
            this.OutputLabel.Text = "出力先：";
            this.OutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(73, 11);
            this.OutputTextBox.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(210, 23);
            this.OutputTextBox.TabIndex = 1;
            // 
            // OutputButton
            // 
            this.OutputButton.Location = new System.Drawing.Point(289, 11);
            this.OutputButton.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.OutputButton.Name = "OutputButton";
            this.OutputButton.Size = new System.Drawing.Size(60, 23);
            this.OutputButton.TabIndex = 2;
            this.OutputButton.Text = "...";
            this.OutputButton.UseVisualStyleBackColor = true;
            // 
            // GeneralGroupBox
            // 
            this.GeneralGroupBox.Controls.Add(this.GeneralPanel);
            this.GeneralGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralGroupBox.Location = new System.Drawing.Point(12, 53);
            this.GeneralGroupBox.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.GeneralGroupBox.Name = "GeneralGroupBox";
            this.GeneralGroupBox.Padding = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.GeneralGroupBox.Size = new System.Drawing.Size(340, 144);
            this.GeneralGroupBox.TabIndex = 1;
            this.GeneralGroupBox.TabStop = false;
            this.GeneralGroupBox.Text = "詳細";
            // 
            // GeneralPanel
            // 
            this.GeneralPanel.ColumnCount = 2;
            this.GeneralPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.GeneralPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GeneralPanel.Controls.Add(this.ThreadLabel, 0, 3);
            this.GeneralPanel.Controls.Add(this.MethodComboBox, 1, 2);
            this.GeneralPanel.Controls.Add(this.MethodLabel, 0, 2);
            this.GeneralPanel.Controls.Add(this.LevelComboBox, 1, 1);
            this.GeneralPanel.Controls.Add(this.LevelLabel, 0, 1);
            this.GeneralPanel.Controls.Add(this.FormatLabel, 0, 0);
            this.GeneralPanel.Controls.Add(this.FormatComboBox, 1, 0);
            this.GeneralPanel.Controls.Add(this.ThreadNumericUpDown, 1, 3);
            this.GeneralPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralPanel.Location = new System.Drawing.Point(8, 19);
            this.GeneralPanel.Name = "GeneralPanel";
            this.GeneralPanel.RowCount = 4;
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.GeneralPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GeneralPanel.Size = new System.Drawing.Size(324, 117);
            this.GeneralPanel.TabIndex = 0;
            // 
            // ThreadLabel
            // 
            this.ThreadLabel.AutoSize = true;
            this.ThreadLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadLabel.Location = new System.Drawing.Point(3, 90);
            this.ThreadLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ThreadLabel.Name = "ThreadLabel";
            this.ThreadLabel.Size = new System.Drawing.Size(94, 24);
            this.ThreadLabel.TabIndex = 6;
            this.ThreadLabel.Text = "スレッド数：";
            this.ThreadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MethodComboBox
            // 
            this.MethodComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MethodComboBox.FormattingEnabled = true;
            this.MethodComboBox.Location = new System.Drawing.Point(103, 61);
            this.MethodComboBox.Name = "MethodComboBox";
            this.MethodComboBox.Size = new System.Drawing.Size(218, 23);
            this.MethodComboBox.TabIndex = 5;
            // 
            // MethodLabel
            // 
            this.MethodLabel.AutoSize = true;
            this.MethodLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MethodLabel.Location = new System.Drawing.Point(3, 61);
            this.MethodLabel.Margin = new System.Windows.Forms.Padding(3);
            this.MethodLabel.Name = "MethodLabel";
            this.MethodLabel.Size = new System.Drawing.Size(94, 23);
            this.MethodLabel.TabIndex = 4;
            this.MethodLabel.Text = "圧縮方法：";
            this.MethodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LevelComboBox
            // 
            this.LevelComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LevelComboBox.FormattingEnabled = true;
            this.LevelComboBox.Location = new System.Drawing.Point(103, 32);
            this.LevelComboBox.Name = "LevelComboBox";
            this.LevelComboBox.Size = new System.Drawing.Size(218, 23);
            this.LevelComboBox.TabIndex = 3;
            // 
            // LevelLabel
            // 
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LevelLabel.Location = new System.Drawing.Point(3, 32);
            this.LevelLabel.Margin = new System.Windows.Forms.Padding(3);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(94, 23);
            this.LevelLabel.TabIndex = 2;
            this.LevelLabel.Text = "圧縮レベル：";
            this.LevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormatLabel
            // 
            this.FormatLabel.AutoSize = true;
            this.FormatLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormatLabel.Location = new System.Drawing.Point(3, 3);
            this.FormatLabel.Margin = new System.Windows.Forms.Padding(3);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(94, 23);
            this.FormatLabel.TabIndex = 0;
            this.FormatLabel.Text = "ファイル形式：";
            this.FormatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormatComboBox
            // 
            this.FormatComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatComboBox.FormattingEnabled = true;
            this.FormatComboBox.Location = new System.Drawing.Point(103, 3);
            this.FormatComboBox.Name = "FormatComboBox";
            this.FormatComboBox.Size = new System.Drawing.Size(218, 23);
            this.FormatComboBox.TabIndex = 1;
            // 
            // ThreadNumericUpDown
            // 
            this.ThreadNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThreadNumericUpDown.Location = new System.Drawing.Point(103, 90);
            this.ThreadNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadNumericUpDown.Name = "ThreadNumericUpDown";
            this.ThreadNumericUpDown.Size = new System.Drawing.Size(218, 23);
            this.ThreadNumericUpDown.TabIndex = 7;
            this.ThreadNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EncryptionGroupBox
            // 
            this.EncryptionGroupBox.Controls.Add(this.EncryptionPanel);
            this.EncryptionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionGroupBox.Location = new System.Drawing.Point(12, 203);
            this.EncryptionGroupBox.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.EncryptionGroupBox.Name = "EncryptionGroupBox";
            this.EncryptionGroupBox.Padding = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.EncryptionGroupBox.Size = new System.Drawing.Size(340, 174);
            this.EncryptionGroupBox.TabIndex = 2;
            this.EncryptionGroupBox.TabStop = false;
            this.EncryptionGroupBox.Text = "暗号化";
            // 
            // EncryptionPanel
            // 
            this.EncryptionPanel.ColumnCount = 2;
            this.EncryptionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.EncryptionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionPanel.Controls.Add(this.EncryptionComboBox, 1, 4);
            this.EncryptionPanel.Controls.Add(this.EncryptionLabel, 0, 4);
            this.EncryptionPanel.Controls.Add(this.ConfirmTextBox, 1, 2);
            this.EncryptionPanel.Controls.Add(this.ConfirmLabel, 0, 2);
            this.EncryptionPanel.Controls.Add(this.EncryptionCheckBox, 0, 0);
            this.EncryptionPanel.Controls.Add(this.PasswordLabel, 0, 1);
            this.EncryptionPanel.Controls.Add(this.PasswordTextBox, 1, 1);
            this.EncryptionPanel.Controls.Add(this.ShowPasswordCheckBox, 1, 3);
            this.EncryptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionPanel.Location = new System.Drawing.Point(8, 19);
            this.EncryptionPanel.Name = "EncryptionPanel";
            this.EncryptionPanel.RowCount = 5;
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.EncryptionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.EncryptionPanel.Size = new System.Drawing.Size(324, 147);
            this.EncryptionPanel.TabIndex = 0;
            // 
            // EncryptionComboBox
            // 
            this.EncryptionComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncryptionComboBox.FormattingEnabled = true;
            this.EncryptionComboBox.Location = new System.Drawing.Point(103, 121);
            this.EncryptionComboBox.Name = "EncryptionComboBox";
            this.EncryptionComboBox.Size = new System.Drawing.Size(218, 23);
            this.EncryptionComboBox.TabIndex = 7;
            // 
            // EncryptionLabel
            // 
            this.EncryptionLabel.AutoSize = true;
            this.EncryptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionLabel.Location = new System.Drawing.Point(3, 121);
            this.EncryptionLabel.Margin = new System.Windows.Forms.Padding(3);
            this.EncryptionLabel.Name = "EncryptionLabel";
            this.EncryptionLabel.Size = new System.Drawing.Size(94, 23);
            this.EncryptionLabel.TabIndex = 6;
            this.EncryptionLabel.Text = "暗号化方法：";
            this.EncryptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfirmTextBox
            // 
            this.ConfirmTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmTextBox.Location = new System.Drawing.Point(103, 63);
            this.ConfirmTextBox.Name = "ConfirmTextBox";
            this.ConfirmTextBox.Size = new System.Drawing.Size(218, 23);
            this.ConfirmTextBox.TabIndex = 4;
            // 
            // ConfirmLabel
            // 
            this.ConfirmLabel.AutoSize = true;
            this.ConfirmLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConfirmLabel.Location = new System.Drawing.Point(3, 63);
            this.ConfirmLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ConfirmLabel.Name = "ConfirmLabel";
            this.EncryptionPanel.SetRowSpan(this.ConfirmLabel, 2);
            this.ConfirmLabel.Size = new System.Drawing.Size(94, 52);
            this.ConfirmLabel.TabIndex = 3;
            this.ConfirmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EncryptionCheckBox
            // 
            this.EncryptionCheckBox.AutoSize = true;
            this.EncryptionPanel.SetColumnSpan(this.EncryptionCheckBox, 2);
            this.EncryptionCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EncryptionCheckBox.Location = new System.Drawing.Point(3, 3);
            this.EncryptionCheckBox.Name = "EncryptionCheckBox";
            this.EncryptionCheckBox.Size = new System.Drawing.Size(318, 25);
            this.EncryptionCheckBox.TabIndex = 0;
            this.EncryptionCheckBox.Text = "パスワードを設定する";
            this.EncryptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordLabel.Location = new System.Drawing.Point(3, 34);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(3);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(94, 23);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "パスワード：";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PasswordTextBox.Location = new System.Drawing.Point(103, 34);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(218, 23);
            this.PasswordTextBox.TabIndex = 2;
            // 
            // ShowPasswordCheckBox
            // 
            this.ShowPasswordCheckBox.AutoSize = true;
            this.ShowPasswordCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowPasswordCheckBox.Location = new System.Drawing.Point(103, 92);
            this.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox";
            this.ShowPasswordCheckBox.Size = new System.Drawing.Size(218, 23);
            this.ShowPasswordCheckBox.TabIndex = 5;
            this.ShowPasswordCheckBox.Text = "パスワードを表示する";
            this.ShowPasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.ExecuteButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(3, 383);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ButtonsPanel.Size = new System.Drawing.Size(358, 55);
            this.ButtonsPanel.TabIndex = 3;
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(250, 8);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 30);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecuteButton.Location = new System.Drawing.Point(134, 8);
            this.ExecuteButton.Margin = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(100, 30);
            this.ExecuteButton.TabIndex = 1;
            this.ExecuteButton.Text = "OK";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.ExecuteButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(364, 441);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "CubeICE 圧縮詳細設定";
            this.RootPanel.ResumeLayout(false);
            this.OutputPanel.ResumeLayout(false);
            this.OutputPanel.PerformLayout();
            this.GeneralGroupBox.ResumeLayout(false);
            this.GeneralPanel.ResumeLayout(false);
            this.GeneralPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadNumericUpDown)).EndInit();
            this.EncryptionGroupBox.ResumeLayout(false);
            this.EncryptionPanel.ResumeLayout(false);
            this.EncryptionPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.FlowLayoutPanel OutputPanel;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.Button OutputButton;
        private System.Windows.Forms.GroupBox GeneralGroupBox;
        private System.Windows.Forms.GroupBox EncryptionGroupBox;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.TableLayoutPanel GeneralPanel;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.ComboBox FormatComboBox;
        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.ComboBox LevelComboBox;
        private System.Windows.Forms.ComboBox MethodComboBox;
        private System.Windows.Forms.Label MethodLabel;
        private System.Windows.Forms.Label ThreadLabel;
        private System.Windows.Forms.NumericUpDown ThreadNumericUpDown;
        private System.Windows.Forms.TableLayoutPanel EncryptionPanel;
        private System.Windows.Forms.CheckBox EncryptionCheckBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label ConfirmLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox ConfirmTextBox;
        private System.Windows.Forms.CheckBox ShowPasswordCheckBox;
        private System.Windows.Forms.ComboBox EncryptionComboBox;
        private System.Windows.Forms.Label EncryptionLabel;
    }
}