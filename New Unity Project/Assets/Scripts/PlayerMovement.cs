using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float ClimbSpeed = 3f;
    public GameObject[] players;
    bool climbing;

    Vector2 wallPoint;
    Vector2 wallNormal;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private float verticalInput;


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
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0 && !onwall() && isgrounded());
        anim.SetBool("grounded", isgrounded());
        anim.SetBool("onWall", onwall());

        

        if (onwall() && !isgrounded())
        {
            Debug.Log(verticalInput * speed);

            //body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            body.velocity = new Vector2(body.velocity.x, verticalInput * speed);

            body.gravityScale = 1;
            //body.velocity = Vector2.zero;
        }
        else
        {
            //Flips player when they move left or right
            if (horizontalInput > 0.01f)
            {
                transform.localScale = Vector3.one;
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            body.gravityScale = 5;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

    }

    bool NearWall()
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
    }

    private void Run()
    {
        body.gravityScale = 0;

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = (transform.forward * v) + (transform.right * h);

        ApplyMove(move, speed);
    }

    private void ClimbWall()
    {
        body.gravityScale = 0;

        GrabWall();

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = (transform.up * v) + (transform.right * h);

        ApplyMove(move, ClimbSpeed);
    }

    void GrabWall()
    {
        var newPosition = wallPoint + wallNormal * (boxCollider.edgeRadius - 0.1f);
        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime);

        if (wallNormal == Vector2.zero)
            return;

        transform.rotation = Quaternion.LookRotation(-wallNormal);
    }

    private void Jump()
    {
        if (isgrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onwall() && !isgrounded())
        {
            if (verticalInput == 5)
            {
                //This pushes player off the wall
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * jumpPower, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * jumpPower, jumpPower);
                wallJumpCooldown = 0;
            }

            // The line (84) above controls the force of the player jumping off the the wall the fist number is the force of which the player would be pushed away from the wall. And the second one if the force pushing the player up


        }

    }

    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();

        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length > 1)
        {
            Destroy(players[1]);
        }
    }
    
    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

    private void ApplyMove(Vector2 move, float speed)
    {
        body.MovePosition(new Vector2(transform.position.x, transform.position.y) + move * speed * Time.deltaTime);
    }

    private bool isgrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;

    }

    private bool onwall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
