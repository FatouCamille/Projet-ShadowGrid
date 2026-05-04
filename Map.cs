using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace ShadowGrid
{
    public class Map
    {
        public int[,] Grid;
        public int TileSize = 64;
        private Random random = new Random();

        public Map()
        {
            Grid = new int[9, 13];
            GenerateRandomMap();
        }

        public void GenerateRandomMap()
        {
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    if (y == 0 || y == Grid.GetLength(0) - 1 || x == 0 || x == Grid.GetLength(1) - 1)
                    {
                        Grid[y, x] = 1; // Bordures
                    }
                    else
                    {
                        Grid[y, x] = (random.Next(0, 100) < 25) ? 1 : 0; // Murs aléatoires
                    }
                }
            }

            
            Grid[1, 1] = 0;
            Grid[1, 2] = 0;
            Grid[2, 1] = 0;

            
            Grid[3, 3] = 0;
            Grid[3, 4] = 0;
            Grid[3, 5] = 0;
            Grid[3, 2] = 0;

            
            Grid[7, 11] = 2;
        }

        public void Draw()
        {
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    int posX = x * TileSize;
                    int posY = y * TileSize;

                    if (Grid[y, x] == 1) // MUR
                    {
                        Raylib.DrawRectangle(posX, posY, TileSize - 1, TileSize - 1, Color.DarkGray);
                    }
                    else if (Grid[y, x] == 2) // SORTIE
                    {
                        Raylib.DrawRectangle(posX, posY, TileSize - 1, TileSize - 1, Color.Gold);
                    }
                    else // SOL
                    {
                        Raylib.DrawRectangle(posX, posY, TileSize, TileSize, Color.Black);
                    }
                }
            }
        }
    }
}