using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [Header("ìGÇì•ÇÒÇæéûÇÃçÇÇ≥")]
    public float boundHeight;
    [HideInInspector]
    public bool playerStepOn;
    [SerializeField, Header("à⁄ìÆë¨ìx")]
    private float movespeed;
    [SerializeField, Header("çUåÇóÕ")]
    private int attackPower;

    private Rigidbody2D rigit;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigit = GetComponent<Rigidbody2D>(); 
        moveDirection = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ChengeMoveDirection();
        LookMoveDirection();
    }

    private void Move()
    {
        rigit.velocity = new Vector2(moveDirection.x * movespeed, rigit.velocity.y);
    }

    private void ChengeMoveDirection()
    {
        Vector2 halfSize = transform.lossyScale / 5.0f;
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
        if(ray.transform == null)return;
        if(ray.transform.tag == "Floor")
        {
            moveDirection = -moveDirection;
        }
    }

    private void LookMoveDirection()
    {
        if(moveDirection.x < 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        if (moveDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    public void PlayerDamage(Player player)
    {
        player.Damage(attackPower);
    }
}
