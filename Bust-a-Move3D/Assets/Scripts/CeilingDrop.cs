using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDrop : MonoBehaviour {

    [SerializeField]
    public GameObject RowPrefab;

    public GameObject lastRow;

	// Use this for initialization
	void Start () {
        lastRow = null;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
