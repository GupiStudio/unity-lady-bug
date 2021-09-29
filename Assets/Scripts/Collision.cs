using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	class Collision
	{
        private float _minMoveDistance;

        private float _minGroundNormalY;

        public Collision(float minMoveDistance, float minGroundNormalY)
		{
            _minMoveDistance = minMoveDistance;
            _minGroundNormalY = minGroundNormalY;
		}

        public bool IsHit(List<RaycastHit2D> raycastHits) // independent
        {
            return raycastHits != null || raycastHits.Count > 0;
        }

        public bool IsMoving(Vector2 move) // independent
        {
            return move.magnitude > _minMoveDistance;
        }

        public bool IsHitGround(RaycastHit2D hit) // independent
        {
            if (hit.normal.y > _minGroundNormalY)
            {
                return true;
            }

            return false;
        }

        public void DetectGround(ref bool grounded, RaycastHit2D hit) // somehow independent
        {
            grounded = IsHitGround(hit);
        }

        public List<RaycastHit2D> GetOverlapRaycast(Vector2 move, Rigidbody2D rb2d, ContactFilter2D contactFilter, float shellRadius) // independent
        {
            if (IsMoving(move))
            {
                var hitBuffer = new RaycastHit2D[16];

                int hitCount = rb2d.Cast(move, contactFilter, hitBuffer, move.magnitude + shellRadius);

                var raycastHits = new List<RaycastHit2D>();

                for (int i = 0; i < hitCount; i++)
                {
                    raycastHits.Add(hitBuffer[i]);
                }

                return raycastHits;
            }

            return null;
        }

        public float CalculateDistance(Vector2 move, RaycastHit2D hit, float shellRadius) // independent
        {
            float distance = move.magnitude;

            float modifiedDistance = hit.distance - shellRadius;

            distance = modifiedDistance < distance ? modifiedDistance : distance;

            return distance;
        }
    }
}
