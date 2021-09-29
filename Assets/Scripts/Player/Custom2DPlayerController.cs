using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom2DPlayerController : Custom2DPhysics
{
    [SerializeField] private float maxSpeed = 7;
    [SerializeField] private float jumpTakeOffSpeed = 7;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip[] jumpSounds;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump") && Grounded) 
        {
            Velocity.y = jumpTakeOffSpeed;
        } 
        else if (Input.GetButtonUp ("Jump")) 
        {
            if (Velocity.y > 0) 
            {
                Velocity.y = Velocity.y * 0.5f;
            }
        }

        if (animator)
		{
            animator.SetBool("grounded", Grounded);
            animator.SetFloat("velocityX", Mathf.Abs(Velocity.x) / maxSpeed);
            animator.SetFloat("velocityY", Mathf.Abs(Velocity.y) / jumpTakeOffSpeed);
        }

        if (spriteRenderer)
		{
            bool flipSprite = spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0f);

            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

        TargetVelocity = move * maxSpeed;
    }

    private void PlayFootstepSound()
	{
        if (audioSource)
		{
            audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
		}
	}

    private void PlayJumpSound()
	{
        if (audioSource)
		{
            audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
		}
	}
}