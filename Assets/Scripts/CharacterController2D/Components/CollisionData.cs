namespace CharacterController2D
{
    using UnityEngine;

    public class CollisionData : MonoBehaviour
    {
        public LayerMask Mask;
        public bool Above, Below, Left, Right;

        public float MaxSlopeAngle = 65;
        public bool AscendingSlope = false;
        public bool DecendingSlope = false;

        [ HideInInspector ] 
        public float slopeAngle = 0;
        
        [ HideInInspector ] 
        public float previousSlopeAngle = 0;
    }
}