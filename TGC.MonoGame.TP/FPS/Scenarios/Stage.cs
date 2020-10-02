using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.Samples.Geometries.Textures;
using TGC.MonoGame.TP.Collisions;
using System.Collections.Generic;
namespace TGC.MonoGame.TP{
    public class Stage {
        public const string ContentFolderTextures = "Textures/";
        private QuadPrimitive Floor { get; set; }
        private QuadPrimitive WallXp { get; set; }
        private QuadPrimitive WallZn { get; set; }
        private QuadPrimitive WallXn { get; set; }
        private QuadPrimitive WallZp { get; set; }
        private Matrix WoodenBoxWorld { get; set; }
        private Matrix SteelBoxWorld { get; set; }
        private int SteelBoxSize = 80;
        private int WoodenBoxSize = 40;
        float xLenFloor = 800;
        float zLenFloor = 1000;
        int yLenWall = 100;
        int gapMiddleBoxes = 100;
        List<BoxPrimitiveCollidable> SteelBoxes { get; set; }
        List<BoxPrimitiveCollidable> WoodenBoxes { get; set; }
        public Stage(){
            SteelBoxes = new List<BoxPrimitiveCollidable>();
            WoodenBoxes = new List<BoxPrimitiveCollidable>();
            WoodenBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * WoodenBoxSize/2 - Vector3.UnitX * WoodenBoxSize/2 - Vector3.UnitZ * WoodenBoxSize/2);
            SteelBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * SteelBoxSize/2);
        }
        public void LoadContent(ContentManager Content,GraphicsDevice GraphicsDevice,Collision Collision){
            Floor = new QuadPrimitiveCollidable(GraphicsDevice,Vector3.Zero,Vector3.UnitY,Vector3.UnitX,zLenFloor,xLenFloor,
                Content.Load<Texture2D>(ContentFolderTextures + "sand"),new Vector2(10,10),Collision);
            
            WallXp = new QuadPrimitiveCollidable(GraphicsDevice,new Vector3(xLenFloor/2, yLenWall/2, 0),-Vector3.UnitX,Vector3.UnitY,zLenFloor,yLenWall,
                Content.Load<Texture2D>(ContentFolderTextures + "rusty"), new Vector2(8,1),Collision);
            WallXn = new QuadPrimitiveCollidable(GraphicsDevice,new Vector3(-xLenFloor/2, yLenWall/2, 0),Vector3.UnitX,Vector3.UnitY,zLenFloor,yLenWall,
                Content.Load<Texture2D>(ContentFolderTextures + "rusty"), new Vector2(8,1),Collision);
            WallZp = new QuadPrimitiveCollidable(GraphicsDevice,new Vector3(0, yLenWall/2, zLenFloor/2),-Vector3.UnitZ,Vector3.UnitY,xLenFloor,yLenWall,
                Content.Load<Texture2D>(ContentFolderTextures + "ladrillo"), new Vector2(10,1),Collision);
            WallZn = new QuadPrimitiveCollidable(GraphicsDevice,new Vector3(0, yLenWall/2, -zLenFloor/2),Vector3.UnitZ,Vector3.UnitY,xLenFloor,yLenWall,
                Content.Load<Texture2D>(ContentFolderTextures + "ladrillo"), new Vector2(10,1),Collision);

            for(int x=0;x<12;x++){
                WoodenBoxes.Add(new BoxPrimitiveCollidable(GraphicsDevice, Vector3.One * WoodenBoxSize, Content.Load<Texture2D>(ContentFolderTextures + "wood/caja-madera-3"),Collision));
            }
            for(int x=0;x<16;x++){
                SteelBoxes.Add(new BoxPrimitiveCollidable(GraphicsDevice, Vector3.One * SteelBoxSize, Content.Load<Texture2D>(ContentFolderTextures + "steel"),Collision));
            }
        }
        public void Draw(GraphicsDeviceManager Graphics, BasicEffect Effect, Matrix View, Matrix Projection){
            // Paredes
            Floor.Draw(Matrix.CreateTranslation(Vector3.Zero),View,Projection);
            WallXp.Draw(Matrix.CreateTranslation(Vector3.Zero),View,Projection);
            WallXn.Draw(Matrix.CreateTranslation(Vector3.Zero),View,Projection);
            WallZp.Draw(Matrix.CreateTranslation(Vector3.Zero),View,Projection);
            WallZn.Draw(Matrix.CreateTranslation(Vector3.Zero),View,Projection);

            // Cajas Pared
            WoodenBoxes[0].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * xLenFloor/2), View, Projection);
            WoodenBoxes[1].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (xLenFloor/2 - WoodenBoxSize)), View, Projection);

            WoodenBoxes[2].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-xLenFloor/2 + WoodenBoxSize)), View, Projection);
            WoodenBoxes[3].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (-xLenFloor/2 + 2*WoodenBoxSize)), View, Projection);
            
            // Cajas Centro
            SteelBoxes[0].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBoxes[1].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBoxes[2].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBoxes[3].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBoxes[4].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBoxes[5].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * gapMiddleBoxes), View, Projection);
            SteelBoxes[6].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBoxes[7].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBoxes[8].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBoxes[9].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBoxes[10].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBoxes[11].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);

            SteelBoxes[12].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBoxes[13].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -gapMiddleBoxes), View, Projection);
            SteelBoxes[14].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);
            SteelBoxes[15].Draw(SteelBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + SteelBoxSize)), View, Projection);

            // Cajas costados
            WoodenBoxes[4].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);
            WoodenBoxes[5].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);

            WoodenBoxes[6].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);
            WoodenBoxes[7].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * (gapMiddleBoxes + 2*SteelBoxSize + WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);

            WoodenBoxes[8].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);
            WoodenBoxes[9].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * (gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize)), View, Projection);

            WoodenBoxes[10].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);
            WoodenBoxes[11].Draw(WoodenBoxWorld * Matrix.CreateTranslation(Vector3.UnitX * -(gapMiddleBoxes + 2*SteelBoxSize - WoodenBoxSize) + Vector3.UnitZ * -(gapMiddleBoxes + 2*SteelBoxSize - 2*WoodenBoxSize)), View, Projection);

        }
       
    }
}