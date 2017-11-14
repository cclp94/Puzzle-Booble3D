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
    private Rigidbody mRigidBody;

    private Vector3 movementVector;
    private float velocity;
    private SphereCollider mCollider;

    private Queue<Vector3> hitNormals;

    // Use this for initialization
    void Start () {
        hitNormals = new Queue<Vector3>();
        hasLaunched = false;
        isPopping = false;
        targetHit = false;
        bubbleGrid = GameObject.Find("BubbleGrid").GetComponent<Grid>();
        mRigidBody = GetComponent<Rigidbody>();
        mCollider = GetComponent<SphereCollider>();
        this.GetComponent<Rigidbody>().isKinematic = true;
        passedDeathLineAfterLaunch = false;
    }
    bool passedDeathLineAfterLaunch;
	// Update is called once per frame
	void FixedUpdate () {
        if (hasLaunched)
        {
            Vector2 v = Rotate(Vector2.up, -movementAngle);
            transform.Rotate(new Vector3(v.y, 0.0f, -v.x).normalized, 10.0f);
            mRigidBody.MovePosition(mRigidBody.position + movementVector.normalized * velocity * Time.deltaTime);
            velocity -= 20f * Time.deltaTime;
            if (transform.position.z < -9.3f || transform.position.y > 5f)
			{
                if(passedDeathLineAfterLaunch){
                    
					DestroyBubbleObject();
					print("Launched bubble destroyed updating queue");
					bubbleGrid.LaunchedBubbleDestroyed();
                }

            }else{
                passedDeathLineAfterLaunch = true;
            }
        }
        if (isDropping)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
			if (transform.position.z < -9.3f)
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
		
		Vector2 v = Rotate(Vector2.up, -movementAngle);
        movementVector = new Vector3(v.x, 0.0f, v.y);
        RaycastHit hit;
        Collider finalCol = null;
        Vector3 initPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 dir = new Vector3(v.x, 0.01f, v.y); ;
        float distance = 0;
        int counter = 0;
        do
        {
            if(Physics.SphereCast(initPos, mCollider.radius, dir, out hit, 15000)){
                print("cast hit "+hit.collider.gameObject.tag+": " + hit.point);
                distance += hit.distance;
                initPos = hit.point;
                print("Initial direction ray : " + dir);
                var reflect = Vector3.Reflect(dir, hit.normal).normalized;
                dir = new Vector3(reflect.x, dir.y, reflect.z);
                print("Reflected direction ray: " + dir);
                print(hit.normal);
                if(hit.collider.gameObject.tag == "Wall")
                    hitNormals.Enqueue(hit.normal);
                finalCol = hit.collider;
            }
            counter++;
        } while (counter < 10000 && finalCol != null && finalCol.gameObject.tag != "Bubble" && finalCol.gameObject.tag != "WallTop");
        print("Distance:" +distance++);
        float initVel = 0 + Mathf.Sqrt(2*20f * distance);
        print("Initial velocity: " + initVel);
        this.GetComponent<Rigidbody>().isKinematic = false;
        velocity = initVel;

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
            print("Bubble hit: "+coll.contacts[0].point);
            bubbleGrid.SetInGrid(this.gameObject);
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (coll.gameObject.tag.Equals("Wall"))
        {
            Debug.Log("collide to wall");
			Vector3 oldVelocity = this.GetComponent<Rigidbody>().velocity;
			ContactPoint hit = coll.contacts[0];
            print("Amount of contacts: " + coll.contacts.Length);
            Vector3 moveReflect = Vector3.Reflect(movementVector, hit.normal);
			if(hitNormals.Count > 0)
                moveReflect = Vector3.Reflect(movementVector, hitNormals.Dequeue());
            else
            print(hit.normal);
            print("Movement before; " + movementVector);
            movementVector = moveReflect;
            print("Movement After: " + moveReflect);
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
