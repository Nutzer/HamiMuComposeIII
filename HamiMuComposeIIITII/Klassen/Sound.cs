using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using CSCore;
using CSCore.SoundOut;

namespace HamiMuComposeIIITII
{
    public class Sound
    {
        public float offs, add;
        public SoundPlayer sp;
        public IWaveSource sound;
        public ISoundOut sout;
        public bool isplay;
        public Sound()
        {
            sp = new SoundPlayer();
            if (WasapiOut.IsSupportedOnCurrentPlatform)
            {
                sout = new WasapiOut();
            }
            else
            {
                sout = new DirectSoundOut();
            }

        }
        public void loadSound(string url)
        {
            sound = CSCore.Codecs.CodecFactory.Instance.GetCodec(url);
            sout.Initialize(sound);
            isplay = false;
        }
        public void StartStop()
        {
            if (isplay)
                sout.Stop();
            else sout.Play();
            isplay = !isplay;
        }
        public void stopAll()
        {
            sout.Stop();
            sout.Dispose();
        }
        public int getpos()
        {
            return (int)sout.WaveSource.WaveFormat.BytesToMilliseconds(sout.WaveSource.Position);
        }
        public void setpos(int pos)
        {
            sout.WaveSource.Position = sout.WaveSource.WaveFormat.MillisecondsToBytes(pos);
        }
        public int getlen()
        {
            return (int)sout.WaveSource.WaveFormat.BytesToMilliseconds(sout.WaveSource.Length);
        }
    }
}
