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
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP.FPS
{
    public class Player : DrawableGameComponent
    {
        public static Player Instance { get; private set; }

        public static void Init(Game game, FreeCamera camera, IStage stage)
        {
            if (Instance is null)
            {
                Instance = new Player(game);
                Camera = camera;
                Instance.Initialize();
                Stage = stage;
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
        private int maxHealth = 100;
        public int Armor { get; set; }
        private int maxArmor = 100;
        public Weapon CurrentWeapon { get; set; }

        public float Speed = 200f;

        public Weapon[] Weapons { get; set; }

        public Vector3 CurrentPosition { get; set; }
        

        public AABB PlayerBox;

        public Vector3 Position;

        public Vector3 PreviousPosition;

        static FreeCamera Camera;
        static IStage Stage;

        #endregion

        public override void Initialize()
        {
            Health = 75;
            Armor = 45;

            Weapons = new Weapon[3];
            WorldWeapon = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(50, 0, 110);
            View = Matrix.CreateLookAt(new Vector3(30, 20, 150), new Vector3(30, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            PlayerBox = new AABB(GraphicsDevice,new Vector3(20, 80, 20));

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

        public bool sumarVida(int cantidadVida)
        {
            // Si ya esta en maximo no suma nada
            if (this.Health == maxHealth)
            {
                return false;
            }

            // Si no esta en el maximo suma algo
            this.Health = Math.Min(this.Health += cantidadVida, maxHealth);

            return true;
        }

        public bool sumarArmor(int cantidadArmor)
        {
            // Si ya esta en maximo no suma nada
            if (this.Armor == maxArmor)
            {
                return false;
            }

            // Si no esta en el maximo suma algo
            this.Armor = Math.Min(this.Armor += cantidadArmor, maxArmor);

            return true;
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
            PlayerBox.Draw(Camera.View,Camera.Projection);
            base.Draw(gameTime);
        }


        #region CallBacks
        public int StaticCollisionCB(AABB a, AABB b)
        {
            Move(PreviousPosition);
            return 0;
        }
        public int CollectableCollisionCB(ARecolectable r)
        {
            //String timeStamp = GetTimestamp(DateTime.Now);
            //TODO: Use recolectable
            //Debug.WriteLine("[" + timeStamp + "] Collectable Collision: " + r);

            if (Config.recolectablesActivados) 
            {
                // Se delega la responsabilidad de recolectar al recolectable
                r.recolectar(Stage);
            }
            return 0;
        }
        public int ShootableCollisionCB(Enemigo e)
        {
            Debug.WriteLine("Dispare al tgcitoooooo " + Vector3.Transform(Vector3.Zero, e.ModeloTgcitoClassic.World));
            /*if (CurrentWeapon != null)
            {
                Debug.WriteLine("Dispare al tgcitoooooo " + Vector3.Transform(Vector3.Zero, e.ModeloTgcitoClassic.World));
            }*/
            return 0;
        }
        #endregion
    }
}

