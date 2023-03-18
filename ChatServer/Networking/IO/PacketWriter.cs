using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Đây là một lớp ghi gói tin trong C# sử dụng lớp MemoryStream để tạo ra một mảng byte đại diện cho gói tin.
Lớp này có các phương thức để ghi các trường dữ liệu vào gói tin, bao gồm mã hoạt động và chuỗi tin nhắn.

Trong hàm tạo của lớp PacketWriter, một đối tượng MemoryStream được tạo ra để lưu trữ các byte của gói tin.

Phương thức writeOpCode ghi mã hoạt động (opcode) vào gói tin bằng cách gọi phương thức WriteByte của đối tượng MemoryStream.

Phương thức writeMessage ghi chuỗi tin nhắn vào gói tin bằng cách trước tiên ghi độ dài
của chuỗi dưới dạng một số nguyên 32 bit (bằng phương thức Write của đối tượng MemoryStream 
và phương thức BitConverter.GetBytes). Sau đó, phương thức ghi các byte của chuỗi bằng phương thức Encoding.ASCII.GetBytes.

Phương thức getPacketBytes trả về một mảng byte đại diện cho gói tin, được tạo bởi đối tượng MemoryStream.
 */

namespace ChatServer.Networking.IO
{
    class PacketWriter
    {
        MemoryStream _ms;
        public PacketWriter()
        {
            _ms = new MemoryStream();
        }
        public void writeOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }
        public void writeMessage(string msg)
        {
            var msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }
        public byte[] getPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
