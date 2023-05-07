using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using GPUInstancer;

public class GpuMeshInstancer : MonoBehaviour
{
    public List<GpuMeshPrefab> items;
    // reference to the Prefab Manager
    public GPUInstancerPrefabManager prefabManager;

    public void addInstance(ref int id, ref int index, GpuMeshPrefab pref, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        // find item
        if (items == null) items = new List<GpuMeshPrefab>();
        id = -1;
        for (int i = 0; i < items.Count; i++) {
            if (items[i] == pref) id = i;
        }
        if (id == -1)
        {
            id = items.Count;
            items.Add(pref);
            items[items.Count - 1].inizialize();
        }
        // create
        index = items[id].addInstance(this, pos, rot, scale, 
            items[id].colors[Random.Range(0, items[id].colors.Length)]);
    }
    public void removeInstance(int id, int indx)
    {
        items[id].removeInstance(this, indx);
    }
}*/
