using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP
{
    public abstract class AWeaponRecolectable : ARecolectable
    {
        public override void recolectar(IStageBuilder Stage)
        {
            // Todas las armas recolectables tienen la misma logica
            Player.Instance.AgarrarArma(new Weapon(this.Modelo.Model));

            eliminarRecolectableDeLista(Stage);
        }
    }
}
