using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    PlayerControl player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    new BoxCollider2D collider;

    bool didGenerateGround = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>(); //finding the player with the script

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position; // this shows the effect of movement for the player essentially auto running
        pos.x -= player.vel.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if(!didGenerateGround)
        {
            if (groundRight < screenRight) // if the bounds of right ground and screen right is the less then die...
            {
                didGenerateGround = true;
                generateGround(); // invoke the function
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject); // we repeat the gameObject when bounds are found
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();  // get the componenet
        Vector2 pos; // set the position

        float h1 = player.jumpVelocity * player.holdJumpTimer; // we use this first calculation to for the possible time that the player can hold jump for.
        float t = player.jumpVelocity / -player.gravity; //
        float h2 = player.jumpVelocity * t + (0.5f * (-player.gravity * (t * t))); // the natural jump arc when just simply pressing jump to account for gravity
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.7f; // highest point for the next object but still possible to land on.
        maxY += groundHeight; // clamping it
        float minY = 1;
        float actualY = Random.Range(minY, maxY); // finds a space within the world

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = t + player.maxHoldButtonTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.vel.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2;  // set the screen offset value
        go.transform.position = pos; // and set the value of pos.

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);
    }
}
