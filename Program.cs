using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Collections;
				
namespace ip_mac_scan{
    public class Program{
        public static void Main(){
            string yol = "e:\\mac.txt";
            ArrayList arrText = new ArrayList();
            string yedek = null;
            string ip = "192.168.1.";
            bool durum = File.Exists(yol);
            if (durum == true)
            {
                StreamReader objReader = new StreamReader("e:\\mac.txt");
                string oku = "";
                while (oku != null)
                {
                    oku = objReader.ReadLine();
                    if (oku != null)
                        arrText.Add(oku);
                }
                objReader.Close();
                //kontroll
                for (int i = 1; i < 25; i++)
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(ip + i);
                    yedek = pingReply.Status.ToString().Trim();
                    if (yedek.Equals("Success"))
                    {
                        foreach (string eleman in arrText)
                        {
                            int k = eleman.IndexOf("\\");
                            string deneme = eleman.Substring(0, k);
                            if (eleman.Equals(ip + i + "\\" + GetMacAddress(ip + i)) &&(ip + i).Equals(deneme))
                            {
                                Console.WriteLine(eleman + " dogru mac kullaniyor ");                                           
                            }
                            if ((ip + i).Equals(deneme) && !eleman.Equals(ip + i + "\\" + GetMacAddress(ip + i))) {
                                Console.WriteLine(eleman + " yanlis mac kullaniyor ");  
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(ip + i + " agda kullanilmiyor");
                    }
                }
            }
            else
            {
                for (int i = 1; i < 25; i++)
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(ip + i);
                    yedek = pingReply.Status.ToString().Trim();
                    if (yedek.Equals("Success"))
                    {
                        var macAddress = GetMacAddress(ip + i);
                        StreamWriter SW = File.AppendText("e:\\mac.txt");
                        SW.WriteLine(ip + i + "\\" + macAddress);
                        SW.Close();
                    }
                    else
                    {
                        Console.WriteLine(ip+i+" agda kullanilmiyor");
                    }

                }

            }
            Console.ReadLine();
        }
            private static string GetMacAddress(string strAddress)
            {
                var inetAddr = inet_addr(strAddress);

                uint addressLen = 16;
                var macAddress = new byte[addressLen];

                if (SendARP(inetAddr, 0, macAddress, ref addressLen) == 0)
                {
                    var sb = new StringBuilder();

                    for (var index = 0; index < addressLen; index++)
                    {
                        if (index > 0)
                        {
                            sb.Append("-");
                        }

                        sb.Append(
                            string.Format(
                                "{0:X}",
                                macAddress[index]).PadLeft(2, '0'));
                    }

                    return sb.ToString();
                }

                throw new Exception("SendARP call failed.");
            }

            [DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)]
            private static extern uint inet_addr(string address);

            [DllImport("iphlpapi.dll", ExactSpelling = true)]
            private static extern int SendARP(
                uint destinationIp,
                uint sourceIp,
                byte[] macAddress,
                ref uint addressLen);
        }
    
}
