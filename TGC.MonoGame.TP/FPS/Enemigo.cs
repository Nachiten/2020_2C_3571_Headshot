﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using TGC.MonoGame.TP.FPS.Scenarios;
using System.Linq;

namespace TGC.MonoGame.TP
{
    public class Enemigo: Ashootable
    {
        private const string ContentFolder3D = "Models/";

        private Vector3 posicion;

        private Matrix World { get; set; }

        public ModelCollidable Model { get; set; }

        private Weapon Weapon { get; set; }

        public Enemigo(Vector3 posicion)
        {
            this.posicion =  posicion;
            World = Matrix.CreateRotationY(MathHelper.Pi) * ScaleFactor * Matrix.CreateTranslation(posicion);
        }

        //private Vector3 posicionInicial;
        private Vector3 mirandoInicial = new Vector3(0, 0, -1);
        float velocidadMovimiento = 2;
        Vector3 posicionObjetivo = Vector3.Zero;
        Vector3 vectorDireccion = Vector3.Zero;

        private Matrix ScaleFactor = Matrix.CreateScale(.7f);

        float anguloRotacionRadianes;
        Vector3 OldPosition = Vector3.Zero;

        private float health = 100;

        private int offSetY = 100;
        private bool shooting = false;
        Ray LineOfSight;
        Ray AabbMaxSight;
        Ray AabbMinSight;
        Vector3 InitialDirection;
        Vector3 GunOffset = new Vector3(77, 15, -10);

        Vector3[] PosibleDirections = new[]
        {
            Vector3.UnitX,
            Vector3.UnitZ,
            -Vector3.UnitX,
            -Vector3.UnitZ
        };

        private int indexPath = 0;
        private bool foward = true;
        public List<PathTrace> path = new List<PathTrace>();
        public Enemigo(Vector3 _posicion, AWeaponRecolectable weapon, float Angle, List<PathTrace> _path)
        {

            var posicionInicial = _path.First();
            anguloRotacionRadianes = PointTo(posicionInicial.normal);

            this.posicion = new Vector3(posicionInicial.posicion.X, offSetY, posicionInicial.posicion.Y);

            World = Matrix.CreateRotationY(anguloRotacionRadianes) * ScaleFactor * Matrix.CreateTranslation(posicion);

            //InitialDirection = new Vector3(MathF.Cos(MathHelper.PiOver2 + anguloRotacionRadianes), 0, MathF.Sin(MathHelper.PiOver2 + anguloRotacionRadianes));
            LineOfSight = new Ray();
            AabbMaxSight = new Ray();
            AabbMinSight = new Ray();

            path = _path;
            Weapon = new Weapon(new M4(posicion));
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            Model = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "enemy/Hellknight_LATEST", World);
            // Correccion de AABB
            float offsetX = 50;
            float offsetZ = 30;
            float offsetY = 190;
            Model.Aabb.SetManually(new Vector3(posicion.X - offsetX, 0, posicion.Z - offsetZ), new Vector3(posicion.X + offsetX, offsetY, posicion.Z + offsetZ));

            Aabb = Model.Aabb;

            Collision.Instance.AppendStatic(Model.Aabb);
            Collision.Instance.AppendShootable(this);
            Weapon.Gun.LoadContent(Content, GraphicsDevice);

            var modelEffectArmor = (BasicEffect)Model.Model.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.Pink.ToVector3();
            modelEffectArmor.EnableDefaultLighting();

            var modelAlgo = (BasicEffect)Model.Model.Meshes[1].Effects[0];
            modelAlgo.DiffuseColor = Color.Brown.ToVector3();
            modelAlgo.EnableDefaultLighting();

            //Mandibula
            var dindare2 = (BasicEffect)Model.Model.Meshes[1].Effects[1];
            dindare2.DiffuseColor = Color.WhiteSmoke.ToVector3();
            dindare2.EnableDefaultLighting();

