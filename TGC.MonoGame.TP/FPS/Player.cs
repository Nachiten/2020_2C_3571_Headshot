using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGC.MonoGame.TP.FPS
{
    public class Player
    {

        //default 100
        public int Health { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public int Speed { get; set; }

        public int Armor { get; set; }

        public Weapon[] Weapons { get; set; }

        public Matrix CurrentPosition { get; set; }
        public void Shoot(Matrix direction)
        {
            //this.weapon.fire()
        }

        public void Move(Matrix direction)
        {
            //CurrentPosition.CreateTranslation(direction) algo asi
        }

        public void Jump()
        {
            //currnt position move y |
        }

        public void ChangeWeapon(int index)
        {
            //this.CurrentWeapon = Weapons[index]
        }

        public void AgarrarArma(Weapon nuevaArma)
        {
            if(Weapons.Length < 3)
            {
                Weapons.Append(nuevaArma);
            }
        }
    }
}
