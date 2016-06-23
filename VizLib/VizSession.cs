using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Norne_Beta.VizLib
{
    class VizSession
    {

        public string Server { get; set; }
        public int Port { get; set; }

        public VizSession(string server, int port)
        {
            this.Port = port;
            this.Server = server;
        }
        
        public async Task<string> GetFromViz(string message)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(this.Server, this.Port);


                NetworkStream stream = client.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(message);
                stream.Write(ba, 0, ba.Length);

                byte[] bb = new byte[10000];
                String responseData = String.Empty;
                Int32 bytes = await stream.ReadAsync(bb, 0, bb.Length);

                responseData = System.Text.Encoding.UTF8.GetString(bb, 0, bytes);
                stream.Close();
                client.Close();
                parseObjectControl(responseData);
                return  responseData;
            }

            catch (ArgumentNullException eve)
            {
                Console.WriteLine("ArgumentNullException: {0}", eve);
                return "ArgumentNullException";
            }
            catch (SocketException eve)
            {
                Console.WriteLine("SocketException: {0}", eve);
                return "SocektException";
            }
        }
        
    
        public List<ControlObject> parseObjectControl(string data)
        {
            List<ControlObject> res = new List<ControlObject>();
            string[] respond = data.Split('\n');
            foreach (var item in respond)
            {
                string field = item.Split(':')[0];
                res.Add(new ControlObject(field));
            }
            return res;
        }

    }

    public class ControlObject
    {
        public string Field { get; set; }

        public ControlObject(string field)
        {
            this.Field = field;
        }
    }




   

}
