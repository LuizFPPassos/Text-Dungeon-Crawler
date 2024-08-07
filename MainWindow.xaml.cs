﻿using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string map;
        private Program program = new Program();
        private char[,] mapMatrix;
        internal int playerX = 0;
        internal int playerY = 0;
        private int mapWidth;
        private int mapHeight;
        private List<char> forbiddenCharacters;
        private List<char> interactableCharacters;
        private List<string> systemConsole;
        private bool fogOfWar = true;
        bool gameStarted = false;
        private int stepCounter = 0;
        private int displayRadius;
        private int displayRadiusBase;
        private int displayRadiusBaseDefault = 8; // defines the radius around the player to display, ajust as needed
        private int torchRadius;
        private int torchRadiusDefault = 100; // default multiplier for torch radius in %
        private int torchDuration; 
        private int torchDurationDefault = 100; // default multiplier for torch duration in %
        private static readonly Regex _numericRegex = new Regex("[^0-9]+"); // Regex to match non-numeric characters
        private decimal torchRadiusMult;
        private decimal torchDurationMult;
        private int torchDurationBase;
        private int torchDurationBaseDefault = 50; // defines the base duration of a torch in steps, ajust as needed
        private int torchDurationSteps;
        private int torchAmountSetting;
        private int torchAmountSettingDefault = 10; // defines the amount of torches the player starts with, ajust as needed
        private int torchCount;

        public MainWindow()
        {
            InitializeComponent();
            forbiddenCharacters = new List<char> { 'X', 'T', 'E' };
            interactableCharacters = new List<char> { 'T', 'E' };
            systemConsole = new List<string>();

            // set the default values for torch radius, duration and amount
            torchRadius = torchRadiusDefault;
            displayRadiusBase = displayRadiusBaseDefault;
            torchDuration = torchDurationDefault;
            torchDurationBase = torchDurationBaseDefault;
            torchAmountSetting = torchAmountSettingDefault;
            torchCount = torchAmountSetting;

            TextBoxTorchRadius.Text = torchRadius.ToString();
            TextBoxTorchDuration.Text = torchDuration.ToString();
            TextBoxTorchAmount.Text = torchAmountSetting.ToString();

            ButtonUseTorch.IsEnabled = false;
            ButtonUseTorch.Opacity = 50;

            UpdateSystemConsole("Welcome!");
            UpdateSystemConsole("Press 'Generate map' then 'Play' to start the game.");
        }

        private void ButtonGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            UpdateSystemConsole("Generating map...");

            map = program.MainProgram();
            // remove all '.', ',', ';' and 'c' characters from the map
            map = map.Replace(".", " ");
            map = map.Replace(",", " ");
            map = map.Replace(";", " ");
            //map = map.Replace("Xc", "  "); // double width vertical corridors
            map = map.Replace("c", " ");

            TextBoxConsole.Text = map;

            // disable selecting the textbox
            TextBoxConsole.IsReadOnly = true;

            // set the opacity of ButtonPlay to 100 and enable it
            ButtonPlay.Opacity = 100;
            ButtonPlay.IsEnabled = true;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            UpdateSystemConsole("\n" + "Starting game..." + "\n");
            gameStarted = true;

            ButtonGenerateMap.IsEnabled = false;
            ButtonGenerateMap.Opacity = 50;
            ButtonPlay.IsEnabled = false;
            ButtonPlay.Opacity = 50;
            ButtonUseTorch.IsEnabled = true;
            ButtonUseTorch.Opacity = 100;

            TextBoxTorchRadius.IsEnabled = false;
            TextBoxTorchDuration.IsEnabled = false;
            TextBoxTorchAmount.IsEnabled = false;

            torchRadiusMult = (decimal)torchRadius / 100;
            torchDurationMult = (decimal)torchDuration / 100;

            // displayRadius = displayRadiusBase * torchRadiusMult, rounded up
            displayRadius = (int)Math.Ceiling(displayRadiusBase * torchRadiusMult);

            // torchDuration = torchDurationBase * torchDurationMult, rounded up
            torchDurationSteps = (int)Math.Ceiling(torchDurationBase * torchDurationMult);

            torchCount = torchAmountSetting;

            stepCounter = 0;

            mapMatrix = program.DungeonMap;

            RefreshMap();

            // gets the position of the 'S' from the mapMatrix

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (mapMatrix[i, j] == 'S')
                    {
                        playerX = j;
                        playerY = i;
                        break;
                    }
                }
            }

            // transforms 'S' into 'O' in the mapMatrix
            mapMatrix[playerY, playerX] = 'O';

            RefreshMap();

            // activates the keydown event
            TextBoxConsole.KeyDown += TextBoxConsole_KeyDown;

            // focuses on TextBoxConsole
            TextBoxConsole.Focus();
        }

        private void TextBoxConsole_KeyDown(object sender, KeyEventArgs e)
        {
            
            // if the user presses esc, quits the game
            if (e.Key == Key.Escape)
            {
                UpdateSystemConsole("Quitting game..." + "\n");

                TextBoxConsole.KeyDown -= TextBoxConsole_KeyDown;
                TextBoxConsole.Text = "Game over!";

                GameOver();
                return;
            }

            if (e.Key == Key.S)
            {
                MovePlayer(0, 1);
            }
            else if (e.Key == Key.W)
            {
                MovePlayer(0, -1);
            }
            else if (e.Key == Key.A)
            {
                MovePlayer(-1,0);
            }
            else if (e.Key == Key.D)
            {
                MovePlayer(1,0);
            }

            if (e.Key == Key.Enter)
            {
                Use();
            }

        }

        private void RefreshMap()
        {
            map = "";
            mapWidth = mapMatrix.GetLength(0);
            mapHeight = mapMatrix.GetLength(1);

            if (fogOfWar == true)
            {
                // Calculate player's position in mapMatrix
                int playerPosX = playerX;
                int playerPosY = playerY;

                // Loop through each position in the mapMatrix
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        // Calculate distance from player position
                        int dx = x - playerPosX;
                        int dy = y - playerPosY;
                        double distanceSquared = dx * dx + dy * dy;
                        int distance = (int)Math.Sqrt(distanceSquared);

                        // If within display radius or it's the player's current position, display character
                        if (distance <= displayRadius || (x == playerPosX && y == playerPosY))
                        {
                            map += mapMatrix[y, x];
                        }
                        else
                        {
                            map += ' '; // Replace characters outside the display radius with space
                        }
                    }
                    map += "\n"; // Add new line after each row
                }
            }
            else
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        // adds the character to the string
                        map += mapMatrix[i, j];
                    }
                    // adds a new line to the string
                    map += "\n";
                }
            }

            // remove all '.', ',', ';' and 'c' characters from the map
            map = map.Replace(".", " ");
            map = map.Replace(",", " ");
            map = map.Replace(";", " ");
            //map = map.Replace("Xc", "  "); // double width vertical corridors
            map = map.Replace("c", " ");

            TextBoxConsole.Text = map;
        }


        private void MovePlayer(int deltaX, int deltaY)
        {
            //MessageBox.Show($"Player position: {playerY},{playerX}");

            int newPlayerX = playerX + deltaX;
            int newPlayerY = playerY + deltaY;

            if (newPlayerX >= 0 && newPlayerX < mapWidth && newPlayerY >= 0 && newPlayerY < mapHeight)
            {
                // Check if the new position is valid (e.g., not 'X')
                if (!forbiddenCharacters.Contains(mapMatrix[newPlayerY, newPlayerX]))
                {
                    // Update player position
                    mapMatrix[playerY, playerX] = ' '; // Clear current position in mapMatrix
                    playerX = newPlayerX;
                    playerY = newPlayerY;
                    mapMatrix[playerY, playerX] = 'O'; // Set new position in mapMatrix
                    
                    RefreshMap(); // Update display

                    //MessageBox.Show($"Player position: {playerY},{playerX}");

                    if (deltaX == 0 && deltaY == 1)
                    {
                        // Moving south
                        UpdateSystemConsole("Moving south...");
                    }
                    else if (deltaX == 0 && deltaY == -1)
                    {
                        // Moving north
                        UpdateSystemConsole("Moving north...");
                    }
                    else if (deltaX == 1 && deltaY == 0)
                    {
                        // Moving east
                        UpdateSystemConsole("Moving east...");
                    }
                    else if (deltaX == -1 && deltaY == 0)
                    {
                        // Moving west
                        UpdateSystemConsole("Moving west...");
                    }

                    stepCounter++;
                    // decrease the display radius every 20 steps until it reaches 0
                    if (stepCounter % torchDurationSteps == 0 && displayRadius > 0)
                    {
                        displayRadius--;
                        UpdateSystemConsole($"Torch radius: {displayRadius}.");
                        if (displayRadius == 0)
                        {
                            UpdateSystemConsole("Torch has run out, use a torch from inventory to restore light.");
                        }
                    }

                }
                else
                {
                    // Handle collision with wall or obstacle
                }
            }
            else
            {
                // Handle movement outside map boundaries
            }
        }

        private void Use()
        {
            // check around the player for interactable objects

            //MessageBox.Show($"Player position: {playerY},{playerX}");
            UpdateSystemConsole("Using object...");

            // check all characters around the player
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //MessageBox.Show($"Checking position: {playerY + i},{playerX + j}");
                    // check if the character is an interactable object
                    if (interactableCharacters.Contains(mapMatrix[playerY + i, playerX + j]))
                    {
                        // interact with the object
                        if (mapMatrix[playerY + i, playerX + j] == 'T')
                        {
                            //MessageBox.Show($"Found teleporter on position: {playerY + i},{playerX + j}");
                            //MessageBox.Show($"Teleporting! {playerY},{playerX}");

                            UpdateSystemConsole("Interacting with teleporter...");

                            // interact with the teleporter
                            // teleport the player next to a random teleporter 'T' on the map (asside from the current one)
                            Random random = new Random();
                            int randomX = 0;
                            int randomY = 0;
                            while (mapMatrix[randomY, randomX] != 'T')
                            {
                                randomX = random.Next(0, mapWidth);
                                randomY = random.Next(0, mapHeight);
                            }
                            // remove the 'O' from the previous position
                            mapMatrix[playerY, playerX] = ' ';

                            //MessageBox.Show($"Teleporting to position: {randomY},{randomX}");

                            playerX = randomX;
                            playerY = randomY;

                            for (int k = -1; k <= 1; k++)
                            {
                                for (int l = -1; l <= 1; l++)
                                {
                                    //MessageBox.Show($"Checking position: {playerY + k},{playerX + l}");
                                    if (!forbiddenCharacters.Contains(mapMatrix[playerY + k, playerX + l]))
                                    {
                                        //MessageBox.Show($"Found empty position: {playerY + k},{playerX + l}");
                                        playerX = playerX + l;
                                        playerY = playerY + k;
                                        //MessageBox.Show($"Player position: {playerY},{playerX}");

                                        UpdateSystemConsole($"Teleporting to position: {randomY},{randomX}");

                                        mapMatrix[playerY, playerX] = 'O';
                                        RefreshMap();
                                        return;
                                    }
                                }
                            }

                        }
                        else if (mapMatrix[playerY + i, playerX + j] == 'E')
                        {
                            // interact with the exit
                            TextBoxConsole.Text = "You have reached the exit!";

                            UpdateSystemConsole("You have reached the exit!" + "\n");

                            GameOver();
                            return;

                        }

                    }
                }
            }

        }

        private void CheckBoxFogOfWar_Click(object sender, RoutedEventArgs e)
        {
            if (fogOfWar == true)
            {
                fogOfWar = false;
                //MessageBox.Show("Fog of war disabled.");
                UpdateSystemConsole("Fog of war disabled.");
            }
            else
            {
                fogOfWar = true;
                //MessageBox.Show("Fog of war enabled.");
                UpdateSystemConsole("Fog of war enabled.");
            }

            if (gameStarted == true)
            {
                RefreshMap();
            }

            TextBoxConsole.Focus();
        }

        private void TextBoxTorchRadius_GotFocus(object sender, RoutedEventArgs e)
        {
            // clear the text box when it gets focus
            TextBoxTorchRadius.Text = "";
        }

        private void TextBoxTorchDuration_GotFocus(object sender, RoutedEventArgs e)
        {
            // clear the text box when it gets focus
            TextBoxTorchDuration.Text = "";
        }

        private void TextBoxTorchAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            // clear the text box when it gets focus
            TextBoxTorchAmount.Text = "";
        }

        private void TextBoxTorchRadius_LostFocus(object sender, RoutedEventArgs e)
        {
            // only accept values between 0 and 999
            if (int.TryParse(TextBoxTorchRadius.Text, out int result))
            {
                if (result >= 0 && result <= 999)
                {
                    torchRadius = result;
                    UpdateSystemConsole($"Torch radius set to: {torchRadius}%.");
                }
                else
                {
                    torchRadius = torchRadiusDefault;
                    TextBoxTorchRadius.Text = torchRadius.ToString();
                    UpdateSystemConsole($"Torch radius set to: {torchRadius}%.");
                }
            }
            else
            {
                torchRadius = torchRadiusDefault;
                TextBoxTorchRadius.Text = torchRadius.ToString();
                UpdateSystemConsole($"Torch radius set to: {torchRadius}%.");
            }
        }

        private void TextBoxTorchDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            // only accept values between 1 and 999
            if (int.TryParse(TextBoxTorchDuration.Text, out int result))
            {
                if (result > 0 && result <= 999)
                {
                    torchDuration = result;
                    UpdateSystemConsole($"Torch duration set to: {torchDuration}%.");
                }
                else
                {
                    torchDuration = torchDurationDefault;
                    TextBoxTorchDuration.Text = torchDuration.ToString();
                    UpdateSystemConsole($"Torch duration set to: {torchDuration}%.");
                }
            }
            else
            {
                torchDuration = torchDurationDefault;
                TextBoxTorchDuration.Text = torchDuration.ToString();
                UpdateSystemConsole($"Torch duration set to: {torchDuration}%.");
            }
        }

        private void TextBoxTorchAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            // only accept values between 0 and 999
            if (int.TryParse(TextBoxTorchAmount.Text, out int result))
            {
                if (result >= 0 && result <= 999)
                {
                    torchAmountSetting = result;
                    UpdateSystemConsole($"Torch amount set to: {torchAmountSetting}.");
                }
                else
                {
                    torchAmountSetting = torchAmountSettingDefault;
                    TextBoxTorchAmount.Text = torchAmountSetting.ToString();
                    UpdateSystemConsole($"Torch amount set to: {torchAmountSetting}.");
                }
            }
            else
            {
                torchAmountSetting = torchAmountSettingDefault;
                TextBoxTorchAmount.Text = torchAmountSetting.ToString();
                UpdateSystemConsole($"Torch amount set to: {torchAmountSetting}.");
            }
        }

        private void TextBoxTorchRadius_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _numericRegex.IsMatch(e.Text); // Set e.Handled to true if input is non-numeric
        }

        private void TextBoxTorchDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _numericRegex.IsMatch(e.Text); // Set e.Handled to true if input is non-numeric
        }

        private void TextBoxTorchAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _numericRegex.IsMatch(e.Text); // Set e.Handled to true if input is non-numeric
        }

        private void GameOver()
        {
            UpdateSystemConsole("Game over!");

            ButtonGenerateMap.IsEnabled = true;
            ButtonGenerateMap.Opacity = 100;
            ButtonPlay.IsEnabled = false;
            ButtonPlay.Opacity = 50;

            gameStarted = false;

            TextBoxTorchRadius.IsEnabled = true;
            TextBoxTorchDuration.IsEnabled = true;
            TextBoxTorchAmount.IsEnabled = true;
            torchAmountSetting = torchAmountSettingDefault;
            torchCount = torchAmountSetting;
            TextBoxTorchAmount.Text = torchCount.ToString();

            ButtonUseTorch.IsEnabled = false;
            ButtonUseTorch.Opacity = 50;
        }

        private void UpdateSystemConsole(string message)
        {
            systemConsole.Add(message);
            TextBoxSystemConsole.Text = string.Join("\n", systemConsole);
            TextBoxSystemConsole.ScrollToEnd(); // Scroll to bottom
        }

        private void ButtonUseTorch_Click(object sender, RoutedEventArgs e)
        {
            if (torchCount == 0)
            {
                UpdateSystemConsole("No torches left in inventory.");
                TextBoxConsole.Focus();
                return;
            }
            else
            {
                torchCount--;
                TextBoxTorchAmount.Text = torchCount.ToString();

                UpdateSystemConsole("Torch used.");
                UpdateSystemConsole($"Torch amount left: {torchCount}.");
                displayRadius = (int)Math.Ceiling(displayRadiusBase * torchRadiusMult);
                UpdateSystemConsole($"Torch radius: {displayRadius}.");
                RefreshMap();
                TextBoxConsole.Focus();
            }
        }

    }
}