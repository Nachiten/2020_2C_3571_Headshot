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
        private BoxPrimitive WoodenBox { get; set; }
        private Matrix WoodenBoxWorld { get; set; }
        private BoxPrimitive SteelBox { get; set; }
        private Matrix SteelBoxWorld { get; set; }
        private int SteelBoxSize = 80;
        private int WoodenBoxSize = 40;
        int largoPiso = 800;
        int anchoPiso = 1000;
        int altoPared = 100;
        int gapMiddleBoxes = 100;
        public Stage(){
            Vector3 piso1 = new Vector3(-largoPiso/2,0,-anchoPiso/2);
            Vector3 piso2 = new Vector3(-largoPiso/2,0,anchoPiso/2);
            Vector3 piso3 = new Vector3(largoPiso/2,0,-anchoPiso/2);
            Vector3 piso4 = new Vector3(largoPiso/2,0,anchoPiso/2);

            Floor = new Plane(piso1,piso2,piso3,piso4, new Vector2(5,10));
            WallDer = new Plane(piso2, piso2 + Vector3.Up * altoPared, piso4, piso4 + Vector3.Up * altoPared, new Vector2(10,1));
            WallIzq = new Plane(piso1, piso1 + Vector3.Up * altoPared, piso3, piso3 + Vector3.Up * altoPared, new Vector2(10,1));
            WallAtr = new Plane(piso2, piso2 + Vector3.Up * altoPared, piso1, piso1 + Vector3.Up * altoPared, new Vector2(8,1));
            WallAdel = new Plane(piso3, piso3 + Vector3.Up * altoPared, piso4, piso4 + Vector3.Up * altoPared, new Vector2(8,1));

            WoodenBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * WoodenBoxSize/2 - Vector3.UnitX * WoodenBoxSize/2 - Vector3.UnitZ * WoodenBoxSize/2);
            SteelBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * SteelBoxSize/2);
        }
        public void LoadContent(ContentManager Content,GraphicsDevice GraphicsDevice){
            Floor.LoadTexture(ContentFolderTextures + "sand", Content);
            WallDer.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            WallAtr.LoadTexture(ContentFolderTextures + "rusty", Content);
            WallIzq.LoadTexture(ContentFolderTextures + "ladrillo", Content);
            WallAdel.LoadTexture(ContentFolderTextures + "rusty", Content);
            WoodenBox = new BoxPrimitive(GraphicsDevice, Vector3.One * WoodenBoxSize, Content.Load<Texture2D>(ContentFolderTextures + "wood/caja-madera-3"));
            SteelBox = new BoxPrimitive(GraphicsDevice, Vector3.One * SteelBoxSize, Content.Load<Texture2D>(ContentFolderTextures + "steel"));
        }
        public void Draw(GraphicsDeviceManager Graphics, BasicEffect Effect, Matrix View, Matrix Projection){
            // Paredes
            Floor.Draw(Graphics,Effect,View,Projection);
            WallDer.Draw(Graphics,Effect,View,Projection);
            WallAtr.Draw(Graphics,Effect,View,Projection);
            WallIzq.Draw(Graphics,Effect,View,Projection);
            WallAdel.Draw(Graphics,Effect,View,Projection);

            // Cajas Pared
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * largoPiso/2), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (largoPiso/2 - WoodenBoxSize)), View, Projection);

            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-largoPiso/2 + WoodenBoxSize)), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-largoPiso/2 + 2*WoodenBoxSize)), View, Projection);
            
            // Cajas Centro
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBox.Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);

            // Cajas costados
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);

            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);

            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);

            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);
            WoodenBox.Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);

        }
       
    }
}