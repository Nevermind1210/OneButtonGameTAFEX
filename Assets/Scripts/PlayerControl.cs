using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float gravity; // Well the gravity of the player this affects jump velocity and such
    public Vector2 vel; // The velocity....
    public float maxXVelocity = 100; // the max it can go
    public float maxAcceleration = 10; // the max FAST it can go
    public float acceleration = 10; // 
    public float distance = 0; // as it says
    public float jumpVelocity = 20; // how hard you can jump
    public float groundHeight = 10; // the height of the player to the ground.
    public bool isGrounded = false; // basic ground detection.

    public bool IsHoldingButton = false; // this is pretty much what it saids.
    public float maxHoldButtonTime = 0.4f; // the time that the player is allowed to hold it reached regardless if player is holding it will drop and reset
    public float holdButtonCap = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float pixelPerfectJump = 1; // for the feeling of visual feedback the help that the player visually KNOWS it can jump and do it perfectly!

    public bool isDead = false;

    void Update()
    {
        Vector2 pos = transform.position; // defining position
        float groundDistance = Mathf.Abs(pos.y - groundHeight); // taking the position and finding the ground for pixelPerfect jumps

        if (isGrounded || groundDistance <= pixelPerfectJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                vel.y = jumpVelocity;
                IsHoldingButton = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsHoldingButton = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) // well its a dead checker!
        {
            return;
        }

        Vector2 pos = transform.position;

        if (pos.y < -20) // if so you are dead!
        {
            isDead = true;
        }

        if (!isGrounded) // this nested if statement is essentally checking gravity and making sure everything stays in values so you can't jump infinity.
        {
            if (IsHoldingButton)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldButtonTime)
                {
                    IsHoldingButton = false;
                }
            }

            pos.y += vel.y * Time.fixedDeltaTime;
            if (!IsHoldingButton)
            {
                vel.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);// creating a new rayOrigin
            Vector2 rayDirection = Vector2.up; 
            float rayDistance = vel.y * Time.fixedDeltaTime; 
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null) // to check if you are still on the ground 
                {
                    if (pos.y >= ground.groundHeight) // and if so check again then calculate.
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        vel.y = 0;
                        isGrounded = true;
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            Vector2 wallOrigin = new Vector2(pos.x, pos.y); // in the event the building that comes are taller than you then plop dead.
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, vel.x * Time.fixedDeltaTime);
            if (wallHit.collider != null) 
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        vel.x = 0;
                    }
                }
            }
        }

        distance += vel.x * Time.fixedDeltaTime;

        if (isGrounded) // GRAVITY CHECK AGAIN WOOOOOOOO
        {
            float velocityRatio = vel.x / maxXVelocity; // WE DIVIDE THIS
            acceleration = maxAcceleration * (1 - velocityRatio); // MAKE IT ONE OFF
            maxHoldButtonTime = holdButtonCap * velocityRatio; // NOW WE CAN HOLD LONGER

            vel.x += acceleration * Time.fixedDeltaTime; // MAKE SURE GRAVITY CHECK ITSELF
            if (vel.x >= maxXVelocity) // OH GOD TOO MUCH
            {
                vel.x = maxXVelocity; // NOW ITS OKAY NORMALIZE IT
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y); // man 3 raycasts seems excessive but really nessesary for this physics I am doing. probably better solutions but it works so hey.
            Vector2 rayDirection = Vector2.up;
            float rayDistance = vel.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }
        transform.position = pos;
    }
}
