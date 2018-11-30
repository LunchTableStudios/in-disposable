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
        private struct GestureInputEntityFilter
        {
            public readonly Touch TouchComponent;
            public GestureInput InputComponent;
        }

        protected override void OnUpdate()
        {
            foreach( GestureInputEntityFilter entity in GetEntities<GestureInputEntityFilter>() )
            {
                Touch touch = entity.TouchComponent;
                GestureInput gestureInput = entity.InputComponent;

                gestureInput.Value = Gesture.NONE;
                gestureInput.Angle = 0;
                if( touch.Phase == TouchPhase.Ended )
                {
                    float touchDuration = touch.EndTime - touch.StartTime;
                    float travelDistance = Vector2.Distance( touch.StartPosition, touch.EndPosition );

                    if( touchDuration <= 0.18f )
                    {
                        if( travelDistance <= 2 )
                        {
                            gestureInput.Value = Gesture.TAP;
                        }
                        else
                        {
                            float swipeAngle = Mathf.Atan2( touch.EndPosition.y - touch.StartPosition.y, touch.EndPosition.x - touch.StartPosition.x ) * Mathf.Rad2Deg;
                            gestureInput.Value = Gesture.SWIPE;
                            gestureInput.Angle = swipeAngle;
                        }

                        Debug.Log( string.Format( "Type: {0}, Angle: {1}", gestureInput.Value, gestureInput.Angle ) );
                    }
                }                    
            }
        }
    }
}