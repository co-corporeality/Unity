using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGizmo : MonoBehaviour
{
    GizmoOSC gizmoOSC;
    [Header("General Settings")]
    [Range(0.0f, 1.0f)]
    public float noise; 
    public float radius = 1.0f;
    public float strength = 1.0f;
    public bool overrideOSC;

    [HideInInspector]
    public float realStrength;  // user multiplier * osc data
    [HideInInspector]
    public  float poseStrength;
    [Header("Pose Settings")]
    [Range(0, 17)]
    public int id = 0;
    [Range(0, 17)]
    public int rootId = 0;
    

    [Header("Face Settings")]
    [Range(0,4)]
    public int emotionId;
    [HideInInspector]
    public float emotionStrength;
    void Start() {
        gizmoOSC = GameObject.Find("GizmoOSC").GetComponent<GizmoOSC>();
    }

    void Update() {
        if(overrideOSC) {
            realStrength = strength;
        } else {
            if(gizmoOSC.mode.Equals("POSE")) {
                realStrength = strength * poseStrength;
            }
            if(gizmoOSC.mode.Equals("FACE")) {
                //Debug.Log(emotionStrength);
                realStrength = strength * emotionStrength;
            }
        }

    }

       void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
