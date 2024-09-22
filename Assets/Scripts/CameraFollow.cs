using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    public float lerpSpeed, zDistance;
    Vector3 pointToFollow;
    GameObject[] players;
    void Start()
    {   
        
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 2)
        {
            pointToFollow = (players[0].transform.position + players[1].transform.position) / 2;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pointToFollow = player.transform.position;
        }


        float posX = Mathf.Lerp(transform.position.x, pointToFollow.x, lerpSpeed);
        float posZ = Mathf.Lerp(transform.position.z, pointToFollow.z + zDistance, lerpSpeed);
        
        transform.position = new Vector3(posX,transform.position.y,posZ);
    }
}
