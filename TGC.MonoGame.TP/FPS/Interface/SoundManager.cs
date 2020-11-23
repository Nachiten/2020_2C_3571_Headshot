using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP.FPS.Interface
{
    class SoundManager
    {

        #region Singleton
        public static SoundManager Instance { get; private set; }
        public static void Init(ContentManager content)
        {
            if (Instance is null)
            {
                Instance = new SoundManager();
                Instance.Content = content;
                Instance.cargarContenido();
            }

        }
        #endregion

        private Song sonidoDisparo;
        private Song recolectarHealth;
        private Song recolectarArmor;
        private Song recolectarWeapon;
        private ContentManager Content;
        private String soundPath = "Sounds/";

        private void cargarContenido() {
            sonidoDisparo = Content.Load<Song>(soundPath + "GunShot");
            recolectarHealth = Content.Load<Song>(soundPath + "HealthPickUp");
            recolectarArmor = Content.Load<Song>(soundPath + "ArmorPickUp");
            recolectarWeapon = Content.Load<Song>(soundPath + "GunPickUp");
        }

        public void reproducirSonido(int idSonido) {
            switch (idSonido) {
                case 1:
                    MediaPlayer.Play(sonidoDisparo);
                    break;
                case 2:
                    MediaPlayer.Play(recolectarHealth);
                    break;
                case 3:
                    MediaPlayer.Play(recolectarArmor);
                    break;
                case 4:
                    MediaPlayer.Play(recolectarWeapon);
                    break;
                default:
                    Debug.WriteLine("ERROR | Codigo de sonido invalido");
                    break;
            }
        }

    }
}
