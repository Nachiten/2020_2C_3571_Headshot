using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace TGC.MonoGame.TP.Utils{
    public class AABB{
        public Vector3 maxExtents { get; set; }
        public Vector3 minExtents { get; set; }
        //public Matrix world;
        public Vector3 size;
        BoundingBox boundingBox;

        public AABB(Vector3 size){
            this.size = size;
            minExtents = -size;
            maxExtents = size;
            boundingBox = new BoundingBox(minExtents,maxExtents);
            //world = Matrix.CreateRotationY(MathHelper.Pi);
        }
        public AABB(Vector3 min, Vector3 max)
        {
            minExtents = min;
            maxExtents = max;
            boundingBox = new BoundingBox(minExtents, maxExtents);
            //world = Matrix.CreateRotationY(MathHelper.Pi);
        }
        public bool IntersectAABB(AABB other){
            Vector3 distance1 = other.minExtents - maxExtents;
            Vector3 distance2 = minExtents - other.maxExtents;
            Vector3 distance = Vector3.Max(distance1,distance2);
            Console.WriteLine(boundingBox.Intersects(other.boundingBox));
            return MaxCoord(distance) < 0;
        }
        public float? IntersectRay(Ray ray){
            return boundingBox.Intersects(ray);
        }
        public void Translation(Vector3 position){
            minExtents = position - size;
            maxExtents = position + size;
        }
        public void Translation(Matrix world){
            Vector3 pos = Vector3.Transform(Vector3.Zero, world);
            minExtents = pos - size;
            maxExtents = pos + size;
        }
        private float MaxCoord(Vector3 v){
            if(v.X > v.Y && v.X > v.Z) {
                return v.X;
            } else{
                return v.Y > v.Z ? v.Y : v.Z;
            }
        }
    }
}