            //Ojos
            var dindare = (BasicEffect)Model.Model.Meshes[1].Effects[2];
            dindare.DiffuseColor = Color.Yellow.ToVector3();
            dindare.EnableDefaultLighting();

            //Cuerpo
            var dindare3 = (BasicEffect)Model.Model.Meshes[1].Effects[3];
            dindare3.DiffuseColor = Color.DarkGray.ToVector3();
            dindare3.EnableDefaultLighting();

            //torso
            var dindare4 = (BasicEffect)Model.Model.Meshes[1].Effects[4];
            dindare4.DiffuseColor = Color.Black.ToVector3();
            dindare4.EnableDefaultLighting();
        }

        private bool StartedMoving = false;

        private Vector3 convertVector3(Vector2 posicion) {

            return new Vector3(posicion.X, offSetY, posicion.Y);
        }
        public void avanzarPath() 
        {
            
            if (indexPath + 1 < path.Count && foward) {
                if (Vector3.Distance(convertVector3(path[indexPath + 1].posicion), posicion) <= 2)
                {
                    indexPath++;
                    anguloRotacionRadianes = PointTo(path[indexPath].normal);
                }
            }
            else if(indexPath + 1 == path.Count && foward)
            {
                foward = false;
                indexPath--;
                anguloRotacionRadianes = PointTo(-path[indexPath].normal);
            }
            if (!foward) {
                if (indexPath > 0)
                {
                    if (Vector3.Distance(convertVector3(path[indexPath].posicion), posicion) <= 2)
                    {
                        indexPath--;
                        anguloRotacionRadianes = PointTo(-path[indexPath].normal);
                    }
                }
                else if(Vector3.Distance(convertVector3(path[indexPath].posicion), posicion) <=2)
                { 
                    foward = true;
                    indexPath = 0;
                    anguloRotacionRadianes = PointTo(path[indexPath].normal);
                }
                
            }
            UpdateWorld(posicion + (vectorDireccion * velocidadMovimiento), anguloRotacionRadianes);
        }

