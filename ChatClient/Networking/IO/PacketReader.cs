using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
 Đây là một lớp đọc gói tin trong C# sử dụng lớp BinaryReader. Lớp này kế thừa lớp BinaryReader và có thêm một 
phương thức đọc chuỗi tin nhắn từ một luồng mạng (NetworkStream).

Trong hàm tạo của lớp PacketReader, một tham số là NetworkStream được chuyển vào. NetworkStream là một 
lớp quản lý luồng dữ liệu cho một kết nối mạng.

Phương thức readMessage của lớp PacketReader đọc chuỗi tin nhắn từ luồng mạng bằng cách đọc một số nguyên 32 bit đầu tiên, 
đại diện cho độ dài của chuỗi. Sau đó, một mảng byte có độ dài bằng số nguyên này được tạo và đọc từ luồng mạng bằng 
phương thức Read của đối tượng NetworkStream. Cuối cùng, chuỗi được chuyển đổi từ mảng byte bằng phương thức
GetString của lớp Encoding.ASCII.

Chú ý rằng đoạn mã này giả định rằng chuỗi tin nhắn được mã hóa bằng ASCII và không có bất kỳ phần đầu hoặc kết thúc nào khác.
Nếu chuỗi có định dạng khác hoặc chứa các ký tự đặc biệt, phương thức này có thể không hoạt động đúng.
 */
namespace ChatClient.Networking.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }
        public string readMessage()
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);
            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
