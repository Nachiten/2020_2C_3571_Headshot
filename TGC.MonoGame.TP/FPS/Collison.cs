using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TGC.MonoGame.TP.Utils{
    public class Collision{
        public static Collision Instance { get; private set; }
        public List<AABB> StaticElements = new List<AABB>();
        public List<ARecolectable> CollectableElements = new List<ARecolectable>();
        public Collision(){
        }
        public void AppendStatic(AABB elem){
            //Console.WriteLine(elem.size.ToString());
            StaticElements.Add(elem);
        }
        public void AppendCollectable(ARecolectable elem)
        {
            CollectableElements.Add(elem);
        }
        public void RemoveCollectable(ARecolectable elem)
        {
            CollectableElements.Remove(elem);
        }
        public void CheckStatic(AABB elem, Func<AABB, AABB, int> callback)
        {
            foreach(AABB s in StaticElements.Where(x => !x.Equals(elem)).ToList())
            {
                if (elem.IntersectAABB(s))
                {
                    callback(elem,s);
                }
            }
        }
        public void CheckCollectable(AABB elem, Func<ARecolectable, int> callback)
        {
            foreach (ARecolectable r in CollectableElements.ToArray())
            {
                if (elem.IntersectAABB(r.Modelo.Aabb))
                {
                    callback(r);
                }
            }
        }
        public static void Init()
        {
            if (Instance is null)
            {
                Instance = new Collision();
            }
            
        }
    }
}