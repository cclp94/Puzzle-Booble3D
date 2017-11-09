using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    GameObject popPrefab;

    public int bubbleColor; // 0-5 color
    public float speed;
    public Grid bubbleGrid;
    private bool hasLaunched;
    private bool targetHit;
    private float movementAngle;
    private bool isDropping;
    private bool isPopping;
    private Animator animator;

    // Use this for initialization
    void Start () {
        hasLaunched = false;
        isPopping = false;
        targetHit = false;
        bubbleGrid = GameObject.Find("BubbleGrid").GetComponent<Grid>();
        //animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //animator.SetInteger("bubbleColor", bubbleColor);
        //animator.SetBool("isPopping", isPopping);
        if (hasLaunched)
        {
            Vector2 v = Rotate(Vector2.up, -movementAngle);
            //GetComponent<Rigidbody>().AddForceAtPosition((new Vector3(v.x, 0.0f, v.y) * speed * Time.deltaTime), transform.position, ForceMode.)
            GetComponent<Rigidbody>().MovePosition(transform.position + (new Vector3(v.x, 0.0f, v.y) * speed * Time.deltaTime));
            transform.Rotate(new Vector3(v.y, 0.0f, -v.x).normalized, 10.0f);
        }
        if (isDropping)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            //GetComponent<Rigidbody>().MovePosition(transform.position + (new Vector3(0.0f, 0.0f, -1.0f) * speed*1.5f * Time.deltaTime));
            //transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f).normalized, -5.0f);
            if (transform.position.z < -13)
            {
                DestroyBubbleObject();
            }
        }
	}

    void DestroyBubble()
    {
        isPopping = true;
        DestroyBubbleObject();
    }

    void DestroyBubbleObject()
    {
        isPopping = false;
        isDropping = false;
        GameObject pop = Instantiate(popPrefab, transform.position, Quaternion.identity);
        Destroy(pop, 1);
        Destroy(this.gameObject);
    }

    void DropBubble()
    {
        isDropping = true;
    }

    public void setColor(int color)
    {
        gameObject.GetComponent<Renderer>().material.color = GetDefinedColors(color);
       bubbleColor = color;
    }

    public void launch(float eulerAngle)
    {
        movementAngle = eulerAngle;
        hasLaunched = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public bool hitTarget()
    {
        return targetHit;
    }
    Collision lastCollision = null;
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Bubble") && !this.GetComponent<Rigidbody>().isKinematic || coll.gameObject.tag.Equals("WallTop") & !hitTarget())
        {
            hasLaunched = false;
            targetHit = true;
            this.GetComponent<Rigidbody>().isKinematic = true;
            bubbleGrid.SetInGrid(this.gameObject);
        }
        else if (coll.gameObject.tag.Equals("Wall") && coll != lastCollision)
        {
            Debug.Log("collide to wall");
            lastCollision = coll;
            movementAngle = -movementAngle;
        }
    }

    

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public int GetColor()
    {
        return bubbleColor;
    }

    public Color GetDefinedColors(int colorCode)
    {
        switch (colorCode)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.blue;
            case 2:
                return Color.green;
            case 3:
                return Color.yellow;
            case 4:
                return Color.magenta;
            default:
                return Color.cyan;
        }
    }
}
