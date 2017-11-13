using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMesh : MonoBehaviour {
    public float amplitude;
    public int frequency;
    public float waveLength;

    public float height;
    public float width;
    public float depth;

    private static float offset;
    private static bool hasToCreateNewMesh;
    private MeshFilter mf;
    private MeshCollider mc;

    void Start()
	{
        hasToCreateNewMesh = false;
        offset = 0;
        frequency *= 10;
        mf = GetComponent< MeshFilter > ();
        RefreshMesh();
    }
    void FixedUpdate()
    {
        if (hasToCreateNewMesh)
        {
            foreach(WallMesh w in FindObjectsOfType<WallMesh>())
            {
                w.RefreshMesh();
            }
            hasToCreateNewMesh = false;
        }
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, (GetComponent<Renderer>().material.mainTextureOffset.y + (1 *Time.deltaTime ))%100);
    }

    void RefreshMesh()
    {
        var mesh = CreateMesh();
        mf.mesh = mesh;
        mc = GetComponent<MeshCollider>();
        mc.sharedMesh = mesh;
    }

    public Mesh CreateMesh()
    {
        var mesh = new Mesh();

        Vector3[] vertices = new Vector3[frequency * 3 * 2];
        // back
        for (int i = 0; i < frequency; i++)
        {
            float zDepth = (2 * (i / 2.0f)) / frequency;
            Vector3 vTop = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * i), height, depth*zDepth);

            Vector3 vBottom = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * i), 0, depth * zDepth);
            int index = i * 2;
            vertices[index] = vTop;
            vertices[index + 1] = vBottom;
        }
        //front
        for (int i = frequency; i < frequency * 2; i++)
        {
            float zDepth = (2 * ((i - frequency) / 2.0f)) / frequency;
            Vector3 vTop = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * (i - frequency)) + 2, height, depth * zDepth);

            Vector3 vBottom = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * (i - frequency)) + 2, 0, depth * zDepth);
            int index = i * 2;
            vertices[index] = vTop;
            vertices[index + 1] = vBottom;
        }
        //top
        for (int i = frequency * 2; i < frequency * 3; i++)
        {
            float zDepth = (2 * ((i - (frequency * 2)) / 2.0f)) / frequency;
            Vector3 vTop = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * (i - frequency * 2)), height, depth * zDepth);

            Vector3 vBottom = new Vector3(GetPoint(Mathf.Deg2Rad * (360 / (frequency / waveLength)) * (i - frequency * 2)) + 2, height, depth * zDepth);
            int index = i * 2;
            vertices[index] = vTop;
            vertices[index + 1] = vBottom;
        }


        // Triangles
        int[] triangles = new int[(frequency - 1) * 2 * 6 * 3];


        // back
        for (int i = 0; i < ((frequency -1) * 2); i += 2)
        {
            int index = i * 3;

            triangles[index] = i;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = i + 3;

            triangles[index + 3] = i;
            triangles[index + 4] = i + 3;
            triangles[index + 5] = i + 2;

        }



        // front
        for (int i = frequency * 2; i < ((frequency - 1) * 4) + 2; i += 2)
        {
            int index = i * 3;

            triangles[index] = i;
            triangles[index + 1] = i + 3;
            triangles[index + 2] = i + 1;

            triangles[index + 3] = i;
            triangles[index + 4] = i + 2;
            triangles[index + 5] = i + 3;

        }

        //top
        for (int i = (frequency * 4); i < ((frequency -1) * 6)+4; i += 2)
        {
            int index = i * 3;

            triangles[index] = i;
            triangles[index + 1] = i + 3;
            triangles[index + 2] = i + 1;

            triangles[index + 3] = i;
            triangles[index + 4] = i + 2;
            triangles[index + 5] = i + 3;

        }


        Vector3[] normals = new Vector3[frequency * 3 * 2];

        for (int i = 0; i < frequency*2; i++)
        {
            Vector3 n = Quaternion.Euler(0, -90, 0) * (vertices[i + 2] - vertices[i]);
            int index = i;
            normals[index] = n.normalized;
            //normals[index + 1] = Vector3.right;
        }

        for (int i = frequency*2; i < (frequency*4); i++)
        {
            Vector3 n = Quaternion.Euler(0, 90, 0) * (vertices[i + 2] - vertices[i]);
            int index = i;
            normals[index] = n.normalized;
            //normals[index + 1] = Vector3.right;
        }

        for (int i = frequency * 4; i < frequency * 6; i++)
        {
            int index = i ;
            normals[index] = Vector3.up;
        }


        Vector2[] uvs = new Vector2[frequency * 3 * 2];
        float j = 0;
        for (int i = 0; i < frequency * 6; i += 2, j += 0.01f)
        {
            if (j == 1)
                j = 0;
            int index = i;
            uvs[index] = new Vector2(0+j, 1);
            uvs[index + 1] = new Vector2(0+j, 0);
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    public float GetPoint(float t)
    {
        return amplitude * (Mathf.Sin(t * Mathf.PI+ offset)  );
    }

    public void OffsetWall()
    {
        print("Offset Wall");
        offset += Mathf.PI;
        hasToCreateNewMesh = true;
    }
}
