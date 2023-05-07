using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        
        //private int currentWave = 0;
        // Start is called before the first frame update
        void Start()
        {
            //text = GameObject.FindWithTag("");
            world = GameObject.Find("Ground").GetComponent<WorldEvents>();
            self = GameObject.FindGameObjectsWithTag("Player")[0];
            playerSlider = GameObject.FindGameObjectWithTag("PlayerSlider").GetComponent<Slider>();
            playerInput = GetComponent<StarterAssetsInputs>();
            shootingPoint = GetComponent<Rigidbody>().transform;
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

            text.text = world.slainEnemies + "/" + world.enemyLimits[world.currentWave];
        }
        //pick-up
        void OnTriggerEnter(Collider other)
        {
            if(other.name == "LeafKunaiAbility"){
                abilities[0] = true;
                Debug.Log("LeafKunaiPickedUp");
                Destroy(other.gameObject);
            }
            if(other.name == "SmellyCloudAbility"){
                abilities[1] = true;
                Debug.Log("SmellyCloudPickedUp");
                Destroy(other.gameObject);
            }
        }
        void OnTriggerStay(Collider other)
        {
            GameObject otherGameObject = other.gameObject;
            if(otherGameObject.tag == "Enemy"){
                if(otherGameObject.GetComponent<Enemy>().getAtkCD() <= 0f){
                    Debug.Log("Tag works");
                //if(otherGameObject.name == "Rat"){
                health -= 1;
                Debug.Log(health); 
              //  otherGameObject.GetComponent<Enemy>().setAtkCD(3f);
                }
                
                //}
            }
            if(health <= 0){
                Destroy(self);
            }
        }

        void leafKunai(){
            
            GameObject leaf = UnityEngine.Object.Instantiate(leafKunaii,shootingPoint.position + self.transform.forward*0.5f + pos, Quaternion.identity);
            Vector3 rototot = self.transform.forward;
            leaf.transform.Rotate(90f + rototot.x, 0f+ rototot.y, 0f+ rototot.z, Space.Self);
            leaf.GetComponent<Rigidbody>().AddForce(transform.forward*10);
            Destroy(leaf, 2);
            //Debug.Log(leaf.GetComponent<Rigidbody>());
            Debug.Log("shooting");
        }

        void AoeSmellyCloud(){
            smelly  = UnityEngine.Object.Instantiate(smellyCloud,self.transform.position + pos, Quaternion.identity);
            smellyAbilityActivated = true;
            Debug.Log("smellyCloudActivated");
        }
    }
}