using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	public class Body
	{
		private bool _grounded = false;

		private float _shellRadius = 0.01f; // shell prevent the object to stuck inside another collider

		private float _minMoveDistance = 0.001f; // the minimum distance to start to be considered as moving

		private float _translation = 0; // this var going to be overriden in many place and will be used to move the rigid body

		private Vector2 _movement;

		private Vector2 _velocity;

		private Rigidbody2D _rigidbody;

		public Body(ref Rigidbody2D rb)
		{
			_rigidbody = rb;
		}

		public float ShellRadius
		{
			get => _shellRadius;
		}

		public float MinMoveDistance
		{
			get => _minMoveDistance;
			set => _minMoveDistance = value;
		}

		public float Translastion
		{
			set => _translation = value;
		}

		public bool Grounded
		{
			get => _grounded;
			set => _grounded = value;
		}

		public Vector2 Movement
		{
			get => _movement;
			set => _movement = value;
		}

		public Vector2 Velocity
		{
			get => _velocity;
			set => _velocity = value;
		}

		public bool IsMoving()
		{
			return _movement.magnitude > _minMoveDistance;
		}

		public Rigidbody2D GetRigidBody()
		{
			return _rigidbody;
		}

		public void Move()
		{
			_rigidbody.position = _rigidbody.position + _movement.normalized * _translation;
		}
	}
}
