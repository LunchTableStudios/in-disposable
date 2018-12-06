namespace Indisposable.Player
{
	using UnityEngine;
	using Unity.Entities;
	using Input;
	using CharacterController2D;

	public class KeyboardJumpSystem : ComponentSystem
	{
		private struct JumpEntityFilter
		{
			public readonly Jump JumpComponent;
			public readonly CollisionData CollisionComponent;
			public Velocity VelocityComponent;
		}

		private struct InputEntityComponent
		{
			public readonly KeyboardInput InputComponent;
		}

		protected override void OnUpdate()
		{
			bool jumping = false;

			if( GetEntities<InputEntityComponent>().Length > 0 )
			{
				KeyboardInput input = GetEntities<InputEntityComponent>()[0].InputComponent;
				jumping = input.Jump;
			}

			foreach( JumpEntityFilter entity in GetEntities<JumpEntityFilter>() )
			{
				Jump jump = entity.JumpComponent;
				CollisionData collisionData = entity.CollisionComponent;
				Velocity velocity = entity.VelocityComponent;

				if( jumping && collisionData.Below )
				{
					velocity.Value = jump.Force;
					collisionData.Below = false;
				}
			}
		}
	}
}