        public void Update(GameTime gameTime, Vector3 posicionCamara)
        {
            //UpdateLineOfSight();

            // Calculo la posicion a la que me voy a mover
            //posicionObjetivo = new Vector3(posicionCamara.X, posicion.Y, posicionCamara.Z);

            avanzarPath();

            //float distanciaAlObjetivo = Vector3.Distance(posicion, posicionObjetivo);
            //// Si la distancia es menor a un margen comienzo a moverme
            //if (distanciaAlObjetivo < 500 && Config.enemigosFollowActivado && !StartedMoving) {
            //    StartedMoving = true;
            //    //Guardo la posición anterior
            //    OldPosition = posicion;

            //    // Establezco el giro al inicial
            //    World *= Matrix.CreateRotationY(-anguloRotacionRadianes);

            //    anguloRotacionRadianes = MoveTowards(posicionObjetivo);
            //    UpdateWorld(posicion + (vectorDireccion * velocidadMovimiento), anguloRotacionRadianes);
            //    Collision.Instance.CheckStatic(Model.Aabb, StaticCollisionCB);
            //}

            //if (StartedMoving)
            //{
            //    float shortestDistance = GetShortestDistanceToStaticElement();

            //    if(shortestDistance < 50)
            //    {
            //        var dirIdx = 0;
            //        while (shortestDistance < 50 && dirIdx < PosibleDirections.Length)
            //        {
            //            anguloRotacionRadianes = PointTo(PosibleDirections[dirIdx]);
            //            UpdateWorld(posicion, anguloRotacionRadianes);
            //            shortestDistance = GetShortestDistanceToStaticElement();
            //            dirIdx++;
            //        }
            //    }
            //    else
            //    {
            //        anguloRotacionRadianes = MoveTowards(posicionObjetivo);
            //        UpdateWorld(posicion + (vectorDireccion * velocidadMovimiento), anguloRotacionRadianes);
            //        Collision.Instance.CheckStatic(Model.Aabb, StaticCollisionCB);
            //    }

            //    UpdateWorld(posicion + (vectorDireccion * velocidadMovimiento), anguloRotacionRadianes);
            //}

            //if (distanciaAlObjetivo > 500 && distanciaAlObjetivo < 250)
            //    StartedMoving = false;


            if (Math.Round(gameTime.TotalGameTime.TotalMilliseconds) % 2000 == 0 && !shooting)
            {
                shooting = true;
                Collision.Instance.CheckShootable(LineOfSight, this, ShootableCollisionCB);
            }
            else
            {
                shooting = false;
            }
        }
        public void UpdateWorld(Vector3 hacia, float angulo)
        {
            posicion = hacia;
            // Aplico la rotacion que corresponde
            World = ScaleFactor * Matrix.CreateRotationY(MathHelper.Pi + angulo);

            // Muevo el modelo
            World *= Matrix.CreateTranslation(posicion);
            Model.Transform(World, true);

            // Actualizo la vista
            UpdateLineOfSight();
        }
        private float MoveTowards(Vector3 v)
        {
            vectorDireccion = Vector3.Normalize(v - posicion);
            float angle = (float)Math.Acos(Vector3.Dot(vectorDireccion, mirandoInicial)
                    / (Vector3.Distance(vectorDireccion, Vector3.Zero) * Vector3.Distance(mirandoInicial, Vector3.Zero)));

            // Si posX del objetivo es mayor a posX actual => * -1
            // Si el objetivo está en el tercer o cuarto cuadrante (angulo > 180) entonces debo invertir el angulo
            if (v.X > posicion.X)
            {
                angle *= -1;
            }
            return angle;
        }
        private float PointTo(Vector3 v)
        {
            vectorDireccion = v;
            float angle = (float)Math.Acos(Vector3.Dot(vectorDireccion, mirandoInicial)
                    / (Vector3.Distance(vectorDireccion, Vector3.Zero) * Vector3.Distance(mirandoInicial, Vector3.Zero)));

            // Si posX del objetivo es mayor a posX actual => * -1
            // Si el objetivo está en el tercer o cuarto cuadrante (angulo > 180) entonces debo invertir el angulo
            if (v.X > posicion.X)
            {
                angle *= -1;
            }
            return angle;
        }

        private void UpdateLineOfSight()
        {
            LineOfSight.Position = posicion + GunOffset;
            LineOfSight.Direction = vectorDireccion;

            AabbMinSight.Position = Model.Aabb.minExtents;
            AabbMinSight.Direction = vectorDireccion;

            AabbMaxSight.Position = Model.Aabb.maxExtents;
            AabbMaxSight.Direction = vectorDireccion;
        }
        private float GetShortestDistanceToStaticElement()
        {   
            return Math.Min(Collision.Instance.GetShortestDistanceToStaticElement(AabbMinSight, Model.Aabb), Collision.Instance.GetShortestDistanceToStaticElement(AabbMaxSight, Model.Aabb));
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo en las coordenadas actuales
            Model.Draw(view, projection);

            World.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);

            Matrix ww = Matrix.CreateTranslation(GunOffset) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(translation);

            Weapon.Draw(ww, view, projection);
        }

        public override void GetDamaged(int damage)
        {
            if (health - damage < 0)
            {
                health = 0;
            } else
            {
                health -= damage;
            }
        }
        public bool IsDead()
        {
            return health == 0;
        }
        public void Revivir()
        {
            health = 100;
        }

        private int StaticCollisionCB(AABB a, AABB b)
        {
            //TODO: Handle Collision
            posicion = OldPosition;
            Model.Transform(World * Matrix.CreateTranslation(posicion), true);
            return 0;
        }
        public int ShootableCollisionCB(Ashootable e)
        {
            Debug.WriteLine("!!!!Dispare al personaje ");
            e.GetDamaged(Weapon.Damage);
            return 0;
        }
    }
}
