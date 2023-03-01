namespace TCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SuperSimpleTcp.SimpleTcpClient client;
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new(txtIP.Text);
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
            btnSend.Enabled = false;
        }

        private void Events_Disconnected(object sender, SuperSimpleTcp.ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server disconnected.{Environment.NewLine}";

            });
            
        }

        private void Events_DataReceived(object sender, SuperSimpleTcp.DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text+=$"Server: {System.Text.Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";

            });
           
        }

        private void Events_Connected(object sender, SuperSimpleTcp.ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text+=$"Server connected.{Environment.NewLine}";

            });
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                if (!string.IsNullOrEmpty(txtMessage.Text)) 
                {
                    client.Send(txtMessage.Text);
                    txtInfo.Text += $"Me {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}