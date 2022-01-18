namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    partial class AssociateIconWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssociateIconWindow));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.IconListView = new System.Windows.Forms.ListView();
            this.ButtonsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ExecButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootPanel
            //
            this.RootPanel.ColumnCount = 3;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.Controls.Add(this.HeaderLabel, 1, 1);
            this.RootPanel.Controls.Add(this.IconListView, 1, 2);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 4);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 5;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.RootPanel.Size = new System.Drawing.Size(384, 191);
            this.RootPanel.TabIndex = 0;
            //
            // HeaderLabel
            //
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderLabel.Location = new System.Drawing.Point(12, 12);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(3);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(360, 14);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "下の一覧からアイコンを選択";
            this.HeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // IconListView
            //
            this.IconListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IconListView.HideSelection = false;
            this.IconListView.Location = new System.Drawing.Point(12, 32);
            this.IconListView.MultiSelect = false;
            this.IconListView.Name = "IconListView";
            this.IconListView.Size = new System.Drawing.Size(360, 82);
            this.IconListView.TabIndex = 2;
            this.IconListView.UseCompatibleStateImageBehavior = false;
            //
            // ButtonsPanel
            //
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonsPanel.ColumnCount = 4;
            this.RootPanel.SetColumnSpan(this.ButtonsPanel, 3);
            this.ButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.ButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.ButtonsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.ButtonsPanel.Controls.Add(this.ExecButton, 1, 0);
            this.ButtonsPanel.Controls.Add(this.ExitButton, 2, 0);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 137);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.RowCount = 1;
            this.ButtonsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ButtonsPanel.Size = new System.Drawing.Size(384, 54);
            this.ButtonsPanel.TabIndex = 0;
            //
            // ExecButton
            //
            this.ExecButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecButton.Location = new System.Drawing.Point(141, 12);
            this.ExecButton.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.ExecButton.Name = "ExecButton";
            this.ExecButton.Size = new System.Drawing.Size(125, 30);
            this.ExecButton.TabIndex = 1;
            this.ExecButton.Text = "OK";
            this.ExecButton.UseVisualStyleBackColor = true;
            //
            // ExitButton
            //
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitButton.Location = new System.Drawing.Point(272, 12);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 30);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            //
            // AssociateIconWindow
            //
            this.AcceptButton = this.ExitButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(384, 191);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 230);
            this.Name = "AssociateIconWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ファイルの関連付け用アイコンの変更";
            this.RootPanel.ResumeLayout(false);
            this.RootPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.ListView IconListView;
        private System.Windows.Forms.TableLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExecButton;
    }
}