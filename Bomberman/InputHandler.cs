using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Command;
using Bomberman.Spawnables.Obstacles;
using SFML.Window;
using Bomberman.Map;


namespace Bomberman
{
    class InputHandler
    {
        Player _player;
        TileMapFacade _tilemap;
        //Stores all commands for replay and undo
        public static List<IMovement> oldCommands = new List<IMovement>();

        private static IMovement buttonW, buttonS, buttonA, buttonD, buttonZ;

        public InputHandler(Player player, TileMapFacade tileMapFacade)
        {
            this._player = player;
            this._tilemap = tileMapFacade;
            //Bind keys with commands
            buttonW = new MoveForward();
            buttonS = new MoveBackward();
            buttonA = new MoveLeft();
            buttonD = new MoveRight();
        }

        public void Control() // Command invoker
        {
            float movementSpeed = 5;
            float moveDistance = movementSpeed;
            float movementX = 0;
            float movementY = 0;

            List<Obstacle> collidableObstacles = _tilemap.GetTileMap().GetCloseObstacles(_player.Position);

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                if (!_player.CheckMovementCollision(0, -moveDistance, collidableObstacles))
                {
                    buttonW.Execute(_player, -moveDistance);
                    //movementY -= moveDistance;
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                if (!_player.CheckMovementCollision(0, moveDistance, collidableObstacles))
                {
                    buttonS.Execute(_player, moveDistance);
                    //movementY += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (!_player.CheckMovementCollision(moveDistance, 0, collidableObstacles))
                {
                    buttonD.Execute(_player, moveDistance);
                    //movementX += moveDistance;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (!_player.CheckMovementCollision(-moveDistance, 0, collidableObstacles))
                {
                    buttonA.Execute(_player, -moveDistance);
                    // movementX -= moveDistance;
                }
            }

            _player.Translate(movementX, movementY); // move?
        }

    }
}
