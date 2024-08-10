using System.IO;
using System.Windows;
using IrrKlang;

namespace Text_Dungeon_Crawler
{
    internal class SoundManager : IDisposable
    {
        private readonly Dictionary<string, string> _soundPaths = new Dictionary<string, string>
        {
            // fx
            { "ignite", "resources/sounds/fire/ignite.wav" },
            { "teleport", "resources/sounds/teleport/teleport.wav" },

            // step
            
            { "stepstone1", "resources/sounds/step/stone/fst_stone_001.wav" },
            { "stepstone2", "resources/sounds/step/stone/fst_stone_002.wav" },
            { "stepstone3", "resources/sounds/step/stone/fst_stone_003.wav" },
            { "stepstone4", "resources/sounds/step/stone/fst_stone_004.wav" },
            { "stepstone5", "resources/sounds/step/stone/fst_stone_005.wav" },
            { "stepstone6", "resources/sounds/step/stone/fst_stone_006.wav" },
            { "stepstone7", "resources/sounds/step/stone/fst_stone_007.wav" },
            { "stepstone8", "resources/sounds/step/stone/fst_stone_008.wav" },
            { "stepstone9", "resources/sounds/step/stone/fst_stone_009.wav" },
            { "stepstone10", "resources/sounds/step/stone/fst_stone_010.wav" },
            { "stepstone11", "resources/sounds/step/stone/fst_stone_011.wav" },
            { "stepstone12", "resources/sounds/step/stone/fst_stone_012.wav" },
            { "stepstone13", "resources/sounds/step/stone/fst_stone_013.wav" },
            { "stepstone14", "resources/sounds/step/stone/fst_stone_014.wav" },
            { "stepstone15", "resources/sounds/step/stone/fst_stone_015.wav" },
            { "stepstone16", "resources/sounds/step/stone/fst_stone_016.wav" },
            
            { "step1", "resources/sounds/step/step1.wav" },
            { "step2", "resources/sounds/step/step2.wav" },
            { "step3", "resources/sounds/step/step3.wav" },
            { "step4", "resources/sounds/step/step4.wav" },
            { "step5", "resources/sounds/step/step5.wav" },
            { "step6", "resources/sounds/step/step6.wav" },

            // ambient
            { "ambient1", "resources/sounds/ambient/amb_dungeon2d_lp_01.wav"},
            { "ambient2", "resources/sounds/ambient/amb_dungeon2d_lp_02.wav"},
            { "ambient3", "resources/sounds/ambient/amb_dungeon2d_lp_03.wav"},
            { "ambient4", "resources/sounds/ambient/amb_dungeon2d_lp_04.wav"},
            { "ambient5", "resources/sounds/ambient/amb_dungeon2d_lp_05.wav"},
            { "ambient6", "resources/sounds/ambient/amb_dungeon2d_lp_06.wav"},
            { "ambient7", "resources/sounds/ambient/amb_dungeon2d_lp_07.wav"},
            { "ambient8", "resources/sounds/ambient/amb_dungeon2d_lp_08.wav"},
            { "ambient9", "resources/sounds/ambient/amb_dungeon2d_lp_09.wav"},
            { "ambient10", "resources/sounds/ambient/amb_dungeon2d_lp_10.wav"},
            { "ambient11", "resources/sounds/ambient/amb_ayleid_001.wav" },
            { "ambient12", "resources/sounds/ambient/amb_ayleid_002.wav" },
            { "ambient13", "resources/sounds/ambient/amb_cave_generic.wav" },
            { "ambient14", "resources/sounds/ambient/amb_dungeon_fort_001.wav" },
            { "ambient15", "resources/sounds/ambient/amb_dungeon_fort_002.wav" },
            { "ambient16", "resources/sounds/ambient/amb_dungeon_fort_generic.wav"},

            // music
            { "music1", "resources/sounds/music/Daggerfall Soundtrack HQ Remake Theme 005.ogg" },
            { "music2", "resources/sounds/music/Daggerfall Soundtrack HQ Remake Theme 010.ogg" },
            { "music3", "resources/sounds/music/fdungeon_01_v2.ogg" },
            { "music4", "resources/sounds/music/13.ogg" },
            { "music5", "resources/sounds/music/far.ogg" },
            { "music6", "resources/sounds/music/menu1.ogg" },
            { "music7", "resources/sounds/music/nether1.ogg" },
            { "music8", "resources/sounds/music/nether3.ogg" },
            { "music9", "resources/sounds/music/ward.ogg" },

            // menu music
            { "menumusic1", "resources/sounds/music/menu/creative2.ogg" },
            { "menumusic2", "resources/sounds/music/menu/piano1.ogg" },

            // silence
            { "silence", "resources/sounds/silence.wav" },

            // door
            { "doorstoneopen", "resources/sounds/door/stone_open.wav" },
            { "doorstoneclose", "resources/sounds/door/stone_close.wav" }

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
            
            /*
            "stepstone1",
            "stepstone2",
            "stepstone3",
            "stepstone4",
            "stepstone5",     
            "stepstone6",
            "stepstone7",
            "stepstone8",
            "stepstone9",
            "stepstone10",
            "stepstone11",
            "stepstone12",
            "stepstone13",
            "stepstone14",
            "stepstone15",
            "stepstone16"
            */
            // Add more sound names here as needed
        };

