namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;
    
    public class VelocitySystem : ComponentSystem
    {
        private struct VelocityEntityFilter
        {
            public Velocity VelocityComponent;
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.deltaTime;

            foreach( VelocityEntityFilter entity in GetEntities<VelocityEntityFilter>() )
            {
                Velocity velocity = entity.VelocityComponent;

                velocity.Delta = velocity.Value * Time.deltaTime;
            }
        }
    }
}
