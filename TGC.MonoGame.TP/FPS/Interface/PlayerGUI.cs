using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TGC.MonoGame.TP.FPS
{
    public class PlayerGUI : DrawableGameComponent
    {
        public PlayerGUI(Game game) : base(game)
        {
        }

        #region Propiedades
        private SpriteBatch SpriteBatch { get; set; }
        private Texture2D FooterGUI { get; set; }

        private SpriteFont InterfaceFont { get; set; }

        public Player Player { get; set; }
        #endregion
        public override void Initialize()
        {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            FooterGUI = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "interface/footer");
            InterfaceFont = Game.Content.Load<SpriteFont>(FPSManager.ContentFolderSpriteFonts + "Arial");

            base.Initialize();
        }
        public void Initialize(Player player)
        {
            Player = player;
            Initialize();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            Vector2 location = new Vector2(0, 380);
            Rectangle sourceRectangle = new Rectangle(0, 0, 900, 300);
            Vector2 origin = new Vector2(0, 0);

            SpriteBatch.Draw(FooterGUI, location, sourceRectangle, Color.White, 0.01f, origin, .87f, SpriteEffects.None, 1);


            //spriteBatch.Draw(map, new Vector2(10, 10), new Rectangle(0, 0, 500, 500), Color.White, 0.01f, origin, .30f, SpriteEffects.None, 1);
            //spriteBatch.Draw(bullets, new Vector2(750, 440), new Rectangle(0, 0, 250, 250), Color.White, 0.01f, origin, .095f, SpriteEffects.None, 1);


            if (Player.Health <= 0)
            {
                SpriteBatch.DrawString(InterfaceFont, "Dead", new Vector2(50, 432), Color.White);
            }
            else {
                SpriteBatch.DrawString(InterfaceFont, Player.Health.ToString(), new Vector2(50, 432), Color.White);
            }
            
            //spriteBatch.DrawString(FontTexture, "50/250", new Vector2(660, 440), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
