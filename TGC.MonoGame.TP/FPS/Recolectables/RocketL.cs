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
        private Vector3 RocketPosDis = new Vector3(0, -500, 0);
        private Vector3 RocketDirection;
        private Vector3 RocketPosition;
        private float Offset;
        private float DeltaPos = 15f;
        private bool Launching = false;

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

            // Rocket
            RocketWorld = Matrix.CreateScale(0.06f) * Matrix.CreateTranslation(RocketPosDis);
            Rocket = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "weapons/rocket/rocketlauncher", RocketWorld);
            float aabboffset = 10;
            Rocket.Aabb.SetManually(new Vector3(RocketPosDis.X - aabboffset, RocketPosDis.Y - aabboffset, RocketPosDis.Z - aabboffset), new Vector3(RocketPosDis.X + aabboffset, RocketPosDis.Y + aabboffset, RocketPosDis.Z + aabboffset));
            Rocket.Aabb.Translation(RocketWorld);
            Rocket.SetEffect(Effect);
            Rocket.SetLightParameters(.2f, .6f, .2f, 100f);
            Rocket.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "weapons/rocket/rocketlauncher-texture"));
        }
        public override void Update(GameTime gameTime)
        {
            if (Launching)
            {
                Offset += DeltaPos;
                RocketWorld = Matrix.CreateScale(0.06f) * Matrix.CreateTranslation(RocketPosition + RocketDirection * Offset);
                Rocket.Transform(RocketWorld, true);
                Collision.Instance.CheckRocket(Rocket.Aabb, Player.Instance, RocketCollisionShootableCB);
            }
            
        }
        public void StartLaunch(Vector3 Direction)
        {
            Offset = 0;
            RocketPosition = Player.Instance.Position;
            RocketDirection = Vector3.Normalize(Direction);
            Launching = true;
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
            base.Draw(world, view, projection);
            //Debug.WriteLine("Drawing...?");
            Rocket.Draw(RocketWorld, view, projection);
        }
        public int RocketCollisionShootableCB(Ashootable e)
        {
            if(e != null)
            {
                e.GetDamaged(Damage);
            }
            Offset = 0;
            RocketWorld = Matrix.CreateScale(0.06f) * Matrix.CreateTranslation(RocketPosDis);
            Rocket.Transform(RocketWorld, true);
            Launching = false;
            return 0;
        }
    }
}
