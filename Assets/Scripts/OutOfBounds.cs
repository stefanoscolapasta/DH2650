using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutOfBounds : MonoBehaviour
{
    StarterAssets.Interface inter;
    WorldEvents world;
    GameObject[] levels = new GameObject[4];
    // Start is called before the first frame update
    void Awake()
    {
        inter = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.Interface>();
        world = GameObject.Find("level").GetComponent<WorldEvents>();
        levels[0] = GameObject.Find("Spawn0");
        levels[1] = GameObject.Find("Spawn1");
        levels[2] = GameObject.Find("Spawn2");
        levels[3] = GameObject.Find("Spawn3");
    }    

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            inter.health = 0;
            other.gameObject.transform.position = levels[0].transform.position;
        }
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.transform.position = levels[world.AssignedIsland].transform.position;
        }
    }
        
}

