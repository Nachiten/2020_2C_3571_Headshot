using Microsoft.Xna.Framework;
using Grafics = Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.FPS
{
    public class Weapon 
    { 
        public Weapon(Grafics.Model weaponModel)
        {
            WeaponModel = weaponModel;
        }
        public Grafics.Model WeaponModel { get; set; }
        public int Damage { get; set; }

        public int SpeedFire { get; set; }

        public Grafics.Model bullet { get; set; }
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
