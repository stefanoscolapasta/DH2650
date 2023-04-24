using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {   
        for(int i = 0; i < 3; i++){
            GameObject r = Instantiate(enemy, this.transform.position, Quaternion.identity);
            r.name = "Rat " + i;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
