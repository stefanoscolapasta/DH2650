using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    GameObject g;
    GameObject p;
    public int speed = 3;
    public int health = 10;
    private int currentHealth;
    public Slider healthBar;

    void Start(){
        g = gameObject;
        p = GameObject.FindWithTag("Player");
        Debug.Log(g.tag);
    }

    void FixedUpdate() {
        healthBar.value = health;
        if(g.tag == "Enemy"){
            g.transform.LookAt(p.transform.position);
            g.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kunai"){
            Debug.Log("Test2");
            health -= 5;
            if(health <= 0){
                Destroy(g);
            }
            Debug.Log("LeafKunaiPickedUp");
            Destroy(other.gameObject);
        }
    }
}