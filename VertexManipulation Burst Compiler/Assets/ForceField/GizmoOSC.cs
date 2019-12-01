using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoOSC : MonoBehaviour
{
    public OSC osc;
    [HideInInspector]
    public float[] rawData;
    [HideInInspector]
    public Vector3[] rawPositions;
    public Mesh dummy; 
    FieldGizmo[] gizmos; 
    

    void Start()
    {
        gizmos = GetFieldGizmos();
        osc.SetAddressHandler("/pose", newPose);
        rawPositions = new Vector3[18];
    }

    void Update()
    {        
        // dummy Preview
        for(int i = 0; i < rawPositions.Length; i++) {
            Graphics.DrawMesh(dummy, rawPositions[i] * 20.0f, Quaternion.identity, null, 0);            
        }
        for(int i = 0; i < gizmos.Length; i++) {
            int id = gizmos[i].id;
            if(id > -1 && id < 18 && rawPositions[id].x > 0 && rawPositions[id].y > 0) {
                float dist = Vector3.Distance(rawPositions[0], rawPositions[id]);
                gizmos[i].oscStrength = dist;
                
            }
        }
    }

    void newPose(OscMessage message)
    {        
        rawData = (float[])message.values.ToArray(typeof(float));
        int c = 0;
        for(int i = 1; i < rawData.Length; i += 2) {
            rawPositions[c] = new Vector3(rawData[i], 1.0f - rawData[i + 1], 0);
            c++;
        }
    }

    FieldGizmo[] GetFieldGizmos() // finds all Gizmos in the scene
    { 
        List<FieldGizmo> result = new List<FieldGizmo>();
        GameObject[] obj = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        for (var i = 0; i < obj.Length; i++)
        {
            FieldGizmo temp = obj[i].GetComponentInChildren<FieldGizmo>();
            if (temp != null)
            {
                result.Add(temp);
            }
        }
        return result.ToArray();
    }
}
