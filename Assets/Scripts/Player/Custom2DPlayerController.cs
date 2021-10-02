using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom2DPlayerController : Custom2DPhysics
{
    [SerializeField] private float _maxSpeed = 7;
    [SerializeField] private float _jumpTakeOffSpeed = 7;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private AudioClip[] _jumpSounds;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    void Awake() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _animator = GetComponent<Animator>();
    }

    protected override void ModifyVelocity()
    {
        Vector2 movement = HandleMovement();

        DriveAnimation(ref _animator, Body.Grounded, Body.Velocity.x, Body.Velocity.y);

        FlipSprite(ref _spriteRenderer, movement.x);

        Force.Strength = movement * _maxSpeed;
    }

    private Vector2 HandleMovement()
	{
        Vector2 xMovement = Vector2.zero;

        xMovement.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && Body.Grounded)
        {
            Body.Velocity = new Vector2(Body.Velocity.x, _jumpTakeOffSpeed);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (Body.Velocity.y > 0)
            {
                Body.Velocity = new Vector2(Body.Velocity.x, (Body.Velocity.y * 0.5f));
            }
        }

        return xMovement;
    }

    private void FlipSprite(ref SpriteRenderer spriteRenderer, float direction)
	{
        if (_spriteRenderer)
        {
            bool flipSprite = _spriteRenderer.flipX ? (direction > 0.01f) : (direction < 0f);

            if (flipSprite)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }
    }

    private void DriveAnimation(ref Animator animator, bool grounded, float x, float y)
	{
        if (animator)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(x) / _maxSpeed);
            animator.SetFloat("velocityY", Mathf.Abs(y) / _jumpTakeOffSpeed);
        }
    }

    private void PlayFootstepSound()
	{
        if (_audioSource)
		{
            _audioSource.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Length)]);
		}
	}

    private void PlayJumpSound()
	{
        if (_audioSource)
		{
            _audioSource.PlayOneShot(_jumpSounds[Random.Range(0, _jumpSounds.Length)]);
		}
	}
}