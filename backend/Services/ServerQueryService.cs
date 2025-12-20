using System.Net;
using System.Net.Sockets;
using System.Text;

namespace backend.Services;

public interface IServerQueryService
{
    Task<ServerQueryResult?> QueryServerAsync(string ipAddress, int port);
}

public class ServerQueryResult
{
    public string ServerName { get; set; } = string.Empty;
    public string Map { get; set; } = string.Empty;
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public bool IsOnline { get; set; }
}

public class ServerQueryService : IServerQueryService
{
    private const int Timeout = 5000;

    public async Task<ServerQueryResult?> QueryServerAsync(string ipAddress, int port)
    {
        try
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var client = new UdpClient();
                    client.Client.SendTimeout = Timeout;
                    client.Client.ReceiveTimeout = Timeout;

                    var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                    client.Connect(endpoint);

                    byte[] request = new byte[] 
                    { 
                        0xFF, 0xFF, 0xFF, 0xFF, 
                        0x54, 
                        0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 
                        0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 
                        0x51, 0x75, 0x65, 0x72, 0x79, 0x00 
                    };

                    client.Send(request, request.Length);

                    IPEndPoint? receiveEndpoint = null;
                    var response = client.Receive(ref receiveEndpoint);

                    if (response != null && response.Length >= 5)
                    {
                        if (response[4] == 0x41)
                        {
                            byte[] challengeNumber = new byte[4];
                            Array.Copy(response, 5, challengeNumber, 0, 4);

                            byte[] requestWithChallenge = new byte[]
                            {
                                0xFF, 0xFF, 0xFF, 0xFF, 
                                0x54, 
                                0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 
                                0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20,
                                0x51, 0x75, 0x65, 0x72, 0x79, 0x00, 
                                challengeNumber[0], challengeNumber[1], challengeNumber[2], challengeNumber[3]
                            };

                            client.Send(requestWithChallenge, requestWithChallenge.Length);
                            response = client.Receive(ref receiveEndpoint);
                        }

                        if (response.Length > 6 && response[4] == 0x49)
                        {
                            int offset = 5; 

                            byte protocol = response[offset++];

                            var serverName = ReadNullTerminatedString(response, ref offset);
                            var map = ReadNullTerminatedString(response, ref offset);
                            
                            ReadNullTerminatedString(response, ref offset);
                            ReadNullTerminatedString(response, ref offset);
                            
                            if (offset + 2 <= response.Length)
                                offset += 2;

                            int players = offset < response.Length ? response[offset++] : 0;
                            int maxPlayers = offset < response.Length ? response[offset++] : 0;

                            return new ServerQueryResult
                            {
                                ServerName = serverName,
                                Map = map,
                                Players = players,
                                MaxPlayers = maxPlayers,
                                IsOnline = true
                            };
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Socket error querying server {ipAddress}:{port} - {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error querying server {ipAddress}:{port} - {ex.Message}");
                }

                return new ServerQueryResult
                {
                    ServerName = "Server Offline",
                    Map = "unknown",
                    Players = 0,
                    MaxPlayers = 0,
                    IsOnline = false
                };
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Critical error: {ex.Message}");
            return null;
        }
    }

    private static string ReadNullTerminatedString(byte[] buffer, ref int offset)
    {
        var start = offset;
        while (offset < buffer.Length && buffer[offset] != 0)
        {
            offset++;
        }
        
        var result = Encoding.UTF8.GetString(buffer, start, offset - start);
        offset++; 
        return result;
    }
}
