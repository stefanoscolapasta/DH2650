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
    public GameObject dropItem;

    private bool onVine;

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
       attackTimeout -= Time.deltaTime;
        //enemyObj.transform.position += transform.forward * speed;
        move(speed);
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
       if(other.gameObject.tag == "Plant"){
            speed = 0;
        }
        if(other.gameObject.tag == "Player"){
            speed = speed * -1;
            attacking = true;
            if(attackTimeout <= 0f){
                 other.gameObject.GetComponent<StarterAssets.Interface>().health -= 10;
                 attackTimeout = 1.0f;
            }
        }
        
        if(other.gameObject.tag == "Kunai"){
            this.health -= 20;
            if(this.health <= 0){
                world.slainEnemies ++;
                int itemRoll = Random.Range(0,10);
                if(itemRoll < 3){
                    GameObject drop = Instantiate(dropItem,enemyObj.transform.position,Quaternion.identity);
                    drop.transform.localScale /= 4;
                }
                Ray ray = new Ray(enemyObj.transform.position, Vector3.down);
                Debug.DrawRay(enemyObj.transform.position, Vector3.down * 1, Color.yellow);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    Debug.Log("success");
                    Vegetation.Controller.Instance.CreatePlant(
                    0, 0.8f, true, hit.point,
                    Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles,
                    true);
                }
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
        if(other.gameObject.tag == "Plant"){
            speed = 0;
        }
    }
    
}
