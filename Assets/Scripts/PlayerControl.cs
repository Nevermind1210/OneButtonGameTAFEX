using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float gravity; // gravity is well gravity! Weight.
    public Vector2 vel; // Velocity the speed of a thing (player)
    public float maxXVelocity = 100; // Defining the max of a direction
    public float maxAcceleration = 10; // Defining the max Speed of player
    public float distance = 0;
    public float acceleration = 10; // defining speed
    public float jumpVelocity = 20; // defining how HIGH the jump
    public float groundHeight; // defining the height for basic ground dectection
    public bool isGrounded = false; // Ground Detection.
    public bool isJumping = false;

    public bool isHoldingButton = false; // for checking if the player is well holding a button
    public float maxHoldButtonTime = 0.4f; // the time that the player is allowed to hold it if reached regardless if player is holding it will drop and reset.
    public float holdButtonCap = 0.4f;
    public float holdJumpTimer = 0.0f; // self explanatory.

    public float pixelPerfectJump = 1; // for the feeling of visual feedback the help that the player visually KNOWS it can jump and do it perfectly!
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
        Vector2 pos = transform.position; // defining position
        float groundDist = Mathf.Abs(pos.y - groundHeight); // taking the position and finding the ground for pixelperfect jumps!

        if(!isGrounded && isJumping)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
             if (isGrounded || groundDist <= pixelPerfectJump)
            {
            
                //isGrounded = false;
                vel.y = jumpVelocity; // taking the value
                isHoldingButton = true;
                holdJumpTimer = 0; // resets every time to allow the next jump after landing
                isJumping = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingButton = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        
        if(isDead == true)
        {
            return;
        }

        if(pos.y < -20)
        {
            isDead = true;
        }

        Vector2 rayOrigin = gameObject.transform.position; //new Vector2(pos.x + 0.7f, pos.y);
        Vector2 rayDirection = Vector2.down;
        float rayDistance = gameObject.GetComponent<Renderer>().bounds.extents.y + 0.01f;//vel.y * Time.fixedDeltaTime;
        RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        if (hit2D.collider != null)
        {
            Ground ground = hit2D.collider.GetComponent<Ground>();
            if (ground != null)
            {
                if (!isJumping)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = hit2D.collider.bounds.max.y;
                        //groundHeight = ground.groundHeight;
                        pos.y = groundHeight + gameObject.GetComponent<Renderer>().bounds.extents.y;
                        vel.y = 0;
                    }
                }
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

        pos.y += vel.y * Time.fixedDeltaTime;

        if (!isGrounded)
        {
            if(isHoldingButton)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= maxHoldButtonTime)
                {
                    isHoldingButton = false;
                }
            }

            if(!isHoldingButton)
            {
                vel.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, vel.x * Time.fixedDeltaTime);
            if(wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    if(pos.y < ground.groundHeight)
                    {
                        vel.x = 0;
                    }
                }
            }
        }

        distance += vel.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = vel.x / maxXVelocity; // calulating the Velocity for moving towards its jumping
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldButtonTime = holdButtonCap * velocityRatio; // this essentially brings is when player is slow is small jumps and player is fast well HUGE jumps!

            vel.x += acceleration * Time.fixedDeltaTime;
            if (vel.x >= maxXVelocity)
            {
                vel.x = maxXVelocity;
            }

            /*Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = vel.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.magenta);*/
        }



        transform.position = pos;
    }
}
