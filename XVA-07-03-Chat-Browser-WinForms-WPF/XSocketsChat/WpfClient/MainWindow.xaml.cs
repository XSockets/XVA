using System;
using System.Windows;
using System.Windows.Controls;
using XSockets;
using XSockets.Common.Interfaces;

namespace WpfClient
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
    public partial class MainWindow : Window
    {
        private String UserName { get; set; }
        private IController chatController { get; set; }
        const string ServerURI = "ws://localhost:8080";
        const string Origin = "http://localhost";
        private IXSocketClient Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            chatController.Invoke("Send", TextBoxMessage.Text);
            TextBoxMessage.Text = String.Empty;
            TextBoxMessage.Focus();
        }

        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new XSocketClient(ServerURI, Origin, "chat");
            Connection.OnDisconnected += Connection_Disconnected;
            chatController = Connection.Controller("chat");

            //Handle incoming event from server: use Invoke to write to console from XSocket's thread
            chatController.On<string>("AddMessage", (message) =>
                this.Dispatcher.Invoke(() =>
                    RichTextBoxConsole.AppendText(String.Format("{0}\r", message))
                )
            );

            try
            {
                await Connection.Open();

                //Set username
                await chatController.SetProperty("username", UserName);
            }
            catch
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Show chat UI; hide login UI
            SignInPanel.Visibility = Visibility.Collapsed;
            ChatPanel.Visibility = Visibility.Visible;
            ButtonSend.IsEnabled = true;
            TextBoxMessage.Focus();
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        private void Connection_Disconnected(object sender, EventArgs eventArgs)
        {
            try
            {
                //Hide chat UI; show login UI
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
                dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
                dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
                dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
            }
            catch
            {


            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = UserNameTextBox.Text;
            //Connect to server (use async method to avoid blocking UI thread)
            if (!String.IsNullOrEmpty(UserName))
            {
                StatusText.Visibility = Visibility.Visible;
                StatusText.Content = "Connecting to server...";
                ConnectAsync();
            }
        }

        private void WPFClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Connection != null)
            {
                Connection.Disconnect();
            }
        }

        private void OnLocationChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (chatController != null)
                chatController.SetEnum("location", ((ComboBoxItem)ComboBoxLocation.SelectedItem).Content.ToString());
        }
    }
}
