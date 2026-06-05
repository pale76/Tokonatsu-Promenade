using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Ground : MonoBehaviour
{
    private Vector3 initPos;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        FollowPlayer_BackGround();
        FollowPlayer_updown_BackGround();
    }

    private void FollowPlayer_BackGround()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        x = Mathf.Clamp(x, -Mathf.Infinity, Mathf.Infinity);
        
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    private void FollowPlayer_updown_BackGround()
    {
        float y = player.transform.position.y;
        if (Math.Abs(y) + Math.Abs(transform.position.y) > 4)
        {

            y = Mathf.Clamp(y, -Mathf.Infinity, Mathf.Infinity);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
