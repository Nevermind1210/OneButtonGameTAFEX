using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalexBG : MonoBehaviour
{
    [SerializeField] float depth = 1;

    PlayerControl player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float realVelocity = player.vel.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= -21)
            pos.x = 26;

        transform.position = pos;
    }
}
