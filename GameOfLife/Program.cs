
namespace GameOfLife
{
    class Program
    {
        private const int GENERATION_DELAY = 25; // milliseconds
        private const int WIDTH = 140;
        private const int HEIGHT = 55;
        private const double INITIAL_LIVE_PROBABILITY = 0.5; // Probability of a cell being alive at start
        private static readonly bool[,] grid = new bool[HEIGHT, WIDTH];
        private static readonly bool[,] nextGrid = new bool[HEIGHT, WIDTH];
        private static readonly List<string> gridHistory = [];
        private static int loopStart = -1;
        private static int loopLength = -1;

        static void Main()
        {
            Console.WriteLine("Conway's Game of Life");
            // Console.WriteLine("Press any key to start, ESC to exit during simulation");
            //            Console.ReadKey();

            // Initialize with a simple pattern (Glider)
            InitializeGrid();

            Console.Clear();
            Console.CursorVisible = false;

            while (true)
            {
                DisplayGrid();

                string currentGridState = GridToString();
                bool loopDetected = DetectLoop(currentGridState);

                if (loopDetected)
                {
                    DisplayLoopInfo();
                    Console.WriteLine("Loop detected! Press any key to continue or ESC to exit.");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }

                UpdateGrid();
                Thread.Sleep(GENERATION_DELAY); // Delay between generations

                // Check for ESC key to exit
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }
            }

            Console.CursorVisible = true;
            // Console.WriteLine("\nGame ended. Press any key to exit.");
            //            Console.ReadKey();
        }

