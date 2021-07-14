using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using NAudio.Wave;

namespace Hangman
{
	static class Sound
	{
		private static Dictionary<string, byte[]> MusicDictionary = new Dictionary<string, byte[]>();
		private static Dictionary<string, byte[]> SoundDictionary = new Dictionary<string, byte[]>();
        private static WaveOut MusicWaveOutput;
        private static MemoryStream MusicMemoryStream;
        private static LoopStream MusicMp3Looper = null;
        private static Mp3FileReader MusicMp3FileReader;

		public static void SoundExecutePlayMusic()
		{
			if (MusicMp3Looper != null)
			{
                MusicWaveOutput.Stop();
                MusicMp3Looper.Dispose();
                MusicWaveOutput.Dispose();
                MusicMemoryStream.Dispose();
            }
            MusicMemoryStream = new MemoryStream(Properties.Resources.bgm_main);
            MusicMp3FileReader = new Mp3FileReader(MusicMemoryStream);
            MusicMp3Looper = new LoopStream(MusicMp3FileReader);
            MusicWaveOutput = new WaveOut();
            MusicWaveOutput.Init(MusicMp3Looper);
            MusicWaveOutput.Play();
		}
        public static void SoundExecuteStopMusic()
        {
            if (MusicMp3Looper != null)
            {
                MusicWaveOutput.Stop();
                MusicMp3Looper.Dispose();
                MusicWaveOutput.Dispose();
                MusicMemoryStream.Dispose();
            }
        }
		public static void SoundExecutePlaySound(string SoundName)
		{
			MemoryStream Mp3Stream = new MemoryStream(SoundDictionary[SoundName]);
			Mp3FileReader Mp3Reader = new Mp3FileReader(Mp3Stream);
			WaveOutEvent Mp3WaveOut = new WaveOutEvent();
			Mp3WaveOut.Init(Mp3Reader);
			Mp3WaveOut.Play();

			Mp3WaveOut.PlaybackStopped += (object Sender, StoppedEventArgs e) =>
			{
				Mp3WaveOut.Dispose();
                Mp3Reader.Dispose();
                Mp3Stream.Dispose();
			};
		}
	}

    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
        /// or else we will not loop to the start again.</param>
        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return sourceStream.Length; }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
