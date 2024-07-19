namespace WpfApp1
{
    internal class Corridor
    {
        public int width;
        public int height;
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public Corridor(int width, int height, int x1, int y1, int x2, int y2)
        {
            this.width = width;
            this.height = height;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public void GenerateCorridor(Room room1, Room room2, char direction)
        {
            if (direction == 'n')
            {
                if (room1.x2 == room2.x1)
                {
                    this.x1 = room1.x2;
                    this.x2 = this.x1;
                }
                else if (room1.x1 == room2.x2)
                {
                    this.x1 = room1.x1;
                    this.x2 = this.x1;
                }
                else
                {
                    // Gets the average x of the first room
                    decimal averageXDecimal = (decimal)(room1.x1 + room1.x2) / 2;
                    // Gets the average x of the nearest room
                    decimal nearestAverageXDecimal = (decimal)(room2.x1 + room2.x2) / 2;

                    decimal xDecimal = (averageXDecimal + nearestAverageXDecimal) / 2;

                    // Defines x as the average between the averageX and nearestAverageX
                    // if not a whole number, round up
                    this.x1 = (int)Math.Ceiling(xDecimal);
                    this.x2 = this.x1;

                    // if x is greater than x2 of room1, set x to x2 of room1
                    if (this.x1 > room1.x2)
                    {
                        this.x1 = room1.x2;
                        this.x2 = this.x1;
                    }
                    // if x is less than x1 of room1, set x to x1 of room1
                    else if (this.x1 < room1.x1)
                    {
                        this.x1 = room1.x1;
                        this.x2 = this.x1;
                    }
                    else if (this.x1 > room2.x2)
                    {
                        this.x1 = room2.x2;
                        this.x2 = this.x1;
                    }
                    else if (this.x1 < room2.x1)
                    {
                        this.x1 = room2.x1;
                        this.x2 = this.x1;
                    }

                }

                this.y1 = room2.y2 + 1;
                this.y2 = room1.y1 - 1;
            }
            else if (direction == 's')
            {
                if(room1.x2 == room2.x1)
                {
                    this.x1 = room1.x2;
                    this.x2 = this.x1;
                }
                else if(room1.x1 == room2.x2)
                {
                    this.x1 = room1.x1;
                    this.x2 = this.x1;
                }
                else
                {
                    // Gets the average x of the first room
                    decimal averageXDecimal = (decimal)(room1.x1 + room1.x2) / 2;
                    // Gets the average x of the nearest room
                    decimal nearestAverageXDecimal = (decimal)(room2.x1 + room2.x2) / 2;

                    decimal xDecimal = (averageXDecimal + nearestAverageXDecimal) / 2;

                    // Defines x as the average between the averageX and nearestAverageX
                    // if not a whole number, round up
                    this.x1 = (int)Math.Ceiling(xDecimal);
                    this.x2 = this.x1;

                    // if x is greater than x2 of room1, set x to x2 of room1
                    if (this.x1 > room1.x2)
                    {
                        this.x1 = room1.x2;
                        this.x2 = this.x1;
                    }
                    // if x is less than x1 of room1, set x to x1 of room1
                    else if (this.x1 < room1.x1)
                    {
                        this.x1 = room1.x1;
                        this.x2 = this.x1;
                    }
                    else if (this.x1 > room2.x2)
                    {
                        this.x1 = room2.x2;
                        this.x2 = this.x1;
                    }
                    else if (this.x1 < room2.x1)
                    {
                        this.x1 = room2.x1;
                        this.x2 = this.x1;
                    }
                }

                this.y1 = room1.y2 + 1;
                this.y2 = room2.y1 - 1;
            }
            else if (direction == 'w')
            {
                if (room1.y2 == room2.y1)
                {
                    this.y1 = room1.y2;
                    this.y2 = this.y1;
                }
                else if (room1.y1 == room2.y2)
                {
                    this.y1 = room1.y1;
                    this.y2 = this.y1;
                }
                else
                {
                    // Gets the average y of the first room
                    decimal averageYDecimal = (decimal)(room1.y1 + room1.y2) / 2;
                    // Gets the average y of the nearest room
                    decimal nearestAverageYDecimal = (decimal)(room2.y1 + room2.y2) / 2;

                    decimal yDecimal = (averageYDecimal + nearestAverageYDecimal) / 2;

                    // Defines y as the average between the averageY and nearestAverageY
                    // if not a whole number, round up
                    this.y1 = (int)Math.Ceiling(yDecimal);
                    this.y2 = this.y1;

                    // if y is greater than y2 of room1, set y to y2 of room1
                    if (this.y1 > room1.y2)
                    {
                        this.y1 = room1.y2;
                        this.y2 = this.y1;
                    }
                    // if y is less than y1 of room1, set y to y1 of room1
                    else if (this.y1 < room1.y1)
                    {
                        this.y1 = room1.y1;
                        this.y2 = this.y1;
                    }
                    else if (this.y1 > room2.y2)
                    {
                        this.y1 = room2.y2;
                        this.y2 = this.y1;
                    }
                    else if (this.y1 < room2.y1)
                    {
                        this.y1 = room2.y1;
                        this.y2 = this.y1;
                    }
                }

                this.x1 = room2.x2 + 1;
                this.x2 = room1.x1 - 1;
            }
            else if (direction == 'e')
            {
                if (room1.y2 == room2.y1)
                {
                    this.y1 = room1.y2;
                    this.y2 = this.y1;
                }
                else if (room1.y1 == room2.y2)
                {
                    this.y1 = room1.y1;
                    this.y2 = this.y1;
                }
                else
                {
                    // Gets the average y of the first room
                    decimal averageYDecimal = (decimal)(room1.y1 + room1.y2) / 2;
                    // Gets the average y of the nearest room
                    decimal nearestAverageYDecimal = (decimal)(room2.y1 + room2.y2) / 2;

                    decimal yDecimal = (averageYDecimal + nearestAverageYDecimal) / 2;

                    // Defines y as the average between the averageY and nearestAverageY
                    // if not a whole number, round up
                    this.y1 = (int)Math.Ceiling(yDecimal);
                    this.y2 = this.y1;

                    // if y is greater than y2 of room1, set y to y2 of room1
                    if (this.y1 > room1.y2)
                    {
                        this.y1 = room1.y2;
                        this.y2 = this.y1;
                    }
                    // if y is less than y1 of room1, set y to y1 of room1
                    else if (this.y1 < room1.y1)
                    {
                        this.y1 = room1.y1;
                        this.y2 = this.y1;
                    }
                    else if (this.y1 > room2.y2)
                    {
                        this.y1 = room2.y2;
                        this.y2 = this.y1;
                    }
                    else if (this.y1 < room2.y1)
                    {
                        this.y1 = room2.y1;
                        this.y2 = this.y1;
                    }
                }

                this.x1 = room1.x2 + 1;
                this.x2 = room2.x1 - 1;
            }
            this.SetCorridorWidthHeight();
        }

        // Sets the corridor width and height
        public void SetCorridorWidthHeight()
        {
            if (this.x1 == this.x2)
            {
                this.width = 1;
                this.height = Math.Abs(this.y2 - this.y1) + 1;
            }
            else if (this.y1 == this.y2)
            {
                this.width = Math.Abs(this.x2 - this.x1) + 1;
                this.height = 1;
            }
        }
    }
}
