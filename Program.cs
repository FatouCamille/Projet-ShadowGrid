using Raylib_cs;
using System.Numerics;
using ShadowGrid;
using Color = Raylib_cs.Color;


enum GameState { Menu, Playing, GameOver }

class Program
{
    static GameState currentGameState = GameState.Menu;
    static float shakeAmount = 0f;

    static void Main()
    {
        Raylib.InitWindow(832, 576, "Shadow Grid - Audio Edition");

        
        Raylib.InitAudioDevice();

        Raylib.SetTargetFPS(60);

        Map maCarte = new Map();
        Player monJoueur = new Player(new Vector2(80, 80));
        Enemy monEnnemi = new Enemy(new Vector2(200, 200));

        
        Music backgroundMusic = Raylib.LoadMusicStream("Asset/ambiance.mp3");
        Raylib.PlayMusicStream(backgroundMusic);
        Raylib.SetMusicVolume(backgroundMusic, 0.5f);

        int niveauxTermines = 0;

        while (!Raylib.WindowShouldClose())
        {
            
            Raylib.UpdateMusicStream(backgroundMusic);

            
            switch (currentGameState)
            {
                case GameState.Menu:
                    if (Raylib.IsKeyPressed(KeyboardKey.Enter)) currentGameState = GameState.Playing;
                    break;

                case GameState.Playing:
                    UpdateGame(maCarte, monJoueur, monEnnemi, ref niveauxTermines);
                    break;

                case GameState.GameOver:
                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        niveauxTermines = 0;
                        ResetLevel(maCarte, monJoueur, monEnnemi, 0);
                        currentGameState = GameState.Playing;
                    }
                    break;
            }

            // --- RENDU (Draw) ---
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            if (currentGameState == GameState.Playing)
            {
                if (shakeAmount > 0)
                {
                    float offsetX = Raylib.GetRandomValue(-5, 5) * shakeAmount;
                    float offsetY = Raylib.GetRandomValue(-5, 5) * shakeAmount;

                    Camera2D camera = new Camera2D();
                    camera.Offset = new Vector2(offsetX, offsetY);
                    camera.Target = new Vector2(0, 0);
                    camera.Rotation = 0f;
                    camera.Zoom = 1f;

                    Raylib.BeginMode2D(camera);
                    shakeAmount -= 0.1f;
                }

                maCarte.Draw();
                monEnnemi.Draw();
                monJoueur.Draw();

                if (shakeAmount > 0) Raylib.EndMode2D();

                Raylib.DrawText($"NIVEAU : {niveauxTermines + 1}", 10, 10, 20, Color.White);
            }
            else if (currentGameState == GameState.Menu)
            {
                Raylib.DrawText("SHADOW GRID", 250, 200, 50, Color.White);
                Raylib.DrawText("Appuyez sur [ENTREE] pour commencer", 230, 300, 20, Color.Gray);
            }
            else if (currentGameState == GameState.GameOver)
            {
                Raylib.DrawText("GAME OVER", 280, 200, 50, Color.Red);
                Raylib.DrawText($"SCORE : {niveauxTermines}", 360, 280, 25, Color.White);
                Raylib.DrawText("Appuyez sur [ENTREE] pour rejouer", 235, 350, 20, Color.Gray);
            }

            Raylib.EndDrawing();
        }

        // --- NETTOYAGE AUDIO ---
        Raylib.UnloadMusicStream(backgroundMusic);
        Raylib.CloseAudioDevice();

        Raylib.CloseWindow();
    }

    static void UpdateGame(Map maCarte, Player monJoueur, Enemy monEnnemi, ref int score)
    {
        monJoueur.Update(maCarte);
        monEnnemi.Update(maCarte);

        int px = (int)(monJoueur.Position.X + monJoueur.Size / 2) / maCarte.TileSize;
        int py = (int)(monJoueur.Position.Y + monJoueur.Size / 2) / maCarte.TileSize;

        if (py >= 0 && py < maCarte.Grid.GetLength(0) && px >= 0 && px < maCarte.Grid.GetLength(1))
        {
            if (maCarte.Grid[py, px] == 2)
            {
                score++;
                shakeAmount = 2.0f;
                ResetLevel(maCarte, monJoueur, monEnnemi, score);
            }
        }

        Rectangle rJ = new Rectangle(monJoueur.Position.X, monJoueur.Position.Y, monJoueur.Size, monJoueur.Size);
        Rectangle rE = new Rectangle(monEnnemi.Position.X, monEnnemi.Position.Y, monEnnemi.Size, monEnnemi.Size);

        if (Raylib.CheckCollisionRecs(rJ, rE))
        {
            shakeAmount = 4.0f;
            currentGameState = GameState.GameOver;
        }
    }

    static void ResetLevel(Map m, Player p, Enemy e, int lvl)
    {
        m.GenerateRandomMap();
        p.Position = new Vector2(80, 80);
        e.Position = new Vector2(200, 200);
        e.Speed = 3.0f + (lvl * 0.5f);
        e.Direction = new Vector2(1, 0);
    }
}