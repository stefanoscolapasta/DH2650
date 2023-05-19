using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GardenScript : MonoBehaviour
{
    // Start is called before the first frame update
    

    void OnTriggerEnter(Collider other){
        Debug.Log(other.GetComponent<Collider>().name);
        if(other.GetComponent<Collider>().gameObject.tag == "Player"){
            SceneManager.LoadScene("Playground");
        }
    }
        
}

