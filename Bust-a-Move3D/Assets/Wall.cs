using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(baseHeight[i].z) * scale;
            //vertex.y += Mathf.PerlinNoise(baseHeight[i].x, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f));
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
	}

	float scale = 0.3f;
	float speed = 1.0f;
	float noiseStrength = 1f;
	float noiseWalk = 0.1f;

	private Vector3[] baseHeight;

	void Start()
	{
		
	}
}
