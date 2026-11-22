using System;
using System.Numerics;

namespace MohawkGame2D;

public class Game
{
    private Vector2 playerPosition;

    private Enemy[] enemies;
    private Trash[] trashItems;

    private bool gameOver;
    private bool winGame;

    public void Setup()
    {
        playerPosition = new Vector2(400, 300);

        enemies = new Enemy[6];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new Enemy();

            enemies[i].Position = new Vector2(
                100 + i * 100,
                100
            );

            enemies[i].Velocity = new Vector2(2, 1);
        }

        trashItems = new Trash[5];

        for (int i = 0; i < trashItems.Length; i++)
        {
            trashItems[i] = new Trash();

            trashItems[i].Position = new Vector2(
                150 + i * 100,
                450
            );

            trashItems[i].Velocity = new Vector2(-1, 1);
        }
    }

    public void Update()
    {
        if (winGame)
        {
            Draw.SetFillColor(0, 255, 0);
            Draw.Rectangle(0, 0, 800, 600);

            Text.Size = 60;
            Text.Draw("YOU WIN!", 280, 280);

            return;
        }

        if (gameOver)
        {
            Draw.SetFillColor(255, 0, 0);
            Draw.Rectangle(0, 0, 800, 600);

            Text.Size = 60;
            Text.Draw("GAME OVER", 220, 280);

            return;
        }

        Draw.SetFillColor(0);
        Draw.Rectangle(0, 0, 800, 600);

        Vector2 movement = Input.GetAxis2(
            KeyboardKey.A,
            KeyboardKey.D,
            KeyboardKey.W,
            KeyboardKey.S
        );

        playerPosition += movement * 5;

        playerPosition.X = Math.Clamp(playerPosition.X, 0, 740);
        playerPosition.Y = Math.Clamp(playerPosition.Y, 0, 540);

        // Move enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Position += enemies[i].Velocity;

            if (enemies[i].Position.X <= 0 || enemies[i].Position.X >= 750)
            {
                enemies[i].Velocity.X *= -1;
            }

            if (enemies[i].Position.Y <= 0 || enemies[i].Position.Y >= 550)
            {
                enemies[i].Velocity.Y *= -1;
            }
        }

        // Move trash
        for (int i = 0; i < trashItems.Length; i++)
        {
            if (!trashItems[i].Collected)
            {
                trashItems[i].Position += trashItems[i].Velocity;

                if (trashItems[i].Position.X <= 0 || trashItems[i].Position.X >= 750)
                {
                    trashItems[i].Velocity.X *= -1;
                }

                if (trashItems[i].Position.Y <= 0 || trashItems[i].Position.Y >= 550)
                {
                    trashItems[i].Velocity.Y *= -1;
                }
            }
        }

        // Collect trash
        for (int i = 0; i < trashItems.Length; i++)
        {
            if (!trashItems[i].Collected)
            {
                if (Vector2.Distance(playerPosition, trashItems[i].Position) < 60)
                {
                    trashItems[i].Collected = true;
                }
            }
        }

        // Enemy collision
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector2.Distance(playerPosition, enemies[i].Position) < 60)
            {
                gameOver = true;
            }
        }

        // Win condition
        bool allCollected = true;

        for (int i = 0; i < trashItems.Length; i++)
        {
            if (!trashItems[i].Collected)
            {
                allCollected = false;
            }
        }

        if (allCollected)
        {
            winGame = true;
        }

        // Draw enemies
        Draw.SetFillColor(255, 0, 0);

        for (int i = 0; i < enemies.Length; i++)
        {
            Draw.Square(enemies[i].Position, 50);
        }

        // Draw trash
        Draw.SetFillColor(0, 255, 0);

        for (int i = 0; i < trashItems.Length; i++)
        {
            if (!trashItems[i].Collected)
            {
                Draw.Square(trashItems[i].Position, 50);
            }
        }

        // Draw player
        Draw.SetFillColor(0, 0, 255);
        Draw.Square(playerPosition, 60);
    }
}