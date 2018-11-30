namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;
    
    public class GravitySystem : ComponentSystem
    {
        private struct GravityEntityFilter
        {
            public readonly Gravity GravityComponent;
            public readonly CollisionData CollisionComponent;
            public Velocity VelocityComponent;
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;

            foreach( GravityEntityFilter entity in GetEntities<GravityEntityFilter>() )
            {
                Gravity gravity = entity.GravityComponent;
                Velocity velocity = entity.VelocityComponent;
                CollisionData collisionData = entity.CollisionComponent;

                if( !collisionData.Below )
                    velocity.Value.y -= gravity.Value * deltaTime;
            }
        }
    }
}