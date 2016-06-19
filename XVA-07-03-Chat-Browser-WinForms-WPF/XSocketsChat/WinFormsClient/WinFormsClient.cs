using System;
using System.Windows.Forms;
using XSockets;
using XSockets.Common.Interfaces;

namespace WinFormsClient
{
    /// <summary>
    /// XSockets client hosted in a WinForms application. The client
    /// lets the user pick a user name, connect to the server 
    /// and send chat messages to all connected 
    /// clients whether they are hosted in WinForms, WPF, or a web application.
    /// 
    /// You can also target clients based on location that to show how easy it is to target 
    /// clients dynamically
    /// </summary>
    public partial class WinFormsClient : Form
    {
        private String UserName { get; set; }        
        private IController chatController { get; set; }
        const string ServerURI = "ws://localhost:8080";
        const string Origin = "http://localhost";        
        private IXSocketClient Connection { get; set; }
        
        internal WinFormsClient()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            //Call the method "send" on the server
            //Username is not needed since the server knows about it.
            chatController.Invoke("Send", TextBoxMessage.Text);

            TextBoxMessage.Text = String.Empty;
            TextBoxMessage.Focus();
        }

        /// <summary>
        /// Creates and connects the connection. Only uses one controller (chat). This method
        /// is called from SignInButton_Click.
        /// </summary>
        private async void Connect()
        {
            Connection = new XSocketClient(ServerURI,Origin,"chat");
            Connection.OnDisconnected += Connection_Disconnected;
            chatController = Connection.Controller("chat");            
            
            //Handle incoming event from server: use Invoke to write to console from XSocket's thread
            chatController.On<string>("addMessage", message => this.Invoke((Action) (() =>
                RichTextBoxConsole.AppendText(String.Format("{0}" + Environment.NewLine, message))
            )));
            
            try
            {
                await Connection.Open();
                comboBoxLocation.SelectedIndex = 0;
                //Set username
                await chatController.SetProperty("username", UserName);
            }
            catch
            {
                StatusText.Text = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Activate UI
            SignInPanel.Visible = false;
            ChatPanel.Visible = true;
            ButtonSend.Enabled = true;
            TextBoxMessage.Focus();
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + Environment.NewLine);
        }

        private void Connection_Disconnected(object sender, EventArgs eventArgs)
        {
            //Deactivate chat UI; show login UI. 
            this.Invoke((Action)(() => ChatPanel.Visible = false));
            this.Invoke((Action)(() => ButtonSend.Enabled = false));
            this.Invoke((Action)(() => StatusText.Text = "You have been disconnected."));
            this.Invoke((Action)(() => SignInPanel.Visible = true));
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            UserName = UserNameTextBox.Text;
            //Connect to server
            if (!String.IsNullOrEmpty(UserName))
            {
                StatusText.Visible = true;
                StatusText.Text = "Connecting to server...";
                Connect();
            }
        }

        private void WinFormsClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connection != null)
            {
                Connection.Disconnect();                
            }
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            //Set username
            chatController.SetEnum("location", comboBoxLocation.Text);
        }
    }
}
