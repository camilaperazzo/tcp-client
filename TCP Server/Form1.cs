namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SuperSimpleTcp.SimpleTcpServer server;
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SuperSimpleTcp.SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object? sender, SuperSimpleTcp.DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: {System.Text.Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object? sender, SuperSimpleTcp.ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                lstClientIP.Items.Remove(e.IpPort);
            });          
        }

        private void Events_ClientConnected(object? sender, SuperSimpleTcp.ConnectionEventArgs e)
        {         

            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                lstClientIP.Items.Add(e.IpPort);
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text) && lstClientIP.SelectedItem != null) //check message & select ip from listbox
                {
                    server.Send(lstClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

        private void lstClientIP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}