using Microsoft.Xna.Framework;
using System.Diagnostics;
using TGC.MonoGame.TP.Utils;
using Microsoft.Xna.Framework.Graphics;

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

        public void SetEffect(Effect Effect)
        {
            Gun.SetEffect(Effect);
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Matrix WorldModified;
            /*if (Gun.Scalable)
            {
                WorldModified = world * Matrix.CreateScale(Gun.tamanioModelo * 0.8f) * Matrix.CreateTranslation(new Vector3(65,-15,55));
            }
            else
            {
                WorldModified = world;
            }*/
            if(Index == 2)
            {
                WorldModified = world * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(new Vector3(100, 0, 220));
            } else
            {
                WorldModified = world;
            }
            Gun.Draw(WorldModified, view, projection);
        }
      
    }
}
