using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace ShadowGrid
{
    public class Enemy
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed = 4.5f; // On augmente un peu la vitesse
        public int Size = 24;
        private Random rand = new Random();

        public Enemy(Vector2 startPos)
        {
            Position = startPos;
           
            SetRandomDirection();
        }

        private void SetRandomDirection()
        {
            int choix = rand.Next(0, 4);
            if (choix == 0) Direction = new Vector2(1, 0);  // Droite
            else if (choix == 1) Direction = new Vector2(-1, 0); // Gauche
            else if (choix == 2) Direction = new Vector2(0, 1);  // Bas
            else Direction = new Vector2(0, -1); // Haut
        }

        public void Update(Map maCarte)
        {
            Vector2 nextPos = Position + Direction * Speed;

           
            int checkX = (int)(nextPos.X + (Direction.X > 0 ? Size : 0) + (Direction.X < 0 ? -2 : 0)) / maCarte.TileSize;
            int checkY = (int)(nextPos.Y + (Direction.Y > 0 ? Size : 0) + (Direction.Y < 0 ? -2 : 0)) / maCarte.TileSize;

           
            if (checkY >= 0 && checkY < maCarte.Grid.GetLength(0) &&
                checkX >= 0 && checkX < maCarte.Grid.GetLength(1))
            {
                
                if (maCarte.Grid[checkY, checkX] == 1)
                {
                    
                    SetRandomDirection();
                }
                else
                {
                    
                    Position = nextPos;
                }
            }
            else
            {
                
                SetRandomDirection();
            }
        }

        public void Draw()
        {
            Raylib.DrawRectangleV(Position, new Vector2(Size, Size), Color.Blue);
        }
    }
}