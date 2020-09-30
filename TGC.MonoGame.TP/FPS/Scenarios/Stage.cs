using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Samples.Geometries.Textures;
using TGC.MonoGame.TP.Collisions;
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
        // Trash code
        Matrix WoodenBoxWorldPared1;
        Matrix WoodenBoxWorldPared2;
        Matrix WoodenBoxWorldPared3;
        Matrix WoodenBoxWorldPared4;
        Matrix SteelBoxWorldCentro1;
        Matrix SteelBoxWorldCentro2;
        Matrix SteelBoxWorldCentro3;
        Matrix SteelBoxWorldCentro4;
        Matrix SteelBoxWorldCentro5;
        Matrix SteelBoxWorldCentro6;
        Matrix SteelBoxWorldCentro7;
        Matrix SteelBoxWorldCentro8;
        Matrix SteelBoxWorldCentro9;
        Matrix SteelBoxWorldCentro10;
        Matrix SteelBoxWorldCentro11;
        Matrix SteelBoxWorldCentro12;
        Matrix SteelBoxWorldCentro13;
        Matrix SteelBoxWorldCentro14;
        Matrix SteelBoxWorldCentro15;
        Matrix SteelBoxWorldCentro16;
        Matrix WoodenBoxWorldCostados1;
        Matrix WoodenBoxWorldCostados2;
        Matrix WoodenBoxWorldCostados3;
        Matrix WoodenBoxWorldCostados4;
        Matrix WoodenBoxWorldCostados5;
        Matrix WoodenBoxWorldCostados6;
        Matrix WoodenBoxWorldCostados7;
        Matrix WoodenBoxWorldCostados8;
        public Stage(Collision Collison){
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

            // Trash code
            WoodenBoxWorldPared1 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * largoPiso/2);
            WoodenBoxWorldPared2 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (largoPiso/2 - WoodenBoxSize));
            WoodenBoxWorldPared3 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-largoPiso/2 + WoodenBoxSize));
            WoodenBoxWorldPared4 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-largoPiso/2 + 2*WoodenBoxSize));

            SteelBoxWorldCentro1 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes);
            SteelBoxWorldCentro2 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes);
            SteelBoxWorldCentro3 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro4 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro5 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes);
            SteelBoxWorldCentro6 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes);
            SteelBoxWorldCentro7 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro8 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro9 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes);
            SteelBoxWorldCentro10 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes);
            SteelBoxWorldCentro11 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro12 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro13 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes);
            SteelBoxWorldCentro14 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes);
            SteelBoxWorldCentro15 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize));
            SteelBoxWorldCentro16 = SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize));

            WoodenBoxWorldCostados1 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize));
            WoodenBoxWorldCostados2 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize));
            WoodenBoxWorldCostados3 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize));
            WoodenBoxWorldCostados4 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize));
            WoodenBoxWorldCostados5 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize));
            WoodenBoxWorldCostados6 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize));
            WoodenBoxWorldCostados7 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize));
            WoodenBoxWorldCostados8 = WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize));


            //AABB
            // Paredes
            AABB box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldPared2);
            Collison.appendStatic(box);

            box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldPared4);
            Collison.appendStatic(box);

            // Centro
            box = new AABB(new Vector3(2*SteelBoxSize,2*SteelBoxSize,SteelBoxSize));
            box.Translation(SteelBoxWorldCentro1);
            Collison.appendStatic(box);
            
            box = new AABB(new Vector3(2*SteelBoxSize,2*SteelBoxSize,SteelBoxSize));
            box.Translation(SteelBoxWorldCentro5);
            Collison.appendStatic(box);
            
            box = new AABB(new Vector3(2*SteelBoxSize,2*SteelBoxSize,SteelBoxSize));
            box.Translation(SteelBoxWorldCentro9);
            Collison.appendStatic(box);

            box = new AABB(new Vector3(2*SteelBoxSize,2*SteelBoxSize,SteelBoxSize));
            box.Translation(SteelBoxWorldCentro13);
            Collison.appendStatic(box);

            // Costados
            box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldCostados1);
            Collison.appendStatic(box);
            
            box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldCostados3);
            Collison.appendStatic(box);
            
            box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldCostados5);
            Collison.appendStatic(box);

            box = new AABB(new Vector3(2*WoodenBoxSize,WoodenBoxSize,WoodenBoxSize));
            box.Translation(WoodenBoxWorldCostados7);
            Collison.appendStatic(box);
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
            WoodenBox.Draw(WoodenBoxWorldPared1, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldPared2, View, Projection);

            WoodenBox.Draw(WoodenBoxWorldPared3, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldPared4, View, Projection);
            
            // Cajas Centro
            SteelBox.Draw(SteelBoxWorldCentro1, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro2, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro3, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro4, View, Projection);

            SteelBox.Draw(SteelBoxWorldCentro5, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro6, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro7, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro8, View, Projection);

            SteelBox.Draw(SteelBoxWorldCentro9, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro10, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro11, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro12, View, Projection);

            SteelBox.Draw(SteelBoxWorldCentro13, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro14, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro15, View, Projection);
            SteelBox.Draw(SteelBoxWorldCentro16, View, Projection);

            // Cajas costados
            WoodenBox.Draw(WoodenBoxWorldCostados1, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldCostados2, View, Projection);

            WoodenBox.Draw(WoodenBoxWorldCostados3, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldCostados4, View, Projection);

            WoodenBox.Draw(WoodenBoxWorldCostados5, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldCostados6, View, Projection);

            WoodenBox.Draw(WoodenBoxWorldCostados7, View, Projection);
            WoodenBox.Draw(WoodenBoxWorldCostados8, View, Projection);

        }
       
    }
}