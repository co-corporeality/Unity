using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class FieldController : MonoBehaviour
{
    FieldGizmo[] gizmos;    // Gizmos that define the forcefield
	GameObject[] objects;   // subobjects to be manipulated
	Material[] materials;	// The materials used, should all have tesselation shader set 
	

	void Start()
    {
        gizmos = GetFieldGizmos();
        objects = GetChildren(gameObject);
		materials = GetChildrenMaterials(objects);
	}

    void Update()
    {
		
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

	
    GameObject[] GetChildren(GameObject obj)    // get all children of the target Gameobject 
    {
		GameObject[] result = new GameObject[obj.transform.childCount];
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            result[i] = obj.transform.GetChild(i).gameObject;			
		}
        return result;
    }

	Material[] GetChildrenMaterials(GameObject[] objs) {
		Material[] result = new Material[objs.Length];
		for(int i = 0; i < objs.Length; i++) {
			result[i] = objs[i].GetComponent<MeshRenderer>().material;
		}
		return result;
	}

    public static float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

	public static float QuadeaseInOut(float t, float b, float c, float d) {
		if ((t /= d / 2) < 1) return c / 2 * t * t + b;
		return -c / 2 * ((--t) * (t - 2) - 1) + b;
	}
}
