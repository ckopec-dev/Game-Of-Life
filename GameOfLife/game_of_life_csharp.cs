using System;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        private const int Width = 50;
        private const int Height = 30;
        private static bool[,] grid = new bool[Height, Width];
        private static bool[,] nextGrid = new bool[Height, Width];

        static void Main(string[] args)
        {
            Console.WriteLine("Conway's Game of Life");
            Console.WriteLine("Press any key to start, ESC to exit during simulation");
            Console.ReadKey();

            // Initialize with a simple pattern (Glider)
            InitializeGrid();

            Console.Clear();
            Console.CursorVisible = false;

            while (true)
            {
                DisplayGrid();
                UpdateGrid();
                Thread.Sleep(200); // Delay between generations

                // Check for ESC key to exit
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }
            }

            Console.CursorVisible = true;
            Console.WriteLine("\nGame ended. Press any key to exit.");
            Console.ReadKey();
        }

        static void InitializeGrid()
        {
            // Clear the grid
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    grid[i, j] = false;
                }
            }

            // Add some interesting patterns
            
            // Glider pattern
            SetCell(1, 2, true);
            SetCell(2, 3, true);
            SetCell(3, 1, true);
            SetCell(3, 2, true);
            SetCell(3, 3, true);

            // Blinker pattern
            SetCell(10, 10, true);
            SetCell(10, 11, true);
            SetCell(10, 12, true);

            // Block pattern (still life)
            SetCell(15, 15, true);
            SetCell(15, 16, true);
            SetCell(16, 15, true);
            SetCell(16, 16, true);

            // Toad pattern (oscillator)
            SetCell(5, 20, true);
            SetCell(5, 21, true);
            SetCell(5, 22, true);
            SetCell(6, 19, true);
            SetCell(6, 20, true);
            SetCell(6, 21, true);

            // Random pattern in one corner
            Random rand = new Random();
            for (int i = Height - 10; i < Height - 2; i++)
            {
                for (int j = Width - 15; j < Width - 2; j++)
                {
                    if (rand.NextDouble() < 0.3) // 30% chance for each cell
                    {
                        SetCell(i, j, true);
                    }
                }
            }
        }

        static void SetCell(int row, int col, bool alive)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width)
            {
                grid[row, col] = alive;
            }
        }

        static void DisplayGrid()
        {
            Console.SetCursorPosition(0, 0);
            
            // Top border
            Console.Write("┌");
            for (int j = 0; j < Width; j++)
                Console.Write("─");
            Console.WriteLine("┐");

            for (int i = 0; i < Height; i++)
            {
                Console.Write("│");
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(grid[i, j] ? "█" : " ");
                }
                Console.WriteLine("│");
            }

            // Bottom border
            Console.Write("└");
            for (int j = 0; j < Width; j++)
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
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int neighbors = CountNeighbors(i, j);
                    bool currentCell = grid[i, j];

                    // Apply Conway's Game of Life rules
                    if (currentCell)
                    {
                        // Live cell with 2 or 3 neighbors survives
                        nextGrid[i, j] = neighbors == 2 || neighbors == 3;
                    }
                    else
                    {
                        // Dead cell with exactly 3 neighbors becomes alive
                        nextGrid[i, j] = neighbors == 3;
                    }
                }
            }

            // Copy next generation to current grid
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
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
                    if (newRow >= 0 && newRow < Height && 
                        newCol >= 0 && newCol < Width && 
                        grid[newRow, newCol])
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}