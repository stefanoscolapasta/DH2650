using UnityEngine;

public class WorldEvents : MonoBehaviour
{
    GameObject g;
    public GameObject rat;
    int[] enemyLimits = {3,5,9,12,20};
    int spawnedEnemies = 0;
    MeshRenderer render;
    int currentWave = 0;
    bool waveSpawned = false;
    public int slainEnemies = 0;
    private int oldSlainEnemies = 0;
    GameObject progressGrass;

    void Start(){
        g = gameObject;
        render = GetComponent<MeshRenderer>();
        progressGrass = GameObject.FindWithTag("ProgressGrass");
    }

    void FixedUpdate() {
       // int spawn = Random.Range(1,100);
        
        if(waveSpawned == false){
            for(int i = 0; i < enemyLimits[currentWave]; i++){
                int x = -(Random.Range(15,35));
                //float z = Random.Range(0,render.bounds.size.z);
                int z = Random.Range(14,32);
                GameObject enemy = Instantiate(rat,new Vector3(x,47,z),Quaternion.identity);
                enemy.tag = "Enemy";
            }
            waveSpawned = true;
        }
        if(oldSlainEnemies != slainEnemies){
            oldSlainEnemies ++;
            progressGrass.gameObject.transform.localScale += new Vector3(oldSlainEnemies,0,oldSlainEnemies);
        }
            
        
       // var spawnedEnemies : GameObject[];
       // spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
      /*  if(spawn == 1 && spawnedEnemies < enemyLimits[currentWave]){
            spawnedEnemies++;
            //float x = Random.Range(0,render.bounds.size.x);
            int x = Random.Range(1,25);
            //float z = Random.Range(0,render.bounds.size.z);
            int z = Random.Range(1,25);
            GameObject enemy = Instantiate(rat,new Vector3(x,0,z),Quaternion.identity);
            enemy.tag = "Enemy";
        }*/
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            Debug.Log("Wave " + currentWave + " done");
            currentWave++;
            for(int i = 0; i < enemyLimits[currentWave]; i++){
                int x = -(Random.Range(15,35));
                //float z = Random.Range(0,render.bounds.size.z);
                int z = Random.Range(14,32);
                GameObject enemy = Instantiate(rat,new Vector3(x,47,z),Quaternion.identity);
                enemy.tag = "Enemy";
            }
        }

    }

    void OnTriggerEnter(Collider other)
        {
            
        }
}