        private readonly List<string> _ambientSounds = new List<string>
        {
            //"ambient1",
            //"ambient2",
            //"ambient3",
            //"ambient4",
            //"ambient5",
            //"ambient6",
            //"ambient7",
            //"ambient8",
            //"ambient9",
            //"ambient10",
            "ambient11",
            "ambient12",
            "ambient13",
            "ambient14",
            "ambient15",
            "ambient16"
            //"silence",
            //"silence",
            //"silence",
            //"silence",
            //"silence"
            // Add more sound names here as needed
        };

        private readonly List<string> _musicSounds = new List<string>
        {
            "music1",
            "music2",
            "music3",
            "music4",
            "music5",
            "music6",
            "music7",
            "music8",
            "music9"
            // Add more sound names here as needed
        };

        private readonly List<string> _menuMusicSounds = new List<string>
        {
            "menumusic1",
            "menumusic2"
            // Add more sound names here as needed
        };

        private readonly Random _random = new Random();
        private readonly ISoundEngine _soundEngine;
        private readonly Dictionary<string, ISoundSource> _sounds = new Dictionary<string, ISoundSource>();
        public ISound ambientSound;
        System.Timers.Timer timerAmbient = new System.Timers.Timer();
        public ISound music;
        System.Timers.Timer timerMusic = new System.Timers.Timer();

        public SoundManager()
        {
            // Initialize the IrrKlang sound engine
            _soundEngine = new ISoundEngine();

            // Load sounds
            foreach (var soundName in _soundPaths.Keys)
            {
                LoadSound(soundName);
            }

            timerAmbient.Interval = 1000;
            timerMusic.Interval = 1000;
        }

        public void SetMasterVolume(float volume)
        {
            _soundEngine.SoundVolume = volume;
        }

        public void PlayStepSound()
        {
            PlayRandomSound("step");
        }


        // timer that runs and checks if the ambient sound has finished playing
        public void PlayAmbientSoundTimer()
        {
            PlayRandomSound("ambient");
            //MessageBox.Show("Playing ambient sound");

            timerAmbient.Elapsed += (sender, args) =>
            {
                //MessageBox.Show("Checking if ambient sound has finished playing");
                if (ambientSound.Finished)
                {
                    //MessageBox.Show("Ambient sound finished playing");
                    PlayRandomSound("ambient");
                }
            };
            //MessageBox.Show("Starting timer");
            timerAmbient.Start();
        }

        public void PlayMusicTimer(string musicType)
        {
            StopMusicTimer();
            PlayRandomSound(musicType);
            //MessageBox.Show("Playing music");

            timerMusic.Elapsed += (sender, args) =>
            {
                //MessageBox.Show("Checking if music has finished playing");
                if (music.Finished)
                {
                    //MessageBox.Show("Music finished playing");
                    PlayRandomSound(musicType);
                }
            };
            //MessageBox.Show("Starting timer");
            timerMusic.Start();
        }

        // stop timer
        public void StopAmbientSoundTimer()
        {
            //MessageBox.Show("Stopping timer");
            timerAmbient.Stop();
            //MessageBox.Show("Stopping ambient sound");
            ambientSound.Stop();
            //_soundEngine.StopAllSounds(); // stop all sounds
        }

