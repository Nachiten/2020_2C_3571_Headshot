using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.FPS
{
    public abstract class Weapon: DrawableGameComponent
    {
        protected Weapon(Game game) : base(game)
        {

        }
        private Model WeaponModel { get; set; }
        public int Damage { get; set; }

        public int SpeedFire { get; set; }

        public Model bullet { get; set; }
        //sound!!!

        //range??
        public void Fire()
        {
            //disparar bala
        }
        public void Draw()
        {

        }

    }
}
