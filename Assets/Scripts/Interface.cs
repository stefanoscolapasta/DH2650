using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
namespace StarterAssets
{
    public class Interface : MonoBehaviour{
        public int health = 100;
        public bool[] abilities = new bool[10]; 
        private StarterAssetsInputs playerInput;
        private float abilityColdown1 = 0.5f;
        public GameObject leafKunaii;
        public Transform shootingPoint;
        private GameObject self;
        // Start is called before the first frame update
        void Start()
        {
            self = GameObject.FindGameObjectsWithTag("Player")[0];
            playerInput = GetComponent<StarterAssetsInputs>();
            shootingPoint = GetComponent<Rigidbody>().transform;
        }

        // Update is called once per frame
        void Update()
        
        {
            if(abilityColdown1 >= 0f){
                abilityColdown1 -= Time.deltaTime;
            }

            if(playerInput.ability1 && abilities[0] && abilityColdown1 <= 0){
                abilityColdown1 = 0.5f;
                leafKunai();
            }
            if(playerInput.ability2 && abilities[0]){
                Debug.Log("knas plz2");
            }
            if(playerInput.ability3 && abilities[0]){
                Debug.Log("knas plz3");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            
            if(other.name == "LeafKunaiAbility"){
                abilities[0] = true;
                Debug.Log("LeafKunaiPickedUp");
                Destroy(other.gameObject);
            }
        }

        void leafKunai(){
            Vector3 pos = new Vector3(0f, 0.8f, 0f);
            
            GameObject leaf = UnityEngine.Object.Instantiate(leafKunaii,shootingPoint.position + self.transform.forward*0.5f + pos, Quaternion.identity);
            Vector3 rototot = self.transform.forward;
            leaf.transform.Rotate(90f + rototot.x, 0f+ rototot.y, 0f+ rototot.z, Space.Self);
            leaf.GetComponent<Rigidbody>().AddForce(transform.forward*10);
            Debug.Log(leaf.GetComponent<Rigidbody>());
            Destroy(leaf, 3);
            Debug.Log("shooting");
        }

    }
}

