using UnityEngine;

public class WorldEvents : MonoBehaviour
{
    GameObject g;
    public GameObject rat;
    public int[] enemyLimits = {1,2,3};
    int spawnedEnemies = 0;
    MeshRenderer render;
    public int currentWave = 0;
    bool waveSpawned = false;
    public int slainEnemies = 0;
    private int oldSlainEnemies = 0;
    private int scaleEnemies = 0;
    GameObject progressGrass;
    GameObject[] playableLevels;
    GameObject player;
    bool[] clearedIsland = {false,false,false,false};
    Vector3[] islandWGD = new Vector3[4];
    int AssignedIsland;

    void Start(){
        player = GameObject.FindWithTag("Player");
        playableLevels = GameObject.FindGameObjectsWithTag("PlayableLevel");
        InitalizeGame();
        g = gameObject;
        progressGrass = GameObject.FindWithTag("ProgressGress");
        int counter = 0;
        //foreach(GameObject level in playableLevels){
            MeshRenderer islandRenderer = playableLevels[counter].GetComponent<MeshRenderer>(); 
            islandWGD[counter] = islandRenderer.bounds.size;
            counter ++;
        //}
        //playableLevels = GameObject.FindGameObjectsWithTag("PlayableLevel");

    }

    
    void InitalizeGame(){
        playableLevels = GameObject.FindGameObjectsWithTag("PlayableLevel");
        int AssignedIsland = 0;//Random.Range(0,3); 
        player.transform.position = playableLevels[AssignedIsland].transform.position;
        Debug.Log(player.transform.position);//global
        Debug.Log(playableLevels[AssignedIsland].transform.localPosition); //local
    }

    void FixedUpdate() {
       // int spawn = Random.Range(1,100);
        
        if(waveSpawned == false){
            for(int i = 0; i < enemyLimits[currentWave]; i++){
                Vector3 pos = playableLevels[AssignedIsland].transform.position;
                float radius = (islandWGD[AssignedIsland].x + islandWGD[AssignedIsland].z) / 2;
                float angle = (Random.Range(0,Mathf.PI));
                float x = Mathf.Cos(angle)*radius;
                float y = Mathf.Sin(angle)*radius;
                //float z = Random.Range(0,render.bounds.size.z);
                GameObject enemy = Instantiate(rat,pos + new Vector3(x, 0, y) ,Quaternion.identity);
                enemy.tag = "Enemy";
            }
            waveSpawned = true;
        }

        if(slainEnemies > oldSlainEnemies){
            Debug.Log(slainEnemies);
            Debug.Log(oldSlainEnemies);
            oldSlainEnemies ++;
            scaleEnemies ++;
            progressGrass.gameObject.transform.localScale = new Vector3(scaleEnemies,0,scaleEnemies);
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
            slainEnemies = 0;
            oldSlainEnemies = 0;
            for(int i = 0; i < enemyLimits[currentWave]; i++){
                int x = -(Random.Range(15,35));
                //float z = Random.Range(0,render.bounds.size.z);
                int z = Random.Range(14,32);
                GameObject enemy = Instantiate(rat,new Vector3(x,47,z),Quaternion.identity);
                enemy.tag = "Enemy";
            }
        }

    }
}