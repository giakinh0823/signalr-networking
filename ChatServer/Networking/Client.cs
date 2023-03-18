using ChatServer.Networking.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
 Đây là một lớp C# có tên là "Client" đại diện cho một khách hàng trong kịch bản giao tiếp mạng máy tính máy chủ - khách hàng. 
Dưới đây là một giải thích ngắn gọn về các thuộc tính và phương thức của nó:

Thuộc tính:

"username": một thuộc tính chuỗi đại diện cho tên người dùng của khách hàng.
"UID": một thuộc tính Guid đại diện cho định danh duy nhất của khách hàng.
"_clientSocket": một thuộc tính TcpClient đại diện cho socket của khách hàng được sử dụng để giao tiếp với máy chủ.
Phương thức:

"Process ()": một phương thức chạy trong vòng lặp và xử lý các tin nhắn đến từ máy chủ. Nó sử dụng một đối tượng 
PacketReader để đọc các gói tin đến và chuyển đổi trên mã lệnh để xác định loại tin nhắn đã nhận được. 
Nếu mã lệnh là 2, nó đọc tin nhắn sử dụng phương thức "readMessage ()" của PacketReader và phát sóng nó
cho tất cả khách hàng bằng cách sử dụng phương thức "broadCastMessage ()" của lớp Program.

Tổng thể, lớp "Client" chịu trách nhiệm đại diện cho một khách hàng trong mạng và xử lý các tin nhắn đến từ máy chủ.
 */

namespace ChatServer.Networking
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient _clientSocket { get; set; }
        PacketReader _packetReader;
        public Client(TcpClient clientSocket)
        {
            _clientSocket = clientSocket;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(_clientSocket.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.readMessage();

            Console.WriteLine($"{DateTime.Now}: Client has connected with user name: {Username} ");
            Task.Run(() => Process());
        }
        public void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 2:
                            var msg = _packetReader.readMessage();
                            Console.WriteLine($"{DateTime.Now} message received: {msg}");
                            Program.broadCastMessage($"[{DateTime.Now}] : [{Username}] : {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
