using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Core.XSocket.Model;

namespace AudioStreamer.Controllers
{
    public class AudioStream : XSocketController
    {
        const int ChunkSize = 500 * 1024;
 
        public static string AudioFilePath { get; set; }
        public static List<string> AudioFiles { get; set; }

        static AudioStream ()
        {
          AudioFilePath = 
           string.Format(@"{0}mp3\",
                System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath);

            AudioFiles = Directory.EnumerateFiles(AudioFilePath, "*.mp3").ToList();
        }

        private IList<byte> FileBytes { get; set; }

        public AudioStream()
        {

        }


        public void GetSongs()
        {
            this.Invoke(AudioFiles.Select(audioFile => Regex.Replace(audioFile, @"(^[\w]:\\)([\w].+\w\\)",
                string.Empty)).ToList(), "songs");
        }


        public void GetSong(string name)
        {
            this.BytesRead = 0;
            this.FileBytes =  File.ReadAllBytes(AudioFilePath + name);
            this.Invoke(new {loaded = true,size = FileBytes.Count()},"songloaded");
        }

        public int BytesRead { get; set; }

        public void GetChunk()
        {
            var arrayBuffer = FileBytes.Skip(this.BytesRead).Take(ChunkSize).ToArray();
            this.BytesRead = this.BytesRead + ChunkSize;
            var bm = new Message(arrayBuffer, new
            {
                size = FileBytes.Count(),
                read = this.BytesRead,
                final = this.BytesRead >= this.FileBytes.Count
            }, "chunk", this.Alias);
            this.Invoke(bm);
        }

    }
}