using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Utils{
    public class Collision{
        public static Collision Instance { get; private set; }
        public List<AABB> StaticElements = new List<AABB>();

        public List<ARecolectable> CollectableElements = new List<ARecolectable>();
        public List<Enemigo> ShootableElements = new List<Enemigo>();

        public Collision(){
        }
        public void AppendStatic(AABB elem){
            //Console.WriteLine(elem.size.ToString());
            StaticElements.Add(elem);
        }

        public void AppendShootable(Enemigo elem)
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
        public void CheckShootable(Ray Ray, Func<Enemigo, int> callback)
        {
            Enemigo EnemyShooted = null;
            float distanceshoot = -1;
            bool ActualShot = false;

            foreach (Enemigo e in ShootableElements.ToArray())
            {
                var colDis = e.ModeloTgcitoClassic.Aabb.IntersectRay(Ray);
                if (colDis != null)
                {
                    EnemyShooted = e;
                    distanceshoot = (float)colDis;
                    ActualShot = true;
                    Debug.WriteLine("Distance to enemy: " + distanceshoot);
                }
            }
            if(EnemyShooted != null)
            {
                IEnumerable<AABB> EnemyAABB = ShootableElements.Select(x => x.ModeloTgcitoClassic.Aabb);
                foreach (AABB s in StaticElements.Where(x => !EnemyAABB.Contains(x)))
                {
                    var colDis = s.IntersectRay(Ray);
                    if (colDis != null)
                    {
                        Debug.WriteLine("Distance to Static Element: " + colDis);
                        Debug.WriteLine("Static Element: " + s.maxExtents + " ; " + s.minExtents);
                        if (colDis < distanceshoot)
                        {
                            ActualShot = false;
                        }
                    }
                }
            }

            if (ActualShot)
            {
                callback(EnemyShooted);
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