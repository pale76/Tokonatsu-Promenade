using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField , Header("横移動")]
    private float MoveSpeed;

    [SerializeField, Header("ジャンプ力")]
    private float JumpSpeed;

    [SerializeField, Header("空中ジャンプ力")]
    private float air_JumpSpeed;

    [SerializeField, Header("空中ジャンプ回数")]
    private int air_JumpCount;

    [SerializeField, Header("梯子を登る速度")]
    private int climb_speed;

    [Header("踏みつけ判定の高さの割合")]
    public float stepOnRate;

    [SerializeField, Header("無敵時間")]
    private float damageTime;

    [SerializeField, Header("点滅する時間")]
    private float flashTime;


    public float maxHealth = 100;
    private Vector2 inputDirection;
    private Rigidbody2D rigit;
    private bool bJump;
    private int aJump;
    private Animator anim;
    bool thorough = false;
    private Move_object moveObj = null;
    private string moveFloorTag = "MoveFloor";
    private string enemyTag = "Enemy";
    
    private BoxCollider2D Boxcol = null;
    // private bool isOtherJump = false;
    // private float otherJumpHeight;
    private SpriteRenderer spriteRenderer;
    private Player player;
    public float currentHealth;
    public HealthBar healthBar; 


    // Start is called before the first frame update
    void Start()
    {
        player = this;
        rigit = GetComponent<Rigidbody2D>();
        bJump = false;
        aJump = air_JumpCount;
        anim = GetComponent<Animator>();
        Boxcol = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
        
    }

    // Update is called once per frame
    void Update()
    { 
        if(player == null) return;
        if (currentHealth > 0)
        {


            Climb();
            Move();
            LookMoveDirec();
            HitFloor();
        }
        else
        {
            Destroy(gameObject);
            rigit.velocity = new Vector2(0, 0);
        }
       
        
    }

    private void Move()
    {

        if(transform.position.y < -4.1)
        {
            Destroy(gameObject);
        }

        
       //     if (isOtherJump)
            {
       //         rigit.velocity = new Vector2(rigit.velocity.x, 0);
       //         rigit.AddForce(Vector2.up * otherJumpHeight, ForceMode2D.Impulse);
       //         isOtherJump = false;
       //         anim.Play("Jump");
            }
        
       


        if (currentHealth >= 0)
        {
        rigit.velocity = new Vector2(inputDirection.x * MoveSpeed, rigit.velocity.y);
               
            anim.SetBool("Walk", inputDirection.x != 0.0f);     
        }
        else
        {
            rigit.velocity = new Vector2(0,0);
        }

        Vector2 addVelocity = Vector2.zero;
        if(moveObj != null )
        {
            addVelocity = moveObj.GetVelocity();          
        }
        rigit.velocity = new Vector2(rigit.velocity.x, rigit.velocity.y) + addVelocity;
    }

    private void LookMoveDirec()
    {
        if(inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }




    //private void OnCollisionEnter2D(Collision2D collision)
    //{
       // if (collision.gameObject.tag == "Floor")
      //  {
       //     bJump = false;
       //     anim.SetBool("Jump", bJump);
       // }
       
    //}

    private void HitFloor()
    {

        int layerMask = LayerMask.GetMask("Floor", "MoveFloor");
        Vector2 collisionSize = GetComponent<BoxCollider2D>().size;
        Vector2 boxSize =  4 * collisionSize;
        Vector3 rayPos = transform.position - new Vector3(0.0f , boxSize.y / 2.0f + 0.16f);
        Vector3 raySize = new Vector3(boxSize.x - 0.06f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
        
        if (rayHit.transform == null)
        {
            bJump = true;
            anim.SetBool("Jump", bJump);
            return;
        }
        
        if (rayHit.transform.tag == "Floor" && bJump)
        {
            
            bJump = false;
            air_JumpCount = aJump;
            anim.SetBool("Jump", bJump);
           
        }
        
        if (rayHit.transform.tag == "MoveFloor" && bJump)
        {
            bJump = false;
            air_JumpCount = aJump;
            anim.SetBool("Jump", bJump);
        }

       
        

        if (rayHit.transform.tag == "Floor" && bJump == false)
        {
            
        }

        if (thorough)
        {
            Collider2D[] hitPlatforms = Physics2D.OverlapBoxAll(rayPos, raySize, 0, layerMask);
            foreach (Collider2D HitPlatform in hitPlatforms)
            {
                crouch_Platform_Manager platform_Manager = HitPlatform.GetComponent<crouch_Platform_Manager>();
                if (platform_Manager != null)
                {
                    HitPlatform.GetComponent<crouch_Platform_Manager>().Thorough();
                }

            }
        }
        else
        {
            Collider2D[] hitPlatforms = Physics2D.OverlapBoxAll(rayPos, raySize, 0, layerMask);
            foreach (Collider2D HitPlatform in hitPlatforms)
            {
                crouch_Platform_Manager platform_Manager = HitPlatform.GetComponent<crouch_Platform_Manager>();
                if (platform_Manager != null)
                {
                    HitPlatform.GetComponent<crouch_Platform_Manager>().Thorough_cancel();
                }
               
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision != null)
        {
            int layerMask = LayerMask.GetMask("Default", "Floor", "MoveFloor");
            Vector2 collisionSize = GetComponent<BoxCollider2D>().size;
            Vector2 boxSize = 4 * collisionSize;
            Vector3 rayPos = transform.position - new Vector3(0.0f, boxSize.y / 2.0f + 0.16f);
            Vector3 raySize = new Vector3(boxSize.x - 0.06f, 0.1f);
            RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
            if (collision.collider.tag == enemyTag)
            {
                if(currentHealth > 0)
                {

               
                HitEnemy(collision.gameObject);
    
                 }

             //   float stepOnHeight = (Boxcol.size.y * (stepOnRate / 100));

             //   float judgePos = transform.position.y - (Boxcol.size.y / 2) + stepOnHeight;
             //   float judgePoc_x_m = transform.position.x - raySize.x / 2 + 0.1f;
             //   float judgePoc_x_p = transform.position.x + raySize.x / 2 - 0.1f;
             //   foreach (ContactPoint2D p in collision.contacts)
                {
                   
                        


             //       if (p.point.y >= judgePos)
                    {
                    
            //            anim.Play("Hurt");
            //            rife--;
            //            break;
                    //    if (rife <= 0)
                     //   {
                    //        break;
                     //   }

                              
                    }
              //      else
                    {
 
             //                   ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
             //                   if (o != null)
                                {

             //                       otherJumpHeight = o.boundHeight;
             //                       o.playerStepOn = true;
             //                       isOtherJump = true;
             //                       bJump = false;
            //                        break;
                                }
                    }
                }
            }
            else if(collision.gameObject.tag == "Goal")
            {
               
                enabled = false;
                GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<MainManager>().ShowGameClearUI();
            }
           

            if (rayHit.transform.tag == "MoveFloor")
            {
                moveObj = collision.gameObject.GetComponent<Move_object>();             
            }
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.collider.tag == moveFloorTag)
            {
            moveObj = null;
            }
        }
        
       
    }

    private void HitEnemy(GameObject enemy)
    {
        ObjectCollision o = enemy.GetComponent<ObjectCollision>();
        float halfScaleY = transform.lossyScale.y * 0.24f / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y * 0.2f / 2.0f;
        if(transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            rigit.AddForce(Vector2.up * o.boundHeight, ForceMode2D.Impulse);
            anim.Play("crouch");
            anim.Play("Jump");
        }
        else
        {
            enemy.GetComponent<ObjectCollision>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            anim.Play("Hurt");
            StartCoroutine(Damage());

        }

            
    }

    private void Dead()
    {
        if(currentHealth <= 0)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);
        }
    }


    IEnumerator Damage()
    {
        
        Color color = spriteRenderer.color;
        for (int i = 0; i< damageTime; i++)
        {
            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
       
        spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Damage (int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);      
        Dead();
        healthBar.SetHealth(currentHealth);
    }


    private void Climb()
    {
       

        if (Input.GetKey(KeyCode.Q))
        {
            int layerMask = LayerMask.GetMask("Ladder");
            Vector2 collisionSize = GetComponent<BoxCollider2D>().size;
            Vector2 boxSize = 4 * collisionSize;
            Vector3 Ladder_rayPos = transform.position + new Vector3(0.0f, 0.0f);
            Vector3 Ladder_raySize = new Vector3(boxSize.x - 0.1f, boxSize.y - 0.1f);
            Collider2D[] hitPlatforms = Physics2D.OverlapBoxAll(Ladder_rayPos, Ladder_raySize, 0, layerMask);

            if (hitPlatforms.Length > 0)
            {
               
                rigit.velocity = new Vector2(rigit.velocity.x, climb_speed);
                anim.SetBool("Climb", true);
            }
            else
            {
                
                anim.SetBool("Climb", false);

            }
           
        }
        else
        {
            
            anim.SetBool("Climb", false);
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        
        
       
    }

   




    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        

       

        if (context.performed)
        {

            anim.SetBool("crouch", true);
            thorough = true;


            
        }
        else
        {
            anim.SetBool("crouch", false);
            thorough = false;


        }

        
    }

    public void Onjump(InputAction.CallbackContext context)
    {
  



        if (!context.performed || bJump)
        {
            if (!context.performed || air_JumpCount < 1)
            {
                return;
            }
            else
            {
                air_JumpCount--;
                rigit.velocity = new Vector2(rigit.velocity.x, 0);
                rigit.AddForce(Vector2.up * air_JumpSpeed, ForceMode2D.Impulse);
            }
        }
        else
        {
            bJump = true;

            rigit.velocity = new Vector2(rigit.velocity.x, 0);
            rigit.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
            return;
        }

    

       
    }


    
}
