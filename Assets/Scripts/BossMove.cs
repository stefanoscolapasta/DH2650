using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMove : MonoBehaviour
{
    GameObject enemyObj;
    public GameObject weapon;

    public float speed = 2;

    private GameObject player;
    
    float attackTimeout = 0.3f;

    public float changeAttackTimer = 3.0f;

    public int currentAttack = 0;

    public int health = 200;
  //  public GameObject healthBar;
    private WorldEvents world;
    public GameObject healthBar;
    public GameObject motherSeed;
    Animator animator;
    public Material greenMat;
    GameObject cactusBody;
    GameObject wheels;
    bool dead = false;
    GameObject weaponSpawn;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("level").GetComponent<WorldEvents>();
        player = GameObject.FindWithTag("Player");
        enemyObj = this.gameObject;
        animator = gameObject.GetComponent<Animator>();
        cactusBody = transform.Find("Boss/Cactus").gameObject;
        wheels = transform.Find("Boss/Wheels").gameObject;
        weaponSpawn = transform.Find("KunaiSpawn").gameObject;
      // healthBar = GameObject.Find("Healthbar");
       //Physics.IgnoreCollision(weapon.GetComponent<Collider>(),GetComponent<Collider>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!dead){
            enemyObj.transform.position += enemyObj.transform.forward * speed * Time.deltaTime;
            this.healthBar.GetComponent<Slider>().value = this.health;
            attackTimeout -= Time.deltaTime;
            changeAttackTimer -= Time.deltaTime;

            if(changeAttackTimer <= 0){
                animator.SetBool("run",false);
                currentAttack = Random.Range(0,3);
                Debug.Log("boss atk " + currentAttack);
                changeAttackTimer = 3.0f;
            }

            if(currentAttack == 0){ //Follow player
                animator.SetBool("run",true);
                enemyObj.transform.LookAt(player.transform.position);
                speed = 2;
                if(attackTimeout <= 0){
                // animator.SetBool("attack",true);
                animator.Play("shoot",0,0.0f);
                    for(int i = 0; i < 3; i++){
                        GameObject kunai = Instantiate(weapon, weaponSpawn.transform.position,Quaternion.identity);
                        kunai.tag = "BossAtk";
                        kunai.GetComponent<Rigidbody>().AddForce(transform.forward*3);
                        if(i == 0){
                            kunai.GetComponent<Rigidbody>().AddForce(transform.right*2);
                        } else if(i == 2){
                            kunai.GetComponent<Rigidbody>().AddForce((-1)*transform.right*2);
                        }
                        
                        Destroy(kunai, 3);
                    }
                    attackTimeout = 0.8f;
                    
                    //animator.SetBool("attack",false);
                }
            } else if (currentAttack == 1){ //Spin-attack
                speed = 2;
                enemyObj.transform.Rotate(0,6,0,Space.Self);
                if(attackTimeout <= 0){
                        GameObject kunai = Instantiate(weapon, weaponSpawn.transform.transform.position,Quaternion.identity);
                        kunai.tag = "BossAtk";
                        kunai.GetComponent<Rigidbody>().AddForce(transform.forward*5);
                        attackTimeout = 0.2f;
                        Destroy(kunai, 3);
                }
            } else if (currentAttack == 2){
                animator.SetBool("run",true);
                enemyObj.transform.LookAt(player.transform.position);
                speed = 7;
            }    
           // enemyObj.transform.Rotate
            //enemyObj.transform.position += transform.forward * speed;
            if(health <= 0){
                Instantiate(motherSeed, enemyObj.transform.position + new Vector3(5,0,0), Quaternion.identity);
                cactusBody.GetComponent<Renderer>().material = greenMat;
                dead = true;
                this.healthBar.SetActive(false);
                wheels.SetActive(false);
                player.GetComponent<StarterAssets.Interface>().victory.SetActive(true);
                player.GetComponent<StarterAssets.Interface>().button.SetActive(true);
                //Destroy(enemyObj);
            }
        }else{

        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Kunai"){
            Debug.Log("IT HIIIT");
            this.health -= 5;
            Destroy(other.gameObject);
        }
    }
    public bool isDead(){
        return this.dead;
    }
}
