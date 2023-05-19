using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vegetation;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
namespace StarterAssets
{
    public class Interface : MonoBehaviour{
        public int health = 100;
        public bool[] abilities = new bool[10]; 
        private StarterAssetsInputs playerInput;
        private float abilityColdown1 = 0f;
        private float abilityColdown2 = 0f;
        private bool smellyAbilityActivated = false;
        public GameObject leafKunaii;
        public GameObject smellyCloud;
        private GameObject smelly;
        private Transform shootingPoint;
        private GameObject self;
        Vector3 pos = new Vector3(0f, 1f, 0f);
        private Slider playerSlider;
        public TextMeshProUGUI text;
        private WorldEvents world; 
        public GameObject defeat;
        public Button respawnButton;
        private float bossAtkTimeout = 0f;
        bool dead = false;
        
        public 
        //private int currentWave = 0;
        // Start is called before the first frame update
        void Start()
        {
            //text = GameObject.FindWithTag("");
            world = GameObject.Find("level").GetComponent<WorldEvents>();
            self = GameObject.FindGameObjectsWithTag("Player")[0];
            playerSlider = GameObject.FindGameObjectWithTag("PlayerSlider").GetComponent<Slider>();
            playerInput = GetComponent<StarterAssetsInputs>();
            shootingPoint = GetComponent<Rigidbody>().transform;
            defeat.SetActive(false);
            abilities[0] = true;
            respawnButton.onClick.AddListener(RespawnBtnClick);
        }

        // Update is called once per frame
        void FixedUpdate()
        
        {
            playerSlider.value = health;
            //Stuff that happens every GameTic
            if(smellyAbilityActivated == true){
                smelly.transform.position = self.transform.position + pos;
            }

            if(abilityColdown1 > 0f){
                abilityColdown1 -= Time.deltaTime;
            }
            if(abilityColdown2 > 0f){
                abilityColdown2 -= Time.deltaTime;
                if(abilityColdown2 <= 0){
                    Destroy(smelly, 1);
                    smellyAbilityActivated = false;
                }   
            }

            if(playerInput.ability1 && abilities[0] && abilityColdown1 <= 0){
                abilityColdown1 = 1f;
                leafKunai();
            } else{
               playerInput.ability1 = false; 
            }
            if(playerInput.ability2 && abilities[1] && abilityColdown2 <= 0){
                abilityColdown2 = 6.5f;
                AoeSmellyCloud();
               
            } else{
               playerInput.ability2 = false; 
            }
            if(playerInput.ability3 && abilities[0]){
                Debug.Log("knas plz3");
            } else{
               playerInput.ability3 = false; 
            }

            text.text = world.slainEnemies + "/" + world.getCurrentTotalEnemies();
            bossAtkTimeout -= Time.deltaTime;
        }
        //pick-up
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "HealthItem"){
                health += 10;
                Debug.Log("HEALTHPICKUP");
                Destroy(other.gameObject);
            }
            if(other.name == "SmellyCloudAbility"){
                abilities[1] = true;
                Debug.Log("SmellyCloudPickedUp");
                Destroy(other.gameObject);
            }
            Debug.Log(world.AssignedIsland + " : " +  world.oldAssigned);
            if(world.AssignedIsland != world.oldAssigned){
                Debug.Log(world.playableLevels[world.oldAssigned].name + " : " + other.gameObject.name);
                if(world.Islands[world.oldAssigned] == other.gameObject){
                    Debug.Log(playerInput.interaction);
                    if(playerInput.interaction){
                        world.takenPortal = true;
                    }
                }
            }
        }
        void OnTriggerStay(Collider other)
        {
            GameObject otherGameObject = other.gameObject;
            //BOSS ATTACKS
            if(bossAtkTimeout <= 0){
                if(otherGameObject.tag == "BossAtk"){
                    health -= 10;
                    Destroy(otherGameObject);
                    bossAtkTimeout = 0.1f;
                }
                if(otherGameObject.tag == "Boss"){
                    health -= 20;
                    bossAtkTimeout = 0.1f;
                }
            }
            
            if(health <= 0 && !dead){
                abilities[0] = false;
                defeat.SetActive(true);
                playerInput.cursorLocked = false;
                playerInput.cursorInputForLook = false;
                dead=true;
            }
        }

        void leafKunai(){
            GameObject cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
            GameObject leaf = UnityEngine.Object.Instantiate(leafKunaii,cam.transform.position + self.transform.forward * 0.5f, Quaternion.identity);
            leaf.transform.Rotate(Quaternion.Euler(-90.0f,0, 0)*transform.forward);
            Vector3 rototot = self.transform.forward;
            
            //leaf.transform.Rotate(90f + rototot.x, 0f+ rototot.y, 0f+ rototot.z, Space.Self);
            //leaf.GetComponent<Rigidbody>().AddForce(transform.forward*10);
           // Debug.Log(cam.transform.rotation.eulerAngles);
            leaf.GetComponent<Rigidbody>().AddForce(cam.transform.forward *10);
            
            
            Destroy(leaf, 2);
        }

        void AoeSmellyCloud(){
            smelly  = UnityEngine.Object.Instantiate(smellyCloud,self.transform.position + pos, Quaternion.identity);
            smellyAbilityActivated = true;
            Debug.Log("smellyCloudActivated");
        }
        void RespawnBtnClick(){
            Debug.Log("Button Clicked!");
            world.restartGame();
            health = 100;
            abilities[0] = true;
            defeat.SetActive(false);
            playerInput.cursorLocked = true;
            playerInput.cursorInputForLook = true;
            dead=false;
        }
    }
}
