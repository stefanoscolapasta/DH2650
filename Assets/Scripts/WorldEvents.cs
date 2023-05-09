using UnityEngine;
using System.Collections;
public class WorldEvents : MonoBehaviour
{
    GameObject g;
    public GameObject rat;
    public GameObject boss;
    //public int[,] enemyLimits = new int[3,3]{{1,2,2},{2,3,5},{3,5,7}};
    public int[,] enemyLimits = new int[3,3]{{1,1,1},{1,1,1},{1,1,1}}; // FOR TESTING
    public int[] totalEnemies = {3,3,3};
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
    bool bossSpawned = false;
    
    GameObject[] IslandVeg = new GameObject[3]; //Change this to 4 if only 4 islands is used
    GameObject[][] arrayOfVegChild = new GameObject[3][];
    bool[][] activeVeg = new bool[3][];
    int vegIndex = 0;
    float[][] vegScaler = new float[3][];

    void Start(){
        Islands[0] = GameObject.Find("0");
        Islands[1] = GameObject.Find("1");
        Islands[2] = GameObject.Find("2");
        Islands[3] = GameObject.Find("3");

        playableLevels[0] = GameObject.Find("Spawn0");
        playableLevels[1] = GameObject.Find("Spawn1");
        playableLevels[2] = GameObject.Find("Spawn2");
        playableLevels[3] = GameObject.Find("Spawn3");
        
        //Prep for all of vegetation to spawn
        //----------------------------------------------------

        IslandVeg[0] = GameObject.Find("Island0veg");
        IslandVeg[1] = GameObject.Find("Island1veg");
        IslandVeg[2] = GameObject.Find("Island2veg");

        int counter = 0;
        foreach (var vegG in IslandVeg)
        {
            arrayOfVegChild[counter] = new GameObject[vegG.transform.childCount];
            activeVeg[counter] = new bool[vegG.transform.childCount];
            vegScaler[counter] = new float[vegG.transform.childCount];
            for (int i = 0; i < vegG.transform.childCount; i++){
                vegScaler[counter][i] = 0.000001f;
                arrayOfVegChild[counter][i] = vegG.transform.GetChild(i).gameObject;
                arrayOfVegChild[counter][i].transform.localScale = new Vector3(vegScaler[counter][i],vegScaler[counter][i],vegScaler[counter][i]);
                arrayOfVegChild[counter][i].active = false;
                activeVeg[counter][i] = false; //prolly unnecessary but wanna make sure it works as intended.
            }
            counter ++;
        }

        //------------------------------------------------

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
        int outer = 0;
        int inner = 0;
        foreach (var vegitationInProcessPerIsland in activeVeg)
        {
            foreach (bool b in vegitationInProcessPerIsland)
            {
                if(b){
                    if(vegScaler[outer][inner] >= 1f){
                        activeVeg[outer][inner] = false;
                    }
                    else{
                        vegScaler[outer][inner] += 0.01f;
                        arrayOfVegChild[outer][inner].transform.localScale = new Vector3(vegScaler[outer][inner],vegScaler[outer][inner],vegScaler[outer][inner]);
                    }
                }
                inner ++;
            }
            outer ++;
            inner = 0;
        }

        if(waveSpawned == false && bossSpawned == false){
            Debug.Log("isla " + AssignedIsland);
            Debug.Log("wave " + currentWave);
            for(int i = 0; i < enemyLimits[AssignedIsland,currentWave]; i++){
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
            vegIndex = 0;
        }

        if(slainEnemies > oldSlainEnemies){
            oldSlainEnemies ++;
            int totOnIsland = 0;
            foreach (int enemiesSpawn in enemyLimits){
                totOnIsland += enemiesSpawn;
            }
            slainOnIsland ++;
            Debug.Log(slainOnIsland);
            float ratioOfKilledEnemies = (float) slainOnIsland/ (float) totalEnemies[AssignedIsland];
            float radiuss = islandWGD[AssignedIsland].x / 2;
            float x = (((ratioOfKilledEnemies * radiuss) + 1) *1.9f);
            progressGrassArray[AssignedIsland].gameObject.transform.localScale = new Vector3(x,0.00001f,x);
            //Set veg on island to start animate()
            float upperLimitOfVeg = (float) arrayOfVegChild[AssignedIsland].Length * (float) ratioOfKilledEnemies;
            for(int i = vegIndex ; i <= upperLimitOfVeg; i ++){
                if(i >= activeVeg[AssignedIsland].Length){
                    break;
                }
                Debug.Log("vegIndex " + i);
                Debug.Log("ratioOfKilledEnemies " + ratioOfKilledEnemies);
                activeVeg[AssignedIsland][i] = true;
                arrayOfVegChild[AssignedIsland][i].active = true; //not here
                vegIndex = i;
            }
            vegIndex = (int) upperLimitOfVeg;
        }
            
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
            Debug.Log("Wave " + currentWave + " done");
            if(currentWave >= 2){
                if(AssignedIsland < 2){
                    currentWave = 0;
                    clearedIsland[AssignedIsland] = true;
                    AssignedIsland ++;
                }else{
                    bossSpawned = true;
                    summonBoss();
                }
                
            }
            else{
                currentWave++;
            }
            slainEnemies = 0;
            oldSlainEnemies = 0;
            waveSpawned = false;
        }
    }
    public int getCurrentIsland(){
        return this.AssignedIsland;
    }
    
    private void summonBoss(){
        Vector3 pos = playableLevels[AssignedIsland].transform.position;

        GameObject enemy = Instantiate(boss,pos ,Quaternion.identity);
        enemy.tag = "Enemy";
        enemy.transform.localScale *= 40;
        //enemy.GetComponent<RatMove>().health = 200;
    }
}