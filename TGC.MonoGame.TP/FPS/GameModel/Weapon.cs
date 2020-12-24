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

        public void SetEffect(Effect Effect)
        {
            Gun.SetEffect(Effect);
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Matrix WorldModified;
            switch (Index)
            {
                case 2:
                    WorldModified = world * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(new Vector3(100, 0, 220));
                    break;
                case 3:
                    WorldModified = world * Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateTranslation(new Vector3(80, 0, 100));
                    break;
                default:
                    WorldModified = world;
                    break;
            }
            Gun.Draw(WorldModified, view, projection);
        }
      
    }
}
