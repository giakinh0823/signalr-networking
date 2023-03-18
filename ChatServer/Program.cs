using ChatServer.Networking.IO;
using ChatServer.Networking;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

/*
 Đây là một chương trình cơ bản để tạo một ứng dụng chat đơn giản sử dụng TCP sockets. 
hương trình này bao gồm 4 class chính:

PacketReader: lớp này kế thừa từ BinaryReader và có phương thức readMessage() để đọc chuỗi từ một luồng dữ liệu đã cho.

PacketWriter: lớp này tạo ra các gói tin và trả về dữ liệu dạng byte.

Client: lớp này đại diện cho một kết nối client với server, 
lắng nghe và xử lý các tin nhắn từ client và gửi chúng đến tất cả các client khác.

Server: lớp này đại diện cho server, lắng nghe kết nối từ các client mới, đọc và gửi tin nhắn giữa các client.

Các phương thức chính của Server bao gồm connectToServer() để kết nối đến server, 
ReadPackets() để đọc các gói tin và xử lý chúng, sendMessageToServer() để gửi tin nhắn đến server.

Các phương thức chính của Client bao gồm Process() để lắng nghe và xử lý tin nhắn từ server,
broadCastMessage() để gửi tin nhắn đến tất cả các client khác.

Cuối cùng, Program đơn giản chỉ khởi tạo một TcpListener và lắng nghe các kết nối từ client mới, 
tạo một đối tượng Client cho mỗi kết nối và thêm vào danh sách _users. Khi có một client mới kết nối, 
phương thức broadCastConnection() được gọi để thông báo cho tất cả các client khác biết về client mới này. 
Nếu một client gửi một tin nhắn mới, phương thức broadCastMessage() được gọi để gửi tin nhắn này đến tất cả các client khác.
 */

namespace ChatServer
{
    internal class Program
    {
        static List<Client> _users;
        static TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7777);
            _listener.Start();
            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                broadCastConnection();
            }

        }
        public static void broadCastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadPacket = new PacketWriter();
                    broadPacket.writeOpCode(1);
                    broadPacket.writeMessage(usr.Username);
                    broadPacket.writeMessage(usr.UID.ToString());
                    user._clientSocket.Client.Send(broadPacket.getPacketBytes());
                }
            }

        }
        public static void broadCastMessage(string message)
        {
            foreach (var user in _users)
            {
                var messsagePacket = new PacketWriter();
                messsagePacket.writeOpCode(2);
                messsagePacket.writeMessage(message);
                user._clientSocket.Client.Send(messsagePacket.getPacketBytes());
            }
        }
    }
}
