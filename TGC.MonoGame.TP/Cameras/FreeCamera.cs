using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.FPS.Interface;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.Samples.Cameras
{
    public class FreeCamera : Camera
    {
        public readonly Point screenCenter;

        private float pitch;

        // Angles
        private float yaw = -90f;

        public FreeCamera(float aspectRatio, Vector3 position, Point screenCenter) : this(aspectRatio, position)
        {
            this.screenCenter = screenCenter;
        }

        public FreeCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Position = position;
            UpdateCameraVectors();
            CalculateView();
        }

        public void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime){
        }

        public void ProcessMouseMovement(Vector2 mouseDelta)
        {
            yaw += mouseDelta.X;
            pitch -= mouseDelta.Y;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            MouseManager.Instance.ViewChanged = true;
            UpdateCameraVectors();
        }
        public void ResetCamera(Vector3 pos)
        {
            yaw = -90f;
            pitch = 0;
            Position = pos;
            UpdateCameraVectors();
            CalculateView();
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
        public bool InView(AABB box)
        {
            return new BoundingFrustum(View * Projection).Intersects(box.boundingBox);
        }
    }
}