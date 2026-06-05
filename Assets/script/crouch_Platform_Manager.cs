using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crouch_Platform_Manager : MonoBehaviour
{
    PlatformEffector2D platformEffector2D;
   public void Awake()
    {
        platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    public void Thorough()
    {

        platformEffector2D.surfaceArc = 0;

    }

    public void Thorough_cancel()
    {

        platformEffector2D.surfaceArc = 179;

    }

    void Reset()
    {
        platformEffector2D.surfaceArc = 179;
    }

   
    
}
