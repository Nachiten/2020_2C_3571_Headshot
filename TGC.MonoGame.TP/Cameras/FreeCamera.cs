using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.FPS.Scenarios;
using System.Diagnostics;

namespace TGC.MonoGame.Samples.Cameras
{
    public class FreeCamera : Camera
    {
        private readonly bool lockMouse;

        private readonly Point screenCenter;
        private bool changed;

        private Vector2 pastMousePosition;
        private float pitch;

        // Angles
        private float yaw = -90f;
        public AABB cameraBox;
        Vector3 oldPosition;
        IStageBuilder Stage;

        public FreeCamera(float aspectRatio, Vector3 position, Point screenCenter, IStageBuilder stage) : this(aspectRatio, position)
        {
            lockMouse = true;
            this.screenCenter = screenCenter;
            cameraBox = new AABB(new Vector3(20,50,20));
            Stage = stage;
        }

        public FreeCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Position = position;
            pastMousePosition = Mouse.GetState().Position.ToVector2();
            UpdateCameraVectors();
            CalculateView();
        }

        public float MovementSpeed { get; set; } = 200f;
        public float MouseSensitivity { get; set; } = 10f;

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }
        /// <inheritdoc />
        public override void Update(GameTime gameTime){
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);

            if (changed)
                CalculateView();
        }
       
        public int StaticCollisionCB(AABB a, AABB b){
            Position = oldPosition;
            cameraBox.Translation(Position);
            return 0;
        }
        public int CollectableCollisionCB(Recolectable r)
        {
            //TODO: Use recolectable
            Debug.WriteLine("Collectable Collision: " + r);
            Stage.RemoveRecolectable(r);
            return 0;
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();
            oldPosition = Position;

            var currentMovementSpeed = MovementSpeed;
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 5f;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Position += -RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Position += RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                Position += FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                Position += -FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
            Position = new Vector3(Position.X, 100, Position.Z);
            cameraBox.Translation(Position);
            Collision.Instance.CheckStatic(cameraBox, StaticCollisionCB);
            Collision.Instance.CheckCollectable(cameraBox, CollectableCollisionCB);
        }

        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            var mouseDelta = mouseState.Position.ToVector2() - pastMousePosition;
            mouseDelta *= MouseSensitivity * elapsedTime;

            yaw += mouseDelta.X;
            pitch -= mouseDelta.Y;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            changed = true;
            UpdateCameraVectors();

            if (lockMouse)
            {
                Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                Mouse.SetCursor(MouseCursor.Crosshair);
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }

            pastMousePosition = Mouse.GetState().Position.ToVector2();
        }

        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}