using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS;

namespace TGC.MonoGame.TP
{
    class RocketLauncher : AWeaponRecolectable
    {
        public ModelCollidable Rocket;
        public Matrix RocketWorld;
        private Vector3 RocketPosDis = new Vector3(-50, -50, 0);
        private Vector3 RocketDirection;
        private Vector3 RocketPosition;
        private Vector3 RocketInitialDirection = -Vector3.UnitX;
        private float RocketAngle = 0;
        public float StartExplosion = 0f;
        private Effect PostProcessEffect;

        private float Offset;
        private float RocketSpeed = 10f;
        private bool Launching = false;
        double frameTimePlayed = 0;

        public RocketLauncher(Vector3 posicionModelo)
        {
            pathModelo = "weapons/rocket/rocketlauncher";
            tamanioModelo = 0.06f;
            modelColor = Color.Gray.ToVector3();

            Damage = 80;

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
            Index = 3;
        }

        // Hago override ya que debo agregar logica extra
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Ejecuto la logica compartida (sigue siendo necesaria)
            base.LoadContent(Content, GraphicsDevice);
            Modelo.SetLightParameters(.2f, .6f, .2f, 100f);
            Modelo.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "weapons/rocket/rocketlauncher-texture"));
        }
        public void LoadContentRocket(ContentManager Content, GraphicsDevice GraphicsDevice, Effect Effect)
        {
            // Load Model
            RocketWorld = Matrix.CreateScale(1f) * Matrix.CreateTranslation(RocketPosDis);
            Rocket = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "weapons/rocket/rocket", RocketWorld);
            float aabboffset = 5;
            Rocket.Aabb.SetManually(new Vector3(RocketPosDis.X - aabboffset, RocketPosDis.Y - aabboffset, RocketPosDis.Z - aabboffset), new Vector3(RocketPosDis.X + aabboffset, RocketPosDis.Y + aabboffset, RocketPosDis.Z + aabboffset));
            Rocket.Aabb.Translation(RocketWorld);

            // Set Texture
            var modelEffect = (BasicEffect)Rocket.Model.Meshes[0].Effects[0];
            modelEffect.TextureEnabled = true;
            modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/rocket/rocket-texture");

            /*Rocket.SetEffect(Effect);
            Rocket.SetLightParameters(.2f, .6f, .2f, 100f);
            Rocket.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "weapons/rocket/rocket-texture"));*/
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Launching)
            {
                // Movement
                Offset += RocketSpeed;
                RocketWorld = Matrix.CreateTranslation(new Vector3(0, -5, -10)) * Matrix.CreateRotationY(RocketAngle) * Matrix.CreateTranslation(RocketPosition + RocketDirection * Offset);
                Rocket.Transform(RocketWorld, true);
                Rocket.Aabb.Translation(Matrix.CreateTranslation(RocketPosition + RocketDirection * Offset));
                Collision.Instance.CheckRocket(Rocket.Aabb, Player.Instance, RocketCollisionShootableCB);
                if (Offset > 1000)
                {
                    ResetRocket();
                }
                frameTimePlayed = 0;
            } else
            {
                if(frameTimePlayed < 300) // .3s de explosion
                {
                    PostProcessEffect.Parameters["Time"]?.SetValue((float)frameTimePlayed / 1000);
                    frameTimePlayed += gameTime.ElapsedGameTime.TotalMilliseconds;
                } else
                {
                    PostProcessEffect.Parameters["shot"]?.SetValue(0f);
                    frameTimePlayed = 0;
                }
                
            }
        }
        public void StartLaunch(Vector3 Direction)
        {
            if (Launching)
            {
                return;
            }
            Offset = 0;
            RocketPosition = Player.Instance.Position;
            RocketDirection = Vector3.Normalize(Direction);
            Launching = true;
            RocketAngle = AngleBet2Vec(RocketDirection, RocketInitialDirection);
        }
        public float AngleBet2Vec(Vector3 v1, Vector3 v2)
        {
            Vector2 v1_2 = new Vector2(v1.X, v1.Z);
            Vector2 v2_2 = new Vector2(v2.X, v2.Z);
            float angle = MathF.Acos(Vector2.Dot(v1_2, v2_2) / (Vector2Norm(v1_2) * Vector2Norm(v2_2)));
            if (v1_2.Y < 0)
            {
                angle *= -1;
            }
            return angle;
        }
        public float Vector2Norm(Vector2 v)
        {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
        }
        public void DrawRocket(Matrix view, Matrix projection)
        {
            Rocket.Draw(view, projection);
        }
        public int RocketCollisionShootableCB(Ashootable e)
        {
            if(e != null)
            {
                e.GetDamaged(Damage);
            }
            PostProcessEffect.Parameters["shot"]?.SetValue(1f);
            ResetRocket();
            return 0;
        }
        private void ResetRocket()
        {
            Offset = 0;
            RocketWorld = Matrix.CreateTranslation(RocketPosDis);
            Rocket.Transform(RocketWorld, true);
            Launching = false;
        }
        public void SetPostProcessEffect(Effect PostProcessEffect)
        {
            this.PostProcessEffect = PostProcessEffect;
        }
    }
}
