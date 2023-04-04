using UnityEngine;

public class WorldEvents : MonoBehaviour
{
    GameObject g;
    public GameObject rat;
    int[] enemyLimits = {3,5,9};
    int spawnedEnemies = 1;
    MeshRenderer render;
    int currentWave = 0;

    void Start(){
        g = gameObject;
        render = GetComponent<MeshRenderer>();
    }

    void Update() {
        int spawn = Random.Range(1,100);
        if(spawn == 1 && spawnedEnemies < enemyLimits[currentWave]){
            spawnedEnemies++;
            //float x = Random.Range(0,render.bounds.size.x);
            int x = Random.Range(1,25);
            //float z = Random.Range(0,render.bounds.size.z);
            int z = Random.Range(1,25);
            GameObject enemy = Instantiate(rat,new Vector3(x,0,z),Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
        {
            
        }
}