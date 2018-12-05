namespace Indisposable.Player
{
	using UnityEngine;
	using Unity.Entities;
	using Unity.Mathematics;
	using Input;
	using CharacterController2D;

	public class TouchMovementSystem : ComponentSystem
	{
		private struct MovementEntityFilter
		{
			public readonly MoveSpeed SpeedComponent;
			public readonly CollisionData CollisionComponent;
			public Velocity VelocityComponent;
			public MovementSmoothing SmoothingComponent;
		}

		private struct InputEntityComponent
		{
			public readonly TouchInput InputComponent;
		}

		protected override void OnUpdate()
		{
			float horizontalInput = 0;

			if( GetEntities<InputEntityComponent>().Length > 0 )
			{
				TouchInput input = GetEntities<InputEntityComponent>()[0].InputComponent;
				horizontalInput = input.Value.x;
			}

			foreach( MovementEntityFilter entity in GetEntities<MovementEntityFilter>() )
			{
				MoveSpeed speed = entity.SpeedComponent;
				CollisionData collisionData = entity.CollisionComponent;
				Velocity velocity = entity.VelocityComponent;
				MovementSmoothing smoothing = entity.SmoothingComponent;

				if( horizontalInput != 0 )
				{
					velocity.Value.x = Mathf.SmoothDamp( velocity.Value.x, horizontalInput * speed.Value, ref smoothing.Value, 0.2f );
				}
				else
				{
					if( collisionData.Below )
						velocity.Value.x *= 0.85f;
				}
			}
		}
	}
}