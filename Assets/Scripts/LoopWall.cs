using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoopWall : MonoBehaviour
{
    // simple script to make the player's location loop when moving horizontally off screen
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector3 originalPos = collision.transform.position;
            collision.gameObject.transform.position = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
        }
    }
}
