using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    PlayerControl player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false; 

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>(); //finding the player with the script

        collider = GetComponent<BoxCollider2D>();    
        groundHeight = transform.position.y + (collider.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.vel.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (collider.size.x / 2);

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
        pos.x = screenRight + 30; // set the screen offset value
        pos.y = transform.position.y; // the y
        go.transform.position = pos; // and set the value of pos.
    }
}
