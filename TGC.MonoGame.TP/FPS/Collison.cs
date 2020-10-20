using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.FPS;

namespace TGC.MonoGame.TP.Utils{
    public class Collision{
        public static Collision Instance { get; private set; }
        public List<AABB> StaticElements = new List<AABB>();

        public List<ARecolectable> CollectableElements = new List<ARecolectable>();
        public List<Ashootable> ShootableElements = new List<Ashootable>();

        public Collision(){
        }
        public void AppendStatic(AABB elem){
            StaticElements.Add(elem);
        }

        public void AppendShootable(Ashootable elem)
        {
            ShootableElements.Add(elem);
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
            if (!Config.colisionesActivadas) return;

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
            if (!Config.colisionesActivadas) return;

            foreach (ARecolectable r in CollectableElements.ToArray())
            {
                if (elem.IntersectAABB(r.Modelo.Aabb))
                {
                    callback(r);
                }
            }
        }
        public void CheckShootable(Ray Ray, Func<Ashootable, int> callback)
        {
            Ashootable ObjectShooted = null;
            float distanceshoot = -1;
            bool ActualShot = false;

            foreach (Ashootable e in ShootableElements.Where(x => !x.Aabb.PlayerBox))
            {
                var colDis = e.Aabb.IntersectRay(Ray);
                if (colDis != null)
                {
                    ObjectShooted = e;
                    distanceshoot = (float)colDis;
                    ActualShot = true;
                }
            }
            if(ObjectShooted != null)
            {
                IEnumerable<AABB> EnemyAABB = ShootableElements.Select(x => x.Aabb);
                IEnumerable<AABB> FilteredList = StaticElements.Where(x => !EnemyAABB.Contains(x));
                foreach (AABB s in FilteredList)
                {
                    var colDis = s.IntersectRay(Ray);
                    if (colDis != null)
                    {
                        if (colDis < distanceshoot)
                        {
                            ActualShot = false;
                        }
                    }
                }
            }

            if (ActualShot)
            {
                callback(ObjectShooted);
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