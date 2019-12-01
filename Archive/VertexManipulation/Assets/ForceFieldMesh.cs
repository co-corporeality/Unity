using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ForceFieldMesh : MonoBehaviour
{
    GameObject[] objects;   // subobjects to be manipulated
    Mesh[] original;        // meshes before maniulation
    SimplexNoise noise;

    public GameObject centerDummy;
    public float radius;
    public float noiseScale = 0.01f;
    public float noiseSpeed = 0.1f;

    void Start() {
        objects = GetChildren(gameObject);
        original = SaveOriginal(objects);
        noise = new SimplexNoise("12345");
    }

    
    void Update() {
        DeformMeshes(objects);
    }

    void DeformMeshes(GameObject[] objs) {
        for(int i = 0; i < objs.Length; i++) {
            Mesh mesh = objs[i].GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            for(int j = 0; j < vertices.Length; j++) {
                float n = noise.coherentNoise(vertices[j].x * noiseScale,vertices[j].y * noiseScale, vertices[j].z * noiseScale);
                vertices[j] += noiseSpeed * normals[j] * n;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            //mesh.UploadMeshData(false);
        }
    }

    Mesh[] SaveOriginal(GameObject[] objs) {
        Mesh[] result = new Mesh[objs.Length];
         for(int i = 0; i < objs.Length; i++) {
            result[i] = Instantiate(objs[i].GetComponent<MeshFilter>().sharedMesh);
         }
         return result;
    }

    GameObject[] GetChildren(GameObject obj) {
        GameObject[] result = new GameObject[obj.transform.childCount];
        for(int i = 0; i < obj.transform.childCount; i++) {
            result[i] = obj.transform.GetChild(i).gameObject;
        }
        return result;
    }

    public void Reset() {
        Debug.Log("ok");
        for(int i = 0; i < objects.Length; i++) {
            objects[i].GetComponent<MeshFilter>().sharedMesh.vertices = original[i].vertices;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerDummy.transform.position, radius);
    }
}

[CustomEditor(typeof(ForceFieldMesh))]
public class LevelScriptEditor : Editor  {
    public override void OnInspectorGUI(){
        ForceFieldMesh data = (ForceFieldMesh)target;
        if(GUILayout.Button("Reset")) {
            data.Reset();
        }
        DrawDefaultInspector();
    }
}
