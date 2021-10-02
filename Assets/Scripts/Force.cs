using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	public class Force
	{
		private static Force _force;

		private Vector2 _strength;

		private World _world;

		private Force(ref World world)
		{
			_strength = Vector2.zero;

			_world = world;
		}

		public Vector2 Strength
		{
			get => _strength;
			set
			{
				_strength = value;
			}
		}

		public static Force GetInstance(ref World world)
		{
			if (_force == null)
			{
				_force = new Force(ref world);
				return _force;
			}

			return _force;
		}

		public void Affect(ref Body body, ref Collision collision, ref World world)
		{
			ProjectMovement(ref body, ref world);

			collision.DetectCollisionX();

			body.Move();
		}

		private void ProjectMovement(ref Body body, ref World world)
		{
			Vector2 temp = body.Velocity;

			temp.x = _strength.x;

			body.Velocity = temp;

			Vector2 deltaPosition = body.Velocity * Time.deltaTime;

			Vector2 moveAlongGround = new Vector2(world.GroundNormal.y, -world.GroundNormal.x);

			Vector2 move = moveAlongGround * deltaPosition.x;

			body.Movement = move;
		}
	}
}