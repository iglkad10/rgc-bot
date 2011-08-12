using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace rgcbot
{
    public class NetworkStreamHandler
    {
        public event Action<byte[], int> DataAvailable;
        public event Action<Exception, NetworkStream> StreamError;

        protected void ReadFromStream(object worker, DoWorkEventArgs args)
        {
            BackgroundWorker streamWorker = worker as BackgroundWorker;
            NetworkStream stream = args.Argument as NetworkStream;
            try
            {
                HandleStreamInput(stream);
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is ObjectDisposedException || ex is InvalidOperationException)
                {
                    streamWorker.CancelAsync();
                }

                if (ex is IOException || ex is InvalidOperationException)
                {
                    stream.Dispose();
                }

                if (StreamError != null)
                {
                    StreamError(ex, stream);
                }
            }
        }

        private void HandleStreamInput(NetworkStream stream)
        {
            byte[] data = new byte[1024];
            int read = stream.Read(data, 0, 1024);
            if (DataAvailable != null)
            {
                DataAvailable(data, read);
            }
        }
    }
}
