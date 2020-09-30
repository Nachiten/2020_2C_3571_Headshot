using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TGC.MonoGame.TP.Physics
{
    /// <summary>
    /// Bit masks which control whether different members of a group of objects can collide with each other.
    /// </summary>
    public struct SubgroupCollisionFilter
    {
        /// <summary>
        /// A mask of 16 bits, each set bit representing a collision group that an object belongs to.
        /// </summary>
        public ushort SubgroupMembership;
        /// <summary>
        /// A mask of 16 bits, each set bit representing a collision group that an object can interact with.
        /// </summary>
        public ushort CollidableSubgroups;
        /// <summary>
        /// Id of the owner of the object. Objects belonging to different groups always collide.
        /// </summary>
        public int GroupId;

        /// <summary>
        /// Initializes a collision filter that collides with everything in the group.
        /// </summary>
        /// <param name="groupId">Id of the group that this filter operates within.</param>
        public SubgroupCollisionFilter(int groupId)
        {
            GroupId = groupId;
            SubgroupMembership = ushort.MaxValue;
            CollidableSubgroups = ushort.MaxValue;
        }

        /// <summary>
        /// Initializes a collision filter that belongs to one specific subgroup and can collide with any other subgroup.
        /// </summary>
        /// <param name="groupId">Id of the group that this filter operates within.</param>
        /// <param name="subgroupId">Id of the subgroup to put this collidable into.</param>
        public SubgroupCollisionFilter(int groupId, int subgroupId)
        {
            GroupId = groupId;
            //Debug.Assert(subgroupId >= 0 && subgroupId < 16, "The subgroup field is a ushort; it can only hold 16 distinct subgroups.");
            SubgroupMembership = (ushort)(1 << subgroupId);
            CollidableSubgroups = ushort.MaxValue;
        }

        /// <summary>
        /// Disables a collision between this filter and the specified subgroup.
        /// </summary>
        /// <param name="subgroupId">Subgroup id to disable collision with.</param>
        public void DisableCollision(int subgroupId)
        {
            //Debug.Assert(subgroupId >= 0 && subgroupId < 16, "The subgroup field is a ushort; it can only hold 16 distinct subgroups.");
            CollidableSubgroups ^= (ushort)(1 << subgroupId);
        }

        /// <summary>
        /// Modifies the interactable subgroups such that filterB does not interact with the subgroups defined by filter a and vice versa.
        /// </summary>
        /// <param name="a">Filter from which to remove collisions with filter b's subgroups.</param>
        /// <param name="b">Filter from which to remove collisions with filter a's subgroups.</param>
        public static void DisableCollision(ref SubgroupCollisionFilter filterA, ref SubgroupCollisionFilter filterB)
        {
            filterA.CollidableSubgroups &= (ushort)~filterB.SubgroupMembership;
            filterB.CollidableSubgroups &= (ushort)~filterA.SubgroupMembership;
        }

        /// <summary>
        /// Checks if the filters can collide by checking if b's membership can be collided by a's collidable groups.
        /// </summary>
        /// <param name="a">First filter to test.</param>
        /// <param name="b">Second filter to test.</param>
        /// <returns>True if the filters can collide, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllowCollision(in SubgroupCollisionFilter a, in SubgroupCollisionFilter b)
        {
            return a.GroupId != b.GroupId || (a.CollidableSubgroups & b.SubgroupMembership) > 0;
        }

    }
    public struct BodyProperties
    {
        /// <summary>
        /// Controls which collidables the body can collide with.
        /// </summary>
        public SubgroupCollisionFilter Filter;
        /// <summary>
        /// Friction coefficient to use for the body.
        /// </summary>
        public float Friction;
        /// <summary>
        /// True if the body is attached to the camera.
        /// </summary>
        public bool Camera;
        /// <summary>
        /// True if the body is a wall.
        /// </summary>
        public bool Wall;
    }
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public Vector3 Gravity;
        public float LinearDamping;
        public float AngularDamping;
        Vector3 gravityDt;
        float linearDampingDt;
        float angularDampingDt;

        public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        public PoseIntegratorCallbacks(Vector3 gravity, float linearDamping = .03f, float angularDamping = .03f) : this()
        {
            Gravity = gravity;
            LinearDamping = linearDamping;
            AngularDamping = angularDamping;
        }

        public void PrepareForIntegration(float dt)
        {
            //No reason to recalculate gravity * dt for every body; just cache it ahead of time.
            gravityDt = Gravity * dt;
            //Since this doesn't use per-body damping, we can precalculate everything.
            linearDampingDt = MathF.Pow(MathHelper.Clamp(1 - LinearDamping, 0, 1), dt);
            angularDampingDt = MathF.Pow(MathHelper.Clamp(1 - AngularDamping, 0, 1), dt);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IntegrateVelocity(int bodyIndex, in RigidPose pose, in BodyInertia localInertia, int workerIndex, ref BodyVelocity velocity)
        {
            //Note that we avoid accelerating kinematics. Kinematics are any body with an inverse mass of zero (so a mass of ~infinity). No force can move them.
            if (localInertia.InverseMass > 0)
            {
                velocity.Linear = (velocity.Linear + gravityDt) * linearDampingDt;
                velocity.Angular = velocity.Angular * angularDampingDt;
            }
            //Implementation sidenote: Why aren't kinematics all bundled together separately from dynamics to avoid this per-body condition?
            //Because kinematics can have a velocity- that is what distinguishes them from a static object. The solver must read velocities of all bodies involved in a constraint.
            //Under ideal conditions, those bodies will be near in memory to increase the chances of a cache hit. If kinematics are separately bundled, the the number of cache
            //misses necessarily increases. Slowing down the solver in order to speed up the pose integrator is a really, really bad trade, especially when the benefit is a few ALU ops.

            //Note that you CAN technically modify the pose in IntegrateVelocity by directly accessing it through the Simulation.Bodies.ActiveSet.Poses, it just requires a little care and isn't directly exposed.
            //If the PositionFirstTimestepper is being used, then the pose integrator has already integrated the pose.
            //If the PositionLastTimestepper or SubsteppingTimestepper are in use, the pose has not yet been integrated.
            //If your pose modification depends on the order of integration, you'll want to take this into account.

            //This is also a handy spot to implement things like position dependent gravity or per-body damping.
        }

    }
    public unsafe struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
    {
        public SpringSettings ContactSpringiness;
        public CollidableProperty<BodyProperties> Properties;

        public void Initialize(Simulation simulation)
        {
            Properties.Initialize(simulation);
            //Use a default if the springiness value wasn't initialized.
            if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0)
                ContactSpringiness = new SpringSettings(30, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b)
        {
            //It's impossible for two statics to collide, and pairs are sorted such that bodies always come before statics.
            if (b.Mobility != CollidableMobility.Static)
            {
                return SubgroupCollisionFilter.AllowCollision(Properties[a.BodyHandle].Filter, Properties[b.BodyHandle].Filter);
            }
            //While the engine won't even try creating pairs between statics at all, it will ask about kinematic-kinematic pairs.
            //Those pairs cannot emit constraints since both involved bodies have infinite inertia. Since most of the demos don't need
            //to collect information about kinematic-kinematic pairs, we'll require that at least one of the bodies needs to be dynamic.
            return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
        {
            //This function is called for children of compounds, triangles in meshes, and similar cases, but we don't perform any child-level filtering in the tank demo.
            //The top level filter will always run before this function has a chance to, so we don't have to do anything here.
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : struct, IContactManifold<TManifold>
        {
            pairMaterial.FrictionCoefficient = 1f;
            pairMaterial.MaximumRecoveryVelocity = 2f;
            pairMaterial.SpringSettings = ContactSpringiness;

            ref var propertiesA = ref Properties[pair.A.BodyHandle];

            if (propertiesA.Camera || (pair.B.Mobility != CollidableMobility.Static && Properties[pair.B.BodyHandle].Camera)){
                for (int i = 0; i < manifold.Count; ++i)
                {
                    if (manifold.GetDepth(ref manifold, i) >= -1e-3f){
                        //An actual collision was found. 
                        if (propertiesA.Camera){
                            // Handle collision
                        }
                        if (pair.B.Mobility != CollidableMobility.Static && Properties[pair.B.BodyHandle].Camera)
                        {
                            //Could technically combine the locks in the case that both bodies are projectiles, but that's not exactly common.
                            // Handle collision
                        }
                        break;
                    }
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
        {
            return true;
        }

        public void Dispose()
        {
            Properties.Dispose();
        }
    }
}
