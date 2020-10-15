using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.FPS;

namespace TGC.MonoGame.TP
{
    public abstract class AWeaponRecolectable : ARecolectable
    {
        public override void recolectar()
        {
            // Todas las armas recolectables tienen la misma logica
            Player.Instance.AgarrarArma(new Weapon(this.Modelo.Model));
        }
    }
}
