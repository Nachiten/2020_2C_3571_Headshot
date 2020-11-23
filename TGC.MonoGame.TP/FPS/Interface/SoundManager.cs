using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

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

        private SoundEffect sonidoDisparo;
        private SoundEffect recolectarHealth;
        private SoundEffect recolectarArmor;
        private SoundEffect recolectarWeapon;
        private SoundEffect pegarEnemigo;
        private SoundEffect matarEnemigo;
        private Song muerteJugador;
        private Song cancionBackground;
        private ContentManager Content;
        private String soundPath = "Sounds/";

        public enum Sonido
        {
            Disparo = 1,
            RecolectarHealth,
            RecolectarArmor,
            RecolectarWeapon,
            PegarEnemigo,
            MatarEnemigo,
            MuerteJugador
        }

        private void cargarContenido() {
            // Efectos de sonido
            sonidoDisparo = Content.Load<SoundEffect>(soundPath + "GunShot");
            recolectarHealth = Content.Load<SoundEffect>(soundPath + "HealthPickUp");
            recolectarArmor = Content.Load<SoundEffect>(soundPath + "ArmorPickUp");
            recolectarWeapon = Content.Load<SoundEffect>(soundPath + "GunPickUp");
            pegarEnemigo = Content.Load<SoundEffect>(soundPath + "EnemyHitOof");
            matarEnemigo = Content.Load<SoundEffect>(soundPath + "EnemyKillWindowsXP");

            // Canciones
            muerteJugador = Content.Load<Song>(soundPath + "PlayerDeathTitanic");
            cancionBackground = Content.Load<Song>(soundPath + "Song");
        }

        public void reproducirSonido(Sonido idSonido) {
            switch (idSonido) {
                case Sonido.Disparo:
                    sonidoDisparo.Play();
                    break;
                case Sonido.RecolectarHealth:
                    recolectarHealth.Play();
                    break;
                case Sonido.RecolectarArmor:
                    recolectarArmor.Play();
                    break;
                case Sonido.RecolectarWeapon:
                    recolectarWeapon.Play();
                    break;
                case Sonido.PegarEnemigo:
                    pegarEnemigo.Play();
                    break;
                case Sonido.MatarEnemigo:
                    matarEnemigo.Play();
                    break;
                case Sonido.MuerteJugador:
                    MediaPlayer.Play(muerteJugador);
                    break;
                default:
                    Debug.WriteLine("ERROR | Codigo de sonido invalido");
                    break;
            }
        }

        public void detenerMusica() {
            MediaPlayer.Stop();
        }

        public void comenzarMusica() {
            MediaPlayer.Play(cancionBackground);
        }

    }
}
