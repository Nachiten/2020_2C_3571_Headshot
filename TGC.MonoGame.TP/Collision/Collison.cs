using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.Collisions{
    public class Collision{
        public List<AABB> collidableElements { get; set; }
        public Collision(){
            collidableElements = new List<AABB>();
        }
        public void appendStatic(AABB elem){
            collidableElements.Add(elem);
        }
        public void actualCollision(AABB elem,Func<AABB,AABB,int> callback){
            foreach(AABB s in collidableElements){
                if (elem.IntersectAABB(s)){
                    callback(elem,s);
                }
            }
        }
    }
}