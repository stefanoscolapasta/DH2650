using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject g;
    GameObject p;
    public int speed = 3;
    public int health = 10;

    void Start(){
        g = gameObject;
        p = GameObject.FindWithTag("Player");
        Debug.Log(g.tag);
    }

    void FixedUpdate() {
        g.transform.LookAt(p.transform.position);
        
        g.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //g.transform.position.y = 0;
        g.transform.position = new Vector3(g.transform.position.x,1,g.transform.position.z);
        //g.transform.rotation = new Quaternion(0,0,0,0);
    }

    void OnTriggerEnter(Collider other)
        {
            Debug.Log("Testttt1");
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