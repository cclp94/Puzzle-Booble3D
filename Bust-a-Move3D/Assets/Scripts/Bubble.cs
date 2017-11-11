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
    private Rigidbody mRigidBody;

    // Use this for initialization
    void Start () {
        hasLaunched = false;
        isPopping = false;
        targetHit = false;
        bubbleGrid = GameObject.Find("BubbleGrid").GetComponent<Grid>();
        mRigidBody = GetComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().isKinematic = true;
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
			//GetComponent<Rigidbody>().MovePosition(transform.position + (new Vector3(v.x, 0.0f, v.y) * speed * Time.deltaTime));
			//transform.Rotate(new Vector3(v.y, 0.0f, -v.x).normalized, 10.0f);
            //mRigidBody.AddRelativeForce(Vector3.up);
			if (transform.position.z < -16)
			{
				DestroyBubbleObject();
			}
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
		// Find initial velocity
		movementAngle = eulerAngle;
		hasLaunched = true;
		this.GetComponent<Rigidbody>().isKinematic = false;
		this.GetComponent<Rigidbody>().useGravity = true;
		Vector2 v = Rotate(Vector2.up, -movementAngle);
        RaycastHit hit;
        Collider finalCol = null;
        Vector3 initPos = transform.position;
        Vector3 dir = new Vector3(v.x, 0.0f, v.y);
        float distance = 0;
        do
        {
            if(Physics.Raycast(initPos, dir, out hit)){
                distance += hit.distance;
                initPos = hit.point;
                dir = Vector3.Reflect(dir, hit.normal);
                finalCol = hit.collider;
            }
        } while (finalCol.gameObject.tag != "Bubble" && finalCol.gameObject.tag != "WallTop");
        //finalCol.gameObject.transform.position = new Vector3(finalCol.gameObject.transform.position.x, finalCol.gameObject.transform.position.y + 1, finalCol.gameObject.transform.position.z);
        print("Distance:" +distance);
        float initVel = 1 - (2*-0.7f * distance);
        print("Initial velocity: " + initVel);
        GetComponent<Rigidbody>().AddForceAtPosition((new Vector3(v.x, 0.0f, v.y) * initVel), transform.position, ForceMode.Impulse);

	}

    public bool hitTarget()
    {
        return targetHit;
    }
    Collision lastCollision = null;
    void OnCollisionEnter(Collision coll)
    {
        if (hasLaunched && coll.gameObject.tag.Equals("Bubble") && !this.GetComponent<Rigidbody>().isKinematic || coll.gameObject.tag.Equals("WallTop") & !hitTarget())
        {
            hasLaunched = false;
            targetHit = true;

            bubbleGrid.SetInGrid(this.gameObject);
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (coll.gameObject.tag.Equals("Wall") && coll != lastCollision)
        {
           // Debug.Log("collide to wall");
           // lastCollision = coll;
            movementAngle = -movementAngle;
			Vector3 oldVelocity = this.GetComponent<Rigidbody>().velocity;
			ContactPoint hit = coll.contacts[0];
            print(this.GetComponent<Rigidbody>().velocity);
            print(hit.normal);
            Vector3 reflection = Vector3.Reflect(oldVelocity, -hit.normal);
            //this.GetComponent<Rigidbody>().velocity = reflection;
            print(this.GetComponent<Rigidbody>().velocity);
			Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflection);
			//transform.rotation = rotation * transform.rotation;
			//rotationX = transform.rotation.x;
			//rotationY = transform.rotation.y;
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
