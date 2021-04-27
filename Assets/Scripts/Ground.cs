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

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>(); //finding the player with the script

        collider = GetComponent<BoxCollider2D>();    
        groundHeight = transform.position.y + collider.size.y;
        screenRight = Camera.main.transform.position.x;
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.vel.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + collider.size.x;

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if(!didGenerateGround) 
        {
            if (groundRight < screenRight) // if the bounds of right ground and screen right is the less
            {
                didGenerateGround = true; 
                generateGround(); // invoke function
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
        float t = player.jumpVelocity / player.gravity; // 
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t))); // the natural jump arc when just simply pressing jump to account for gravity
        float maxJumpHeight = h1 + h2;
        float maxY = player.transform.position.y + maxJumpHeight; // highest point for the next object but still possible to land on.
        maxY *= 0.7f;
        float minY = 1;
        float actualY = Random.Range(minY, maxY) - goCollider.size.y / 2;

        pos.y = actualY; // the y
        pos.x = screenRight + 17; // set the screen offset value
        go.transform.position = pos; // and set the value of pos.
    }
}
