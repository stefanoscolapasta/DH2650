using UnityEngine;
using static System.Math;
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
    GameObject[] progressGrassArray = new GameObject[4];
    GameObject[] playableLevels = new GameObject[4];
    MeshRenderer[] levelRenders = new MeshRenderer[4];
    GameObject player;
    bool[] clearedIsland = {false,false,false,false};
    GameObject[] Islands = new GameObject[4];
    Vector3[] islandWGD = new Vector3[4];
    int AssignedIsland;
    int oldAssigned = 0;
    int slainOnIsland = 0;

    void Start(){
        Islands[0] = GameObject.Find("0");
        Islands[1] = GameObject.Find("1");
        Islands[2] = GameObject.Find("2");
        Islands[3] = GameObject.Find("3");

        playableLevels[0] = GameObject.Find("Spawn0");
        playableLevels[1] = GameObject.Find("Spawn1");
        playableLevels[2] = GameObject.Find("Spawn2");
        playableLevels[3] = GameObject.Find("Spawn3");
        
        player = GameObject.FindWithTag("Player");
        //playableLevels = GameObject.FindGameObjectsWithTag("PlayableLevel");
        progressGrass = GameObject.FindGameObjectsWithTag("ProgressGress")[0];
        InitalizeGame();
        g = gameObject;
    }

    
    void InitalizeGame(){
        int AssignedIsland = 0;
        player.transform.position = playableLevels[AssignedIsland].transform.position;
        int counter = 0;
        foreach(GameObject mg in Islands){
            levelRenders[counter] = mg.GetComponent<MeshRenderer>(); 
            islandWGD[counter] = levelRenders[counter].bounds.size;
            counter ++;
        }
        for (int i = 0; i < 4; i++){
            progressGrassArray[i] = Instantiate(progressGrass, playableLevels[i].transform.position ,Quaternion.identity);
        }
    }

    void FixedUpdate() {
        
        if(waveSpawned == false){
            for(int i = 0; i < enemyLimits[currentWave]; i++){
                Vector3 pos = playableLevels[AssignedIsland].transform.position;
                float radius = islandWGD[AssignedIsland].x / 2 - 2;
                float angle = (Random.Range(0,Mathf.PI));
                float x = Mathf.Cos(angle)*radius;
                float y = Mathf.Sin(angle)*radius;
                GameObject enemy = Instantiate(rat,pos + new Vector3(x, 1, y) ,Quaternion.identity);
                enemy.tag = "Enemy";
            }
            waveSpawned = true;
        }

        if(AssignedIsland != oldAssigned){
            oldAssigned = AssignedIsland;
            player.transform.position = playableLevels[AssignedIsland].transform.position;
            slainOnIsland = 0;
        }

        if(slainEnemies > oldSlainEnemies){
            oldSlainEnemies ++;
            int totOnIsland = 0;
            foreach (int enemiesSpawn in enemyLimits){
                totOnIsland += enemiesSpawn;
            }
            slainOnIsland ++;
            Debug.Log(slainOnIsland);
            float radiuss = islandWGD[AssignedIsland].x / 2;
            float x = (((((float) slainOnIsland/ (float) totOnIsland) * radiuss) + 1) *1.9f);
            progressGrassArray[AssignedIsland].gameObject.transform.localScale = new Vector3(x,0.00001f,x);
        }
            
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            Debug.Log("Wave " + currentWave + " done");
            if(currentWave >= enemyLimits.Length - 1){
                currentWave = 0;
                clearedIsland[AssignedIsland] = true;
                AssignedIsland ++;
            }
            else{
                currentWave++;
            }
            slainEnemies = 0;
            oldSlainEnemies = 0;
            waveSpawned = false;
        }

    }
}