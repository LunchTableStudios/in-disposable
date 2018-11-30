namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    [ UpdateBefore( typeof( UnityEngine.Experimental.PlayerLoop.FixedUpdate ) ) ]
    public class MovementSystem : ComponentSystem
    {
        private struct MovementEntityFilter
        {
            public readonly Movement MovementComponent;
            public Transform TransformComponent;
        }

        protected override void OnUpdate()
        {
            foreach( MovementEntityFilter entity in GetEntities<MovementEntityFilter>() )
            {
                Movement movement = entity.MovementComponent;
                Transform transform = entity.TransformComponent;

                Vector3 movementVector = new Vector3( movement.Value.x, movement.Value.y, 0 );

                transform.Translate( movementVector );
            }
        }
    }
}