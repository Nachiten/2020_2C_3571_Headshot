using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.MonoGame.TP.FPS.Interface
{
    public class MouseManager
    {
        MouseState previousMouseState;
        Ray ShotDirection;
        public bool ViewChanged;
        private Vector2 pastMousePosition;
        public float MouseSensitivity { get; set; } = 10f;
        static FreeCamera Camera;
        #region Singleton
        public static MouseManager Instance { get; private set; }
        public static void Init(FreeCamera camera)
        {
            if (Instance is null)
            {
                Instance = new MouseManager();
                Camera = camera;
            }

        }
        #endregion

        public MouseManager()
        {
            previousMouseState = Mouse.GetState();
            pastMousePosition = Mouse.GetState().Position.ToVector2();
            
        }
        public void Update(float elapsedTime, Func<Ashootable, int> ShootCallback) {
            
            // Handle Click
            if (previousMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Player.Instance.TriggerShot = true;
                ShotDirection = new Ray(Camera.Position, Camera.FrontDirection);
                Collision.Instance.CheckShootable(ShotDirection, Player.Instance, ShootCallback);
            }
            previousMouseState = Mouse.GetState();

            // Handle Movement
            ProcessMouseMovement(elapsedTime);
            if (ViewChanged)
                Camera.CalculateView();

        }
        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            var mouseDelta = mouseState.Position.ToVector2() - pastMousePosition;
            mouseDelta *= MouseSensitivity * elapsedTime;

            Camera.ProcessMouseMovement(mouseDelta);

            if (Config.bloquearMouse)
            {
                Mouse.SetPosition(Camera.screenCenter.X, Camera.screenCenter.Y);
                Mouse.SetCursor(MouseCursor.Crosshair);
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }

            pastMousePosition = Mouse.GetState().Position.ToVector2();
        }
    }
}
