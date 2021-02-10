using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.TP.FPS.Interface;

namespace TGC.MonoGame.TP
{
    class RocketLauncher : AWeaponRecolectable
    {
        public ModelCollidable Rocket;
        public ModelCollidable Explosion;
        private Matrix ExplosionScale = Matrix.CreateScale(0.002f);
        private Matrix ExplosionBasePosition = Matrix.CreateTranslation(0, -100, 0);
        public Matrix RocketWorld;
        private Vector3 RocketPosDis = new Vector3(-50, -50, 0);
        private Vector3 RocketDirection;
        public Vector3 RocketPosition;
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

            // Explosion
            Explosion = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "explosion/sphere", ExplosionScale * ExplosionBasePosition);
            Explosion.SetEffect(Effect);
            Explosion.SetLightParameters(1f, 0f, 0f, 0f);
            Explosion.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "explosion/sphere_tex"));

            // Set Texture
            /*var modeleEffect = (BasicEffect)Explosion.Model.Meshes[0].Effects[0];
            modeleEffect.TextureEnabled = true;
            modeleEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "explosion/sphere_tex");*/

            /*Rocket.SetEffect(Effect);
            Rocket.SetLightParameters(.2f, .6f, .2f, 100f);
            Rocket.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "weapons/rocket/rocket-texture"));*/
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ExplosionStarted(gameTime))
            {
                Explosion.Transform(ExplosionScale * Matrix.CreateTranslation(RocketPosition), false);
                Explosion.SetLight(new Light { Position = RocketPosition, AmbientColor = Color.White, DiffuseColor = Color.White, SpecularColor = Color.White });
            }
            if (ExplosionEnded(gameTime))
            {
                Explosion.Transform(ExplosionScale * ExplosionBasePosition, false);
            }
            
            if (Launching)
            {
                // Movement
                RocketPosition += RocketDirection * RocketSpeed;
                Offset += RocketSpeed;
                RocketWorld = Matrix.CreateTranslation(new Vector3(0, -5, -10)) * Matrix.CreateRotationY(RocketAngle) * Matrix.CreateTranslation(RocketPosition);
                Rocket.Transform(RocketWorld, true);
                Rocket.Aabb.Translation(Matrix.CreateTranslation(RocketPosition));
                Collision.Instance.CheckRocket(Rocket.Aabb, Player.Instance, RocketCollisionShootableCB);
                if (Offset > 2000)
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
        bool startlightshoot = false;
        double shottime = 0;
        public bool ShootStarted(GameTime GameTime)
        {
            if (startlightshoot && shottime == 0)
            {
                shottime = GameTime.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
        public bool ShootEnded(GameTime GameTime)
        {
            double timedifference = GameTime.TotalGameTime.TotalMilliseconds - shottime;
            if (startlightshoot && timedifference > 100)
            {
                startlightshoot = false;
                shottime = 0;
                return true;
            }
            return false;
        }

        bool explosioneffect = false;
        double explosiontime = 0;
        public bool ExplosionStarted(GameTime GameTime)
        {
            if (explosioneffect && explosiontime == 0)
            {
                explosiontime = GameTime.TotalGameTime.TotalMilliseconds;
                return true;
            }
            return false;
        }
        public bool ExplosionEnded(GameTime GameTime)
        {
            double timedifference = GameTime.TotalGameTime.TotalMilliseconds - explosiontime;
            if (explosioneffect && timedifference > 100)
            {
                explosioneffect = false;
                explosiontime = 0;
                return true;
            }
            return false;
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
            Explosion.Draw(view, projection);
        }
        public int RocketCollisionShootableCB(Ashootable e)
        {
            if(e != null)
            {
                e.GetDamaged(Damage);
            }
            SoundManager.Instance.reproducirSonido(SoundManager.Sonido.Explosion);
            startlightshoot = true;
            explosioneffect = true;
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
