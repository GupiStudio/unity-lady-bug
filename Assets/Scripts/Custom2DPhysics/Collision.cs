using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wekonu.CartoonPhysics
{
	public class Collision
	{
        public event EventHandler HitGround;

        private World _world;

        private Body _body;

        private ContactFilter2D _contactFilter;

        public Collision(ref Body body, ref World world, ref ContactFilter2D contactFilter)
		{
            _body = body;
            _world = world;
            _contactFilter = contactFilter;
		}

        private bool IsHit(List<RaycastHit2D> raycastHits)
        {
            return raycastHits != null || raycastHits.Count > 0;
        }

        private bool IsHitGround(RaycastHit2D hit)
        {
            if (hit.normal.y > _world.MinGroundNormalY)
            {
                return true;
            }

            return false;
        }

        private List<RaycastHit2D> GetOverlapRaycast()
        {
            if (_body.IsMoving())
            {
                var hitBuffer = new RaycastHit2D[16];

                int hitCount = _body.GetRigidBody().Cast(_body.Movement, _contactFilter, hitBuffer, _body.Movement.magnitude + _body.ShellRadius);

                var raycastHits = new List<RaycastHit2D>();

                for (int i = 0; i < hitCount; i++)
                {
                    raycastHits.Add(hitBuffer[i]);
                }

                return raycastHits;
            }

            return null;
        }

        private float CalculateDistance(RaycastHit2D hit)
        {
            float distance = _body.Movement.magnitude;

            float modifiedDistance = hit.distance - _body.ShellRadius;

            distance = modifiedDistance < distance ? modifiedDistance : distance;

            return distance;
        }

        private void DetectGround(RaycastHit2D hit)
        {
            _body.Grounded = IsHitGround(hit);
            if (IsHitGround(hit))
			{
                OnHitGround(EventArgs.Empty);
                _body.Grounded = true;
            }
        }

        private void Projection(ref Body body, Vector2 normal)
        {
            float projection = Vector2.Dot(body.Velocity, normal);

            if (projection < 0)
            {
                body.Velocity = body.Velocity - (projection * normal);
            }
        }

        public void DetectCollisionX()
        {
            List<RaycastHit2D> raycastHits = GetOverlapRaycast();

            float distance = _body.Movement.magnitude;

            if (_body.IsMoving() && IsHit(raycastHits))
            {
                foreach (RaycastHit2D hit in raycastHits)
                {
                    DetectGround(hit);

                    Projection(ref _body, hit.normal);

                    distance = CalculateDistance(hit);
                }
            }

            _body.Translastion = distance;
        }

        public void DetectCollisionY()
        {
            List<RaycastHit2D> raycastHits = GetOverlapRaycast();

            float distance = _body.Movement.magnitude;

            if (_body.IsMoving() && IsHit(raycastHits))
            {
                foreach (RaycastHit2D hit in raycastHits)
                {
                    DetectGround(hit);

                    Vector2 currentNormal = new Vector2(0, hit.normal.y);

                    _world.GroundNormal = hit.normal;

                    Projection(ref _body, currentNormal);

                    distance = CalculateDistance(hit);
                }
            }

            _body.Translastion = distance;
        }

        protected virtual void OnHitGround(EventArgs e)
        {
            HitGround?.Invoke(this, e);
        }
    }
}
