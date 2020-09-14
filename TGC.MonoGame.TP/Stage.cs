using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Samples.Geometries.Textures;
namespace TGC.MonoGame.TP{
    public class Stage {
        public const string ContentFolderTextures = "Textures/";
        private Plane Floor { get; set; }
        private Plane WallDer { get; set; }
        private Plane WallAtr { get; set; }
        private Plane WallIzq { get; set; }
        private Plane WallAdel { get; set; }
        private BoxPrimitive Box { get; set; }
        private Matrix BoxWorld { get; set; }
        public Stage(){

            int largoPiso = 800;
            int anchoPiso = 2000;
            int altoPared = 100;

            Vector3 piso1 = new Vector3(-largoPiso/2,0,-anchoPiso/2);
            Vector3 piso2 = new Vector3(-largoPiso/2,0,anchoPiso/2);
            Vector3 piso3 = new Vector3(largoPiso/2,0,-anchoPiso/2);
            Vector3 piso4 = new Vector3(largoPiso/2,0,anchoPiso/2);

            Floor = new Plane(piso1,piso2,piso3,piso4, new Vector2(5,10));
            WallDer = new Plane(piso2, piso2 + Vector3.Up*altoPared, piso4, piso4 + Vector3.Up*altoPared, new Vector2(10,1));
            WallIzq = new Plane(piso1, piso1 + Vector3.Up*altoPared, piso3, piso3 + Vector3.Up*altoPared, new Vector2(10,1));
            WallAtr = new Plane(piso2, piso2 + Vector3.Up*altoPared, piso1, piso1 + Vector3.Up*altoPared, new Vector2(20,1));
            WallAdel = new Plane(piso3, piso3 + Vector3.Up*altoPared, piso4, piso4 + Vector3.Up*altoPared, new Vector2(20,1));

            BoxWorld = Matrix.CreateTranslation(Vector3.UnitY * 10);   
        }
        public void LoadContent(ContentManager Content,GraphicsDevice GraphicsDevice){
            Floor.LoadTexture(ContentFolderTextures + "sand", Content);
            WallDer.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            WallAtr.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            WallIzq.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            WallAdel.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            Box = new BoxPrimitive(GraphicsDevice, Vector3.One * 20, Content.Load<Texture2D>(ContentFolderTextures + "wood/caja-madera-3"));
        }
        public void Draw(GraphicsDeviceManager Graphics, BasicEffect Effect, Matrix View, Matrix Projection){
            Floor.Draw(Graphics,Effect,View,Projection);
            WallDer.Draw(Graphics,Effect,View,Projection);
            WallAtr.Draw(Graphics,Effect,View,Projection);
            WallIzq.Draw(Graphics,Effect,View,Projection);
            WallAdel.Draw(Graphics,Effect,View,Projection);
            Box.Draw(BoxWorld, View, Projection);
        }
       
    }
}