/*
using System.IO;
using System.Windows;
using IrrKlang;

namespace Text_Dungeon_Crawler
{
    internal class SoundManager : IDisposable
    {
        private readonly Dictionary<string, string> _soundPaths = new Dictionary<string, string>
        {
            { "ignite", "resources/sounds/fire/ignite.wav" },
            { "teleport", "resources/sounds/teleport/teleport.wav" },
            { "step1", "resources/sounds/step/step1.wav" },
            { "step2", "resources/sounds/step/step2.wav" },
            { "step3", "resources/sounds/step/step3.wav" },
            { "step4", "resources/sounds/step/step4.wav" },
            { "step5", "resources/sounds/step/step5.wav" },
            { "step6", "resources/sounds/step/step6.wav" }
            // Add more sounds here as needed
        };

        private readonly List<string> _stepSounds = new List<string>
        {
            "step1",
            "step2",
            "step3",
            "step4",
            "step5",
            "step6"
            // Add more sound names here as needed
        };

        private readonly Random _random = new Random();
        private readonly ISoundEngine _soundEngine;
        private readonly Dictionary<string, ISoundSource> _sounds = new Dictionary<string, ISoundSource>();

        public SoundManager()
        {
            // Initialize the IrrKlang sound engine
            _soundEngine = new ISoundEngine();

            // Load sounds
            foreach (var soundName in _soundPaths.Keys)
            {
                LoadSound(soundName);
            }       
        }

        public void SetMasterVolume(float volume)
        {
            _soundEngine.SoundVolume = volume;
        }

        public void PlayStepSound()
        {
            // Select a random sound name from the list
            string randomSoundName = _stepSounds[_random.Next(_stepSounds.Count)];

            // Play the selected sound
            PlaySound(randomSoundName);
        }

        public void PlaySound(string soundName)
        {
            if (!_sounds.TryGetValue(soundName.ToLower(), out ISoundSource soundSource))
            {
                Console.WriteLine($"Sound name '{soundName}' not recognized.");
                MessageBox.Show("Sound name not recognized. Please make sure the sound name is correct.");
                return;
            }

            ISound sound = _soundEngine.Play2D(soundSource, false, false, true);

            if (sound != null)
            {
                // Get the sound effect control interface
                ISoundEffectControl effectControl = sound.SoundEffectControl;

                if (effectControl != null)
                {
                    // Enable and configure reverb

                    // I3DL2Reverb
                    //
                    //effectControl.EnableI3DL2ReverbSoundEffect(-1000, -100, 0f, 1.49f, 0.83f, -2602, 0.007f, 200, 0.011f, 100f, 100f, 5000f);
                    //
                    // 1-fRoomHF, 2-fRoomLF, 3-fDecayTime, 4-fDecayHFRatio, 5-fDecayLFRatio, 6-fReflections,
                    // 7-fReflectionsDelay, 8-fReverb, 9-fReverbDelay, 10-fDiffusion, 11-fDensity, 12-fHFReference
                    //                                           1     2    3    4      5       6      7      8     9      10    11    12
                    //effectControl.EnableI3DL2ReverbSoundEffect(-1000, -100, 0f, 1.49f, 0.83f, -2602, 0.3f, 200, 0.011f, 100f, 100f, 5000f);

                    // WavesReverb
                    //
                    //effectControl.EnableWavesReverbSoundEffect(0f, 0f, 1000f, 0.001f);
                    //
                    // 1-fInGain, 2-fReverbMix, 3-fReverbTime, 4-fHighFreqRTRatio
                    //                                         1    2   3     4
                    //effectControl.EnableWavesReverbSoundEffect(0f, 0f, 100f, 0.001f);
                }
            }
        }

        private void LoadSound(string soundName)
        {
            if (!_soundPaths.TryGetValue(soundName.ToLower(), out string relativePath))
            {
                Console.WriteLine($"Sound name '{soundName}' not recognized.");
                MessageBox.Show($"Sound name '{soundName}' not recognized.", "Error");
                return;
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"Sound file '{fullPath}' not found.");
                MessageBox.Show($"Sound file '{fullPath}' not found.", "Error");
                return;
            }

            // Load the sound file into IrrKlang
            _sounds[soundName.ToLower()] = _soundEngine.AddSoundSourceFromFile(fullPath);
        }

        public void Dispose()
        {
            // Dispose of the IrrKlang sound engine
            _soundEngine.Dispose();
        }
    }
}
*/
