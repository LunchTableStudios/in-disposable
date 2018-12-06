namespace Indisposable.Player
{
    using UnityEngine;

    public class Movement : MonoBehaviour
    {
        public float Speed = 8;
        public float GroundAcceleration = 0.2f;
        public float AirAcceleration = 0.1f;
        public float GroundFriction = 0.85f;

        [ HideInInspector ]
        public float Smoothing;
    }
}