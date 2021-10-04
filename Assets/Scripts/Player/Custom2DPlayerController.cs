using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Crouch,
    Run,
    Jump,
    DoubleJump,
    TripleJump
};

public class Custom2DPlayerController : Custom2DPhysics
{
    [Header("Movement Setting")]
    [SerializeField] [Range(1, 50)] private float _maxRunSpeed = 7;
    [SerializeField] [Range(1, 50)] private float _jumpForce = 7;

    [Header("SFX Setting")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private AudioClip[] _jumpSounds;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private PlayerState _playerState = PlayerState.Idle;

    void Awake() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();    
        _animator = GetComponent<Animator>();
    }

    protected override void ModifyVelocity()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();

        //DebugState();

        DriveAnimation(ref _animator, Body.Grounded, Body.Velocity.x, Body.Velocity.y);
    }

    private void HandleHorizontalMovement()
	{
        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");

        FlipSprite(ref _spriteRenderer, movement.x);

        Force.Strength = movement * _maxRunSpeed;

        if (Body.Grounded && Mathf.Abs(movement.x) <= 0.01f)
        {
            _playerState = PlayerState.Idle;
        }
        else if (Body.Grounded && Mathf.Abs(movement.x) > 0.01f)
		{
            _playerState = PlayerState.Run;
		}
    }

    private void HandleVerticalMovement()
	{
        if (Input.GetButtonDown("Jump") && Body.Grounded)
        {
            Body.Velocity = new Vector2(Body.Velocity.x, _jumpForce);
            _playerState = PlayerState.Jump;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (Body.Velocity.y > 0)
            {
                Body.Velocity = new Vector2(Body.Velocity.x, (Body.Velocity.y * 0.5f));
            }
        }
    }

	private void DebugState()
	{
        switch(_playerState)
		{
            case PlayerState.Idle:
                Debug.Log("state: IDLE");
                break;
            case PlayerState.Crouch:
                Debug.Log("state: CROUCH");
                break;
            case PlayerState.Run:
                Debug.Log("state: RUN");
                break;
            case PlayerState.Jump:
                Debug.Log("state: JUMP");
                break;
            case PlayerState.DoubleJump:
                Debug.Log("state: DOUBLE_JUMP");
                break;
            case PlayerState.TripleJump:
                Debug.Log("state: TRIPLE_JUMP");
                break;
            default:
                Debug.Log("state: UNKNOWN");
                break;
        }
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
            animator.SetFloat("velocityX", Mathf.Abs(x) / _maxRunSpeed);
            animator.SetFloat("velocityY", Mathf.Abs(y) / _jumpForce);
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