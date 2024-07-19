namespace WpfApp1
{
    internal class Program
    {
        // Matrix for a dungeon map grid with mapWidth x mapHeight cells
        public char[,] DungeonMap{get; set;}

        public string MainProgram()
        {
            int mapWidth = 128;
            int mapHeight = 128;
            DungeonMap = new char[mapWidth, mapHeight];

            // Fill the map with 'X' for walls
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    DungeonMap[i, j] = 'X';
                }
            }

            /*
            // Fill the map with the y coordinate of each cell, repeating from 0 to 9
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    dungeonMap[i, j] = (char)(j % 10 + 48);
                }
            }
            */


            /*
            
            // Randomly generate an entry point 'S' on the first row and an exit point 'E' on the last row
            Random random = new Random();
            int entryPoint = random.Next(3, mapWidth - 3);
            int exitPoint = random.Next(3, mapHeight - 3);
            dungeonMap[0, entryPoint] = 'S';
            dungeonMap[mapHeight - 1, exitPoint] = 'E';

            // Generate the starting room, that must be 5x5 cells and be connected to the entryPoint
            Room startingRoom = new Room(5, 5, 0, 0, 0, 0);

            startingRoom.x1 = entryPoint - 2;
            startingRoom.y1 = 1;
            startingRoom.x2 = entryPoint + 2;
            startingRoom.y2 = 5;

            Console.WriteLine("Starting room: " + startingRoom.x1 + "," + startingRoom.y1 + " " + startingRoom.x2 + "," + startingRoom.y2);
            Console.WriteLine();

            // Fill the starting room with '.'
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    dungeonMap[i, entryPoint - 2 + j] = '.';
                }
            }

            */

            // Initialize a list of rooms
            List<Room> rooms = new List<Room>();

            //rooms.Add(startingRoom);

            // Generate rooms ranging from minSize x minSize to maxSize x maxSize cells, rooms cannot overlap with the borders of the map
            int numRooms = 40;
            //int minSize = 8;
            //int maxSize = 15;
            int minWidth = 8;
            int maxWidth = 16;
            int minHeight = 10;
            int maxHeight = 20;
            int spacing = 3;

            //startingRoom.GenerateRooms(numRooms, minSize, maxSize, spacing, mapWidth, mapHeight, rooms);

            Room startingRoom = new Room(5, 5, 0, 0, 0, 0);
            Room exitRoom = new Room(5, 5, 0, 0, 0, 0);
            startingRoom.GenerateRooms(numRooms, minWidth, minHeight, maxWidth, maxHeight, spacing, mapWidth, mapHeight, rooms);

            // select a random room as the starting room
            Console.WriteLine();
            Console.WriteLine("Selecting a random room as the starting room...");
            Random random = new Random();
            startingRoom = rooms[random.Next(0, rooms.Count)];
            Console.Write($"Starting room: Room {rooms.IndexOf(startingRoom) + 1} {startingRoom.width}x{startingRoom.height}");
            Console.WriteLine(" " + startingRoom.x1 + "," + startingRoom.y1 + " " + startingRoom.x2 + "," + startingRoom.y2);
            Console.WriteLine();

            // Fill the rooms with '.'
            foreach (Room room in rooms)
            {
                for (int i = room.y1; i <= room.y2; i++)
                {
                    for (int j = room.x1; j <= room.x2; j++)
                    {
                        DungeonMap[i, j] = '.';
                    }
                }
            }

            // fill the starting room with ','
            for (int i = startingRoom.y1; i <= startingRoom.y2; i++)
            {
                for (int j = startingRoom.x1; j <= startingRoom.x2; j++)
                {
                    DungeonMap[i, j] = ',';
                }
            }

            // Initialize a list of rooms connected to another room
            List<Room> roomsConnected = new List<Room>();

            // Initialize a list of corridors
            List<Corridor> corridors = new List<Corridor>();

            // Find the nearest room to the starting room
            Room nearestRoom = startingRoom.NearestRoom(rooms, roomsConnected, corridors, DungeonMap);
            if(nearestRoom == null)
            {
                Console.WriteLine("No nearest room found");
                Console.ReadLine();
                return null;
            }

            Room room1 = new Room(5, 5, 0, 0, 0, 0);
            room1 = nearestRoom;

            roomsConnected.Add(startingRoom);

            for (int i = 0; i < (numRooms - 1) ; i++)
            {
                roomsConnected.Add(room1);

                // Find the next nearest room
                Room nextRoom = room1.NearestRoom(rooms, roomsConnected, corridors, DungeonMap);
                if (nextRoom == null)
                {
                    Console.WriteLine("No next nearest room found");
                    Console.ReadLine();

                    Console.WriteLine("Generating exit room...");
                    // set that room as the exit room
                    exitRoom = room1;
                    Console.Write($"Exit room: Room {rooms.IndexOf(exitRoom) + 1} {exitRoom.width}x{exitRoom.height}");
                    Console.WriteLine(" " + exitRoom.x1 + "," + exitRoom.y1 + " " + exitRoom.x2 + "," + exitRoom.y2);
                    // fill the exit room with ';'
                    for (int j = exitRoom.y1; j <= exitRoom.y2; j++)
                    {
                        for (int k = exitRoom.x1; k <= exitRoom.x2; k++)
                        {
                            DungeonMap[j, k] = ';';
                        }
                    }
                    Console.ReadLine();

                    break;
                }

                room1 = nextRoom;
            }


            // Generate extra corridors between the two random rooms
            for (int i = 0; i < 0; i++)
            {
                Room randomRoom1 = rooms[random.Next(0, rooms.Count)];
                Room randomRoom2 = rooms[random.Next(0, rooms.Count)];

                char randomDirection = randomRoom1.Direction(randomRoom2);

                Corridor corridor = new Corridor(0, 0, 0, 0, 0, 0);
                corridor.GenerateCorridor(randomRoom1, randomRoom2, randomDirection);

                // Aborts the corridor generation if the corridor is too long
                if (corridor.width > 15 || corridor.height > 15)
                {
                    Console.WriteLine("Corridor too long, aborting...");
                    i = i - 1;
                    continue;
                }

                /*
                if (corridor.x1 != corridor.x2)
                {
                    if ((corridor.x2 - corridor.x1) > 10)
                    {
                        break;
                    }
                }
                else if (corridor.y1 != corridor.y2)
                {
                    if ((corridor.y2 - corridor.y1) > 10)
                    {
                        break;
                    }
                }
                */

                Console.WriteLine("Corridor generated");
                corridors.Add(corridor);
            }

            // Fill the corridors with 'c'
            foreach (Corridor c in corridors)
            {
                for (int i = c.y1; i <= c.y2; i++)
                {
                    for (int j = c.x1; j <= c.x2; j++)
                    {
                        DungeonMap[i, j] = 'c';
                    }
                }
            }

            // generates a corridor between the exit room and the nearest room
            exitRoom.NearestRoomToExit(roomsConnected, corridors, DungeonMap);

            string map = null;

            /*
            // Fill unconnected rooms with 'X'
            foreach (Room room in rooms)
            {
                if (!roomsConnected.Contains(room))
                {
                    for (int i = room.y1; i <= room.y2; i++)
                    {
                        for (int j = room.x1; j <= room.x2; j++)
                        {
                            dungeonMap[i, j] = 'X';
                        }
                    }
                }
            }
            */

            // If there's an unconnected room, select a random tile and replace it with a 'T', and replace a random tile of a random connected room with a 'T'. Repeat for every unconnected room found.
            for (int i = 0; i < rooms.Count; i++)
            {
                if (!roomsConnected.Contains(rooms[i]))
                {
                    int randomTileX = random.Next(rooms[i].x1, rooms[i].x2);
                    int randomTileY = random.Next(rooms[i].y1, rooms[i].y2);
                    DungeonMap[randomTileY, randomTileX] = 'T';

                    int randomConnectedRoomIndex = random.Next(0, roomsConnected.Count);
                    int randomConnectedTileX = random.Next(roomsConnected[randomConnectedRoomIndex].x1, roomsConnected[randomConnectedRoomIndex].x2);
                    int randomConnectedTileY = random.Next(roomsConnected[randomConnectedRoomIndex].y1, roomsConnected[randomConnectedRoomIndex].y2);
                    DungeonMap[randomConnectedTileY, randomConnectedTileX] = 'T';
                }
            }

            // for only one teleporter:
            /*
            if (rooms.Count != roomsConnected.Count)
            {
                Room unconnectedRoom = new Room(5, 5, 0, 0, 0, 0);
                foreach (Room room in rooms)
                {
                    if (!roomsConnected.Contains(room))
                    {
                        unconnectedRoom = room;
                        break;
                    }
                }

                Room connectedRoom = roomsConnected[random.Next(0, roomsConnected.Count)];

                int randomTileX = random.Next(unconnectedRoom.x1, unconnectedRoom.x2);
                int randomTileY = random.Next(unconnectedRoom.y1, unconnectedRoom.y2);

                dungeonMap[randomTileY, randomTileX] = 'T';

                randomTileX = random.Next(connectedRoom.x1, connectedRoom.x2);
                randomTileY = random.Next(connectedRoom.y1, connectedRoom.y2);

                dungeonMap[randomTileY, randomTileX] = 'T';
            }
            */


            // replace a random tile of the starting room with 'S'
            int randomSTileX = random.Next(startingRoom.x1, startingRoom.x2);
            int randomSTileY = random.Next(startingRoom.y1, startingRoom.y2);
            DungeonMap[randomSTileY, randomSTileX] = 'S';

            // replace a random tile of the exit room with 'E'
            int randomETileX = random.Next(exitRoom.x1, exitRoom.x2);
            int randomETileY = random.Next(exitRoom.y1, exitRoom.y2);
            DungeonMap[randomETileY, randomETileX] = 'E';

            // Print the map
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    Console.Write(DungeonMap[i, j]);
                    // adds the character to the string
                    map += DungeonMap[i, j];

                }
                // adds a new line to the string
                map += "\n";
                Console.WriteLine();
            }


            return map;
        }

    }
}
