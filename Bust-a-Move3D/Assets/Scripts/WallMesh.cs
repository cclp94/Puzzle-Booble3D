using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMesh : MonoBehaviour {

    public Vector3[] newVertices;
	public Vector2[] newUV;
    public Mesh cube;
    public ExtrudeShape es;
	public int[] newTriangles;
	void Start()
	{
        ExtrudeShape es = new ExtrudeShape();
        Vector2[] a = { new Vector2(0, 0), new Vector2(1, 0) };

        es.verts = a;
        BezierCurve s = new BezierCurve(newVertices);
        OrientedPoint[] op = { s.GetOrientedPoint(0), s.GetOrientedPoint(0.5f), s.GetOrientedPoint(1) };
        Vector3 v1 = s.GetPoint(0);
        Vector3 v2 = s.GetPoint(1);
        s.Extrude(cube, es,op);
        print(v1);
        print(v2);
	}
}
