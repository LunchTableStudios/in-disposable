namespace Indisposable.Input
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    [ UpdateAfter( typeof( TouchSystem ) ) ]
    public class TouchInputSystem : ComponentSystem
    {
        private struct TouchInputEntityFilter
        {
            public readonly Touch TouchComponent;
            public TouchInput InputComponent;
        }

        protected override void OnUpdate()
        {
            foreach( TouchInputEntityFilter entity in GetEntities<TouchInputEntityFilter>() )
            {
                Touch touch = entity.TouchComponent;
                TouchInput touchInput = entity.InputComponent;

                touchInput.Value = float2.zero;

                if( touch.Phase == TouchPhase.Stationary || touch.Phase == TouchPhase.Moved )
                {
                    float horizontalDistance = math.abs( touch.StartPosition.x - touch.EndPosition.x );
                    float verticalDistance = math.abs( touch.StartPosition.y - touch.EndPosition.y );

                    float horizontalInput = 0;
                    float verticalInput = 0;

                    if( horizontalDistance >= touchInput.DeadZoneThreshold.x )
                        horizontalInput = math.sign( touch.EndPosition.x - touch.StartPosition.x );

                    if( verticalDistance >= touchInput.DeadZoneThreshold.y )
                        verticalInput = math.sign( touch.EndPosition.y - touch.StartPosition.y );

                    touchInput.Value = new float2( horizontalInput, verticalInput );
                }                    
            }
        }
    }
}