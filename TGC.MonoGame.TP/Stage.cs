using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.MonoGame.TP{
    public class Stage {
        private Plane Floor { get; set; }
        private Plane WallDer { get; set; }
        private Plane WallAtr { get; set; }
        private Plane WallIzq { get; set; }
        private Plane WallAdel { get; set; }
        public Stage(){

            int largoPiso = 800;
            int anchoPiso = 2000;
            int altoPared = 100;

            Vector3 piso1 = new Vector3(-largoPiso/2,0,-anchoPiso/2);
            Vector3 piso2 = new Vector3(-largoPiso/2,0,anchoPiso/2);
            Vector3 piso3 = new Vector3(largoPiso/2,0,-anchoPiso/2);
            Vector3 piso4 = new Vector3(largoPiso/2,0,anchoPiso/2);

            Floor = new Plane(piso1,piso2,piso3,piso4, new Vector2(5,5));
            WallDer = new Plane(piso2, piso2 + Vector3.Up*100, piso4, piso4 + Vector3.Up*100, new Vector2(10,1));
            WallIzq = new Plane(piso1, piso1 + Vector3.Up*100, piso3, piso3 + Vector3.Up*100, new Vector2(10,1));
            WallAtr = new Plane(piso2, piso2 + Vector3.Up*100, piso1, piso1 + Vector3.Up*100, new Vector2(10,1));
            WallAdel = new Plane(piso3, piso3 + Vector3.Up*100, piso4, piso4 + Vector3.Up*100, new Vector2(10,1));

            /*Floor = new Plane(new Vector3(-400, 0, -400), new Vector3(-400, 0, 400), new Vector3(400, 0, -400), new Vector3(400, 0, 400), new Vector2(5,5));
            WallDer = new Plane(new Vector3(-400, 0, 400), new Vector3(-400, 100, 400), new Vector3(400, 0, 400), new Vector3(400, 100, 400), new Vector2(10,1));
            WallIzq = new Plane(new Vector3(-400, 0, -400), new Vector3(-400, 100, -400), new Vector3(400, 0, -400), new Vector3(400, 100, -400), new Vector2(10,1));
            WallAtr = new Plane(new Vector3(-400, 0, 400), new Vector3(-400, 100, 400), new Vector3(-400, 0, -400), new Vector3(-400, 100, -400), new Vector2(10,1));
            WallAdel = new Plane(new Vector3(400, 0, -400), new Vector3(400, 100, -400), new Vector3(400, 0, 400), new Vector3(400, 100, 400), new Vector2(10,1));*/
        }
        public void LoadContent(GraphicsDevice GraphicsDevice){
            Floor.LoadTexture(TGCGame.ContentFolderTextures + "sand.jpg", GraphicsDevice);
            WallDer.LoadTexture(TGCGame.ContentFolderTextures + "ladrillo.png", GraphicsDevice);
            WallAtr.LoadTexture(TGCGame.ContentFolderTextures + "ladrillo.png", GraphicsDevice);
            WallIzq.LoadTexture(TGCGame.ContentFolderTextures + "ladrillo.png", GraphicsDevice);
            WallAdel.LoadTexture(TGCGame.ContentFolderTextures + "ladrillo.png", GraphicsDevice);
        }
        public void Draw(GraphicsDeviceManager Graphics, BasicEffect Effect, Matrix View, Matrix Projection){
            Floor.Draw(Graphics,Effect,View,Projection);
            WallDer.Draw(Graphics,Effect,View,Projection);
            WallAtr.Draw(Graphics,Effect,View,Projection);
            WallIzq.Draw(Graphics,Effect,View,Projection);
            WallAdel.Draw(Graphics,Effect,View,Projection);
        }
       
    }
}