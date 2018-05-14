namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    partial class ContextCustomizationFrom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextCustomizationFrom));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddButton = new System.Windows.Forms.Button();
            this.DestinationTreeView = new System.Windows.Forms.TreeView();
            this.SourceTreeView = new System.Windows.Forms.TreeView();
            this.DestinationLabel = new System.Windows.Forms.Label();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.SubCommandPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.NewCategoryButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.FooterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.SubCommandPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 6;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.Controls.Add(this.AddButton, 2, 2);
            this.RootPanel.Controls.Add(this.DestinationTreeView, 3, 2);
            this.RootPanel.Controls.Add(this.SourceTreeView, 1, 2);
            this.RootPanel.Controls.Add(this.DestinationLabel, 3, 1);
            this.RootPanel.Controls.Add(this.SourceLabel, 1, 1);
            this.RootPanel.Controls.Add(this.SubCommandPanel, 4, 2);
            this.RootPanel.Controls.Add(this.FooterPanel, 1, 3);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 5;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.Size = new System.Drawing.Size(634, 311);
            this.RootPanel.TabIndex = 0;
            //
            // AddButton
            //
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.Location = new System.Drawing.Point(239, 123);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(50, 40);
            this.AddButton.TabIndex = 9;
            this.AddButton.Text = "→";
            this.AddButton.UseVisualStyleBackColor = true;
            //
            // DestinationTreeView
            //
            this.DestinationTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationTreeView.Location = new System.Drawing.Point(295, 36);
            this.DestinationTreeView.Name = "DestinationTreeView";
            this.DestinationTreeView.Size = new System.Drawing.Size(221, 215);
            this.DestinationTreeView.TabIndex = 8;
            //
            // SourceTreeView
            //
            this.SourceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceTreeView.Location = new System.Drawing.Point(12, 36);
            this.SourceTreeView.Name = "SourceTreeView";
            this.SourceTreeView.Size = new System.Drawing.Size(221, 215);
            this.SourceTreeView.TabIndex = 7;
            //
            // DestinationLabel
            //
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationLabel.Location = new System.Drawing.Point(295, 12);
            this.DestinationLabel.Margin = new System.Windows.Forms.Padding(3);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new System.Drawing.Size(221, 18);
            this.DestinationLabel.TabIndex = 6;
            this.DestinationLabel.Text = "現在のコンテキストメニュー";
            this.DestinationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SourceLabel
            //
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceLabel.Location = new System.Drawing.Point(12, 12);
            this.SourceLabel.Margin = new System.Windows.Forms.Padding(3);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new System.Drawing.Size(221, 18);
            this.SourceLabel.TabIndex = 5;
            this.SourceLabel.Text = "追加可能なコンテキストメニュー";
            this.SourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SubCommandPanel
            //
            this.SubCommandPanel.ColumnCount = 1;
            this.SubCommandPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubCommandPanel.Controls.Add(this.RemoveButton, 0, 3);
            this.SubCommandPanel.Controls.Add(this.RenameButton, 0, 4);
            this.SubCommandPanel.Controls.Add(this.NewCategoryButton, 0, 2);
            this.SubCommandPanel.Controls.Add(this.DownButton, 0, 1);
            this.SubCommandPanel.Controls.Add(this.UpButton, 0, 0);
            this.SubCommandPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubCommandPanel.Location = new System.Drawing.Point(519, 33);
            this.SubCommandPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SubCommandPanel.Name = "SubCommandPanel";
            this.SubCommandPanel.RowCount = 6;
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.SubCommandPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SubCommandPanel.Size = new System.Drawing.Size(106, 221);
            this.SubCommandPanel.TabIndex = 3;
            //
            // RemoveButton
            //
            this.RemoveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveButton.Location = new System.Drawing.Point(3, 111);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(100, 30);
            this.RemoveButton.TabIndex = 5;
            this.RemoveButton.Text = "削除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            //
            // RenameButton
            //
            this.RenameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameButton.Location = new System.Drawing.Point(3, 147);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(100, 30);
            this.RenameButton.TabIndex = 4;
            this.RenameButton.Text = "名前の変更";
            this.RenameButton.UseVisualStyleBackColor = true;
            //
            // NewCategoryButton
            //
            this.NewCategoryButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewCategoryButton.Location = new System.Drawing.Point(3, 75);
            this.NewCategoryButton.Name = "NewCategoryButton";
            this.NewCategoryButton.Size = new System.Drawing.Size(100, 30);
            this.NewCategoryButton.TabIndex = 3;
            this.NewCategoryButton.Text = "新しいカテゴリー";
            this.NewCategoryButton.UseVisualStyleBackColor = true;
            //
            // DownButton
            //
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownButton.Location = new System.Drawing.Point(3, 39);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(100, 30);
            this.DownButton.TabIndex = 2;
            this.DownButton.Text = "下へ";
            this.DownButton.UseVisualStyleBackColor = true;
            //
            // UpButton
            //
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpButton.Location = new System.Drawing.Point(3, 3);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(100, 30);
            this.UpButton.TabIndex = 1;
            this.UpButton.Text = "上へ";
            this.UpButton.UseVisualStyleBackColor = true;
            //
            // FooterPanel
            //
            this.FooterPanel.ColumnCount = 3;
            this.RootPanel.SetColumnSpan(this.FooterPanel, 4);
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.FooterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.FooterPanel.Controls.Add(this.ExitButton, 2, 0);
            this.FooterPanel.Controls.Add(this.ApplyButton, 1, 0);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.Location = new System.Drawing.Point(9, 266);
            this.FooterPanel.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
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
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 30);
            this.ExitButton.TabIndex = 7;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // ApplyButton
            //
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ApplyButton.Location = new System.Drawing.Point(407, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "OK";
            this.ApplyButton.UseVisualStyleBackColor = true;
            //
            // ContextSettingsForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(634, 311);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(540, 320);
            this.Name = "ContextSettingsForm";
            this.Text = "コンテキストメニューのカスタマイズ";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.SubCommandPanel.ResumeLayout(false);
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.TableLayoutPanel SubCommandPanel;
        private System.Windows.Forms.TableLayoutPanel FooterPanel;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.Button NewCategoryButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.TreeView DestinationTreeView;
        private System.Windows.Forms.TreeView SourceTreeView;
        private System.Windows.Forms.Label DestinationLabel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Button AddButton;
    }
}