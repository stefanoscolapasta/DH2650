using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vegetation
{
    public class Controller : MonoBehaviour
    {
        public static Controller Instance;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null)
                Instance = this;
        else if (Instance != this)
                Destroy(gameObject);
        }
        public List<Plant> plants;

        private class PlantInstance
        {
            private Plant plant;
            private GameObject go;
            private bool isStatic;

            public PlantInstance(Plant plant, GameObject go, bool isStatic)
            {
                this.plant = plant;
                this.go = go;
                this.isStatic = isStatic;
            }
        }
        private List<PlantInstance> instances;

        [SerializeField] private GameObject bridgePref;

        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject creator;

        public GameObject CreatePlant(int id, float size, bool climbable, 
            Vector3 position, Vector3 rotation, bool isStatic)
        {
            GameObject go = Instantiate(prefab, this.transform);
            go.transform.position = position;
            go.transform.eulerAngles = rotation;

            go.GetComponent<Generator>().Generate(plants[id], size, climbable);

            if (instances == null) instances = new List<PlantInstance>();
            instances.Add(new PlantInstance(plants[id], go, isStatic));

            return go;
        }

        public GameObject CreatePlant(int id, float size, bool climbable,
            Vector3 position, Vector3 rotation, bool isStatic, float destroyAfter)
        {
            GameObject go = CreatePlant(id, size, climbable, position, rotation, isStatic);
            Destroy(go, destroyAfter);
            return go;
        }

        public void CreateBridge(Vector3 start, Vector3 finish)
        {
            GameObject go = Instantiate(bridgePref, this.transform);
            go.transform.position = finish;
            go.GetComponent<Bridge>().Generate(this, start, finish);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(Controller))]
        public class PrefabControllerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                Controller b = (Controller)target;
                if (GUILayout.Button("Create Plant"))
                {
                    b.CreatePlant(0, .1f, false,
                    b.creator.transform.position,
                    b.creator.transform.eulerAngles, false);
                }
            }
        }
#endif
    }
}