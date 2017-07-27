namespace Cube.FileSystem.App.Ice.Settings
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.RootPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SettingsPanel = new Cube.Forms.SettingsControl();
            this.SettingsTabControl = new System.Windows.Forms.TabControl();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.GeneralPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AssociateGroupBox = new System.Windows.Forms.GroupBox();
            this.AssociatePanel = new System.Windows.Forms.TableLayoutPanel();
            this.AssociateMenuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AssociateButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AssociateClearButton = new System.Windows.Forms.Button();
            this.AssociateAllButton = new System.Windows.Forms.Button();
            this.ContextGroupBox = new System.Windows.Forms.GroupBox();
            this.ContextPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ContextButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ContextResetButton = new System.Windows.Forms.Button();
            this.ContextCustomizeButton = new System.Windows.Forms.Button();
            this.ContextMailPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ContextExtractPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ContextMailCheckBox = new System.Windows.Forms.CheckBox();
            this.ContextExtractCheckBox = new System.Windows.Forms.CheckBox();
            this.ContextArchiveCheckBox = new System.Windows.Forms.CheckBox();
            this.ContextArchivePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DesktopGroupBox = new System.Windows.Forms.GroupBox();
            this.DesktopPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DesktopArchiveCheckBox = new System.Windows.Forms.CheckBox();
            this.DesktopArchiveComboBox = new System.Windows.Forms.ComboBox();
            this.DesktopExtractCheckBox = new System.Windows.Forms.CheckBox();
            this.DesktopSettingsCheckBox = new System.Windows.Forms.CheckBox();
            this.ArchiveTabPage = new System.Windows.Forms.TabPage();
            this.ArchivePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ExtractTabPage = new System.Windows.Forms.TabPage();
            this.ExtractPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DetailsTabPage = new System.Windows.Forms.TabPage();
            this.DetailsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.VersionTabPage = new System.Windows.Forms.TabPage();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.RootPanel.SuspendLayout();
            this.SettingsPanel.SuspendLayout();
            this.SettingsTabControl.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.GeneralPanel.SuspendLayout();
            this.AssociateGroupBox.SuspendLayout();
            this.AssociatePanel.SuspendLayout();
            this.AssociateButtonsPanel.SuspendLayout();
            this.ContextGroupBox.SuspendLayout();
            this.ContextPanel.SuspendLayout();
            this.ContextButtonsPanel.SuspendLayout();
            this.DesktopGroupBox.SuspendLayout();
            this.DesktopPanel.SuspendLayout();
            this.ArchiveTabPage.SuspendLayout();
            this.ExtractTabPage.SuspendLayout();
            this.DetailsTabPage.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RootPanel
            // 
            this.RootPanel.ColumnCount = 1;
            this.RootPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.Controls.Add(this.SettingsPanel, 0, 0);
            this.RootPanel.Controls.Add(this.ButtonsPanel, 0, 1);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(0, 0);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.RowCount = 2;
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.RootPanel.Size = new System.Drawing.Size(534, 631);
            this.RootPanel.TabIndex = 0;
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Controls.Add(this.SettingsTabControl);
            this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsPanel.Location = new System.Drawing.Point(3, 3);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Padding = new System.Windows.Forms.Padding(8);
            this.SettingsPanel.Size = new System.Drawing.Size(528, 575);
            this.SettingsPanel.TabIndex = 0;
            // 
            // SettingsTabControl
            // 
            this.SettingsTabControl.Controls.Add(this.GeneralTabPage);
            this.SettingsTabControl.Controls.Add(this.ArchiveTabPage);
            this.SettingsTabControl.Controls.Add(this.ExtractTabPage);
            this.SettingsTabControl.Controls.Add(this.DetailsTabPage);
            this.SettingsTabControl.Controls.Add(this.VersionTabPage);
            this.SettingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTabControl.Location = new System.Drawing.Point(8, 8);
            this.SettingsTabControl.Name = "SettingsTabControl";
            this.SettingsTabControl.SelectedIndex = 0;
            this.SettingsTabControl.Size = new System.Drawing.Size(512, 559);
            this.SettingsTabControl.TabIndex = 0;
            // 
            // GeneralTabPage
            // 
            this.GeneralTabPage.Controls.Add(this.GeneralPanel);
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 24);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.GeneralTabPage.Size = new System.Drawing.Size(504, 531);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "一般";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // GeneralPanel
            // 
            this.GeneralPanel.AutoScroll = true;
            this.GeneralPanel.Controls.Add(this.AssociateGroupBox);
            this.GeneralPanel.Controls.Add(this.ContextGroupBox);
            this.GeneralPanel.Controls.Add(this.DesktopGroupBox);
            this.GeneralPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralPanel.Location = new System.Drawing.Point(12, 8);
            this.GeneralPanel.Name = "GeneralPanel";
            this.GeneralPanel.Size = new System.Drawing.Size(480, 515);
            this.GeneralPanel.TabIndex = 0;
            // 
            // AssociateGroupBox
            // 
            this.AssociateGroupBox.Controls.Add(this.AssociatePanel);
            this.AssociateGroupBox.Location = new System.Drawing.Point(3, 3);
            this.AssociateGroupBox.Name = "AssociateGroupBox";
            this.AssociateGroupBox.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.AssociateGroupBox.Size = new System.Drawing.Size(474, 180);
            this.AssociateGroupBox.TabIndex = 0;
            this.AssociateGroupBox.TabStop = false;
            this.AssociateGroupBox.Text = "ファイルの関連付け";
            // 
            // AssociatePanel
            // 
            this.AssociatePanel.ColumnCount = 1;
            this.AssociatePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AssociatePanel.Controls.Add(this.AssociateMenuPanel, 0, 0);
            this.AssociatePanel.Controls.Add(this.AssociateButtonsPanel, 0, 1);
            this.AssociatePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociatePanel.Location = new System.Drawing.Point(8, 19);
            this.AssociatePanel.Name = "AssociatePanel";
            this.AssociatePanel.RowCount = 2;
            this.AssociatePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AssociatePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.AssociatePanel.Size = new System.Drawing.Size(458, 158);
            this.AssociatePanel.TabIndex = 0;
            // 
            // AssociateMenuPanel
            // 
            this.AssociateMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociateMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.AssociateMenuPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AssociateMenuPanel.Name = "AssociateMenuPanel";
            this.AssociateMenuPanel.Size = new System.Drawing.Size(458, 127);
            this.AssociateMenuPanel.TabIndex = 0;
            // 
            // AssociateButtonsPanel
            // 
            this.AssociateButtonsPanel.Controls.Add(this.AssociateClearButton);
            this.AssociateButtonsPanel.Controls.Add(this.AssociateAllButton);
            this.AssociateButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociateButtonsPanel.Location = new System.Drawing.Point(0, 127);
            this.AssociateButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.AssociateButtonsPanel.Name = "AssociateButtonsPanel";
            this.AssociateButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AssociateButtonsPanel.Size = new System.Drawing.Size(458, 31);
            this.AssociateButtonsPanel.TabIndex = 1;
            // 
            // AssociateClearButton
            // 
            this.AssociateClearButton.Location = new System.Drawing.Point(355, 3);
            this.AssociateClearButton.Name = "AssociateClearButton";
            this.AssociateClearButton.Size = new System.Drawing.Size(100, 25);
            this.AssociateClearButton.TabIndex = 0;
            this.AssociateClearButton.Text = "すべて解除";
            this.AssociateClearButton.UseVisualStyleBackColor = true;
            // 
            // AssociateAllButton
            // 
            this.AssociateAllButton.Location = new System.Drawing.Point(249, 3);
            this.AssociateAllButton.Name = "AssociateAllButton";
            this.AssociateAllButton.Size = new System.Drawing.Size(100, 25);
            this.AssociateAllButton.TabIndex = 0;
            this.AssociateAllButton.Text = "すべて選択";
            this.AssociateAllButton.UseVisualStyleBackColor = true;
            // 
            // ContextGroupBox
            // 
            this.ContextGroupBox.Controls.Add(this.ContextPanel);
            this.ContextGroupBox.Location = new System.Drawing.Point(3, 189);
            this.ContextGroupBox.Name = "ContextGroupBox";
            this.ContextGroupBox.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.ContextGroupBox.Size = new System.Drawing.Size(474, 255);
            this.ContextGroupBox.TabIndex = 1;
            this.ContextGroupBox.TabStop = false;
            this.ContextGroupBox.Text = "コンテキストメニュー";
            // 
            // ContextPanel
            // 
            this.ContextPanel.ColumnCount = 3;
            this.ContextPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.ContextPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.ContextPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.ContextPanel.Controls.Add(this.ContextButtonsPanel, 0, 2);
            this.ContextPanel.Controls.Add(this.ContextMailPanel, 2, 1);
            this.ContextPanel.Controls.Add(this.ContextExtractPanel, 1, 1);
            this.ContextPanel.Controls.Add(this.ContextMailCheckBox, 2, 0);
            this.ContextPanel.Controls.Add(this.ContextExtractCheckBox, 1, 0);
            this.ContextPanel.Controls.Add(this.ContextArchiveCheckBox, 0, 0);
            this.ContextPanel.Controls.Add(this.ContextArchivePanel, 0, 1);
            this.ContextPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextPanel.Location = new System.Drawing.Point(8, 19);
            this.ContextPanel.Name = "ContextPanel";
            this.ContextPanel.RowCount = 3;
            this.ContextPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ContextPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContextPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.ContextPanel.Size = new System.Drawing.Size(458, 233);
            this.ContextPanel.TabIndex = 0;
            // 
            // ContextButtonsPanel
            // 
            this.ContextPanel.SetColumnSpan(this.ContextButtonsPanel, 3);
            this.ContextButtonsPanel.Controls.Add(this.ContextResetButton);
            this.ContextButtonsPanel.Controls.Add(this.ContextCustomizeButton);
            this.ContextButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextButtonsPanel.Location = new System.Drawing.Point(0, 202);
            this.ContextButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContextButtonsPanel.Name = "ContextButtonsPanel";
            this.ContextButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ContextButtonsPanel.Size = new System.Drawing.Size(458, 31);
            this.ContextButtonsPanel.TabIndex = 6;
            // 
            // ContextResetButton
            // 
            this.ContextResetButton.Location = new System.Drawing.Point(355, 3);
            this.ContextResetButton.Name = "ContextResetButton";
            this.ContextResetButton.Size = new System.Drawing.Size(100, 25);
            this.ContextResetButton.TabIndex = 1;
            this.ContextResetButton.Text = "リセット";
            this.ContextResetButton.UseVisualStyleBackColor = true;
            // 
            // ContextCustomizeButton
            // 
            this.ContextCustomizeButton.Enabled = false;
            this.ContextCustomizeButton.Location = new System.Drawing.Point(249, 3);
            this.ContextCustomizeButton.Name = "ContextCustomizeButton";
            this.ContextCustomizeButton.Size = new System.Drawing.Size(100, 25);
            this.ContextCustomizeButton.TabIndex = 0;
            this.ContextCustomizeButton.Text = "カスタマイズ";
            this.ContextCustomizeButton.UseVisualStyleBackColor = true;
            // 
            // ContextMailPanel
            // 
            this.ContextMailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextMailPanel.Enabled = false;
            this.ContextMailPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ContextMailPanel.Location = new System.Drawing.Point(314, 24);
            this.ContextMailPanel.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ContextMailPanel.Name = "ContextMailPanel";
            this.ContextMailPanel.Size = new System.Drawing.Size(144, 178);
            this.ContextMailPanel.TabIndex = 5;
            // 
            // ContextExtractPanel
            // 
            this.ContextExtractPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextExtractPanel.Enabled = false;
            this.ContextExtractPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ContextExtractPanel.Location = new System.Drawing.Point(163, 24);
            this.ContextExtractPanel.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ContextExtractPanel.Name = "ContextExtractPanel";
            this.ContextExtractPanel.Size = new System.Drawing.Size(139, 178);
            this.ContextExtractPanel.TabIndex = 4;
            // 
            // ContextMailCheckBox
            // 
            this.ContextMailCheckBox.AutoSize = true;
            this.ContextMailCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextMailCheckBox.Location = new System.Drawing.Point(305, 3);
            this.ContextMailCheckBox.Name = "ContextMailCheckBox";
            this.ContextMailCheckBox.Size = new System.Drawing.Size(150, 18);
            this.ContextMailCheckBox.TabIndex = 0;
            this.ContextMailCheckBox.Text = "圧縮してメール送信";
            this.ContextMailCheckBox.UseVisualStyleBackColor = true;
            // 
            // ContextExtractCheckBox
            // 
            this.ContextExtractCheckBox.AutoSize = true;
            this.ContextExtractCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextExtractCheckBox.Location = new System.Drawing.Point(154, 3);
            this.ContextExtractCheckBox.Name = "ContextExtractCheckBox";
            this.ContextExtractCheckBox.Size = new System.Drawing.Size(145, 18);
            this.ContextExtractCheckBox.TabIndex = 0;
            this.ContextExtractCheckBox.Text = "解凍";
            this.ContextExtractCheckBox.UseVisualStyleBackColor = true;
            // 
            // ContextArchiveCheckBox
            // 
            this.ContextArchiveCheckBox.AutoSize = true;
            this.ContextArchiveCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextArchiveCheckBox.Location = new System.Drawing.Point(3, 3);
            this.ContextArchiveCheckBox.Name = "ContextArchiveCheckBox";
            this.ContextArchiveCheckBox.Size = new System.Drawing.Size(145, 18);
            this.ContextArchiveCheckBox.TabIndex = 0;
            this.ContextArchiveCheckBox.Text = "圧縮";
            this.ContextArchiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // ContextArchivePanel
            // 
            this.ContextArchivePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContextArchivePanel.Enabled = false;
            this.ContextArchivePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ContextArchivePanel.Location = new System.Drawing.Point(12, 24);
            this.ContextArchivePanel.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ContextArchivePanel.Name = "ContextArchivePanel";
            this.ContextArchivePanel.Size = new System.Drawing.Size(139, 178);
            this.ContextArchivePanel.TabIndex = 3;
            // 
            // DesktopGroupBox
            // 
            this.DesktopGroupBox.Controls.Add(this.DesktopPanel);
            this.DesktopGroupBox.Location = new System.Drawing.Point(3, 450);
            this.DesktopGroupBox.Name = "DesktopGroupBox";
            this.DesktopGroupBox.Padding = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.DesktopGroupBox.Size = new System.Drawing.Size(474, 54);
            this.DesktopGroupBox.TabIndex = 2;
            this.DesktopGroupBox.TabStop = false;
            this.DesktopGroupBox.Text = "デスクトップに作成するショートカット";
            // 
            // DesktopPanel
            // 
            this.DesktopPanel.Controls.Add(this.DesktopArchiveCheckBox);
            this.DesktopPanel.Controls.Add(this.DesktopArchiveComboBox);
            this.DesktopPanel.Controls.Add(this.DesktopExtractCheckBox);
            this.DesktopPanel.Controls.Add(this.DesktopSettingsCheckBox);
            this.DesktopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DesktopPanel.Location = new System.Drawing.Point(12, 19);
            this.DesktopPanel.Name = "DesktopPanel";
            this.DesktopPanel.Size = new System.Drawing.Size(450, 29);
            this.DesktopPanel.TabIndex = 0;
            // 
            // DesktopArchiveCheckBox
            // 
            this.DesktopArchiveCheckBox.Location = new System.Drawing.Point(3, 3);
            this.DesktopArchiveCheckBox.Name = "DesktopArchiveCheckBox";
            this.DesktopArchiveCheckBox.Size = new System.Drawing.Size(55, 23);
            this.DesktopArchiveCheckBox.TabIndex = 0;
            this.DesktopArchiveCheckBox.Text = "圧縮";
            this.DesktopArchiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // DesktopArchiveComboBox
            // 
            this.DesktopArchiveComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DesktopArchiveComboBox.Enabled = false;
            this.DesktopArchiveComboBox.FormattingEnabled = true;
            this.DesktopArchiveComboBox.Location = new System.Drawing.Point(64, 3);
            this.DesktopArchiveComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.DesktopArchiveComboBox.Name = "DesktopArchiveComboBox";
            this.DesktopArchiveComboBox.Size = new System.Drawing.Size(120, 23);
            this.DesktopArchiveComboBox.TabIndex = 1;
            // 
            // DesktopExtractCheckBox
            // 
            this.DesktopExtractCheckBox.Location = new System.Drawing.Point(227, 3);
            this.DesktopExtractCheckBox.Name = "DesktopExtractCheckBox";
            this.DesktopExtractCheckBox.Size = new System.Drawing.Size(80, 23);
            this.DesktopExtractCheckBox.TabIndex = 2;
            this.DesktopExtractCheckBox.Text = "解凍";
            this.DesktopExtractCheckBox.UseVisualStyleBackColor = true;
            // 
            // DesktopSettingsCheckBox
            // 
            this.DesktopSettingsCheckBox.Location = new System.Drawing.Point(313, 3);
            this.DesktopSettingsCheckBox.Name = "DesktopSettingsCheckBox";
            this.DesktopSettingsCheckBox.Size = new System.Drawing.Size(80, 23);
            this.DesktopSettingsCheckBox.TabIndex = 3;
            this.DesktopSettingsCheckBox.Text = "設定";
            this.DesktopSettingsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ArchiveTabPage
            // 
            this.ArchiveTabPage.Controls.Add(this.ArchivePanel);
            this.ArchiveTabPage.Location = new System.Drawing.Point(4, 24);
            this.ArchiveTabPage.Name = "ArchiveTabPage";
            this.ArchiveTabPage.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.ArchiveTabPage.Size = new System.Drawing.Size(504, 531);
            this.ArchiveTabPage.TabIndex = 1;
            this.ArchiveTabPage.Text = "圧縮";
            this.ArchiveTabPage.UseVisualStyleBackColor = true;
            // 
            // ArchivePanel
            // 
            this.ArchivePanel.AutoScroll = true;
            this.ArchivePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArchivePanel.Location = new System.Drawing.Point(12, 8);
            this.ArchivePanel.Name = "ArchivePanel";
            this.ArchivePanel.Size = new System.Drawing.Size(480, 515);
            this.ArchivePanel.TabIndex = 0;
            // 
            // ExtractTabPage
            // 
            this.ExtractTabPage.Controls.Add(this.ExtractPanel);
            this.ExtractTabPage.Location = new System.Drawing.Point(4, 24);
            this.ExtractTabPage.Name = "ExtractTabPage";
            this.ExtractTabPage.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.ExtractTabPage.Size = new System.Drawing.Size(504, 531);
            this.ExtractTabPage.TabIndex = 2;
            this.ExtractTabPage.Text = "解凍";
            this.ExtractTabPage.UseVisualStyleBackColor = true;
            // 
            // ExtractPanel
            // 
            this.ExtractPanel.AutoScroll = true;
            this.ExtractPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExtractPanel.Location = new System.Drawing.Point(12, 8);
            this.ExtractPanel.Name = "ExtractPanel";
            this.ExtractPanel.Size = new System.Drawing.Size(480, 515);
            this.ExtractPanel.TabIndex = 1;
            // 
            // DetailsTabPage
            // 
            this.DetailsTabPage.Controls.Add(this.DetailsPanel);
            this.DetailsTabPage.Location = new System.Drawing.Point(4, 24);
            this.DetailsTabPage.Name = "DetailsTabPage";
            this.DetailsTabPage.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.DetailsTabPage.Size = new System.Drawing.Size(504, 531);
            this.DetailsTabPage.TabIndex = 3;
            this.DetailsTabPage.Text = "詳細";
            this.DetailsTabPage.UseVisualStyleBackColor = true;
            // 
            // DetailsPanel
            // 
            this.DetailsPanel.AutoScroll = true;
            this.DetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailsPanel.Location = new System.Drawing.Point(12, 8);
            this.DetailsPanel.Name = "DetailsPanel";
            this.DetailsPanel.Size = new System.Drawing.Size(480, 515);
            this.DetailsPanel.TabIndex = 2;
            // 
            // VersionTabPage
            // 
            this.VersionTabPage.Location = new System.Drawing.Point(4, 24);
            this.VersionTabPage.Name = "VersionTabPage";
            this.VersionTabPage.Padding = new System.Windows.Forms.Padding(12);
            this.VersionTabPage.Size = new System.Drawing.Size(504, 531);
            this.VersionTabPage.TabIndex = 4;
            this.VersionTabPage.Text = "バージョン情報";
            this.VersionTabPage.UseVisualStyleBackColor = true;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.ApplyButton);
            this.ButtonsPanel.Controls.Add(this.ExitButton);
            this.ButtonsPanel.Controls.Add(this.ExecuteButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(3, 584);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ButtonsPanel.Size = new System.Drawing.Size(528, 44);
            this.ButtonsPanel.TabIndex = 1;
            // 
            // ApplyButton
            // 
            this.ApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ApplyButton.Enabled = false;
            this.ApplyButton.Location = new System.Drawing.Point(416, 3);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.TabIndex = 2;
            this.ApplyButton.Text = "適用 (&A)";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(310, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 30);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "キャンセル";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ExecuteButton.Location = new System.Drawing.Point(204, 3);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(100, 30);
            this.ExecuteButton.TabIndex = 0;
            this.ExecuteButton.Text = "OK";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.ExecuteButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(534, 631);
            this.Controls.Add(this.RootPanel);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(570, 700);
            this.MinimumSize = new System.Drawing.Size(550, 250);
            this.Name = "SettingsForm";
            this.Text = "CubeICE 設定";
            this.RootPanel.ResumeLayout(false);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsTabControl.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralPanel.ResumeLayout(false);
            this.AssociateGroupBox.ResumeLayout(false);
            this.AssociatePanel.ResumeLayout(false);
            this.AssociateButtonsPanel.ResumeLayout(false);
            this.ContextGroupBox.ResumeLayout(false);
            this.ContextPanel.ResumeLayout(false);
            this.ContextPanel.PerformLayout();
            this.ContextButtonsPanel.ResumeLayout(false);
            this.DesktopGroupBox.ResumeLayout(false);
            this.DesktopPanel.ResumeLayout(false);
            this.ArchiveTabPage.ResumeLayout(false);
            this.ExtractTabPage.ResumeLayout(false);
            this.DetailsTabPage.ResumeLayout(false);
            this.ButtonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel RootPanel;
        private Forms.SettingsControl SettingsPanel;
        private System.Windows.Forms.TabControl SettingsTabControl;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage ArchiveTabPage;
        private System.Windows.Forms.TabPage ExtractTabPage;
        private System.Windows.Forms.TabPage DetailsTabPage;
        private System.Windows.Forms.TabPage VersionTabPage;
        private System.Windows.Forms.FlowLayoutPanel GeneralPanel;
        private System.Windows.Forms.GroupBox AssociateGroupBox;
        private System.Windows.Forms.TableLayoutPanel AssociatePanel;
        private System.Windows.Forms.FlowLayoutPanel AssociateMenuPanel;
        private System.Windows.Forms.FlowLayoutPanel AssociateButtonsPanel;
        private System.Windows.Forms.Button AssociateClearButton;
        private System.Windows.Forms.Button AssociateAllButton;
        private System.Windows.Forms.GroupBox ContextGroupBox;
        private System.Windows.Forms.TableLayoutPanel ContextPanel;
        private System.Windows.Forms.FlowLayoutPanel ContextMailPanel;
        private System.Windows.Forms.FlowLayoutPanel ContextExtractPanel;
        private System.Windows.Forms.CheckBox ContextMailCheckBox;
        private System.Windows.Forms.CheckBox ContextExtractCheckBox;
        private System.Windows.Forms.CheckBox ContextArchiveCheckBox;
        private System.Windows.Forms.FlowLayoutPanel ContextArchivePanel;
        private System.Windows.Forms.FlowLayoutPanel ContextButtonsPanel;
        private System.Windows.Forms.Button ContextResetButton;
        private System.Windows.Forms.Button ContextCustomizeButton;
        private System.Windows.Forms.GroupBox DesktopGroupBox;
        private System.Windows.Forms.FlowLayoutPanel DesktopPanel;
        private System.Windows.Forms.CheckBox DesktopArchiveCheckBox;
        private System.Windows.Forms.ComboBox DesktopArchiveComboBox;
        private System.Windows.Forms.CheckBox DesktopExtractCheckBox;
        private System.Windows.Forms.CheckBox DesktopSettingsCheckBox;
        private System.Windows.Forms.FlowLayoutPanel ButtonsPanel;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.FlowLayoutPanel ArchivePanel;
        private System.Windows.Forms.FlowLayoutPanel ExtractPanel;
        private System.Windows.Forms.FlowLayoutPanel DetailsPanel;
    }
}

