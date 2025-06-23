using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float initialSpeed = 1f;
    public float groundedAccelRate = 2f;
    public float aerialAccelRate = 1.25f;
    public float maxVelocity = 10f;
    public float jumpPower = 16f;
    public float walljumpPower = 32f;
    public bool isFacingRight = true;
    public float rayGap = 0.1f;
    public float respawnTimeInSeconds = 1.0f;
    private Animator animator;
    private Vector3 respawnPoint;
    private float horizontal;
    private bool isAlive = true;
    private float respawnTimeRemaining;
    private float airTime = 0.0f;
    private float escHoldTime = 0.0f;
    private float escHoldTimeNeeded = 1.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource deadAudioSource;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;
        animator.SetBool("Alive", true);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
        Flip();
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            escHoldTime += Time.deltaTime;
            if (escHoldTime > escHoldTimeNeeded)
                SceneManager.LoadScene("MainMenu");
        }
        else
            escHoldTime = 0.0f;
        if (IsGrounded() && !CheckWallClinging())
            airTime = 0.0f;
        else
            airTime += Time.deltaTime;
        if (isAlive)
        {
            Run();
            ChangeAnimation();
        }
        else
        {
            Debug.Log("Waiting to respawn");
            respawnTimeRemaining -= Time.deltaTime;
            if (respawnTimeRemaining <= 0.0f)
            {
                enableRB();
                transform.position = respawnPoint;
                isAlive = true;
                animator.SetTrigger("RespawnTrigger");
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    Destroy(g);
                }
                tag = "Player";
                Debug.Log("Respawned");
                animator.SetBool("Alive", true);
            }
            animator.ResetTrigger("RespawnTrigger");
        }
        animator.SetFloat("AirTime", airTime);
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetBool("WallClinging", CheckWallClinging());
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Stationary", horizontal == 0);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.linearVelocityY = jumpPower;
                jumpAudioSource.Play();
            }
            else if (isHoldingWall() && horizontal != 0)
            {
                rb.linearVelocityX = -0.5f * walljumpPower * horizontal;
                rb.linearVelocityY = jumpPower * 0.75f;
                jumpAudioSource.Play();
            }
        }
        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0f)
                rb.linearVelocityY *= 0.5f;
    }

    private void Run()
    {
        if (horizontal == 0 && IsGrounded())
        {
            rb.linearVelocityX *= 0.9f;
        }
        else if (horizontal != 0)
        {
            if (Math.Abs(rb.linearVelocityX) <= 0.0001)
            {
                rb.linearVelocityX = horizontal * initialSpeed;
            }
            
            if (IsGrounded())
            {
                rb.linearVelocityX += horizontal * groundedAccelRate / 10f;
                if (!walkingAudioSource.isPlaying)
                {
                    walkingAudioSource.Play(Convert.ToUInt32(0.25));
                }
            }
            else
            {
                rb.linearVelocityX += horizontal * aerialAccelRate / 10f;
                walkingAudioSource.Stop();
            }
            if (rb.linearVelocityX > maxVelocity)
                rb.linearVelocityX = maxVelocity;
        }
        if (Math.Abs(rb.linearVelocityX) <= 0.0001)
        {
            rb.linearVelocityX = 0;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
    }

    private bool isHoldingWall()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.1f, groundLayer);
        
    }

    private void Flip()
    {
        if ((isFacingRight ^ rb.linearVelocityX > 0.0f) && rb.linearVelocityX != 0.0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;
        }
    }

    private bool CheckWallClinging()
    {
        if (isHoldingWall() && !IsGrounded() && horizontal != 0)
        {
            return true;
        }
        return false;
    }

    private void ChangeAnimation()
    {
        if (!IsGrounded() && isAlive)
        {
            if(rb.linearVelocityY > 0 && isAlive)
            {
                animator.SetTrigger("JumpTrigger");
            }
            if( (rb.linearVelocityY < 0 || airTime > 0.5)  && isAlive)
            {
                animator.SetTrigger("FallTrigger");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Hazard" || collider.gameObject.tag == "Bullet")
        {
            if (isAlive) 
            {
                isAlive = false;
                disableRB();
                animator.SetTrigger("DeathTrigger");
                animator.SetBool("Alive", false);
                deadAudioSource.Play();
                respawnTimeRemaining = respawnTimeInSeconds;
                tag = "Dead";
            }
        }
    }

    private void disableRB()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void enableRB()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
