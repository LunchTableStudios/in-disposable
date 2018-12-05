namespace Indisposable.Input
{
    using UnityEngine;
    using Unity.Mathematics;

    public class TouchInput : MonoBehaviour
    {
        public float2 Value;
        public float2 DeadZoneThreshold = new float2( 30, 30 );
    }
}