namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;
    
    public class VelocitySystem : ComponentSystem
    {
        private struct VelocityEntityFilter
        {
            public readonly Velocity VelocityComponent;
            public Movement MovementComponent;
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;

            foreach( VelocityEntityFilter entity in GetEntities<VelocityEntityFilter>() )
            {
                Velocity velocity = entity.VelocityComponent;
                Movement movement = entity.MovementComponent;

                movement.Value = velocity.Value * Time.deltaTime;
            }
        }
    }
}
