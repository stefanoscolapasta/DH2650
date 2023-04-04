using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    GameObject g;
    GameObject p;
    public int speed = 3;
    public int maxHealth = 10;
    private int currentHealth;
    [SerializeField] private TextMeshProUGUI HealthBarUI;

    void Start(){
        currentHealth = maxHealth;
        //HealthBarUI.text = currentHealth + "/" + maxHealth;
        g = gameObject;
        p = GameObject.FindWithTag("Player");
        Debug.Log(g.tag);
    }

    void FixedUpdate() {
        g.transform.LookAt(p.transform.position);
        g.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //putHealhBarOnEnemy();
        //g.transform.position.y = 0;
        //g.transform.position = new Vector3(g.transform.position.x,1,g.transform.position.z);
        //g.transform.rotation = new Quaternion(0,0,0,0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kunai"){
            Debug.Log("Test2");
            currentHealth -= 5;
            HealthBarUI.text = currentHealth + "/" + maxHealth;
            if(currentHealth <= 0){
                Destroy(g);
            }
            Debug.Log("LeafKunaiPickedUp");
            Destroy(other.gameObject);
        }
    }

    void putHealhBarOnEnemy(){
        Vector3 pos = g.transform.position + new Vector3(0,10,0);
        Vector3 rot = g.transform.rotation.ToEulerAngles();

        
        HealthBarUI.transform.position = pos;
        HealthBarUI.transform.Rotate(new Vector3(0,rot.y,0));
    }


}