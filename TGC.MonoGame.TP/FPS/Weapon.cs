using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.FPS
{
    public class Weapon 
    { 
        public Weapon(Model weaponModel)
        {
            WeaponModel = weaponModel;
        }
        public Model WeaponModel { get; set; }
        public int Damage { get; set; }

        public int SpeedFire { get; set; }

        public Model bullet { get; set; }
        //sound!!!
        //range??
        public void Fire()
        {
            //disparar bala
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            WeaponModel.Draw(world, view, projection);
        }
      
    }
}
