using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TGC.MonoGame.TP.Utils{
    public class Collision{
        public static Collision Instance { get; private set; }
        public List<AABB> StaticElements = new List<AABB>();
        public List<Recolectable> CollectableElements = new List<Recolectable>();
        public Collision(){
        }
        public void AppendStatic(AABB elem){
            //Console.WriteLine(elem.size.ToString());
            StaticElements.Add(elem);
        }
        public void AppendCollectable(Recolectable elem)
        {
            CollectableElements.Add(elem);
        }
        public void RemoveCollectable(Recolectable elem)
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
        public void CheckCollectable(AABB elem, Func<Recolectable, int> callback)
        {
            foreach (Recolectable r in CollectableElements.ToArray())
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