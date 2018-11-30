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
            
        }
    }
}