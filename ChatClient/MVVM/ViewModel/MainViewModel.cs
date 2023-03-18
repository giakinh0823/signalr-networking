using ChatClient.Core;
using ChatClient.MVVM.Model;
using ChatClient.Networking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        private Server _server;
        public string Username { get; set; }
        public string Message { get; set; }
        public MainViewModel()
        {
            Users = new ObservableCollection<User>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            ConnectToServerCommand = new RelayCommand(o => _server.connectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => _server.sendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));

        }

        private void MessageReceived()
        {
            var msg = _server.packetReader.readMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserConnected()
        {
            var user = new User()
            {
                Username = _server.packetReader.readMessage(),
                UID = _server.packetReader.readMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
