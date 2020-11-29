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

        // Effects
        private SoundEffect sonidoDisparo;
        private SoundEffect recolectarHealth;
        private SoundEffect recolectarArmor;
        private SoundEffect recolectarWeapon;
        private SoundEffect pegarEnemigo;
        private SoundEffect matarEnemigo;
        private SoundEffect pegarJugador;
        // Songs
        private Song muerteJugadorSong;
        private Song menuSong;
        private Song librarySong;
        private Song mazeSong;
        // Other Stuff
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
            MuerteJugador,
            PegarJugador,
            Menu
        }
        public enum Musica
        {
            LibraryStage = 1,
            MazeStage
        }

        private void cargarContenido() {
            // Efectos de sonido
            sonidoDisparo = Content.Load<SoundEffect>(soundPath + "GunShot");
            recolectarHealth = Content.Load<SoundEffect>(soundPath + "HealthPickUp");
            recolectarArmor = Content.Load<SoundEffect>(soundPath + "ArmorPickUp");
            recolectarWeapon = Content.Load<SoundEffect>(soundPath + "GunPickUp");
            pegarEnemigo = Content.Load<SoundEffect>(soundPath + "EnemyHitOof");
            matarEnemigo = Content.Load<SoundEffect>(soundPath + "EnemyKillWindowsXP");
            pegarJugador = Content.Load<SoundEffect>(soundPath + "PlayerHit");

            // Canciones
            muerteJugadorSong = Content.Load<Song>(soundPath + "PlayerDeathTitanic");
            menuSong = Content.Load<Song>(soundPath + "vaporwave");
            librarySong = Content.Load<Song>(soundPath + "yyz");
            mazeSong = Content.Load<Song>(soundPath + "yyz");
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
                    MediaPlayer.Play(muerteJugadorSong);
                    break;
                case Sonido.Menu:
                    MediaPlayer.Play(menuSong);
                    break;
                case Sonido.PegarJugador:
                    pegarJugador.Play();
                    break;
                default:
                    Debug.WriteLine("ERROR | Codigo de sonido invalido");
                    break;
            }
        }

        public void detenerMusica() {
            MediaPlayer.Stop();
        }

        public void comenzarMusica(Musica idMusica) {
            switch (idMusica)
            {
                case Musica.LibraryStage:
                    MediaPlayer.Play(librarySong);
                    break;
                case Musica.MazeStage:
                    MediaPlayer.Play(mazeSong);
                    break;
                default:
                    Debug.WriteLine("ERROR | Codigo de cancion invalido");
                    break;
            }
        }

    }
}
