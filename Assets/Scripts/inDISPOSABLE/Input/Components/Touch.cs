namespace Indisposable.Input
{
    using UnityEngine;
    
    public class Touch : MonoBehaviour
    {
        public TouchPhase Phase;

        public float StartTime;
        public float EndTime;

        public Vector2 Velocity;

        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public bool ActiveLastFrame = false;
    }
}