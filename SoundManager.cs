﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using NAudio.Wave;

namespace Text_Dungeon_Crawler
{
    internal class SoundManager
    {
        // Dictionary to map sound names to file paths
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

        // List of sound names to choose from
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

        // Master volume setting (0.0 to 1.0)
        public float MasterVolume { get; set; } = 1.0f;

        // Method to play a random step sound
        public void PlayStepSound(float volume = 1.0f)
        {
            // Select a random sound name from the list
            string randomSoundName = _stepSounds[_random.Next(_stepSounds.Count)];

            // Play the selected sound with volume control
            PlaySound(randomSoundName, volume);
        }

        // Method to play a specific sound
        public void PlaySound(string soundName, float volume = 1.0f)
        {
            if (!_soundPaths.TryGetValue(soundName.ToLower(), out string relativePath))
            {
                Console.WriteLine($"Sound name '{soundName}' not recognized.");
                MessageBox.Show("Sound name not recognized. Please make sure the sound name is correct.");
                return;
            }

            // Construct the full path to the sound file
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            try
            {
                // Create a new AudioFileReader and WaveOutEvent instance
                using (var audioFile = new AudioFileReader(fullPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);

                    // Apply master volume and individual volume
                    outputDevice.Volume = MasterVolume * volume; // Volume range is 0.0 to 1.0
                    outputDevice.Play();

                    // Wait for playback to finish
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                // Handle the case where the file is not found
                Console.WriteLine($"Sound file not found: {ex.Message}");
                MessageBox.Show("Sound file not found. Please make sure the sound file is in the correct location.");
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine($"An error occurred while playing the sound: {ex.Message}");
                MessageBox.Show("An error occurred while playing the sound.");
            }
        }
    }
}
