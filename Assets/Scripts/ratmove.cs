using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatMove : MonoBehaviour
{
    GameObject enemyObj;
    public GameObject[] wheels;
    public float speed;
    public float wheelTurnSpeed = 1.0f;
    public float tiltAng = 15;
    private float turn;

    private bool attacking = false;

    private GameObject player;
    
    float attackTimeout = 0.0f;

    public int health = 20;
    public GameObject healthBar;
    private WorldEvents world;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("level").GetComponent<WorldEvents>();
        turn = wheelTurnSpeed;
        player = GameObject.FindWithTag("Player");
        enemyObj = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        turn += wheelTurnSpeed * Time.deltaTime;
        enemyObj.transform.LookAt(player.transform.position);
       
        //enemyObj.transform.position += transform.forward * speed;
        move(speed);
    }

    void move(float speed){
        enemyObj.transform.position += enemyObj.transform.forward * speed * Time.deltaTime;
        Debug.Log("SPEEEED" + speed);
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
       attackTimeout -= Time.deltaTime;
        if(other.gameObject.tag == "Player"){
            speed = speed * -1;
            attacking = true;
            if(attackTimeout <= 0f){
                 other.gameObject.GetComponent<StarterAssets.Interface>().health -= 10;
                 attackTimeout = 1.0f;
            }
        }
        
        if(other.gameObject.tag == "Kunai"){
            Debug.Log(this.health);
            this.health -= 5;
            if(this.health <= 0){
                world.slainEnemies ++;
                Destroy(enemyObj);
            }
            this.healthBar.GetComponent<Slider>().value = this.health;
            Debug.Log(this.gameObject.name + " got hit");
            Destroy(other.gameObject);
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
