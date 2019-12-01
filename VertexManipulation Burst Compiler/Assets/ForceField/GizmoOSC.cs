using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GizmoOSC : MonoBehaviour
{
    [HideInInspector]
    public string mode;
    public OSC osc;
    [HideInInspector]
    public float[] rawData;
    [HideInInspector]
    public Vector3[] rawPositions;    
    [HideInInspector]
    public float[] rawEmotions;
    FieldGizmo[] gizmos; 
    //public Mesh dummy; 
    

    void Start()
    {
        gizmos = GetFieldGizmos();
        if(mode.Equals("POSE")) {
            osc.SetAddressHandler("/pose", newPose);
        } else {
            osc.SetAddressHandler("/face", newFace);
        }
        
        rawPositions = new Vector3[18];
        rawEmotions = new float[5];
    }

    void Update()
    {        
        // dummy Preview
        //for(int i = 0; i < rawPositions.Length; i++) {
        //    Graphics.DrawMesh(dummy, rawPositions[i] * 20.0f, Quaternion.identity, null, 0);            
        //} 
        if(mode.Equals("POSE")) {
            setPoseGizmos(); 
        }
        if(mode.Equals("FACE")) {
            setFaceGizmos();
        }
        
    }

    void setFaceGizmos() {
        for(int i = 0; i < gizmos.Length; i++) {
            int id = gizmos[i].emotionId;
            if(id > -1 && id < 5 && rawEmotions[id] > 0 ) {
                gizmos[i].emotionStrength = rawEmotions[id];
            }
        }
    }

    void setPoseGizmos() {
        for(int i = 0; i < gizmos.Length; i++) {
            int id = gizmos[i].id;
            int root = gizmos[i].rootId;
            if(id > -1 && id < 18 && rawPositions[id].x > 0 && rawPositions[id].y > 0) {
                float dist = Vector3.Distance(rawPositions[root], rawPositions[id]);
                gizmos[i].poseStrength = dist;                
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

    void newFace(OscMessage message)
    {        
        System.Object[] rawData = (System.Object[])message.values.ToArray(typeof(System.Object));
        int c = 0;
        //string em = "";
        for(int i = 4; i < 13; i +=2) {
            rawEmotions[c] = (float)rawData[i];
            c++;
        }
        //Debug.Log(c);
        //for(int i = 0; i < rawData.Length; i ++) {
        //    em += rawData[i].ToString() + " ";
        //}
        //Debug.Log(em);
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

[CustomEditor(typeof(GizmoOSC))]
 public class SomeEditor : Editor
 {
   string[] _choices = new [] { "POSE", "FACE" };
   int _choiceIndex = 0;
 
   public override void OnInspectorGUI ()
   {
       var GizmoOSC = target as GizmoOSC;
        _choiceIndex = System.Array.IndexOf(_choices, GizmoOSC.mode);
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
    
        GizmoOSC.mode = _choices[_choiceIndex];
  
     DrawDefaultInspector();
     EditorUtility.SetDirty(target);
   }
 }
