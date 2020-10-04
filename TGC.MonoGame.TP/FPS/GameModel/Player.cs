﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGC.MonoGame.TP.FPS
{
    public class Player: DrawableGameComponent
    {
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
        public int Speed { get; set; }

        public int Armor { get; set; }

        public Weapon[] Weapons { get; set; }

        public Vector3 CurrentPosition { get; set; }

        #endregion
        //default 100

        public override void Initialize()
        {
            Health = 100;

            Weapons = new Weapon[3];
            WorldWeapon = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(50, 0, 110);
            View = Matrix.CreateLookAt(new Vector3(30, 20, 150), new Vector3(30, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

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

        public void Move()
        {
            //CurrentPosition += moveTo;
            //CurrentPosition.CreateTranslation(direction) algo asi
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
    }
}

