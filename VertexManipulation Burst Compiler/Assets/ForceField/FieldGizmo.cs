using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGizmo : MonoBehaviour
{
    [Header("Basic Settings")]
    [Range(0.0f, 1.0f)]
    public float noise; 
    public float radius = 1.0f;
    public float strength = 1.0f;
    [HideInInspector]
    public float realStrength;  // user multiplier * osc data
    [HideInInspector]
    public  float oscStrength;
    [Range(0, 17)]
    public int id = 0;

    [Header("Advanced Settings")]
    [Range(0, 17)]
    public int rootId = 0;
    public bool overrideOSC;
    
    void Start() {
        
    }

    void Update() {
        if(overrideOSC) {
            realStrength = strength;
        } else {
             realStrength = strength * oscStrength;
        }
    }

       void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
