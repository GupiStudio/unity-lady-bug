using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 11f;

    private const string WalkAnimation = "Walk";

    private Rigidbody2D myBody;

    private Animator animator;

    private SpriteRenderer sr;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatePlayer(PlayerMoveKeyboard());
    }

    private PlayerState PlayerMoveKeyboard()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;
        sr.flipX = movementX == 0 ? sr.flipX : (movementX < 0); // this line of code is not supposed to be here

        if (movementX != 0)
        {
            return PlayerState.Walk;
        }

        return PlayerState.Idle;
    }

    private void AnimatePlayer(PlayerState playerState)
    {
        if (playerState == PlayerState.Walk)
        {
            animator.SetBool(WalkAnimation, true);
            return;
        }

        animator.SetBool(WalkAnimation, false);
    }
}
