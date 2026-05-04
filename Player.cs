using Raylib_cs;
using System.Numerics;
using System.Collections.Generic; 
using Color = Raylib_cs.Color;

namespace ShadowGrid
{
    public class Player
    {
        public Vector2 Position;
        public float Speed = 4.0f;
        public int Size = 24;
        private float rotation = 0f;

        
        private List<Vector2> trail = new List<Vector2>();
        private int maxTrailLength = 10;

        public Player(Vector2 startPos)
        {
            Position = startPos;
        }

        public void Update(Map maCarte)
        {
            Vector2 nextPos = Position;
            bool moved = false;

            if (Raylib.IsKeyDown(KeyboardKey.Right)) { nextPos.X += Speed; rotation = 90f; moved = true; }
            if (Raylib.IsKeyDown(KeyboardKey.Left)) { nextPos.X -= Speed; rotation = 270f; moved = true; }
            if (Raylib.IsKeyDown(KeyboardKey.Up)) { nextPos.Y -= Speed; rotation = 0f; moved = true; }
            if (Raylib.IsKeyDown(KeyboardKey.Down)) { nextPos.Y += Speed; rotation = 180f; moved = true; }

            int centerX = (int)(nextPos.X + Size / 2) / maCarte.TileSize;
            int centerY = (int)(nextPos.Y + Size / 2) / maCarte.TileSize;

            if (centerY >= 0 && centerY < maCarte.Grid.GetLength(0) &&
                centerX >= 0 && centerX < maCarte.Grid.GetLength(1))
            {
                if (maCarte.Grid[centerY, centerX] == 0 || maCarte.Grid[centerY, centerX] == 2)
                {
                    Position = nextPos;

                    // Si on bouge, on ajoute la position actuelle à la traînée
                    if (moved)
                    {
                        trail.Add(Position);
                        if (trail.Count > maxTrailLength) trail.RemoveAt(0);
                    }
                }
            }

            // Si on ne bouge pas, la traînée se résorbe doucement
            if (!moved && trail.Count > 0) trail.RemoveAt(0);
        }

        public void Draw()
        {
            
            for (int i = 0; i < trail.Count; i++)
            {
                
                float alpha = (float)i / trail.Count;
                Color trailColor = Raylib.Fade(Color.Red, alpha * 0.4f);

                Vector2 trailCenter = new Vector2(trail[i].X + Size / 2, trail[i].Y + Size / 2);
                Raylib.DrawPoly(trailCenter, 3, Size * alpha, rotation, trailColor);
            }

           
            Vector2 center = new Vector2(Position.X + Size / 2, Position.Y + Size / 2);

            
            Raylib.DrawPoly(center, 3, Size, rotation, Color.Red);
            
            Raylib.DrawPolyLinesEx(center, 3, (float)Size + 1, rotation, 2, Color.White);
        }
    }
}