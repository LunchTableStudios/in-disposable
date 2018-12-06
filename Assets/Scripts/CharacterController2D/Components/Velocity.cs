namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Mathematics;

    public class Velocity : MonoBehaviour
    {
        public float2 Value = float2.zero;
        
        [ HideInInspector ] 
        public float2 Delta = float2.zero;
    }
}