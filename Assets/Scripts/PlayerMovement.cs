using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    private float xtrans;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 11.5f;
    [SerializeField] private float jumpRequestTimeBuffer = .5f;
    private bool jumpRequestActive = false;
    private Coroutine jumpBufferCoroutine = null;

    [Header("Ground Detection")]
    [SerializeField] private Vector2 groundDetectBoxSize = Vector2.one;
    [SerializeField] private float groundDetectCastDistance = .5f;

    [Header("Collision")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    private int groundLayerInt, playerLayerInt;

    [SerializeField] private Animator anim;
    private Vector3 originalScale; // faces right
    private bool hasAnim = false;

    void Awake()
    {
        originalScale = transform.localScale;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hasAnim = anim != null;

        groundLayerInt = Mathf.RoundToInt(Mathf.Log(groundLayer.value, 2));
        playerLayerInt = Mathf.RoundToInt(Mathf.Log(playerLayer.value, 2));

        jumpRequestActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        xtrans = Input.GetAxisRaw("Horizontal") * speed;
        if (xtrans > 0) // determines which way the player is facing
        {
            FlipRight();
        }
        else if (xtrans < 0)
        {
            FlipLeft();
        }

        if (Input.GetButtonDown("Jump")) // requesting the jump and starting the timer
        {
            StopAllCoroutines();
            jumpBufferCoroutine = null;
            jumpRequestActive = true;
        }

        if (jumpRequestActive && IsGrounded()) // only allows jumping if not already up
        {
            myRigidbody.AddForce(new Vector2(myRigidbody.velocity.x, jumpForce), ForceMode2D.Impulse);
            if(hasAnim)
                anim.SetTrigger("Jump");

            jumpRequestActive = false;
            StopAllCoroutines();
            jumpBufferCoroutine= null;
        }
        else if (jumpRequestActive && jumpBufferCoroutine == null)
        {
            jumpBufferCoroutine = StartCoroutine(JumpBuffer());
        }

        if (hasAnim)
        {
            anim.SetFloat("XSpeed", Mathf.Abs(xtrans));
            anim.SetFloat("YSpeed", myRigidbody.velocity.y);
        }
    }

    private IEnumerator JumpBuffer()
    {
        float timeDelta = 0;
        while (jumpRequestActive && timeDelta <= jumpRequestTimeBuffer)
        {
            // in the buffer frames, if the jump becomes avaliable, then make the player jump
            if (IsGrounded())
            {
                myRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                if (hasAnim)
                    anim.SetTrigger("Jump");

                break;
            }
            timeDelta += Time.deltaTime;
            yield return null;
        }
        jumpRequestActive = false;
        jumpBufferCoroutine = null;
    }

    private bool IsGrounded()
    {
        return (Physics2D.BoxCast(transform.position, groundDetectBoxSize, 0, -transform.up, groundDetectCastDistance, groundLayer));
    }

    private void FixedUpdate()
    {
        // Moves the player on a fixed update for consistency
        transform.Translate(xtrans * Time.fixedDeltaTime, 0, 0);
        // allows the player to jump through the bottom of the platforms on the ground layer
        Physics2D.IgnoreLayerCollision(groundLayerInt, playerLayerInt, (myRigidbody.velocity.y > 0.1));
    }

#region Visuals
    private void FlipRight()
    {
        transform.localScale = originalScale;
    }
    private void FlipLeft()
    {
        Vector3 newScale = originalScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
#endregion

 #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundDetectCastDistance, groundDetectBoxSize);
    }
#endregion
}
