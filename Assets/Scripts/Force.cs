using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	class Force
	{
		private static Force _force;

		private float _strength;

		private Force()
		{
			//
		}

		public float Strength
		{
			get => _strength;
			set
			{
				_strength = value;
			}
		}

		public static Force GetInstance()
		{
			if (_force == null)
			{
				_force = new Force();
				return _force;
			}

			return _force;
		}

		public Vector2 GetNewPosition(Vector2 velocity, Vector2 groundNormal) // independent
		{
			Vector2 deltaPosition = velocity * Time.deltaTime;

			Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

			Vector2 move = moveAlongGround * deltaPosition.x;

			return move;
		}
	}
}