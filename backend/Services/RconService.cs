using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services;

public class RconService : IRconService
{
    private const int AUTH_PACKET = 3;
    private const int EXEC_PACKET = 2;
    private const int RESPONSE_VALUE = 0;

    public async Task<bool> ExecuteCommandsAsync(string ip, int port, string password, params string[] commands)
    {
        if (string.IsNullOrEmpty(password) || commands == null || commands.Length == 0)
            return false;

        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(ip, port);
            using var stream = client.GetStream();

            async Task<int> SendPacketAsync(int type, string body)
            {
                var bodyBytes = Encoding.ASCII.GetBytes(body);
                var id = Environment.TickCount;
                var packetLen = 4 + 4 + bodyBytes.Length + 2;

                var buffer = new byte[4 + packetLen]; 
                Array.Copy(BitConverter.GetBytes(packetLen), 0, buffer, 0, 4);
                Array.Copy(BitConverter.GetBytes(id), 0, buffer, 4, 4);
                Array.Copy(BitConverter.GetBytes(type), 0, buffer, 8, 4);
                Array.Copy(bodyBytes, 0, buffer, 12, bodyBytes.Length);

                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();

                var lenBuf = new byte[4];
                var read = await stream.ReadAsync(lenBuf, 0, 4);
                if (read != 4) return int.MinValue;
                var respLen = BitConverter.ToInt32(lenBuf, 0);

                var respBuf = new byte[respLen];
                var total = 0;
                while (total < respLen)
                {
                    var r = await stream.ReadAsync(respBuf, total, respLen - total);
                    if (r == 0) break;
                    total += r;
                }

                if (total < 8) return int.MinValue;
                var respId = BitConverter.ToInt32(respBuf, 0);
                var respType = BitConverter.ToInt32(respBuf, 4);
                return respId;
            }

            var authId = await SendPacketAsync(AUTH_PACKET, password);
            if (authId == int.MinValue || authId == -1)
            {
                return false; 
            }

            foreach (var cmd in commands)
            {
                await SendPacketAsync(EXEC_PACKET, cmd);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
