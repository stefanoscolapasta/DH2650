using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    [SerializeField] private List<Mesh> flowers;
    [SerializeField] private float radius;
    [SerializeField] private float growTime = 1f;
    private float growCount;
    private Material mat;

    private int state = 0;

    private void Update()
    {
        if (state == 0)
        {
            transform.localScale *= Random.Range(0.9f, 1.3f);
            mat = GetComponent<Renderer>().material;
            GetComponent<MeshFilter>().mesh 
                = flowers[Random.Range(0, flowers.Count)];
            mat.SetFloat("_Distance", radius);
            mat.SetFloat("_Grow", 0f);
            state++;
        } else
        {
            if (growCount < growTime)
            {
                growCount += Time.deltaTime;
                mat.SetFloat("_Grow", growCount * growTime);
            } else
            {
                //todo add global mesh combine

                this.enabled = false;
            }
        }
    }
}
