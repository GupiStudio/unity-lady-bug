using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	public class World
	{
		private Vector2 _groundNormal;
		private float _minGroundNormalY = .65f;

		public World()
		{
			_groundNormal = new Vector2();
		}

		public Vector2 GroundNormal
		{
			get => _groundNormal;
			set => _groundNormal = value;
		}

		public float MinGroundNormalY
		{
			get => _minGroundNormalY;
			set => _minGroundNormalY = value;
		}
	}
}
