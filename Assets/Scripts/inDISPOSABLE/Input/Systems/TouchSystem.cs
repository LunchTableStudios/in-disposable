namespace Indisposable.Input
{
    using UnityEngine;
    using Unity.Entities;

    public class TouchSystem : ComponentSystem
    {
        private struct TouchEntityFilter
        {
            public Touch TouchComponent;
        }

        protected override void OnUpdate()
        {
            foreach( TouchEntityFilter entity in GetEntities<TouchEntityFilter>() )
            {
                Touch touch = entity.TouchComponent;

                touch.Phase = TouchPhase.Stationary;

                if( Input.touches.Length > 0 )
                {
                    if( !touch.ActiveLastFrame )
                    {
                        touch.StartTime = Time.time;
                        touch.StartPosition = Input.touches[0].position;
                    }

                    touch.Velocity = Vector2.Lerp( touch.Velocity, Input.touches[0].deltaPosition, Time.deltaTime * 10 );

                    touch.EndTime = Time.time;
                    touch.EndPosition = Input.touches[0].position;

                    touch.ActiveLastFrame = true;
                }
                else
                {
                    if( touch.ActiveLastFrame )
                    {
                        touch.EndTime = Time.time;
                        touch.Phase = TouchPhase.Ended;
                    }
                    else
                    {
                        touch.StartPosition = Vector2.zero;
                        touch.EndPosition = Vector2.zero;
                    }

                    touch.ActiveLastFrame = false;   
                }
            }
        }
    }
}