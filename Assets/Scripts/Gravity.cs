using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	class Gravity
	{
		private static Gravity _gravity;

		private float _strength;

		private Gravity(float strength)
		{
			_strength = strength;
		}

		public float Strength
		{
			get => _strength;
			set
			{
				_strength = value;
			}
		}

		public static Gravity GetInstance(float strength)
		{
			if (_gravity == null)
			{
				_gravity = new Gravity(strength);
				return _gravity;
			}

			return _gravity;
		}

		public Vector2 GetNewPosition(Vector2 velocity)
		{
			Vector2 deltaPosition = velocity * Time.deltaTime;

			Vector2 move = Vector2.up * deltaPosition.y;

			return move;
		}

		public void Affect(ref Body body, ref Collision collision)
		{
			body.Grounded = false;

			ProjectMovement(ref body);

			collision.DetectCollisionY();

			body.Move();
		}

		public void ProjectMovement(ref Body body)
		{
			body.Velocity += _strength * Physics2D.gravity * Time.deltaTime;

			Vector2 deltaPosition = body.Velocity * Time.deltaTime;

			Vector2 move = Vector2.up * deltaPosition.y;

			body.Movement = move;
		}
	}
}
