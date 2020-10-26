using Microsoft.Xna.Framework;
using System.Diagnostics;
using Grafics = Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.FPS
{
    public class Weapon 
    {
        public int Index;
        public Weapon(AWeaponRecolectable weapon)
        {
            Gun = weapon;
            Damage = Gun.Damage;
            Index = Gun.Index;
        }
        public AWeaponRecolectable Gun { get; set; }
        public int Damage { get; set; }

        public int SpeedFire { get; set; }

        //public Grafics.Model bullet { get; set; }
        //sound!!!
        //range??
        public void Fire()
        {
            //disparar bala
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            if (Gun.Scalable)
            {
                Gun.Modelo.Model.Draw(world * Matrix.CreateScale(Gun.tamanioModelo*0.8f) * Matrix.CreateTranslation(Vector3.UnitZ * 55 + Vector3.UnitX * 65 - Vector3.UnitY * 15) , view, projection);
            }
            else
            {
                Gun.Modelo.Model.Draw(world, view, projection);
            }
        }
      
    }
}
