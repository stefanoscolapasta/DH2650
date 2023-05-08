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

    public int health = 200;
  //  public GameObject healthBar;
    private WorldEvents world;

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
        attackTimeout -= Time.deltaTime;
        //enemyObj.transform.LookAt(player.transform.position);
        enemyObj.transform.Rotate(0,3,0,Space.Self);
        if(attackTimeout <= 0){
            for(int i = 0; i < 3; i++){
                GameObject kunai = Instantiate(weapon, enemyObj.transform.position,Quaternion.identity);
               // kunai.transform.Rotate(0,0,i,Space.Self);
                kunai.GetComponent<Rigidbody>().AddForce(transform.forward*15);
            }
            attackTimeout = 0.3f;
        }
       
        //enemyObj.transform.position += transform.forward * speed;
    }
}