        public void StopMusicTimer()
        {
            //MessageBox.Show("Stopping timer");
            timerMusic.Stop();
            if (music != null)
            {
                //MessageBox.Show("Stopping music");
                music.Stop();
            }
            //_soundEngine.StopAllSounds(); // stop all sounds
        }

        public void PlayRandomSound(string randomSoundType)
        {
            List<string> randomSoundsList;
            switch (randomSoundType)
            {
                case "ambient":
                    randomSoundsList = _ambientSounds;
                    break;
                case "music":
                    randomSoundsList = _musicSounds;
                    break;
                case "menumusic":
                    randomSoundsList = _menuMusicSounds;
                    break;
                case
                    "step":
                    randomSoundsList = _stepSounds;
                    break;
                default:
                    Console.WriteLine($"Sound type '{randomSoundType}' not recognized.");
                    MessageBox.Show("Sound type not recognized. Please make sure the sound type is correct.");
                    return;
            }

            // Select a random sound name from the list
            string randomSoundName = randomSoundsList[_random.Next(randomSoundsList.Count)];

            if (!_sounds.TryGetValue(randomSoundName.ToLower(), out ISoundSource soundSource))
            {
                Console.WriteLine($"Sound name '{randomSoundName}' not recognized.");
                MessageBox.Show("Sound name not recognized. Please make sure the sound name is correct.");
                return;
            }
            switch(randomSoundType)
            {
                case "ambient":
                    ambientSound = _soundEngine.Play2D(soundSource, false, false, false);
                    break;
                case "music":
                    music = _soundEngine.Play2D(soundSource, false, false, false);
                    break;
                case "menumusic":
                    music = _soundEngine.Play2D(soundSource, false, false, false);
                    break;
                case "step":
                    ISound sound = _soundEngine.Play2D(soundSource, false, false, false);
                    //PlaySound(randomSoundName);
                    break;
                default:
                    break;
            }
            //MessageBox.Show("Playing random sound" + randomSoundName);
        }

        public void PlaySound(string soundName)
        {
            if (!_sounds.TryGetValue(soundName.ToLower(), out ISoundSource soundSource))
            {
                Console.WriteLine($"Sound name '{soundName}' not recognized.");
                MessageBox.Show("Sound name not recognized. Please make sure the sound name is correct.");
                return;
            }

            ISound sound = _soundEngine.Play2D(soundSource, false, false, false);

            // for reverb enabled sounds:
            /*
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
                    //                                              1     2    3    4      5       6     7      8     9      10    11    12
                    //effectControl.EnableI3DL2ReverbSoundEffect(-1000, -100, 0f, 1.49f, 0.83f, -2602, 0.007f, 200, 0.011f, 100f, 100f, 5000f);
                    //
                    // 1-fRoomHF, 2-fRoomLF, 3-fDecayTime, 4-fDecayHFRatio, 5-fDecayLFRatio, 6-fReflections,
                    // 7-fReflectionsDelay, 8-fReverb, 9-fReverbDelay, 10-fDiffusion, 11-fDensity, 12-fHFReference
                    //                                           1     2    3    4      5       6      7    8     9    10    11    12
                    //effectControl.EnableI3DL2ReverbSoundEffect(-1000, -100, 0f, 1.49f, 0.83f, -2602, 1.0f, 100, 1.0f, 100f, 100f, 5000f);

                    // WavesReverb
                    //
                    //effectControl.EnableWavesReverbSoundEffect(0f, 0f, 1000f, 0.001f);
                    //
                    // 1-fInGain, 2-fReverbMix, 3-fReverbTime, 4-fHighFreqRTRatio
                    //                                           1    2   3     4
                    //effectControl.EnableWavesReverbSoundEffect(0f, 0f, 400f, 0.001f);
                }
            }
            */
            
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
            _soundEngine.StopAllSounds(); // stop all sounds
            _soundEngine.RemoveAllSoundSources(); // remove all sound sources
            _sounds.Clear(); // unload all sounds
            _soundEngine.Dispose(); // dispose of the sound engine
        }
    }
}
