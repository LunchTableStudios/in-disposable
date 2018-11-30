namespace Indisposable.Input
{
    using UnityEngine;
    
    public class Touch : MonoBehaviour
    {
        public int Index;

        public float StartTime;
        public float EndTime;

        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public bool ActiveLastFrame = false;
    }
}