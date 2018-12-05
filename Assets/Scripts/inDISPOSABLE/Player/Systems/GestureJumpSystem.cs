namespace Indisposable.Player
{
	using UnityEngine;
	using Unity.Entities;
	using Input;
	using CharacterController2D;

	public class GestureJumpSystem : ComponentSystem
	{
		private struct JumpEntityFilter
		{
			public readonly Jump JumpComponent;
			public readonly CollisionData CollisionComponent;
			public Velocity VelocityComponent;
		}

		private struct InputEntityComponent
		{
			public readonly GestureInput InputComponent;
		}

		protected override void OnUpdate()
		{
		   Gesture gesture = Gesture.NONE;
		   Vector2 gestureVelocity = Vector2.zero;

		   if( GetEntities<InputEntityComponent>().Length > 0 )
		   {
			   GestureInput input = GetEntities<InputEntityComponent>()[0].InputComponent;
			   gesture = input.Value;
			   gestureVelocity = input.Velocity;
		   }

		   foreach( JumpEntityFilter entity in GetEntities<JumpEntityFilter>() )
		   {
			   Jump jump = entity.JumpComponent;
			   CollisionData collisionData = entity.CollisionComponent;
			   Velocity velocity = entity.VelocityComponent;

			   if( gesture == Gesture.SWIPE )
			   {
				   if( collisionData.Below && gestureVelocity.y > 0 )
				   {
					   velocity.Value = gestureVelocity.normalized * jump.Force;
					   collisionData.Below = false;
				   }
			   }
		   }
		}
	}
}