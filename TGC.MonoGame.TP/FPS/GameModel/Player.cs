using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS.Interface;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.MonoGame.TP.FPS
{
    public class Player: DrawableGameComponent
    {
        public static Player Instance { get; private set; }

        public static void Init(Game game, FreeCamera camera)
        {
            if (Instance is null)
            {
                Instance = new Player(game);
                Camera = camera;
                Instance.Initialize();
            }

        }

        public Player(Game game) : base(game)
        {
        }

        #region Componentes
        
        
        #endregion

        #region Propiedades
        private Matrix WorldWeapon { get; set; }
        private Matrix Projection { get; set; }

        private Matrix View { get; set; }
        public int Health { get; set; }
        public Weapon CurrentWeapon { get; set; }

        public float Speed = 200f;

        public int Armor { get; set; }

        public Weapon[] Weapons { get; set; }

        public Vector3 CurrentPosition { get; set; }

        public AABB PlayerBox;

        public Vector3 Position;

        public Vector3 PreviousPosition;

        static FreeCamera Camera;
        #endregion
        //default 100

        public override void Initialize()
        {
            Health = 100;

            Weapons = new Weapon[3];
            WorldWeapon = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(50, 0, 110);
            View = Matrix.CreateLookAt(new Vector3(30, 20, 150), new Vector3(30, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            PlayerBox = new AABB(new Vector3(20, 80, 20));

            //inicializar el current weapon con Fist
            base.Initialize();
        }

        /// <summary>
        /// Toma el arma y lanza en direccion de donde este el mouse un projectil
        /// </summary>
        /// <param name="direction"></param>
        public void Shoot()
        {
            //Deberiamos ver como obtenemos la direccion a la que esta aputando el arma.
            //this.weapon.fire()
        }

        public void Move(Vector3 NewPosition)
        {
            Camera.Position = NewPosition;
            Position = NewPosition;
            PlayerBox.Translation(Position);
        }
        public override void Update(GameTime GameTime)
        {
            var elapsedTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            MouseManager.Instance.ViewChanged = false;
            KeyboardManager.Instance.Update(elapsedTime, this);
            MouseManager.Instance.Update(elapsedTime, ShootableCollisionCB);

            Collision.Instance.CheckStatic(PlayerBox, StaticCollisionCB);
            Collision.Instance.CheckCollectable(PlayerBox, CollectableCollisionCB);
        }

        public void RecibirDisparo(int cantidadDanio) {
            if (this.Health > 0) {
                this.Health -= cantidadDanio;
            }
        }

        public void Jump()
        {
            //currnt position move y |
        }

        public void ChangeWeapon(int index)
        {
            CurrentWeapon = Weapons[index];
        }

        public void AgarrarArma(Weapon nuevaArma)
        {
            CurrentWeapon = nuevaArma;
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (CurrentWeapon != null) {
                CurrentWeapon.Draw(WorldWeapon, View, Projection);
            }
            base.Draw(gameTime);
        }


        #region CallBacks
        public int StaticCollisionCB(AABB a, AABB b)
        {
            Move(PreviousPosition);
            return 0;
        }
        public int CollectableCollisionCB(Recolectable r)
        {
            /*//TODO: Use recolectable
            Debug.WriteLine("Collectable Collision: " + r);
            Stage.RemoveRecolectable(r);

            switch (r.tipoRecolectable)
            {
                case TipoRecolectable.m4:
                    Player.Instance.AgarrarArma(new Weapon(r.Modelo.Model));
                    break;
                case TipoRecolectable.cuchillo:
                    Player.Instance.AgarrarArma(new Weapon(r.Modelo.Model));
                    break;
                case TipoRecolectable.armor:
                    // TODO | Sumar armor del player
                    break;
                case TipoRecolectable.vida:
                    // TODO | Sumar vida del player
                    break;

            }*/

            return 0;
        }
        public int ShootableCollisionCB(Enemigo e)
        {
            if (CurrentWeapon != null)
            {
                Debug.WriteLine("Dispare al tgcitoooooo " + Vector3.Transform(Vector3.Zero, e.ModeloTgcitoClassic.World));
            }
            return 0;
        }
        #endregion
    }
}

