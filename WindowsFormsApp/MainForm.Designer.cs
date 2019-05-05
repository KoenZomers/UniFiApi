namespace WindowsFormsApp
{
    partial class MainForm
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
            this.UniFiServerGroupBox = new System.Windows.Forms.GroupBox();
            this.UniFiServerAddressLabel = new System.Windows.Forms.Label();
            this.UniFiServerAddressTextBox = new System.Windows.Forms.TextBox();
            this.UniFiServerConnectButton = new System.Windows.Forms.Button();
            this.UniFiServerUsernameLabel = new System.Windows.Forms.Label();
            this.UniFiServerUsernameTextBox = new System.Windows.Forms.TextBox();
            this.UniFiServerPasswordLabel = new System.Windows.Forms.Label();
            this.UniFiServerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.ActionsGroupBox = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.BlockDevicesTabPage = new System.Windows.Forms.TabPage();
            this.BlockMacAddressButton = new System.Windows.Forms.Button();
            this.BlockMacAddressLabel = new System.Windows.Forms.Label();
            this.BlockMacAddressTextBox = new System.Windows.Forms.TextBox();
            this.UnblockMacAddressButton = new System.Windows.Forms.Button();
            this.UniFiServerDisconnectButton = new System.Windows.Forms.Button();
            this.AuthorizeGuestsTabPage = new System.Windows.Forms.TabPage();
            this.AuthorizeGuestButton = new System.Windows.Forms.Button();
            this.AuthorizeGuestMacAddressLabel = new System.Windows.Forms.Label();
            this.AuthorizeGuestMacAddressTextBox = new System.Windows.Forms.TextBox();
            this.UnauthorizeGuestButton = new System.Windows.Forms.Button();
            this.UniFiServerGroupBox.SuspendLayout();
            this.ActionsGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.BlockDevicesTabPage.SuspendLayout();
            this.AuthorizeGuestsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // UniFiServerGroupBox
            // 
            this.UniFiServerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerDisconnectButton);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerPasswordLabel);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerPasswordTextBox);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerUsernameLabel);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerUsernameTextBox);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerAddressLabel);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerAddressTextBox);
            this.UniFiServerGroupBox.Controls.Add(this.UniFiServerConnectButton);
            this.UniFiServerGroupBox.Location = new System.Drawing.Point(12, 12);
            this.UniFiServerGroupBox.Name = "UniFiServerGroupBox";
            this.UniFiServerGroupBox.Size = new System.Drawing.Size(582, 146);
            this.UniFiServerGroupBox.TabIndex = 3;
            this.UniFiServerGroupBox.TabStop = false;
            this.UniFiServerGroupBox.Text = "UniFi Server";
            // 
            // UniFiServerAddressLabel
            // 
            this.UniFiServerAddressLabel.AutoSize = true;
            this.UniFiServerAddressLabel.Location = new System.Drawing.Point(19, 28);
            this.UniFiServerAddressLabel.Name = "UniFiServerAddressLabel";
            this.UniFiServerAddressLabel.Size = new System.Drawing.Size(79, 13);
            this.UniFiServerAddressLabel.TabIndex = 5;
            this.UniFiServerAddressLabel.Text = "Server Address";
            // 
            // UniFiServerAddressTextBox
            // 
            this.UniFiServerAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerAddressTextBox.Location = new System.Drawing.Point(114, 25);
            this.UniFiServerAddressTextBox.Name = "UniFiServerAddressTextBox";
            this.UniFiServerAddressTextBox.Size = new System.Drawing.Size(451, 20);
            this.UniFiServerAddressTextBox.TabIndex = 4;
            this.UniFiServerAddressTextBox.Text = "https://192.168.1.1:8443";
            // 
            // UniFiServerConnectButton
            // 
            this.UniFiServerConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerConnectButton.Location = new System.Drawing.Point(371, 104);
            this.UniFiServerConnectButton.Name = "UniFiServerConnectButton";
            this.UniFiServerConnectButton.Size = new System.Drawing.Size(92, 29);
            this.UniFiServerConnectButton.TabIndex = 3;
            this.UniFiServerConnectButton.Text = "Connect";
            this.UniFiServerConnectButton.UseVisualStyleBackColor = true;
            this.UniFiServerConnectButton.Click += new System.EventHandler(this.UniFiServerConnectButton_Click);
            // 
            // UniFiServerUsernameLabel
            // 
            this.UniFiServerUsernameLabel.AutoSize = true;
            this.UniFiServerUsernameLabel.Location = new System.Drawing.Point(19, 55);
            this.UniFiServerUsernameLabel.Name = "UniFiServerUsernameLabel";
            this.UniFiServerUsernameLabel.Size = new System.Drawing.Size(55, 13);
            this.UniFiServerUsernameLabel.TabIndex = 7;
            this.UniFiServerUsernameLabel.Text = "Username";
            // 
            // UniFiServerUsernameTextBox
            // 
            this.UniFiServerUsernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerUsernameTextBox.Location = new System.Drawing.Point(114, 52);
            this.UniFiServerUsernameTextBox.Name = "UniFiServerUsernameTextBox";
            this.UniFiServerUsernameTextBox.Size = new System.Drawing.Size(451, 20);
            this.UniFiServerUsernameTextBox.TabIndex = 6;
            this.UniFiServerUsernameTextBox.Text = "admin";
            // 
            // UniFiServerPasswordLabel
            // 
            this.UniFiServerPasswordLabel.AutoSize = true;
            this.UniFiServerPasswordLabel.Location = new System.Drawing.Point(19, 81);
            this.UniFiServerPasswordLabel.Name = "UniFiServerPasswordLabel";
            this.UniFiServerPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.UniFiServerPasswordLabel.TabIndex = 9;
            this.UniFiServerPasswordLabel.Text = "Password";
            // 
            // UniFiServerPasswordTextBox
            // 
            this.UniFiServerPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerPasswordTextBox.Location = new System.Drawing.Point(114, 78);
            this.UniFiServerPasswordTextBox.Name = "UniFiServerPasswordTextBox";
            this.UniFiServerPasswordTextBox.Size = new System.Drawing.Size(451, 20);
            this.UniFiServerPasswordTextBox.TabIndex = 8;
            this.UniFiServerPasswordTextBox.Text = "mypassword";
            // 
            // ActionsGroupBox
            // 
            this.ActionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsGroupBox.Controls.Add(this.tabControl1);
            this.ActionsGroupBox.Enabled = false;
            this.ActionsGroupBox.Location = new System.Drawing.Point(12, 164);
            this.ActionsGroupBox.Name = "ActionsGroupBox";
            this.ActionsGroupBox.Size = new System.Drawing.Size(582, 263);
            this.ActionsGroupBox.TabIndex = 4;
            this.ActionsGroupBox.TabStop = false;
            this.ActionsGroupBox.Text = "Actions";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.BlockDevicesTabPage);
            this.tabControl1.Controls.Add(this.AuthorizeGuestsTabPage);
            this.tabControl1.Location = new System.Drawing.Point(14, 22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(551, 235);
            this.tabControl1.TabIndex = 6;
            // 
            // BlockDevicesTabPage
            // 
            this.BlockDevicesTabPage.Controls.Add(this.BlockMacAddressButton);
            this.BlockDevicesTabPage.Controls.Add(this.BlockMacAddressLabel);
            this.BlockDevicesTabPage.Controls.Add(this.BlockMacAddressTextBox);
            this.BlockDevicesTabPage.Controls.Add(this.UnblockMacAddressButton);
            this.BlockDevicesTabPage.Location = new System.Drawing.Point(4, 22);
            this.BlockDevicesTabPage.Name = "BlockDevicesTabPage";
            this.BlockDevicesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.BlockDevicesTabPage.Size = new System.Drawing.Size(543, 209);
            this.BlockDevicesTabPage.TabIndex = 0;
            this.BlockDevicesTabPage.Text = "Block Device";
            this.BlockDevicesTabPage.UseVisualStyleBackColor = true;
            // 
            // BlockMacAddressButton
            // 
            this.BlockMacAddressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockMacAddressButton.Location = new System.Drawing.Point(336, 54);
            this.BlockMacAddressButton.Name = "BlockMacAddressButton";
            this.BlockMacAddressButton.Size = new System.Drawing.Size(92, 29);
            this.BlockMacAddressButton.TabIndex = 14;
            this.BlockMacAddressButton.Text = "Block";
            this.BlockMacAddressButton.UseVisualStyleBackColor = true;
            this.BlockMacAddressButton.Click += new System.EventHandler(this.BlockMacAddressButton_Click);
            // 
            // BlockMacAddressLabel
            // 
            this.BlockMacAddressLabel.AutoSize = true;
            this.BlockMacAddressLabel.Location = new System.Drawing.Point(16, 12);
            this.BlockMacAddressLabel.Name = "BlockMacAddressLabel";
            this.BlockMacAddressLabel.Size = new System.Drawing.Size(69, 13);
            this.BlockMacAddressLabel.TabIndex = 13;
            this.BlockMacAddressLabel.Text = "Mac Address";
            // 
            // BlockMacAddressTextBox
            // 
            this.BlockMacAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockMacAddressTextBox.Location = new System.Drawing.Point(19, 28);
            this.BlockMacAddressTextBox.Name = "BlockMacAddressTextBox";
            this.BlockMacAddressTextBox.Size = new System.Drawing.Size(507, 20);
            this.BlockMacAddressTextBox.TabIndex = 12;
            // 
            // UnblockMacAddressButton
            // 
            this.UnblockMacAddressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnblockMacAddressButton.Location = new System.Drawing.Point(434, 54);
            this.UnblockMacAddressButton.Name = "UnblockMacAddressButton";
            this.UnblockMacAddressButton.Size = new System.Drawing.Size(92, 29);
            this.UnblockMacAddressButton.TabIndex = 11;
            this.UnblockMacAddressButton.Text = "Unblock";
            this.UnblockMacAddressButton.UseVisualStyleBackColor = true;
            this.UnblockMacAddressButton.Click += new System.EventHandler(this.UnblockMacAddressButton_Click);
            // 
            // UniFiServerDisconnectButton
            // 
            this.UniFiServerDisconnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UniFiServerDisconnectButton.Enabled = false;
            this.UniFiServerDisconnectButton.Location = new System.Drawing.Point(469, 104);
            this.UniFiServerDisconnectButton.Name = "UniFiServerDisconnectButton";
            this.UniFiServerDisconnectButton.Size = new System.Drawing.Size(92, 29);
            this.UniFiServerDisconnectButton.TabIndex = 10;
            this.UniFiServerDisconnectButton.Text = "Disconnect";
            this.UniFiServerDisconnectButton.UseVisualStyleBackColor = true;
            this.UniFiServerDisconnectButton.Click += new System.EventHandler(this.UniFiServerDisconnectButton_Click);
            // 
            // AuthorizeGuestsTabPage
            // 
            this.AuthorizeGuestsTabPage.Controls.Add(this.AuthorizeGuestButton);
            this.AuthorizeGuestsTabPage.Controls.Add(this.AuthorizeGuestMacAddressLabel);
            this.AuthorizeGuestsTabPage.Controls.Add(this.AuthorizeGuestMacAddressTextBox);
            this.AuthorizeGuestsTabPage.Controls.Add(this.UnauthorizeGuestButton);
            this.AuthorizeGuestsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AuthorizeGuestsTabPage.Name = "AuthorizeGuestsTabPage";
            this.AuthorizeGuestsTabPage.Size = new System.Drawing.Size(543, 209);
            this.AuthorizeGuestsTabPage.TabIndex = 1;
            this.AuthorizeGuestsTabPage.Text = "Authorize Guest";
            this.AuthorizeGuestsTabPage.UseVisualStyleBackColor = true;
            // 
            // AuthorizeGuestButton
            // 
            this.AuthorizeGuestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthorizeGuestButton.Location = new System.Drawing.Point(336, 54);
            this.AuthorizeGuestButton.Name = "AuthorizeGuestButton";
            this.AuthorizeGuestButton.Size = new System.Drawing.Size(92, 29);
            this.AuthorizeGuestButton.TabIndex = 18;
            this.AuthorizeGuestButton.Text = "Authorize";
            this.AuthorizeGuestButton.UseVisualStyleBackColor = true;
            this.AuthorizeGuestButton.Click += new System.EventHandler(this.AuthorizeGuestButton_Click);
            // 
            // AuthorizeGuestMacAddressLabel
            // 
            this.AuthorizeGuestMacAddressLabel.AutoSize = true;
            this.AuthorizeGuestMacAddressLabel.Location = new System.Drawing.Point(16, 12);
            this.AuthorizeGuestMacAddressLabel.Name = "AuthorizeGuestMacAddressLabel";
            this.AuthorizeGuestMacAddressLabel.Size = new System.Drawing.Size(69, 13);
            this.AuthorizeGuestMacAddressLabel.TabIndex = 17;
            this.AuthorizeGuestMacAddressLabel.Text = "Mac Address";
            // 
            // AuthorizeGuestMacAddressTextBox
            // 
            this.AuthorizeGuestMacAddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthorizeGuestMacAddressTextBox.Location = new System.Drawing.Point(19, 28);
            this.AuthorizeGuestMacAddressTextBox.Name = "AuthorizeGuestMacAddressTextBox";
            this.AuthorizeGuestMacAddressTextBox.Size = new System.Drawing.Size(507, 20);
            this.AuthorizeGuestMacAddressTextBox.TabIndex = 16;
            // 
            // UnauthorizeGuestButton
            // 
            this.UnauthorizeGuestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnauthorizeGuestButton.Location = new System.Drawing.Point(434, 54);
            this.UnauthorizeGuestButton.Name = "UnauthorizeGuestButton";
            this.UnauthorizeGuestButton.Size = new System.Drawing.Size(92, 29);
            this.UnauthorizeGuestButton.TabIndex = 15;
            this.UnauthorizeGuestButton.Text = "Unauthorize";
            this.UnauthorizeGuestButton.UseVisualStyleBackColor = true;
            this.UnauthorizeGuestButton.Click += new System.EventHandler(this.UnauthorizeGuestButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 439);
            this.Controls.Add(this.ActionsGroupBox);
            this.Controls.Add(this.UniFiServerGroupBox);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sample UniFi API Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.UniFiServerGroupBox.ResumeLayout(false);
            this.UniFiServerGroupBox.PerformLayout();
            this.ActionsGroupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.BlockDevicesTabPage.ResumeLayout(false);
            this.BlockDevicesTabPage.PerformLayout();
            this.AuthorizeGuestsTabPage.ResumeLayout(false);
            this.AuthorizeGuestsTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox UniFiServerGroupBox;
        private System.Windows.Forms.Label UniFiServerPasswordLabel;
        private System.Windows.Forms.TextBox UniFiServerPasswordTextBox;
        private System.Windows.Forms.Label UniFiServerUsernameLabel;
        private System.Windows.Forms.TextBox UniFiServerUsernameTextBox;
        private System.Windows.Forms.Label UniFiServerAddressLabel;
        private System.Windows.Forms.TextBox UniFiServerAddressTextBox;
        private System.Windows.Forms.Button UniFiServerConnectButton;
        private System.Windows.Forms.GroupBox ActionsGroupBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage BlockDevicesTabPage;
        private System.Windows.Forms.Button BlockMacAddressButton;
        private System.Windows.Forms.Label BlockMacAddressLabel;
        private System.Windows.Forms.TextBox BlockMacAddressTextBox;
        private System.Windows.Forms.Button UnblockMacAddressButton;
        private System.Windows.Forms.Button UniFiServerDisconnectButton;
        private System.Windows.Forms.TabPage AuthorizeGuestsTabPage;
        private System.Windows.Forms.Button AuthorizeGuestButton;
        private System.Windows.Forms.Label AuthorizeGuestMacAddressLabel;
        private System.Windows.Forms.TextBox AuthorizeGuestMacAddressTextBox;
        private System.Windows.Forms.Button UnauthorizeGuestButton;
    }
}

