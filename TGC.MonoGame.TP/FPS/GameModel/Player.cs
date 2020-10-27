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
    public class Player : Ashootable, IDisposable
    {
        public static Player Instance { get; private set; }

        public static void Init(Game game, FreeCamera camera, AStage stage)
        {
            if (Instance is null)
            {
                Instance = new Player(game);
                Camera = camera;
                Instance.Initialize();
                Stage = stage;
            }

        }
        public void Dispose()
        {
            Player.Instance = null;
        }

        public Player(Game game)
        {
            GraphicsDevice = game.GraphicsDevice;
        }

        #region Componentes


        #endregion

        #region animation
        public bool TriggerShot = false;
        double frameTimePlayed = 0; //Amount of time (out of animationTime) that the animation has been playing for
        bool IsAnimating = false; //Self-Explanitory
        int animationTime = 500; //For .5 seconds of animation.
        Matrix PreviousWorldWeapon;
        #endregion

        #region Propiedades
        GraphicsDevice GraphicsDevice;

        private Matrix WorldWeapon { get; set; }
        private Matrix Projection { get; set; }

        private Matrix View { get; set; }
        public int Health { get; set; }
        private int maxHealth = 100;
        public int Armor { get; set; }
        private int maxArmor = 100;

        public int Score = 0;
        public Weapon CurrentWeapon { get; set; }

        public float Speed = 300f;

        public Weapon[] Weapons { get; set; }

        public Vector3 CurrentPosition { get; set; }

        public Vector3 Position;

        public Vector3 PreviousPosition;

        static FreeCamera Camera;
        static AStage Stage;

        #endregion

        public void Initialize()
        {
            Health = maxHealth;
            Armor = maxArmor;

            Weapons = new Weapon[2];
            WorldWeapon = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(50, 0, 110);
            View = Matrix.CreateLookAt(new Vector3(30, 20, 150), new Vector3(30, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            Aabb = new AABB(GraphicsDevice,new Vector3(20, 80, 20));
            Collision.Instance.AppendShootable(this);

            Move(new Vector3(100, 100, 100));

            //inicializar el current weapon con Fist
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
            Aabb.Translation(Position);
        }
        public void Update(GameTime GameTime)
        {
            var elapsedTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            MouseManager.Instance.ViewChanged = false;
            KeyboardManager.Instance.Update(elapsedTime, this);
            MouseManager.Instance.Update(elapsedTime, ShootableCollisionCB);

            Collision.Instance.CheckStatic(Aabb, StaticCollisionCB);
            Collision.Instance.CheckCollectable(Aabb, CollectableCollisionCB);

            //If it is not already animating and there is a trigger, start animating
            if (!IsAnimating && TriggerShot)
            {
                IsAnimating = true;
                PreviousWorldWeapon = WorldWeapon;
            }

            //Increment the frameTimePlayed by the time (in milliseconds) since the last frame
            if (IsAnimating)
                frameTimePlayed += GameTime.ElapsedGameTime.TotalMilliseconds;
            //If playing and we have not exceeded the time limit
            if (IsAnimating && frameTimePlayed < animationTime)
            {
                WorldWeapon *= Matrix.CreateTranslation(Vector3.UnitZ * (float)Math.Sin(frameTimePlayed/80)*0.5f);
                // TODO: Add update logic here, such as animation.Update()
                // And increment your frames (Using division to figure out how many frames per second)
            }
            //If exceeded time, reset variables and stop playing
            else if (IsAnimating && frameTimePlayed >= animationTime)
            {
                WorldWeapon = PreviousWorldWeapon;
                frameTimePlayed = 0;
                IsAnimating = false;
                TriggerShot = false;
                // TODO: Possibly custom animation.Stop(), depending on your animation class
            }
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
        public void AddScore(int score)
        {
            Score += score;
        }

        public void Jump()
        {
            //currnt position move y |
        }

        public void ChangeWeapon(int index)
        {
            if(Weapons[index-1] != null)
            {
                CurrentWeapon = Weapons[index-1];
            }   
        }

        public void AgarrarArma(Weapon nuevaArma)
        {
            Weapons[nuevaArma.Index-1] = nuevaArma;
            CurrentWeapon = nuevaArma;   
        }
        
        public void Draw(GameTime gameTime)
        {
            if (CurrentWeapon != null) {
                CurrentWeapon.Draw(WorldWeapon, View, Projection);
            }
        }

        public override void GetDamaged(int damage)
        {
            int armorDamage = damage;
            int healthDamage;
            if (Armor < 0)
                healthDamage = damage;
            else
                healthDamage = damage / 2;
            // Armadura
            if (Armor - armorDamage < 0)
            {
                Armor = 0;
            }
            else
            {
                Armor -= armorDamage;
            }
            // Vida
            if (Health - healthDamage < 0)
            {
                Health = 0;
            }
            else
            {
                Health -= healthDamage;
            }
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
        public int ShootableCollisionCB(Ashootable e)
        {
            //Debug.WriteLine("!!!!Dispare al tgcitoooooo " + Vector3.Transform(Vector3.Zero, ((Enemigo)e).ModeloTgcitoClassic.World));
            
            if (CurrentWeapon != null)
            {
                Debug.WriteLine("!!!!Dispare al tgcitoooooo ");
                e.GetDamaged(CurrentWeapon.Damage);
            }
            return 0;
        }

        #endregion
        
    }
}

