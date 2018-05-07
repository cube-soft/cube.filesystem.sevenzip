namespace Cube.FileSystem.SevenZip.App.Ice.Settings
{
    partial class ContextSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextSettingsForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SubCommandPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ResetButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.NewFolderButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.DestinationPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DestinationTreeView = new System.Windows.Forms.TreeView();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.SourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.SourceTreeView = new System.Windows.Forms.TreeView();
            this.MainCommandPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.SubCommandPanel.SuspendLayout();
            this.DestinationPanel.SuspendLayout();
            this.SourcePanel.SuspendLayout();
            this.MainCommandPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 4;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.RootPanel.Controls.Add(this.SubCommandPanel, 3, 0);
            this.RootPanel.Controls.Add(this.DestinationPanel, 2, 0);
            this.RootPanel.Controls.Add(this.SourcePanel, 0, 0);
            this.RootPanel.Controls.Add(this.MainCommandPanel, 1, 0);
            this.RootPanel.Controls.Add(this.FooterPanel, 0, 1);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 2;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.RootPanel.Size = new System.Drawing.Size(634, 281);
            this.RootPanel.TabIndex = 0;
            //
            // SubCommandPanel
            //
            this.SubCommandPanel.ColumnCount = 1;
            this.SubCommandPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubCommandPanel.Controls.Add(this.ResetButton, 0, 5);
            this.SubCommandPanel.Controls.Add(this.RenameButton, 0, 3);
            this.SubCommandPanel.Controls.Add(this.NewFolderButton, 0, 2);
            this.SubCommandPanel.Controls.Add(this.DownButton, 0, 1);
            this.SubCommandPanel.Controls.Add(this.UpButton, 0, 0);
            this.SubCommandPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubCommandPanel.Location = new System.Drawing.Point(519, 9);
            this.SubCommandPanel.Margin = new System.Windows.Forms.Padding(3, 9, 9, 3);
            this.SubCommandPanel.Name = "SubCommandPanel";
            this.SubCommandPanel.RowCount = 6;
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.Size = new System.Drawing.Size(106, 209);
            this.SubCommandPanel.TabIndex = 3;
            //
            // ResetButton
            //
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetButton.Location = new System.Drawing.Point(3, 176);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(100, 30);
            this.ResetButton.TabIndex = 5;
            this.ResetButton.Text = "リセット";
            this.ResetButton.UseVisualStyleBackColor = true;
            //
            // RenameButton
            //
            this.RenameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameButton.Location = new System.Drawing.Point(3, 111);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(100, 30);
            this.RenameButton.TabIndex = 4;
            this.RenameButton.Text = "名前の変更";
            this.RenameButton.UseVisualStyleBackColor = true;
            //
            // NewFolderButton
            //
            this.NewFolderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewFolderButton.Location = new System.Drawing.Point(3, 75);
            this.NewFolderButton.Name = "NewFolderButton";
            this.NewFolderButton.Size = new System.Drawing.Size(100, 30);
            this.NewFolderButton.TabIndex = 3;
            this.NewFolderButton.Text = "新しいフォルダ";
            this.NewFolderButton.UseVisualStyleBackColor = true;
            //
            // DownButton
            //
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownButton.Location = new System.Drawing.Point(3, 39);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(100, 30);
            this.DownButton.TabIndex = 2;
            this.DownButton.Text = "↓下へ";
            this.DownButton.UseVisualStyleBackColor = true;
            //
            // UpButton
            //
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpButton.Location = new System.Drawing.Point(3, 3);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(100, 30);
            this.UpButton.TabIndex = 1;
            this.UpButton.Text = "↑ 上へ";
            this.UpButton.UseVisualStyleBackColor = true;
            //
            // DestinationPanel
            //
            this.DestinationPanel.ColumnCount = 1;
            this.DestinationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DestinationPanel.Controls.Add(this.DestinationTreeView, 0, 1);
            this.DestinationPanel.Controls.Add(this.DestinationLabel, 0, 0);
            this.DestinationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationPanel.Location = new System.Drawing.Point(317, 12);
            this.DestinationPanel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.DestinationPanel.Name = "DestinationPanel";
            this.DestinationPanel.RowCount = 2;
            this.DestinationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.DestinationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DestinationPanel.Size = new System.Drawing.Size(196, 206);
            this.DestinationPanel.TabIndex = 2;
            //
            // DestinationTreeView
            //
            this.DestinationTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationTreeView.Location = new System.Drawing.Point(3, 27);
            this.DestinationTreeView.Name = "DestinationTreeView";
            this.DestinationTreeView.Size = new System.Drawing.Size(190, 176);
            this.DestinationTreeView.TabIndex = 2;
            //
            // DestinationLabel
            //
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationLabel.Location = new System.Drawing.Point(3, 3);
            this.DestinationLabel.Margin = new System.Windows.Forms.Padding(3);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new System.Drawing.Size(190, 18);
            this.DestinationLabel.TabIndex = 1;
            this.DestinationLabel.Text = "現在のコンテキストメニュー";
            this.DestinationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SourcePanel
            //
            this.SourcePanel.ColumnCount = 1;
            this.SourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.Controls.Add(this.SourceLabel, 0, 0);
            this.SourcePanel.Controls.Add(this.SourceTreeView, 0, 1);
            this.SourcePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourcePanel.Location = new System.Drawing.Point(12, 12);
            this.SourcePanel.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.SourcePanel.Name = "SourcePanel";
            this.SourcePanel.RowCount = 2;
            this.SourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.SourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SourcePanel.Size = new System.Drawing.Size(187, 206);
            this.SourcePanel.TabIndex = 0;
            //
            // SourceLabel
            //
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceLabel.Location = new System.Drawing.Point(3, 3);
            this.SourceLabel.Margin = new System.Windows.Forms.Padding(3);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(181, 18);
            this.SourceLabel.TabIndex = 0;
            this.SourceLabel.Text = "追加可能なコンテキストメニュー";
            this.SourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SourceTreeView
            //
            this.SourceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceTreeView.Location = new System.Drawing.Point(3, 27);
            this.SourceTreeView.Name = "SourceTreeView";
            this.SourceTreeView.Size = new System.Drawing.Size(181, 176);
            this.SourceTreeView.TabIndex = 1;
            //
            // MainCommandPanel
            //
            this.MainCommandPanel.ColumnCount = 1;
            this.MainCommandPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainCommandPanel.Controls.Add(this.AddButton, 0, 1);
            this.MainCommandPanel.Controls.Add(this.RemoveButton, 0, 2);
            this.MainCommandPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainCommandPanel.Location = new System.Drawing.Point(205, 3);
            this.MainCommandPanel.Name = "MainCommandPanel";
            this.MainCommandPanel.RowCount = 4;
            this.MainCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.MainCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.MainCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainCommandPanel.Size = new System.Drawing.Size(106, 215);
            this.MainCommandPanel.TabIndex = 1;
            //
            // AddButton
            //
            this.AddButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddButton.Location = new System.Drawing.Point(3, 74);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(100, 30);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "追加 →";
            this.AddButton.UseVisualStyleBackColor = true;
            //
            // RemoveButton
            //
            this.RemoveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveButton.Location = new System.Drawing.Point(3, 110);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(100, 30);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "← 削除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            this.FooterPanel.ColumnCount = 3;
            this.RootPanel.SetColumnSpan(this.FooterPanel, 4);
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.FooterPanel.Controls.Add(this.ExitButton, 2, 0);
            this.FooterPanel.Controls.Add(this.ApplyButton, 1, 0);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.Location = new System.Drawing.Point(9, 233);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.RowCount = 1;
            this.FooterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.Size = new System.Drawing.Size(616, 36);
            this.FooterPanel.TabIndex = 4;
            //
            // ExitButton
            //
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitButton.Location = new System.Drawing.Point(513, 3);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 30);
            this.ExitButton.TabIndex = 7;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // ApplyButton
            //
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplyButton.Location = new System.Drawing.Point(401, 3);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "OK";
            this.ApplyButton.UseVisualStyleBackColor = true;
            //
            // ContextSettingsForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(634, 281);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContextSettingsForm";
            this.Text = "コンテキストメニューのカスタマイズ";
            this.RootPanel.ResumeLayout(false);
            this.SubCommandPanel.ResumeLayout(false);
            this.DestinationPanel.ResumeLayout(false);
            this.DestinationPanel.PerformLayout();
            this.SourcePanel.ResumeLayout(false);
            this.SourcePanel.PerformLayout();
            this.MainCommandPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TableLayoutPanel SubCommandPanel;
        private System.Windows.Forms.TableLayoutPanel DestinationPanel;
        private System.Windows.Forms.TableLayoutPanel SourcePanel;
        private System.Windows.Forms.TableLayoutPanel MainCommandPanel;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.TreeView SourceTreeView;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.TreeView DestinationTreeView;
        private System.Windows.Forms.Label DestinationLabel;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.Button NewFolderButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ApplyButton;
    }
}