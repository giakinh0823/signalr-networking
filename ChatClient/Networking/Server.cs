using ChatClient.Networking.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


/*
 Đây là một lớp đại diện cho một máy chủ trong mạng TCP/IP, sử dụng lớp TcpClient để kết nối với máy chủ. 
Lớp này có các phương thức để kết nối đến máy chủ, gửi tin nhắn và đọc các gói tin từ máy chủ.

Trong hàm tạo của lớp Server, một đối tượng TcpClient được tạo ra để thực hiện kết nối đến máy chủ.

Phương thức connectToServer được sử dụng để kết nối đến máy chủ với địa chỉ IP 127.0.0.1 và cổng 7777. 
Sau khi kết nối thành công, một đối tượng PacketReader được tạo ra để đọc các gói tin từ máy chủ. 
Nếu có tên người dùng được cung cấp, một gói tin đăng nhập được tạo ra và gửi đến máy chủ.

Phương thức ReadPackets được sử dụng để đọc các gói tin từ máy chủ và xử lý chúng. Vòng lặp while(true)
được sử dụng để đọc liên tục các gói tin từ máy chủ. Đầu tiên, một byte đầu tiên được đọc để xác định mã
hoạt động của gói tin. Mã hoạt động này được sử dụng để xác định loại gói tin và xử lý tương ứng. 
Nếu mã hoạt động là 1, sự kiện connectedEvent được kích hoạt, nếu là 2, sự kiện msgReceivedEvent được kích hoạt, 
nếu không thì một thông báo đơn giản được in ra console.

Phương thức sendMessageToServer được sử dụng để gửi một tin nhắn tới máy chủ. Đầu tiên, 
một đối tượng PacketWriter được tạo ra để tạo gói tin mới. Mã hoạt động của gói tin là 2,
và tin nhắn được thêm vào gói tin bằng phương thức writeMessage. 
Cuối cùng, gói tin được gửi đến máy chủ bằng phương thức Send của đối tượng TcpClient.
 */

namespace ChatClient.Networking
{
    class Server
    {
        TcpClient _client;
        public PacketReader packetReader;
        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public Server()
        {
            _client = new TcpClient();
        }

        public void connectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7777);
                packetReader = new PacketReader(_client.GetStream());
                if (!string.IsNullOrEmpty(username))
                {
                    var _packetWriter = new PacketWriter();
                    _packetWriter.writeOpCode(0);
                    _packetWriter.writeMessage(username);
                    _client.Client.Send(_packetWriter.getPacketBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 2:
                            msgReceivedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("FPT Hola");
                            break;
                    }
                }
            });
        }
        public void sendMessageToServer(string message)
        {
            var messagePacket = new PacketWriter();
            messagePacket.writeOpCode(2);
            messagePacket.writeMessage(message);
            _client.Client.Send(messagePacket.getPacketBytes());
        }
    }
}
