namespace CharacterController2D
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    [ UpdateAfter( typeof( GravitySystem ) ) ]
    [ UpdateBefore( typeof( MovementSystem ) ) ]
    public class CollisionSystem : ComponentSystem
    {
        private struct CollidableEntityFilter
        {
            public Movement MovementComponent;
            public CollisionData CollisionComponent;
            public readonly BoxCollider2D ColliderComponent;
        }

        public struct RaycastOrigins
        {
            public Vector2 TopLeft, TopRight, BottomLeft, BottomRight;
        }

        public struct RaycastSpacing
        {
            public int HorizontalRayCount;
            public int VerticalRayCount;
            public float HorizontalRaySpacing;
            public float VerticalRaySpacing;
        }

        const int RAYCASTS_PER_UNIT = 4;

        protected override void OnUpdate()
        {
            foreach( CollidableEntityFilter entity in GetEntities<CollidableEntityFilter>() )
            {
                Movement movement = entity.MovementComponent;
                CollisionData collisionData = entity.CollisionComponent;
                BoxCollider2D collider = entity.ColliderComponent;

                collisionData.previousSlopeAngle = collisionData.slopeAngle;
                ResetCollisionState( collisionData );

                float2 movementDelta = movement.Value;

                RaycastOrigins raycastOrigins = CalculateRaycastOrigins( collider );
                RaycastSpacing raycastSpacing = CalculateRaycastSpacing( collider );

                if( movementDelta.y < 0 )
                    HandleSlopeDecend( ref movementDelta, collider.edgeRadius, collisionData, raycastOrigins );

                if( movementDelta.x != 0 )
                    HandleHorizontalMovement( ref movementDelta, collider.edgeRadius, collisionData, raycastOrigins, raycastSpacing );

                if( movementDelta.y != 0 )
                    HandleVerticalMovement( ref movementDelta, collider.edgeRadius, collisionData, raycastOrigins, raycastSpacing );

                movement.Value = movementDelta;
            }
        }

        private void HandleHorizontalMovement( ref float2 movementDelta, float skinWidth, CollisionData collisionData, RaycastOrigins raycastOrigins, RaycastSpacing raycastSpacing )
        {
            float horizontalDirection = math.sign( movementDelta.x );
            float raycastDistance = math.max( math.abs( movementDelta.x ) + skinWidth, 2 * skinWidth );
            Vector2 raycastOrigin = ( horizontalDirection == 1 ) ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;

            for( int i = 0; i < raycastSpacing.HorizontalRayCount; i++ )
            {
                Vector2 ray = raycastOrigin + ( Vector2.up * ( raycastSpacing.HorizontalRaySpacing * i ) );

                Debug.DrawRay( ray, Vector2.right * horizontalDirection * raycastDistance, Color.red, 0.1f );

                RaycastHit2D hit = Physics2D.Raycast( ray, Vector2.right * horizontalDirection, raycastDistance, collisionData.Mask );

                if( hit )
                {
                    if( hit.distance == 0 )
                        continue;
                    
                    float slopeAngle = Vector2.Angle( hit.normal, Vector2.up );

                    if( i == 0 && slopeAngle <= collisionData.MaxSlopeAngle )
                    {
                        if( collisionData.DecendingSlope )
                            collisionData.DecendingSlope = false;

                        float distanceToSlope = 0;

                        if( slopeAngle != collisionData.previousSlopeAngle )
                        {
                            distanceToSlope = hit.distance - skinWidth;
                            movementDelta.x -= distanceToSlope * horizontalDirection;
                        }

                        HandleSlopeAscend( ref movementDelta, slopeAngle, collisionData );

                        movementDelta.x += distanceToSlope * horizontalDirection;
                    }

                    if( !collisionData.AscendingSlope || slopeAngle > collisionData.MaxSlopeAngle )
                    {
                        movementDelta.x = ( hit.distance - skinWidth ) * horizontalDirection;
                        raycastDistance = hit.distance;

                        if( collisionData.AscendingSlope )
                            movementDelta.y = math.tan( math.radians( collisionData.slopeAngle ) ) * math.abs( movementDelta.x );

                        collisionData.Right = ( horizontalDirection == 1 );
                        collisionData.Left = ( horizontalDirection == -1 );
                    }
                }
            }
        }

        private void HandleVerticalMovement( ref float2 movementDelta, float skinWidth, CollisionData collisionData, RaycastOrigins raycastOrigins, RaycastSpacing raycastSpacing )
        {
            float verticalDirection = math.sign( movementDelta.y );
            float raycastDistance = math.abs( movementDelta.y ) + skinWidth;
            Vector2 raycastOrigin = ( verticalDirection == 1 ) ? raycastOrigins.TopLeft : raycastOrigins.BottomLeft;
            RaycastHit2D hit;

            for( int i = 0; i < raycastSpacing.VerticalRayCount; i++ )
            {
                Vector2 ray = raycastOrigin + ( Vector2.right * ( raycastSpacing.VerticalRaySpacing * i + movementDelta.x ) );

                Debug.DrawRay( ray, Vector2.up * verticalDirection * raycastDistance, Color.red, 0.1f );

                hit = Physics2D.Raycast( ray, Vector2.up * verticalDirection, raycastDistance, collisionData.Mask );

                if( hit )
                {
                    // ToDo one way playforms / moving platforms

                    movementDelta.y = ( hit.distance - skinWidth ) * verticalDirection;
                    raycastDistance = hit.distance;

                    if( collisionData.AscendingSlope )
                        movementDelta.x = movementDelta.y / math.tan( math.radians( collisionData.slopeAngle ) ) * math.sign( movementDelta.x );

                    collisionData.Above = ( verticalDirection == 1 );
                    collisionData.Below = ( verticalDirection == -1 );
                }
            }

            if( collisionData.AscendingSlope )
            {
                float horizontalDirection = math.sign( movementDelta.x );
                raycastDistance = math.abs( movementDelta.x ) + skinWidth;
                Vector2 ray = ( ( horizontalDirection == 1 ) ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft ) + ( Vector2.up * movementDelta.y );
                hit = Physics2D.Raycast( ray, Vector2.right * horizontalDirection, raycastDistance, collisionData.Mask );

                if( hit )
                {
                    float slopeAngle = Vector2.Angle( hit.normal, Vector2.up );

                    if( slopeAngle != collisionData.slopeAngle )
                    {
                        movementDelta.x = ( hit.distance - skinWidth ) * horizontalDirection;
                        collisionData.slopeAngle = slopeAngle;
                    }
                }
            }
        }

        private void HandleSlopeAscend( ref float2 movementDelta, float slopeAngle, CollisionData collisionData )
        {
            float radianSlopeAngle = math.radians( slopeAngle );
            float moveDistance = math.abs( movementDelta.x );
            float verticalAscendDistance = math.sin( radianSlopeAngle ) * moveDistance;

            if( movementDelta.y <= verticalAscendDistance )
            {
                movementDelta.x = math.cos( radianSlopeAngle ) * moveDistance * math.sign( movementDelta.x );
                movementDelta.y = verticalAscendDistance;
                collisionData.Below = true;
                collisionData.AscendingSlope = true;
                collisionData.slopeAngle = slopeAngle;
            }

        }

        private void HandleSlopeDecend( ref float2 movementDelta, float skinWidth, CollisionData collisionData, RaycastOrigins raycastOrigins  )
        {
            float horizontalDirection = math.sign( movementDelta.x );
            Vector2 raycastOrigin = ( horizontalDirection == 1 ) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;

            RaycastHit2D hit = Physics2D.Raycast( raycastOrigin, Vector2.down, Mathf.Infinity, collisionData.Mask );

            if( hit )
            {
                float slopeAngle =  Vector2.Angle( hit.normal, Vector2.up );
                float radianSlopeAngle = math.radians( slopeAngle );

                if( slopeAngle != 0 && slopeAngle <= collisionData.MaxSlopeAngle )
                {
                    if( math.sign( hit.normal.x ) == horizontalDirection )
                    {
                        float moveDistance = math.abs( movementDelta.x );
                        if( hit.distance - skinWidth <= math.tan( radianSlopeAngle ) * moveDistance )
                        {
                            movementDelta.x = math.cos( radianSlopeAngle ) * moveDistance * horizontalDirection;
                            movementDelta.y -= math.sin( radianSlopeAngle ) * moveDistance;

                            collisionData.slopeAngle = slopeAngle;
                            collisionData.DecendingSlope = true;
                            collisionData.Below = true;
                        }
                    }
                }
            }
        }

        private void ResetCollisionState( CollisionData collisionData )
        {
            collisionData.Above = collisionData.Below = collisionData.Left = collisionData.Right = false;
            collisionData.AscendingSlope = false;
            collisionData.DecendingSlope = false;
            collisionData.slopeAngle = 0;
        }

        private RaycastOrigins CalculateRaycastOrigins( BoxCollider2D collider )
        {
            float skinWidth = collider.edgeRadius;
            Bounds bounds = collider.bounds;
            bounds.Expand( skinWidth * -2f );

            RaycastOrigins origins = new RaycastOrigins();

            origins.TopLeft = new Vector2( bounds.min.x, bounds.max.y );
            origins.TopRight = bounds.max;
            origins.BottomLeft = bounds.min;
            origins.BottomRight = new Vector2( bounds.max.x, bounds.min.y );

            return origins;
        }

        private RaycastSpacing CalculateRaycastSpacing( BoxCollider2D collider )
        {
            float skinWidth = collider.edgeRadius;
            Bounds bounds = collider.bounds;
            bounds.Expand( skinWidth * -2f );

            RaycastSpacing spacing = new RaycastSpacing();

            int horizontalRayCount = Mathf.CeilToInt( RAYCASTS_PER_UNIT * bounds.size.x );
            int verticalRayCount = Mathf.CeilToInt( RAYCASTS_PER_UNIT * bounds.size.y );

            float horizontalSpacing = bounds.size.y / ( horizontalRayCount - 1 );
            float verticalSpacing = bounds.size.x / ( verticalRayCount - 1 );

            spacing.HorizontalRayCount = horizontalRayCount;
            spacing.VerticalRayCount = verticalRayCount;
            spacing.HorizontalRaySpacing = horizontalSpacing;
            spacing.VerticalRaySpacing = verticalSpacing;

            return spacing;
        }
    }
}