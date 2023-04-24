using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratmove : MonoBehaviour
{
    public GameObject enemyObj;
    public GameObject[] wheels;
    public float speed;
    public float wheelTurnSpeed = 1.0f;
    public float tiltAng = 15;
    private float turn;

    private bool attacking = false;

    private GameObject player;
    


    // Start is called before the first frame update
    void Start()
    {
       turn = wheelTurnSpeed;
       player = GameObject.FindWithTag("Player");
      
    }

    // Update is called once per frame
    void Update()
    {
        

        turn += wheelTurnSpeed * Time.deltaTime;
        enemyObj.transform.LookAt(player.transform.position);
       
        //enemyObj.transform.position += transform.forward * speed;
        if(speed != 0.0f){
            move(speed);
        }
        else{
            move(speed);
        }
        

        
    }

    void move(float speed){
        enemyObj.transform.position += enemyObj.transform.forward * speed * Time.deltaTime;
        
        if(attacking){

            enemyObj.transform.position += Vector3.up * Time.deltaTime;
        }else{
            for(int i = 0; i < wheels.Length; i++){
                wheels[i].transform.localRotation = Quaternion.Euler(turn * speed,0,0);
            }
        }
         
    }

    void OnTriggerEnter(Collider other){
       // Debug.Log(other.gameObject.tag + " Enter ");
        if(other.gameObject.tag == "Player"){
            speed = speed * -1;
            attacking = true;
            other.gameObject.GetComponent<StarterAssets.Interface>().health -= 10;
            Debug.Log(other.gameObject.GetComponent<StarterAssets.Interface>().health);
        }
    }

    void OnTriggerExit(Collider other){
       // Debug.Log(other.gameObject.tag + " Exit ");
        if(other.gameObject.tag == "Player"){
            speed = speed * -1;
            attacking = false;
        }
    }
}
