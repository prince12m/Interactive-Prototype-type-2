using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    /*[SerializeField] private float ClimbSpeed = 3f;*/

    /*bool climbing;

    Vector2 wallPoint;
    Vector2 wallNormal;*/

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;


    private void Awake()
    {
        //Grab referances for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flips player when they move left or right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Wall Jump Logic
        if (wallJumpCooldown > 0.2f)
        {


            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.UpArrow))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;

        /*if (NearWall())
        {
            if (FacingWall())
            {
                // is player presses the climb button
                if (Input.GetKeyUp(KeyCode.W))
                {
                    climbing = !climbing;
                }
            }
            else
            {
                Jump();
            }
        }*/
    }

    /*bool NearWall()
    {
        return Physics.CheckSphere(transform.position, 3f, wallLayer);
    }

    bool FacingWall()
    {
        RaycastHit hit;

        var facingWall = Physics.Raycast(transform.position, transform.forward, out hit, boxCollider.edgeRadius + 1f, wallLayer);

        wallPoint = hit.point;
        wallNormal = hit.normal;
        return facingWall;
    }*/

    /*private void Run()
    {
        body.gravityScale = 0;

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = transform.forward * v + transform.right * h;

        ApplyMove(move, speed);
    }*/

    /*private void ClimbWall()
    {
        body.gravityScale = 0;

        GrabWall();

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = transform.up * v + transform.right * h;

        ApplyMove(move, ClimbSpeed);
    }*/

    /*void GrabWall()
    {
        var newPosition = wallPoint + wallNormal * (boxCollider.edgeRadius - 0.1f);
        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime);

        if (wallNormal == Vector2.zero)
            return;

        transform.rotation = Quaternion.LookRotation(-wallNormal);
    }*/

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                //This pushes player off the wall
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 30, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 30, 30);
            wallJumpCooldown = 0;

            // The line (84) above controls the force of the player jumping off the the wall the fist number is the force of which the player would be pushed away from the wall. And the second one if the force pushing the player up


        }

    }

    /*private void ApplyMove(Vector2 move, float speed)
    {
        body.MovePosition(new Vector2(transform.position.x, transform.position.y) + move * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }*/

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;

    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;

    }
}
