using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class FieldController : MonoBehaviour
{
    FieldGizmo[] gizmos;    // Gizmos that define the forcefield

	newData[] objects;   // subobjects to be manipulated
	MeshData[] original;    // meshes before maniulation
	MeshJob[] meshJob;
	JobHandle[] meshJobHandle;

	public struct newData {
		public GameObject obj;
		public Vector3[] vertices;
		public NativeArray<Vector3> nativeVert;
	}

	public struct MeshData {
		public Transform obj;
		public Mesh original;
		public NativeArray<Vector3> vertices;
		public NativeArray<Vector3> normals;
		public Vector3[] modifiedVertices;
		
	}

	public struct MeshJob : IJobParallelFor {
		[ReadOnly]
		public NativeArray<Vector3> origVert;
		public NativeArray<Vector3> vert;
		[ReadOnly]
		public NativeArray<Vector3> norm;
		[ReadOnly]
		public NativeArray<Vector3> gizmoCenter;
		[ReadOnly]
		public NativeArray<float> gizmoRadius;
		[ReadOnly]
		public NativeArray<float> gizmoStrength;

		public void Execute(int i) {
			float forceSum = 0.0f;    // acculmulate all forces
			for (int j = 0; j < gizmoCenter.Length; j++) {
				float d = Vector3.Distance(gizmoCenter[j], origVert[i]);
				if (d < gizmoRadius[j]) {
					float m = map(d, gizmoRadius[j], 1f, 0f, 1f);
					forceSum += m * gizmoStrength[j];
				}
			}			
			vert[i] = origVert[i] + norm[i] * forceSum;
		}
	}

	void Start()
    {
        gizmos = GetFieldGizmos();
        objects = GetChildren(gameObject);
        original = GetOriginalMeshes(objects);
		meshJob = new MeshJob[objects.Length];
		meshJobHandle = new JobHandle[objects.Length];
	}

    void Update()
    {
		//DeformMeshes();
		Vector3[] gizmoCenterTemp = new Vector3[gizmos.Length];
		float[] gizmoRadiusTemp = new float[gizmos.Length];
		float[] gizmoStengthTemp = new float[gizmos.Length];
		for(int i = 0; i < gizmos.Length; i++) {
			gizmoCenterTemp[i] = gizmos[i].transform.position;
			gizmoRadiusTemp[i] = gizmos[i].radius;
			gizmoStengthTemp[i] = gizmos[i].strength;
		}
		for (int i = 0; i < objects.Length; i++) {
			meshJob[i] = new MeshJob() {
				origVert = objects[i].nativeVert,
				vert = original[i].vertices,
				norm = original[i].normals,
				gizmoCenter = new NativeArray<Vector3>(gizmoCenterTemp, Allocator.TempJob),
				gizmoRadius = new NativeArray<float>(gizmoRadiusTemp,Allocator.TempJob),
				gizmoStrength = new NativeArray<float>(gizmoStengthTemp, Allocator.TempJob)

			};
			meshJobHandle[i] = meshJob[i].Schedule(original[i].vertices.Length, 64);
		}	
    }

	public void LateUpdate() {
		for (int i = 0; i < objects.Length; i++) {
			meshJobHandle[i].Complete();			
			meshJob[i].vert.CopyTo(objects[i].vertices);
			meshJob[i].gizmoCenter.Dispose();
			meshJob[i].gizmoRadius.Dispose();
			meshJob[i].gizmoStrength.Dispose();

			Mesh mesh = objects[i].obj.GetComponent<MeshFilter>().sharedMesh;
			mesh.vertices = objects[i].vertices;
			mesh.RecalculateNormals();

		}

	}

	private void OnDestroy() {
		for (int i = 0; i < objects.Length; i++) {
			original[i].vertices.Dispose();
			original[i].normals.Dispose();
			meshJob[i].origVert.Dispose();
			
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

	MeshData[] GetOriginalMeshes(newData[] objs) // Save original MeshData before manipualtion
    {
		MeshData[] result = new MeshData[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
			result[i].obj = objs[i].obj.transform;
			result[i].original = Instantiate(objs[i].obj.GetComponent<MeshFilter>().mesh);
			result[i].vertices = new NativeArray<Vector3>(result[i].original.vertices, Allocator.Persistent);
			result[i].normals = new NativeArray<Vector3>(result[i].original.normals, Allocator.Persistent);
		}
        return result;
    }

    newData[] GetChildren(GameObject obj)    // get all children of the target Gameobject 
    {
		newData[] result = new newData[obj.transform.childCount];
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            result[i].obj = obj.transform.GetChild(i).gameObject;
			result[i].vertices = new Vector3[result[i].obj.GetComponent<MeshFilter>().sharedMesh.vertices.Length];
			result[i].nativeVert = new NativeArray<Vector3>(result[i].obj.GetComponent<MeshFilter>().sharedMesh.vertices, Allocator.Persistent);
		}
        return result;
    }

    public static float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}