        static void InitializeGrid()
        {
            Random rand = new();

            // Set new random cells
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    //grid[i, j] = false;

                    if (rand.NextDouble() < INITIAL_LIVE_PROBABILITY) // % chance for each cell
                    {
                        SetCell(i, j, true);
                    }
                    else
                    {
                        SetCell(i, j, false);
                    }
                }
            }

            //// Add some interesting patterns
            
            //// Glider pattern
            //SetCell(1, 2, true);
            //SetCell(2, 3, true);
            //SetCell(3, 1, true);
            //SetCell(3, 2, true);
            //SetCell(3, 3, true);

            //// Blinker pattern
            //SetCell(10, 10, true);
            //SetCell(10, 11, true);
            //SetCell(10, 12, true);

            //// Block pattern (still life)
            //SetCell(15, 15, true);
            //SetCell(15, 16, true);
            //SetCell(16, 15, true);
            //SetCell(16, 16, true);

            //// Toad pattern (oscillator)
            //SetCell(5, 20, true);
            //SetCell(5, 21, true);
            //SetCell(5, 22, true);
            //SetCell(6, 19, true);
            //SetCell(6, 20, true);
            //SetCell(6, 21, true);

            //// Random pattern in one corner
            //Random rand = new();
            //for (int i = HEIGHT - 10; i < HEIGHT - 2; i++)
            //{
            //    for (int j = WIDTH - 15; j < WIDTH - 2; j++)
            //    {
            //        if (rand.NextDouble() < 0.3) // 30% chance for each cell
            //        {
            //            SetCell(i, j, true);
            //        }
            //    }
            //}
        }

        static void SetCell(int row, int col, bool alive)
        {
            if (row >= 0 && row < HEIGHT && col >= 0 && col < WIDTH)
            {
                grid[row, col] = alive;
            }
        }

        static void DisplayGrid()
        {
            Console.SetCursorPosition(0, 0);
            
            // Top border
            Console.Write("┌");
            for (int j = 0; j < WIDTH; j++)
                Console.Write("─");
            Console.WriteLine("┐");

            for (int i = 0; i < HEIGHT; i++)
            {
                Console.Write("│");
                for (int j = 0; j < WIDTH; j++)
                {
                    // Change color based on number of neighbors

                    int neighbors = CountNeighbors(i, j);
                    
                    if (grid[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.ForegroundColor = neighbors switch
                        {
                            0 or 1 => ConsoleColor.DarkGray,// Underpopulation
                            2 or 3 => ConsoleColor.Green,// Stable
                            4 or 5 => ConsoleColor.Yellow,// Overpopulation
                            _ => ConsoleColor.Red,// High overpopulation
                        };
                    }
     
                    Console.Write(grid[i, j] ? "█" : " ");
                    Console.ResetColor();
                }
                Console.WriteLine("│");
            }

            // Bottom border
            Console.Write("└");
            for (int j = 0; j < WIDTH; j++)
                Console.Write("─");
            Console.WriteLine("┘");

            Console.WriteLine("Generation: " + GetGenerationCount());
            Console.WriteLine("Press ESC to exit");
        }

        static int generationCount = 0;
        static int GetGenerationCount()
        {
            return generationCount;
        }

        static void UpdateGrid()
        {
            // Calculate next generation
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    int neighbors = CountNeighbors(i, j);
                    bool currentCell = grid[i, j];

                    // Apply Conway's Game of Life rules
                    if (currentCell)
                    {
                        // Original rule: Live cell with 2 or 3 neighbors survives
                        //nextGrid[i, j] = neighbors == 2 || neighbors == 3;
                        // Modified rule: 
                        nextGrid[i, j] = neighbors >= 2 && neighbors <= 3;
                    }
                    else
                    {
                        // Original rule: Dead cell with exactly 3 neighbors becomes alive
                        //nextGrid[i, j] = neighbors == 3;
                        // Modified rule: 
                        nextGrid[i, j] = neighbors == 3;
                    }
                }
            }

            // Copy next generation to current grid
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    grid[i, j] = nextGrid[i, j];
                }
            }

            generationCount++;
        }

        static int CountNeighbors(int row, int col)
        {
            int count = 0;
            
            // Check all 8 neighboring cells
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Skip the center cell
                    if (i == 0 && j == 0)
                        continue;

                    int newRow = row + i;
                    int newCol = col + j;

                    // Check bounds and count living neighbors
                    if (newRow >= 0 && newRow < HEIGHT && 
                        newCol >= 0 && newCol < WIDTH && 
                        grid[newRow, newCol])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        static string GridToString()
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    result.Append(grid[i, j] ? '1' : '0');
                }
            }
            return result.ToString();
        }

        static bool DetectLoop(string currentState)
        {
            // Add current state to history
            gridHistory.Add(currentState);

            // Only check for loops if we have enough history
            if (gridHistory.Count < 3)
                return false;

            // Check for immediate repetition (period 1 - still life)
            if (gridHistory.Count >= 2 && gridHistory[^1] == gridHistory[^2])
            {
                loopStart = gridHistory.Count - 2;
                loopLength = 1;
                return true;
            }

            // Check for longer loops (up to half the current history length)
            int maxPeriod = Math.Min(20, gridHistory.Count / 2); // Limit check to avoid performance issues

            for (int period = 2; period <= maxPeriod; period++)
            {
                if (gridHistory.Count >= period * 2)
                {
                    bool isLoop = true;

                    // Check if the last 'period' states repeat
                    for (int i = 0; i < period; i++)
                    {
                        int currentIndex = gridHistory.Count - 1 - i;
                        int previousIndex = currentIndex - period;

                        if (gridHistory[currentIndex] != gridHistory[previousIndex])
                        {
                            isLoop = false;
                            break;
                        }
                    }

                    if (isLoop)
                    {
                        loopStart = gridHistory.Count - period * 2;
                        loopLength = period;
                        return true;
                    }
                }
            }

            // Clean up history if it gets too long (keep last 100 generations)
            if (gridHistory.Count > 100)
            {
                gridHistory.RemoveAt(0);
            }

            return false;
        }

        static void DisplayLoopInfo()
        {
            Console.SetCursorPosition(0, HEIGHT + 5);
            Console.WriteLine("═══ LOOP DETECTED ═══");

            if (loopLength == 1)
            {
                Console.WriteLine("Pattern has reached a stable state (still life).");
            }
            else if (loopLength == 2)
            {
                Console.WriteLine("Pattern is oscillating with period 2 (blinker-like).");
            }
            else
            {
                Console.WriteLine($"Pattern is repeating with period {loopLength}.");
            }

            Console.WriteLine($"Loop started at generation {loopStart}.");
            Console.WriteLine($"Current generation: {generationCount}");
            Console.WriteLine("════════════════════");
        }
    }
}