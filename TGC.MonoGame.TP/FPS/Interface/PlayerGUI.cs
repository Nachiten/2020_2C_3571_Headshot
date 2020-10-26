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
        private Texture2D FooterTexture { get; set; }

        private Texture2D HeartTexture { get; set; }
        private Texture2D ArmorTexture { get; set; }

        private SpriteFont InterfaceFont { get; set; }
        #endregion
        public override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FooterTexture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "interface/footer");
            HeartTexture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "interface/Heart");
            ArmorTexture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "interface/Armor");
            InterfaceFont = Game.Content.Load<SpriteFont>(FPSManager.ContentFolderSpriteFonts + "Arial");
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);

            Vector2 location = new Vector2(0, 380);
            Rectangle sourceRectangle = new Rectangle(0, 0, 900, 300);
            Vector2 origin = new Vector2(0, 0);

            SpriteBatch.Draw(FooterTexture, location, sourceRectangle, Color.White, 0.01f, origin, .87f, SpriteEffects.None, 1);

            SpriteBatch.Draw(HeartTexture, new Vector2(60, 440), new Rectangle(0, 0, 32, 32), Color.White, 0.01f, origin, .87f, SpriteEffects.None, 1);
            SpriteBatch.Draw(ArmorTexture, new Vector2(710, 440), new Rectangle(0, 0, 32, 32), Color.White, 0.01f, origin, .87f, SpriteEffects.None, 1);

            if (Player.Instance.Health <= 0)
            {
                SpriteBatch.DrawString(InterfaceFont, "Dead", new Vector2(50, 432), Color.White);
            }
            else
            {
                SpriteBatch.DrawString(InterfaceFont, Player.Instance.Health.ToString(), new Vector2(100, 440), Color.White);
                SpriteBatch.DrawString(InterfaceFont, Player.Instance.Armor.ToString(), new Vector2(660, 440), Color.White);
            }
            SpriteBatch.End();

        }
    }
}
