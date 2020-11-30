using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSawController : MonoBehaviour
{
    public GameObject player;
    private bool playerBendDown;

    /** 
     *  Update is called once per frame. Rotate the sprite of the saw
     */
    void Update()
    {
        transform.Rotate(Vector3.forward * +2f);
        CheckBendDown();
    }

    /** 
     *  Check is the player is bend down. Modify the hitbox of the saw if he is
     */
    void CheckBendDown()
    {
        
        //IF U WANT TO PRESS S BY YOURSELF
        // playerBendDown = Input.GetKey(KeyCode.S);
        // if (playerBendDown)
        // {
        //     // gameObject.GetComponent<CircleCollider2D>().enabled = false;
        //     gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
        // }
        // else
        // {
        //     gameObject.GetComponent<CircleCollider2D>().radius = 0.5f;
        // }

        if (PlayerController.checkToBend)
            {
            gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
            }
        else
        {
            gameObject.GetComponent<CircleCollider2D>().radius = 0.5f;
        }
                
    }

}
