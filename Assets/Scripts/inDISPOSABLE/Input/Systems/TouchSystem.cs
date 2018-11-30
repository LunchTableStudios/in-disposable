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

        private struct TouchData
        {
            public int touchCount;
            public Vector2 position;
        }

        protected override void OnUpdate()
        {
            foreach( TouchEntityFilter entity in GetEntities<TouchEntityFilter>() )
            {
                Touch touch = entity.TouchComponent;

                TouchData touchData = GetTouchData();
                
                touch.Phase = TouchPhase.Stationary;

                if( touchData.touchCount > 0 )
                {
                    if( !touch.ActiveLastFrame )
                    {
                        touch.StartTime = Time.time;
                        touch.StartPosition = touchData.position;

                        touch.Phase = TouchPhase.Began;
                    }

                    touch.EndPosition = touchData.position;

                    if( touch.EndPosition != touch.StartPosition )
                        touch.Phase = TouchPhase.Moved;

                    touch.ActiveLastFrame = true;
                }
                else
                {
                    if( touch.ActiveLastFrame )
                    {
                        touch.EndTime = Time.time;
                        touch.Phase = TouchPhase.Ended;
                    }

                    touch.ActiveLastFrame = false;
                }
            }
        }

        private TouchData GetTouchData()
        {
            TouchData touchData = new TouchData();
            touchData.touchCount = 0;

            if( Input.touchCount > 0 )
            {
                touchData.touchCount = Input.touchCount;
                touchData.position = Input.touches[0].position;
            }
            else if( Input.GetMouseButton(0) )
            {
                touchData.touchCount = 1;
                touchData.position = Input.mousePosition;
            }

            return touchData;
        }
    }
}