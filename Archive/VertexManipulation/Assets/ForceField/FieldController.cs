using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldController : MonoBehaviour
{
    FieldGizmo[] gizmos;    // Gizmos that define the forcefield

    GameObject[] objects;   // subobjects to be manipulated
    Mesh[] original;        // meshes before maniulation

    void Start()
    {
        gizmos = GetFieldGizmos();
        objects = GetChildren(gameObject);
        original = GetOriginalMeshes(objects);
    }

    void Update()
    {
        DeformMeshes();
    }

    void DeformMeshes() // runs all forceGizmos on all Meshes 
    {   
        for (int n = 0; n < objects.Length; n++)
        {
            Mesh mesh = objects[n].GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] originalVertices = original[n].vertices;
            Vector3[] originalNormals = original[n].normals;
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
            {
                float forceSum = 0f;    // acculmulate all forces
                for (int j = 0; j < gizmos.Length; j++)
                {
                    float d = Vector3.Distance(gizmos[j].transform.position, originalVertices[i] + objects[n].transform.position);
                    if (d < gizmos[j].radius) {
                        float m = map(d, gizmos[j].radius, 1f, 0f, 1f);
                        forceSum += m * gizmos[j].strength;  
                    }         
                }
                vertices[i] = originalVertices[i] + originalNormals[i] * forceSum;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
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

    Mesh[] GetOriginalMeshes(GameObject[] objs) // Save original MeshData before manipualtion
    {   
        Mesh[] result = new Mesh[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            result[i] = Instantiate(objs[i].GetComponent<MeshFilter>().mesh);
        }
        return result;
    }

    GameObject[] GetChildren(GameObject obj)    // get all children of the target Gameobject 
    {  
        GameObject[] result = new GameObject[obj.transform.childCount];
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            result[i] = obj.transform.GetChild(i).gameObject;
        }
        return result;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}
