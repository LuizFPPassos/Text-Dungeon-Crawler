namespace WpfApp1
{
    internal class Room
    {
        public int width;
        public int height;
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public Room(int width, int height, int x1, int y1, int x2, int y2)
        {
            this.width = width;
            this.height = height;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public void GenerateRooms(int numRooms, int minWidth, int minHeight, int maxWidth, int maxHeight, int spacing, int mapWidth, int mapHeight, List<Room> rooms)
        {
            Console.WriteLine("Generating rooms...");
            Random random = new Random();
            for (int i = 0; i < numRooms; i++)
            {
                int roomWidth = random.Next(minWidth, maxWidth + 1);
                int roomHeight = random.Next(minHeight, maxHeight + 1);

                int x1 = random.Next(1, mapWidth - roomWidth - 1);
                int y1 = random.Next(1, mapHeight - roomHeight - 1);
                int x2 = x1 + (roomWidth - 1);
                int y2 = y1 + (roomHeight - 1);

                Room newRoom = new Room(roomWidth, roomHeight, x1, y1, x2, y2);
                bool failed = false;

                foreach (Room room in rooms)
                {
                    if (newRoom.Intersect(room, spacing))
                    {
                        failed = true;
                        i = i - 1;
                        break;
                    }
                }

                if (!failed)
                {
                    rooms.Add(newRoom);
                    Console.Write("Room " + (i + 1) + ": " + roomWidth + "x" + roomHeight);
                    Console.WriteLine(" " + x1 + "," + y1 + " " + x2 + "," + y2);
                    
                }
            }
        }

        // Checks if a room is at least num tiles away from another room
        public bool Intersect(Room otherRoom, int spacing)
        {
            return (this.x1 <= otherRoom.x2 + spacing && this.x2 >= otherRoom.x1 - spacing && this.y1 <= otherRoom.y2 + spacing && this.y2 >= otherRoom.y1 - spacing);
        }

        // Search for the nearest room from room1 in the list of rooms
        public Room NearestRoom(List<Room> rooms, List<Room> roomsConnected, List<Corridor> corridors, char[,] dungeonMap) // add corridor generation and check if it intersects, if it does, delete corridor and skip to next nearest room
        {
            Console.WriteLine("Generating nearest room...");
            Room nearestRoom = null;
            Corridor nextCorridor = new Corridor(1, 1, 0, 0, 0, 0);
            int minDistance = 1000;
            bool canConnect = true;

            foreach (Room room in rooms)
            {
                //Console.WriteLine("Room index: " + rooms.IndexOf(room));
                if (room != this && roomsConnected.Contains(room) == false)
                {
                    // check if the nearestRoom can have a straight line path to the current room
                    if (room.x2 < this.x1 || room.x1 > this.x2)
                    {
                        if (room.y2 < this.y1)
                        {
                            canConnect = false;
                        }
                        else if (room.y1 > this.y2)
                        {
                            canConnect = false;
                        }
                        else
                        {
                            canConnect = true;
                        }
                    }
                    else
                    {
                        canConnect = true;
                    }
                    //Console.WriteLine("Can connect: " + canConnect);

                    if (canConnect == true)
                    {
                        int distance = Math.Abs(room.x1 - this.x1) + Math.Abs(room.y1 - this.y1);
                        //Console.WriteLine("Distance: " + distance);
                        if (distance < minDistance)
                        {
                            minDistance = distance;

                            if (room == null)
                            {
                                Console.WriteLine("No nearest room found");
                                Console.ReadLine();
                                continue;
                            }
                            else if (roomsConnected.Contains(room))
                            {
                                Console.WriteLine("Room already connected");
                                Console.ReadLine();
                                continue;
                            }
                            else
                            {
                                char nextDirection = Direction(room);

                                // Connect the next room to the nearest room with a corridor

                                Console.WriteLine("Generating corridor...");
                                nextCorridor.GenerateCorridor(this, room, nextDirection);
                                corridors.Add(nextCorridor);

                                for (int i = nextCorridor.y1; i <= nextCorridor.y2; i++)
                                {
                                    for (int j = nextCorridor.x1; j <= nextCorridor.x2; j++)
                                    {
                                        if (dungeonMap[i, j] == '.' || dungeonMap[i, j] == ',' || dungeonMap[i, j] == ';')
                                        {
                                            Console.WriteLine("Corridor intersects room");
                                            // remove corridor from the list
                                            corridors.Remove(nextCorridor);

                                            continue;
                                        }
                                    }
                                }

                            }
                            nearestRoom = room;
                        }
                    }
                }
            }
            return nearestRoom;
        }

        // Determine the direction of the nearest room (n, s, e, w)
        public char Direction(Room nearestRoom)
        {
            char direction = ' ';
            if (nearestRoom.x2 < this.x1)
            {
                direction = 'w';
            }
            else if (nearestRoom.x1 > this.x2)
            {
                direction = 'e';
            }
            else if (nearestRoom.y2 < this.y1)
            {
                direction = 'n';
            }
            else if (nearestRoom.y1 > this.y2)
            {
                direction = 's';
            }
            //Console.WriteLine("Direction: " + direction);
            return direction;
        }

        // Search for the nearest room from room1 in the list of rooms
        public void NearestRoomToExit(List<Room> roomsConnected, List<Corridor> corridors, char[,] dungeonMap) // add corridor generation and check if it intersects, if it does, delete corridor and skip to next nearest room
        {
            Corridor nextCorridor = new Corridor(1, 1, 0, 0, 0, 0);
            int minDistance = 1000;
            bool canConnect = true;
            bool alreadyConnected = false;

            // check if the outer edges of the exit room have a 'c' character adjacent to it

            
            for (int i = this.x1; i <= this.x2; i++)
            {
                // checking upper edge
                Console.WriteLine($"Checking position: {i},{this.y1 - 1}");
                Console.WriteLine($"Character: {dungeonMap[i, this.y1 - 1]}");
                if (dungeonMap[i, this.y1 - 1] == 'c')
                {
                    alreadyConnected = true;
                    break;
                }

                // checking lower edge
                Console.WriteLine($"Checking position: {i},{this.y2 + 1}");
                Console.WriteLine($"Character: {dungeonMap[i, this.y2 + 1]}");
                if (dungeonMap[i, this.y2 + 1] == 'c')
                {
                    alreadyConnected = true;
                    break;
                }
            }

            
            for (int i = this.y1; i <= this.y2; i++)
            {
                // checking left edge
                Console.WriteLine($"Checking position: {this.x1 - 1},{i}");
                Console.WriteLine($"Character: {dungeonMap[this.x1 - 1, i]}");
                if (dungeonMap[this.x1-1, i] == 'c')
                {
                    alreadyConnected = true;
                    break;
                }

                // checking right edge
                Console.WriteLine($"Checking position: {this.x2 + 1},{i}");
                Console.WriteLine($"Character: {dungeonMap[this.x2 - 1, i]}");
                if (dungeonMap[this.x2 - 1, i] == 'c')
                {
                    alreadyConnected = true;
                    break;
                }
            }

            if (alreadyConnected == true)
            {
                Console.WriteLine("Exit room already connected");
                return;
            }
            else
            {
                Console.WriteLine("Exit room is not connected, calculating nearest room...");
                foreach (Room room in roomsConnected)
                {
                    //Console.WriteLine("Room index: " + rooms.IndexOf(room));
                    if (room != this)
                    {
                        // check if the nearestRoom can have a straight line path to the current room
                        if (room.x2 < this.x1 || room.x1 > this.x2)
                        {
                            if (room.y2 < this.y1)
                            {
                                canConnect = false;
                            }
                            else if (room.y1 > this.y2)
                            {
                                canConnect = false;
                            }
                            else
                            {
                                canConnect = true;
                            }
                        }
                        else
                        {
                            canConnect = true;
                        }
                        //Console.WriteLine("Can connect: " + canConnect);

                        if (canConnect == true)
                        {
                            int distance = Math.Abs(room.x1 - this.x1) + Math.Abs(room.y1 - this.y1);
                            //Console.WriteLine("Distance: " + distance);
                            if (distance < minDistance)
                            {
                                minDistance = distance;

                                if (room == null)
                                {
                                    Console.WriteLine("No nearest room found");
                                    Console.ReadLine();
                                    continue;
                                }
                                else
                                {
                                    char nextDirection = Direction(room);

                                    // Connect the next room to the nearest room with a corridor

                                    Console.WriteLine("Generating corridor...");
                                    nextCorridor.GenerateCorridor(this, room, nextDirection);
                                    corridors.Add(nextCorridor);

                                    for (int i = nextCorridor.y1; i <= nextCorridor.y2; i++)
                                    {
                                        for (int j = nextCorridor.x1; j <= nextCorridor.x2; j++)
                                        {
                                            if (dungeonMap[i, j] == '.' || dungeonMap[i, j] == ',' || dungeonMap[i, j] == ';')
                                            {
                                                Console.WriteLine("Corridor intersects room");
                                                // remove corridor from the list
                                                corridors.Remove(nextCorridor);

                                                continue;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Corridor does not intersect room, generating...");
                                                return;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            
        }


    }
}
