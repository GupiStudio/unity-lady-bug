using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wekonu.CartoonPhysics;

public class Custom2DPhysics : MonoBehaviour
{
    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    private const float _minMoveDistance = 0.001f; // the minimum distance to start casting raycast
    private const float _shellRadius = 0.01f; // shell prevent the object to stuck inside another collider

    private bool _grounded = false;

    protected Vector2 Velocity;
    protected Vector2 TargetVelocity;
    
    private Rigidbody2D _rb2d;
    
    private ContactFilter2D _contactFilter;

    private Vector2 _groundNormal;

    private Gravity _gravity;

    private Force _force;

    private Wekonu.CartoonPhysics.Collision _collision;

    protected virtual void ComputeVelocity() {}

    void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D> ();
        _gravity = Gravity.GetInstance(gravityModifier);
        _force = Force.GetInstance();
        _collision = new Wekonu.CartoonPhysics.Collision(_minMoveDistance, minGroundNormalY);
    }

    void Start () 
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    void Update () 
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity ();    
    }

    void FixedUpdate()
    {
        //_gravity.Affect(ref Velocity, ref _rb2d);

        _grounded = false;

        Velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = TargetVelocity.x;

        Vector2 yMovement = _gravity.GetNewPosition(Velocity);
        Vector2 xMovement = _force.GetNewPosition(Velocity, _groundNormal);

        List<RaycastHit2D> hitY = _collision.GetOverlapRaycast(yMovement, _rb2d, _contactFilter, _shellRadius);
        List<RaycastHit2D> hitX = _collision.GetOverlapRaycast(xMovement, _rb2d, _contactFilter, _shellRadius);

        DetectCollisionY(hitY, yMovement);
		DetectCollisionX(hitX, xMovement);
	}

    protected bool Grounded
	{
        get => _grounded;
	}

    private void DetectCollisionX(List<RaycastHit2D> raycastHits, Vector2 move)
    {
        float distance = move.magnitude;

        if (_collision.IsMoving(move) && _collision.IsHit(raycastHits))
        {
            foreach (RaycastHit2D hit in raycastHits)
            {
                _collision.DetectGround(ref _grounded, hit);

                Projection(ref Velocity, hit.normal);

                distance = _collision.CalculateDistance(move, hit, _shellRadius);
            }

        }

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }

    private void DetectCollisionY(List<RaycastHit2D> raycastHits, Vector2 move)
    {
        float distance = move.magnitude;

        if (_collision.IsMoving(move) && _collision.IsHit(raycastHits))
        {
            foreach (RaycastHit2D hit in raycastHits)
            {
                Vector2 currentNormal = new Vector2(0, hit.normal.y);

                _groundNormal = hit.normal;

                _collision.DetectGround(ref _grounded, hit);

                Projection(ref Velocity, currentNormal);

                distance = _collision.CalculateDistance(move, hit, _shellRadius);
            }

        }

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }

    private void Projection(ref Vector2 velocity, Vector2 normal)
	{
        float projection = Vector2.Dot(velocity, normal);

        if (projection < 0)
		{
            velocity = velocity - (projection * normal);
        }
	}
}