using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wekonu.CartoonPhysics;

public class Custom2DPhysics : MonoBehaviour
{
    [SerializeField]
    private float _gravityStrength = 1f;
    
    protected Body Body;
    
    protected Force Force;

    private Rigidbody2D _rb2d;

    private ContactFilter2D _contactFilter;

    private Gravity _gravity;

    private World _world;

    private Wekonu.CartoonPhysics.Collision _collision;

    protected virtual void ModifyVelocity() {}

    void OnEnable()
    {
        _world = new World();
        
        Force = Force.GetInstance(ref _world);

        _rb2d = GetComponent<Rigidbody2D>();

        Body = new Body(ref _rb2d);

        _gravity = Gravity.GetInstance(_gravityStrength);

		_collision = new Wekonu.CartoonPhysics.Collision(ref Body, ref _world, ref _contactFilter);

		_collision.HitGround += _collision_HitGround;
	}

	private void _collision_HitGround(object sender, System.EventArgs e)
	{
		// can put any code later
	}

	void Start () 
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    void Update () 
    {
        Force.Strength = Vector2.zero;
        
        ModifyVelocity();    
    }

    void FixedUpdate()
    {
        _gravity.Affect(ref Body, ref _collision);

		Force.Affect(ref Body, ref _collision, ref _world);
	}
}