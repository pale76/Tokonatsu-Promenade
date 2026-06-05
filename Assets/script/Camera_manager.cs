using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Camera_manager : MonoBehaviour
{

    //  [SerializeField, Header("振動時間＿条件")]
    //  private float shaketime＿条件;
    //  [SerializeField, Header("振動Size＿条件")]
    //  private float shakeMagnitude＿条件;

    private Vector3 initPos;
    private  Player player;
    private Player playerrife;
   //  private オブジェクト名 オブジェクト名;
   //  private float shakeCount;
    // private int current＿感知する変数;


    // Start is called before the first frame update
    void Start()
    {
        
        initPos = transform.position;
        player = FindObjectOfType<Player>();
      //  オブジェクト名 = FindObjectOfType<オブジェクト名>();
        // current＿感知する変数 = 本人.Get本物の感知する変数();
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null) return;
        FollowPlayer();
            FollowPlayer_updown();
            //   ShakeCheck();
        
       
    }
    

    private void FollowPlayer()
    {
        float x = player.transform.position.x;
        x = Mathf.Clamp(x, -Mathf.Infinity, Mathf.Infinity);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    private void FollowPlayer_updown()
    {
        float  y = player.transform.position.y;
        if (  Math.Abs(y) + Math.Abs(transform.position.y) > 4)
        {
            
            y = Mathf.Clamp(y, -Mathf.Infinity, Mathf.Infinity);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    // private void ShakeCheck()
    // {

    // if (current＿感知する変数 != 本人.Get本物の感知する変数())
    // {
    //     current＿感知する変数 != 本人.Get本物の感知する変数();
    //     shakeCount = 0.0f;
    //     StartCoroutine(shake());
    // }
    // }


    // IEnumerator Shake()
    // {
    //     Vector3 initPos = transform.position;

    //    while(shakeCount < shekeTime＿条件)
    //    {
    //        float x = initPos.x + Random.Range(- shakeMagnitube＿条件, shakeMagnitude＿条件);
    //        float y = initPos.y + Random.Range(- shakeMagnitube＿条件, shakeMagnitude＿条件);
    //        transform.position = new Vector3(x, y, initPos.z);
    //
    //        shakeCount += Time.deltaTime;

    //        yield return null;
    //    }

    //     transform.position = initPos;
    // }
}
