using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioUtilities
{
    public static class Mixer
    {
        /// <summary>
        /// Concatenates 2 wave files adding a delay between them
        /// </summary>
        /// <param name="firstPath">first file to be executed in the resulting audio file</param>
        /// <param name="secondPath">second file to be executed in the resulting audio file</param>
        /// <param name="delay">delay between the two files in milliseconds</param>
        /// <param name="outputPath">where to save the resulting file</param>
        static public void ConcatenateWave(string firstPath, string secondPath, double delay, string outputPath)
        {
            string tempFilePath = "temp.wav";
            var mixer = new WaveMixerStream32();
            using (var speech = new WaveFileReader(firstPath))
            {
                var soundEffect = new WaveFileReader(secondPath);

                mixer.AutoStop = true;

                var speech32 = new WaveChannel32(speech);
                speech32.PadWithZeroes = false;
                speech32.Volume = 1f;

                mixer.AddInputStream(speech32);

                var soundEffectOffsetted = new WaveOffsetStream(soundEffect, speech.TotalTime.Add(TimeSpan.FromMilliseconds(delay)), TimeSpan.FromSeconds(0), soundEffect.TotalTime);
                var soundEffect32 = new WaveChannel32(soundEffectOffsetted);
                soundEffect32.PadWithZeroes = false;
                soundEffect32.Volume = 1f;
                mixer.AddInputStream(soundEffect32);
                WaveFileWriter.CreateWaveFile(tempFilePath, new Wave32To16Stream(mixer));
            }
            if (System.IO.File.Exists(outputPath))
                System.IO.File.Delete(outputPath);
            System.IO.File.Move(tempFilePath, outputPath);
            
        }

    }
}
