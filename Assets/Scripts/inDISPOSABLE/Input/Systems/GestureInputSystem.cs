namespace Indisposable.Input
{
    using UnityEngine;
    using Unity.Entities;

    public enum Gesture
    {
        NONE,
        TAP,
        SWIPE
    }

    [ UpdateAfter( typeof( TouchSystem ) ) ]
    public class GestureInputSystem : ComponentSystem
    {
        private struct InputEntityFilter
        {
            public readonly Touch TouchComponent;
            public GestureInput InputComponent;
        }

        protected override void OnUpdate()
        {
            foreach( InputEntityFilter entity in GetEntities<InputEntityFilter>() )
            {
                Touch touch = entity.TouchComponent;
                GestureInput gestureInput = entity.InputComponent;

                gestureInput.Value = Gesture.NONE;
                gestureInput.Velocity = Vector2.zero;
                if( touch.Phase == TouchPhase.Ended )
                {
                    float touchDuration = touch.EndTime - touch.StartTime;
                    float touchDistance = Vector2.Distance( touch.EndPosition, touch.StartPosition );
                    
                    if( ( touchDuration <= gestureInput.DurationThreshold && touchDistance > 0.1f ) || touch.Velocity.magnitude > gestureInput.VelocityTheshold )
                    {
                        gestureInput.Value = Gesture.SWIPE;
                        gestureInput.Velocity = touch.Velocity;
                    }
                    else if( ( touchDuration <= gestureInput.DurationThreshold && touchDistance <= 0.1f ) )
                    {
                        gestureInput.Value = Gesture.TAP;
                    }
                    else
                    {
                        gestureInput.Value = Gesture.NONE;
                    }
                }                    
            }
        }
    }
}