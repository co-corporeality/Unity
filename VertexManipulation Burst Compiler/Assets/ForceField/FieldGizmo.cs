using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGizmo : MonoBehaviour
{
    public float radius = 1.0f;
    public float strength = 1.0f;
    [HideInInspector]
    public float realStrength;  // user multiplier * osc data
    [HideInInspector]
    public  float oscStrength;
    public int id = -1;
    
    void Start() {
        
    }

    void Update() {
        realStrength = strength * oscStrength;
    }

       void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
