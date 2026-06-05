using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backobject : MonoBehaviour
{
    private Vector3 initPos;
    private Player player;

    [SerializeField, Header("‰¡ˆÚ“®")]
    private float Slide_MoveSpeed;
    float neutralpoint;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        player = FindObjectOfType<Player>();
        neutralpoint = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        FollowPlayer_Backobject();
       
    }

    private void FollowPlayer_Backobject()
    {
        float x = player.transform.position.x + neutralpoint;
        x = Mathf.Clamp(x, -Mathf.Infinity, Mathf.Infinity);

        transform.position = new Vector3(neutralpoint + player.transform.position.x * Slide_MoveSpeed, transform.position.y, transform.position.z);
    }
    
}
