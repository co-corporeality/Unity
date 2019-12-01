using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGizmo : MonoBehaviour
{
    public float radius = 1.0f;
    public float strength = 1.0f;
    public int id = -1;
    
    void Start() {
        
    }

    void Update() {
        
    }

       void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
