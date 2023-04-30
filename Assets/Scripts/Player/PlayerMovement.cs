using System.Collections;
using System.Collections.Generic;
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

    private float currentMovementSpeed = 0;

    private Rigidbody2D body2D;
    private PlayerAiming aiming;
    private bool setYPos = false;
    private float collisionYPos = 0f;
    private bool jumped = false;
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
        // movement
        // - horizontal arrows (axis?) high acceleration, high dampening
        // - vertical
        //   - constant down accel
        //   - jump = vertical speed boost

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentMovementSpeed = Mathf.Max(currentMovementSpeed - movementAcceleration * Time.deltaTime, -maxMovementSpeed);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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

        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumped)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, 5f);
            jumped = true;
        }

        body2D.velocity = body2D.velocity - new Vector2(0, 10f * Time.deltaTime);

        int mask = ~LayerMask.NameToLayer("Ground");
        RaycastHit2D hitFeet = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, mask);
        
        float ySpeed = -Mathf.Min(body2D.velocity.y, 0f) * Time.deltaTime + 0.05f;
        RaycastHit2D hitLower = Physics2D.Raycast(transform.position, Vector2.down, 0.5f + ySpeed, mask);

        // Debug.Log($"Feet: {hitFeet.point}, Lower: {hitLower.point}");

        if (hitFeet.collider != null && body2D.velocity.y < 0)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, 0);
            collisionYPos = hitLower.point.y + 0.5f;
            setYPos = true;
            jumped = false;
        }
        else if (hitLower.collider != null && body2D.velocity.y < 0)
        {

            // Debug.Log($"Hit collider while falling. setting y pos to {hitLower.point.y}");
            body2D.velocity = new Vector2(body2D.velocity.x, 0);
            collisionYPos = hitLower.point.y + 0.5f;
            setYPos = true;
            jumped = false;
        }

        bool walkingBackwards = aiming.IsFlipped == body2D.velocity.x >= 0;

        if (moving)
        {
            Debug.Log("Playing");
            moveUpperAnim.SetBool("walking", true);
            moveLowerAnim.SetBool("walking", true);
            moveUpperAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
            moveLowerAnim.SetFloat("speedMult", walkingBackwards ? -1f : 1f);
        }
        else
        {
            Debug.Log("Not playing");
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