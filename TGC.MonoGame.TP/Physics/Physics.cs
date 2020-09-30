using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.Physics
{
    public class Physics{
        private BufferPool BufferPool { get; set; }
        private Simulation Simulation { get; set; }
        private SimpleThreadDispatcher ThreadDispatcher { get; set; }
        public PositionFirstTimestepper timeStepper = new PositionFirstTimestepper();
        private List<BodyHandle> BodiesHandle { get; set; }
        private CollidableProperty<BodyProperties> bodyProperties;
        //private List<SpherePrimitive> Bodies { get; set; }
        public Physics(){
            BufferPool = new BufferPool();
            bodyProperties = new CollidableProperty<BodyProperties>();
            Simulation = Simulation.Create(BufferPool, new NarrowPhaseCallbacks() { Properties = bodyProperties },new PoseIntegratorCallbacks(new NumericVector3(0, -100, 0)), timeStepper);
            //Bodies = new List<SpherePrimitive>();
            BodiesHandle = new List<BodyHandle>();

            // Creates a floor
            Simulation.Statics.Add(new StaticDescription(new NumericVector3(0, -0.5f, 0), new CollidableDescription(Simulation.Shapes.Add(new Box(2500, 1, 2500)), 0.1f)));
            ThreadDispatcher = new SimpleThreadDispatcher(Environment.ProcessorCount);
        }
        public void AppendBody(){
            int radius = 30;
            float mass = .4f;
            var BoundingBox = new Box(radius, radius, radius);
            BoundingBox.ComputeInertia(mass, out var bbInertia);
            var bbIndex = Simulation.Shapes.Add(BoundingBox);
            var position = new NumericVector3(1, 1, 1);

            var bodyDescription = BodyDescription.CreateDynamic(position, bbInertia, new CollidableDescription(bbIndex, 0.1f), new BodyActivityDescription(0.01f));
            //Bodies.Add(new SpherePrimitive(GraphicsDevice, radius, 16, Color.Pink));
            var bodyHandle = Simulation.Bodies.Add(bodyDescription);
            BodiesHandle.Add(bodyHandle);
        }
        public void Update(){
            Simulation.Timestep(1 / 60f, ThreadDispatcher);
        }
    }
}