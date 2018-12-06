namespace Indisposable.Player
{
	using UnityEngine;
	using Unity.Entities;
	using Unity.Mathematics;
	using Input;
	using CharacterController2D;

	public class KeyboardMovementSystem : ComponentSystem
	{
		private struct MovementEntityFilter
		{
			public readonly Movement MovementComponent;
			public readonly CollisionData CollisionComponent;
			public Velocity VelocityComponent;
		}

		private struct InputEntityComponent
		{
			public readonly KeyboardInput InputComponent;
		}

		protected override void OnUpdate()
		{
			float horizontalInput = 0;

			if( GetEntities<InputEntityComponent>().Length > 0 )
			{
				KeyboardInput input = GetEntities<InputEntityComponent>()[0].InputComponent;
				horizontalInput = input.Movement.x;
			}

			foreach( MovementEntityFilter entity in GetEntities<MovementEntityFilter>() )
			{
				Movement movement = entity.MovementComponent;
				CollisionData collisionData = entity.CollisionComponent;
				Velocity velocity = entity.VelocityComponent;

				if( horizontalInput != 0 )
				{
					float acceleration = ( collisionData.Below ) ? movement.GroundAcceleration : movement.AirAcceleration;
					velocity.Value.x = Mathf.SmoothDamp( velocity.Value.x, horizontalInput * movement.Speed, ref movement.Smoothing, acceleration );
				}
				else
				{
					if( collisionData.Below )
						velocity.Value.x *= movement.GroundFriction;
				}
			}
		}
	}
}