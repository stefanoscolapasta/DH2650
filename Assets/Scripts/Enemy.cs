using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    GameObject g;
    GameObject p;
    public GameObject grass;
    public int speed = 3;
    public int health = 10;
    private int currentHealth;
    public Slider healthBar;
    bool attacking = false;
    float attackCooldown = 3f;
    WorldEvents world;
    void Awake(){
        world = GameObject.Find("Ground").GetComponent<WorldEvents>();
    }
    void Start(){
        g = gameObject;
        p = GameObject.FindWithTag("Player");
        Debug.Log(g.tag);
    }

    void FixedUpdate() {
        healthBar.value = health;
        if(g.tag == "Enemy"){
            g.transform.LookAt(p.transform.position);
            float distToPlayer = Vector3.Distance(p.transform.position,transform.position);
            if(distToPlayer > 5 || attackCooldown < 0f){
                g.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            } else{
                if(attackCooldown > 0f){
                    attackCooldown -= Time.deltaTime;
                }
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kunai"){
            Debug.Log("Test2");
            health -= 5;
            if(health <= 0){
                GameObject grassClone = Instantiate(grass,new Vector3(g.transform.position.x,47.25f,g.transform.position.z),Quaternion.identity);
                world.slainEnemies ++;
                //Debug.Log(world.slainEnemies);
                Destroy(g);
            }
            Debug.Log("LeafKunaiPickedUp");
            Destroy(other.gameObject);
        }
    }

    public float getAtkCD(){
        return this.attackCooldown;
    }
    public void setAtkCD(float cd){
        this.attackCooldown = cd;
    }
}
