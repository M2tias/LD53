using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float movementAcceleration;

    [SerializeField]
    private float movementDampening;

    [SerializeField]
    private Animator moveUpperAnim;
    [SerializeField]
    private Animator moveLowerAnim;

    private float jumpSpeed = 5f;

    private float currentMovementSpeed = 0;

    private Rigidbody2D body2D;
    private PlayerAiming aiming;
    private bool setYPos = false;
    private float collisionYPos = 0f;
    private bool setXPos = false;
    private float collisionXPos = 0f;
    private int jumped = 0;
    private int maxJumps = 2;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        aiming = GetComponent<PlayerAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        if (setYPos)
        {
            transform.position = new Vector3(transform.position.x, collisionYPos, transform.position.z);
            setYPos = false;
        }
        if (setXPos)
        {
            transform.position = new Vector3(collisionXPos, transform.position.y, transform.position.z);
            setXPos = false;
        }

        controlsAndMovement();

        body2D.velocity = body2D.velocity - new Vector2(0, 10f * Time.deltaTime);

        collisions();

        animateWalk();
    }

    private void controlsAndMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            currentMovementSpeed = Mathf.Max(currentMovementSpeed - movementAcceleration * Time.deltaTime, -maxMovementSpeed);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            currentMovementSpeed = Mathf.Min(currentMovementSpeed + movementAcceleration * Time.deltaTime, maxMovementSpeed);
            moving = true;
        }
        else if (Mathf.Abs(currentMovementSpeed) > 0.1f)
        {
            float decel = Mathf.Sign(currentMovementSpeed) * movementDampening * Time.deltaTime;
            currentMovementSpeed = currentMovementSpeed - decel;
        }
        else
        {
            currentMovementSpeed = 0f;
            moving = false;
        }

        body2D.velocity = new Vector2(currentMovementSpeed, body2D.velocity.y);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumped < maxJumps)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, jumpSpeed);
            jumped++;
        }
    }

    private void collisions()
    {
        int mask = LayerMask.GetMask("Ground");
        RaycastHit2D hitFeet = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, mask);
        RaycastHit2D hitLeftUpper = Physics2D.Raycast(transform.position + Vector3.up * 0.35f, Vector2.left, 0.15f, mask);
        RaycastHit2D hitLeftLower = Physics2D.Raycast(transform.position - Vector3.up * 0.35f, Vector2.left, 0.15f, mask);
        RaycastHit2D hitRightUpper = Physics2D.Raycast(transform.position + Vector3.up * 0.35f, Vector2.right, 0.15f, mask);
        RaycastHit2D hitRightLower = Physics2D.Raycast(transform.position - Vector3.up * 0.35f, Vector2.right, 0.15f, mask);
        RaycastHit2D hitTopLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.1f, Vector2.up, 0.47f, mask);
        RaycastHit2D hitTopRight = Physics2D.Raycast(transform.position + Vector3.right * 0.1f, Vector2.up, 0.47f, mask);

        if (hitFeet.collider != null && body2D.velocity.y < 0)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, 0);
            collisionYPos = hitFeet.point.y + 0.5f;
            setYPos = true;
            jumped = 0;
        }

        bool sideHit = false;

        bool leftCollided = hitLeftUpper.collider != null || hitLeftLower.collider != null;
        if (leftCollided && body2D.velocity.x < 0)
        {
            body2D.velocity = new Vector2(0, body2D.velocity.y);
            if (hitLeftLower.collider != null)
            {
                collisionXPos = hitLeftLower.point.x + 0.15f;
            }
            else if (hitLeftUpper.collider != null)
            {
                collisionXPos = hitLeftUpper.point.x + 0.15f;
            }
            setXPos = true;
            // Debug.Log("left collided");
            sideHit = true;
        }

        bool rightCollided = hitRightUpper.collider != null || hitRightLower.collider != null;
        if (rightCollided && body2D.velocity.x > 0)
        {
            body2D.velocity = new Vector2(0, body2D.velocity.y);
            if (hitRightLower.collider != null)
            {
                collisionXPos = hitRightLower.point.x - 0.15f;
            }
            else if (hitRightUpper.collider != null)
            {
                collisionXPos = hitRightUpper.point.x - 0.15f;
            }
            setXPos = true;
            // Debug.Log("right collided");
            sideHit = true;
        }

        bool topCollided = (hitTopLeft.collider != null || hitTopRight.collider != null) && !sideHit;
        if (topCollided && body2D.velocity.y > 0)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, 0);
            if (hitTopLeft.collider != null)
            {
                collisionYPos = hitTopLeft.point.y - 0.47f;
            }
            else
            {
                collisionYPos = hitTopRight.point.y - 0.47f;
            }
            setYPos = true;
            // Debug.Log("top collided");
        }
    }

    private void animateWalk()
    {
        bool walkingBackwards = aiming.IsFlipped == body2D.velocity.x >= 0;

        if (moving)
        {
            moveUpperAnim.SetBool("walking", true);
            moveLowerAnim.SetBool("walking", true);
            moveUpperAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
            moveLowerAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
        }
        else
        {
            moveUpperAnim.SetBool("walking", false);
            moveLowerAnim.SetBool("walking", false);
            moveUpperAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
            moveLowerAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
        }
    }

    private void FixedUpdate()
    {
    }
}