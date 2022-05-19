using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using Newtonsoft.Json;

namespace ArduinoCom
{
    class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {
                runCom();
            }
        }

        static void runTest()
        {
            int count = 0;
            while (true)
            {
                runCom();
                count++;
                if (count == 20)
                {
                    Console.WriteLine("continue?");

                    string answ = Console.ReadLine();
                    if (answ == "y")
                    {
                        count = 0;
                    }
                }

            }
        }

        static Dictionary<string, string>[] getJsonArr()
        {
            using (WebClient wc = new WebClient())
            {
                string jsonEthStr = wc.DownloadString("https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=ETH,USD");
                string jsonBtcStr = wc.DownloadString("https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=BTC,USD");

                Dictionary<string, string> ETHDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonEthStr);
                Dictionary<string, string> BTCDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonBtcStr);

                Dictionary<string, string>[] stats = new Dictionary<string, string>[] { ETHDict, BTCDict };


                return stats;
            }
        }

        static string etherInfo(Dictionary<string, string> dict)
        {
            return "1 ETH = " + dict["USD"] + " USD";
        }

        static string btcInfo(Dictionary<string, string> dict)
        {
            return "1 BTC = " + dict["USD"] + " USD";
        }

        static void runCom()
        {
            //var stop = new Stopwatch();
            //stop.Start();
            SerialPort port = new SerialPort("COM3", 9600);
            port.Open();

            string cryptoInfo = btcInfo(getJsonArr()[1]) + "\n\n" + etherInfo(getJsonArr()[0]) + "_";

            char[] tx = cryptoInfo.ToCharArray();

            foreach (char Char in tx)
            {
                port.Write(Char.ToString());
                Console.WriteLine("sent: {0}", Char);
                System.Threading.Thread.Sleep(150);
            }
            port.DiscardOutBuffer();
            port.Dispose();
            port.Close();
            //stop.Stop();
            //Console.WriteLine(stop.ElapsedMilliseconds);
        }
    }
}
