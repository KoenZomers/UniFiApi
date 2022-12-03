using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KoenZomers.UniFi.Api;
using KoenZomers.UniFi.Api.Responses;

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

            if (UniFiServerSiteTextBox.Text == "")
            {
                _uniFiApi = new Api(new Uri(UniFiServerAddressTextBox.Text));
            }
            else
            {
                _uniFiApi = new Api(new Uri(UniFiServerAddressTextBox.Text), UniFiServerSiteTextBox.Text);
            }

            // Disable SSL validation as UniFi uses a self signed certificate
            _uniFiApi.DisableSslValidation();

            // Authenticate to UniFi
            var connectionResult = await _uniFiApi.Authenticate(UniFiServerUsernameTextBox.Text, UniFiServerPasswordTextBox.Text);

            if(connectionResult)
            {
                //MessageBox.Show(this, "Connection to UniFi server successful", "Connected to UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Connection to UniFi server failed", "Unable to connect to UniFi server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            ActionsGroupBox.Enabled = connectionResult;
            UniFiServerDisconnectButton.Enabled = connectionResult;
            UniFiServerConnectButton.Enabled = !connectionResult;
        }

        private async void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {        
            if(_uniFiApi != null && _uniFiApi.IsAuthenticated)
            {
                await _uniFiApi.Logout();
            }
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

        private async void GetClientsButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClientsListBox.Items.Clear();

                var clients = await _uniFiApi.GetAllClients();

                foreach (var client in clients)
                {
                    ClientsListBox.Items.Add($"{(!string.IsNullOrWhiteSpace(client.FriendlyName) ? client.FriendlyName : client.Hostname)} ({client.MacAddress})");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async void buttonGetPorts_Click(object sender, EventArgs e)
        {
            try
            {
                PortsRichTextBox.Text = "";
                var devices = await _uniFiApi.GetDevices();
                var profiles = await _uniFiApi.GetProfiles();
                PortsRichTextBox.SelectionTabs = new int[] { 50, 100, 300, 400 ,500,600,700};
                foreach (var device in devices)
                {
                    foreach (var port in device.Port_table)
                    {
                        foreach (var mac in port.Mac_table)
                        {
                            PortsRichTextBox.Text += $"{device.Name}\t{port.Port_idx}\t{mac.Hostname}\t{mac.Mac}\t{mac.IP}\t{mac.Vlan}\t{mac.Uptime}\t{mac.Is_only_station_on_port}\t{mac.Age}\r\n";
                        }
                    }
                }

                foreach (var device in devices)
                {
                    if (device.Port_overrides_table != null)
                    {
                        foreach (var overrides in device.Port_overrides_table)
                        {
                            PortsRichTextBox.Text += $"{device.Name}\t{overrides.Port_idx}\t{overrides.Op_mode}\t{overrides.aggregate_num_ports}\t{overrides.Portconf_id}\t{PortconfidtoName(overrides.Portconf_id, profiles)}\r\n";
                            //foreach (var mac in overrides.mac)
                            //{
                            //    PortsRichTextBox.Text += $"{device.Name}\t{port.Port_idx}\t{fillspaces(mac.Hostname, 0)}\t{mac.Mac}\t{mac.IP}\t{mac.Vlan}\t{mac.Uptime}\t{mac.Is_only_station_on_port}\t{mac.Age}\r\n";
                            //}
                        }
                    }

                }


                foreach (var profile in profiles)
                {
                    PortsRichTextBox.Text += $"{profile.ID}\t{profile.Name}\t\r\n";
                }

                var clients = await _uniFiApi.GetAllClients();
                foreach (var client in clients)
                {
                    PortsRichTextBox.Text += $"{client.MacAddress}\t{client.Hostname}\t{client.FriendlyName}\t{client.Vlan}\t{client.UserId}\t{client.IsWired}\t{client.LastSeenRaw}\t{client.IpAddress}\t{client.Id}\t\r\n";
                }
                clients = await _uniFiApi.GetActiveClients();
                foreach (var client in clients)
                {
                    PortsRichTextBox.Text += $"{client.MacAddress}\t{client.Hostname}\t{client.FriendlyName}\t{client.Vlan}\t{client.UserId}\t{client.IsWired}\t{client.LastSeenRaw}\t{client.IpAddress}\t{client.Id}\t\r\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string PortconfidtoName(string portconf_id, List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                if (profile.ID == portconf_id)
                {
                    return profile.Name;
                }
            }
            return "";
        }
    }
}
