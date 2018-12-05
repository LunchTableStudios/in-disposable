namespace Indisposable.Input
{
    using UnityEngine;

    public class GestureInput : MonoBehaviour
    {
        public float DurationThreshold = 0.28f;
        public float VelocityTheshold = 2f;
        public Gesture Value;
        public Vector2 Velocity;
    }
}