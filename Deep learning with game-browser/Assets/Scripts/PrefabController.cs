using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TypeOfPlatform { leftArm, rightArm, jump, bendDown, neutral };

public class PrefabController : MonoBehaviour
{
    public TypeOfPlatform myType;
    public int width;
    public int height;
    public int InitialSpawnWeight;
    private int weight;
    public int Weight
    {
        get { return weight; }
        set { weight = value; }
    }
    public GameObject[] possibleNeighbours;
    private GameObject destructionPoint;

    /** 
     *  Initialization
     */
    void Start()
    {
        Weight = InitialSpawnWeight;
        destructionPoint = GameObject.Find("DestructionPoint");
        possibleNeighbours = possibleNeighbours.OrderBy(i => i.GetComponent<PrefabController>().InitialSpawnWeight).ToArray();
    }

    /** 
     *  Update is called once per frame. Destroy the prefab when it is behind the destruction point
     */
    void Update()
    {
        // if (gameObject.name == "platform6(Clone)")
        // {
        //     if (PlayerEmotionController.smileee >= 85)
        //     {
        //         gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        //     } else {
        //         gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //     }
        // }


        if (transform.position.x < destructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter(Collision collision) {
    //   if(collision.collider.tag == "water" || gameObject.name == "platform6(Clone)" || gameObject.name == "platform6")
    //   {
    //         if (PlayerEmotionController.smileee >= 85)
    //         {
    //             gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    //             gameObject.GetComponent<BoxCollider2D>().enabled = false;
    //             Debug.Log("water");
    //         } else {
    //             gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    //         }

    //   }

    }
}
