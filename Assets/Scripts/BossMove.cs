using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMove : MonoBehaviour
{
    GameObject enemyObj;
    public GameObject weapon;

    public float speed;

    private bool attacking = false;

    private GameObject player;
    
    float attackTimeout = 0.3f;

    float changeAttackTimer = 3.0f;

    int currentAttack = 0;

    public int health = 200;
  //  public GameObject healthBar;
    private WorldEvents world;
    public GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("level").GetComponent<WorldEvents>();
        player = GameObject.FindWithTag("Player");
        enemyObj = this.gameObject;
      // healthBar = GameObject.Find("Healthbar");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.healthBar.GetComponent<Slider>().value = this.health;
        attackTimeout -= Time.deltaTime;
        changeAttackTimer -= Time.deltaTime;

        if(changeAttackTimer <= 0){
            currentAttack = Random.Range(0,2);
            Debug.Log("boss atk " + currentAttack);
            changeAttackTimer = 3.0f;
        }

        if(currentAttack == 0){ //Follow player
            enemyObj.transform.LookAt(player.transform.position);
            if(attackTimeout <= 0){
                for(int i = 0; i < 3; i++){
                    GameObject kunai = Instantiate(weapon, enemyObj.transform.position,Quaternion.identity);
                    kunai.tag = "BossAtk";
                    kunai.GetComponent<Rigidbody>().AddForce(transform.forward*10);
                    if(i == 0){
                        kunai.GetComponent<Rigidbody>().AddForce(transform.right*5);
                    } else if(i == 2){
                        kunai.GetComponent<Rigidbody>().AddForce((-1)*transform.right*5);
                    }
                    
                    Destroy(kunai, 3);
                }
                attackTimeout = 0.4f;
            }
        } else if (currentAttack == 1){ //Spin-attack
            enemyObj.transform.Rotate(0,6,0,Space.Self);
            if(attackTimeout <= 0){
                    GameObject kunai = Instantiate(weapon, enemyObj.transform.position,Quaternion.identity);
                    kunai.tag = "BossAtk";
                    kunai.GetComponent<Rigidbody>().AddForce(transform.forward*20);
                    attackTimeout = 0.2f;
                    Destroy(kunai, 3);
            }
        }
        
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Kunai"){
            this.health -= 5;
            Destroy(other.gameObject);
        }
    }
        
       
        //enemyObj.transform.position += transform.forward * speed;
    }
}
