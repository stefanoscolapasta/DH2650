using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float plantSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                Vegetation.Controller.Instance.CreatePlant(
                                0, plantSize, hit.transform.tag == "Climbable", hit.point,
                                Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles,
                                true);
            }
        }
    }
}
