using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratmove : MonoBehaviour
{
    public GameObject enemyObj;
    public GameObject[] wheels;
    public float speed = 1.0f;
    public float wheelTurnSpeed = 1.0f;
    public float tiltAng = 15;
    private float turn;
    


    // Start is called before the first frame update
    void Start()
    {
       turn = wheelTurnSpeed;
      
    }

    // Update is called once per frame
    void Update()
    {
      //  enemyObj.transform.position += transform.forward * speed;

        turn += wheelTurnSpeed * Time.deltaTime;
        if(speed < 0){
            move(-1);
        }else
        {
            move(1);
        }
        

        
    }

    void move(float speed){
        for(int i = 0; i < wheels.Length; i++){
                    wheels[i].transform.localRotation = Quaternion.Euler(turn * speed,0,0);
        }
         enemyObj.transform.localRotation = Quaternion.Euler(-tiltAng * speed ,0,0);
    }
}
