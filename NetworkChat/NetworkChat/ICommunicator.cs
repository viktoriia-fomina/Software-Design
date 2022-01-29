﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkChat
{
    public abstract class ICommunicator
    {
        protected TcpClient _client = new();
        protected NetworkStream _stream;

        public abstract Task Run();

        protected void SendMessage()
        {
            Task.Run(async () =>
            {
                var writer = new StreamWriter(_stream);
                var data = Console.ReadLine();
                await writer.WriteLineAsync(data);
                await writer.FlushAsync();

                if (data == "exit")
                {
                    Shutdown();
                }
            });
        }


        protected void GetMessage()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var reader = new StreamReader(_stream);
                    var data = await reader.ReadLineAsync();
                    Console.WriteLine(data);

                    if (data == "exit")
                    {
                        Shutdown();
                        break;
                    }
                }
            });
        }

        private void Shutdown()
        {
            _stream.Close();
            _client.Close();
            Environment.Exit(0);
        }
    }
}