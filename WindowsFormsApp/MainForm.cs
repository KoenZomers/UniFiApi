using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.UniFi.Api;

namespace WindowsFormsApp
{
    public partial class MainForm : Form
    {
        private Api _uniFiApi;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void UniFiServerConnectButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(UniFiServerAddressTextBox.Text) || string.IsNullOrWhiteSpace(UniFiServerUsernameTextBox.Text) || string.IsNullOrWhiteSpace(UniFiServerPasswordTextBox.Text))
            {
                MessageBox.Show(this, "Please fill out the UniFi server details first", "Unable to connect to UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            _uniFiApi = new Api(new Uri(UniFiServerAddressTextBox.Text));

            // Disable SSL validation as UniFi uses a self signed certificate
            _uniFiApi.DisableSslValidation();

            // Authenticate to UniFi
            var connectionResult = await _uniFiApi.Authenticate(UniFiServerUsernameTextBox.Text, UniFiServerPasswordTextBox.Text);

            if(connectionResult)
            {
                MessageBox.Show(this, "Connection to UniFi server successful", "Connected to UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Connection to UniFi server failed", "Unable to connect to UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            ActionsGroupBox.Enabled = connectionResult;
            UniFiServerDisconnectButton.Enabled = connectionResult;
            UniFiServerConnectButton.Enabled = !connectionResult;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {            
            // Clean up resources used by the UniFi API
            _uniFiApi?.Dispose();
        }

        private async void BlockMacAddressButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(BlockMacAddressTextBox.Text))
            {
                MessageBox.Show(this, "Please fill out the mac address to block first", "Unable to block mac address", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            var result = await _uniFiApi.BlockClient(BlockMacAddressTextBox.Text);

            MessageBox.Show(this, $"Client block: {result.meta.ResultCode}", "Block mac address", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void UnblockMacAddressButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BlockMacAddressTextBox.Text))
            {
                MessageBox.Show(this, "Please fill out the mac address to unblock first", "Unable to unblock mac address", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            var result = await _uniFiApi.UnblockClient(BlockMacAddressTextBox.Text);

            MessageBox.Show(this, $"Client unblock: {result.meta.ResultCode}", "Unblock mac address", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void UniFiServerDisconnectButton_Click(object sender, EventArgs e)
        {
            var disconnectResult = await _uniFiApi.Logout();
            _uniFiApi?.Dispose();
            _uniFiApi = null;

            if (disconnectResult)
            {
                MessageBox.Show(this, "Successfully disconnected from UniFi server", "Disconnected from UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Failed to log out from UniFi server", "Unable to disconnect from UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            ActionsGroupBox.Enabled = !disconnectResult;
            UniFiServerDisconnectButton.Enabled = !disconnectResult;
            UniFiServerConnectButton.Enabled = disconnectResult;
        }

        private async void AuthorizeGuestButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AuthorizeGuestMacAddressTextBox.Text))
            {
                MessageBox.Show(this, "Please fill out the mac address to authorize first", "Unable to authorize mac address", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            var result = await _uniFiApi.AuthorizeGuest(AuthorizeGuestMacAddressTextBox.Text);

            MessageBox.Show(this, $"Client authorization: {result.meta.ResultCode}", "Authorize mac address", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void UnauthorizeGuestButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AuthorizeGuestMacAddressTextBox.Text))
            {
                MessageBox.Show(this, "Please fill out the mac address to unauthorize first", "Unable to unauthorize mac address", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            var result = await _uniFiApi.UnauthorizeGuest(AuthorizeGuestMacAddressTextBox.Text);

            MessageBox.Show(this, $"Client unauthorize: {result.meta.ResultCode}", "Unauthorize mac address", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
