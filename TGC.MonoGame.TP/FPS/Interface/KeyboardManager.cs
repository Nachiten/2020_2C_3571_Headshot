using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.MonoGame.TP.FPS.Interface
{
    public struct ControlSettings
    {
        //move
        public static Keys Forward = Keys.W;
        public static Keys Backward = Keys.S;
        public static Keys Right = Keys.D;
        public static Keys Left = Keys.A;
        public static Keys ChangeLastWeapon = Keys.Q;
        public static Keys PrimaryWeapon = Keys.D1;
        public static Keys SecondaryWeapon = Keys.D2;
        public static Keys Knife = Keys.D3;
        public static Keys Jump = Keys.Back;

        //weapon controller
    }
    public class KeyboardManager
    {
       

        #region Singleton
        public static KeyboardManager Instance { get; private set; }
        public static void Init(FreeCamera camera)
        {
            if (Instance is null)
            {
                Instance = new KeyboardManager();
                Camera = camera;
            }

        }
        #endregion

        static FreeCamera Camera;

        public void Update(float elapsedTime, Player Player)
        {
            var keyboardState = Keyboard.GetState();
            Player.PreviousPosition = Player.Position;

            var currentMovementSpeed = Player.Speed;

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                //Exit();
            }

            // Desbloquear el mouse tocando la P
            // if (Keyboard.GetState().IsKeyDown(Keys.P) && !presionadoTeclaQ) { 
            //     Config.bloquearMouse = !Config.bloquearMouse;
            //     presionadoTeclaQ = true;
            // }

            // if (Keyboard.GetState().IsKeyUp(Keys.P))
            //     presionadoTeclaQ = false;

            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 2f;

            if (keyboardState.IsKeyDown(Keys.D1))
                Player.Instance.ChangeWeapon(1);

            if (keyboardState.IsKeyDown(Keys.D2))
                Player.Instance.ChangeWeapon(2);
            if (keyboardState.IsKeyDown(Keys.D3))
                Player.Instance.ChangeWeapon(3);

            Vector3 currpos = Player.Position;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                currpos += -Camera.RightDirection * currentMovementSpeed; //* elapsedTime;
                MouseManager.Instance.ViewChanged = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                currpos += Camera.RightDirection * currentMovementSpeed; //* elapsedTime;
                MouseManager.Instance.ViewChanged = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                currpos += Camera.FrontDirection * currentMovementSpeed; //* elapsedTime;
                MouseManager.Instance.ViewChanged = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                currpos += -Camera.FrontDirection * currentMovementSpeed; //* elapsedTime;
                MouseManager.Instance.ViewChanged = true;
            }
            Player.Move(new Vector3(currpos.X, 100, currpos.Z));
        }
    }
}
