using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TGC.MonoGame.TP.FPS
{
    public class PlayerGUI : DrawableGameComponent
    {
        public PlayerGUI(Game game) : base(game)
        {
        }

        #region Propiedades
        private SpriteBatch SpriteBatch { get; set; }
        private Button FooterButt;
        private Button HeartButt;
        private Button ArmorButt;

        private SpriteFont InterfaceFont { get; set; }
        #endregion
        public override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FooterButt = ((TGCGame)Game).CreateButton(FPSManager.ContentFolderTextures + "interface/footer", 0, 0.45f);
            HeartButt = ((TGCGame)Game).CreateButton(FPSManager.ContentFolderTextures + "interface/Heart", -0.3f, 0.47f);
            ArmorButt = ((TGCGame)Game).CreateButton(FPSManager.ContentFolderTextures + "interface/Armor", 0.3f, 0.47f);
            InterfaceFont = Game.Content.Load<SpriteFont>(FPSManager.ContentFolderSpriteFonts + "Arial");
        }
        public void Draw(GameTime gameTime, Texture2D RenderTarget)
        {
            SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);

            SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            SpriteBatch.Draw(FooterButt.Texture, FooterButt.Rectangle, Color.White);
            SpriteBatch.Draw(HeartButt.Texture, HeartButt.Rectangle, Color.White);
            SpriteBatch.Draw(ArmorButt.Texture, ArmorButt.Rectangle, Color.White);

            DrawCenteredString("Score " + Player.Instance.Score.ToString(), 0, 0.4f);
            DrawCenteredString(Player.Instance.Health.ToString(), -0.26f, 0.47f);
            DrawCenteredString(Player.Instance.Armor.ToString(), 0.26f, 0.47f);

            SpriteBatch.End();
        }
        public void DrawCenteredString(String str, float offsetXPercent, float offsetYPercent)
        {
            Vector2 strMes = InterfaceFont.MeasureString(str);
            var r = ((TGCGame)Game).resolution;
            Vector2 pos = new Vector2((r.Width / 2) - strMes.X / 2, (r.Height / 2) - strMes.Y / 2);
            pos.X += r.Width * offsetXPercent;
            pos.Y += r.Height * offsetYPercent;
            SpriteBatch.DrawString(InterfaceFont, str, pos, Color.White);
        }
    }
}
