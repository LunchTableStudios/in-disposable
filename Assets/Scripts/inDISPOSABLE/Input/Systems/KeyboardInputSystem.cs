namespace Indisposable.Input
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;
    
    public class KeyboardInputSystem : ComponentSystem
    {
        private struct InputEntityFilter
        {
            public KeyboardInput InputComponent;
        }

        protected override void OnUpdate()
        {
            foreach( InputEntityFilter entity in GetEntities<InputEntityFilter>() )
            {
                KeyboardInput input = entity.InputComponent;

                input.Movement.x = UnityEngine.Input.GetAxis( "Horizontal" );
                input.Movement.y = UnityEngine.Input.GetAxis( "Vertical" );

                input.Jump = UnityEngine.Input.GetKey( KeyCode.Space );
            }
        }
    }
}