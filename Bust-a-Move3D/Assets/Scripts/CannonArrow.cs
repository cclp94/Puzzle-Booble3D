using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonArrow : MonoBehaviour {
    private float rotation;
    private bool hasLaunchedBall;

    [SerializeField]
    public GameObject bubblePrefab;
    [SerializeField]
    public AudioSource launchSound;
    [SerializeField]
    public AudioSource turningGears;
    //public Transform cannonBase;
    public Vector3 spawnPosition;

    private GameObject nextBubble;
    private GameObject queueBubble;

    private double lastShotTimeStamp;

    // Use this for initialization
    void Start () {
        rotation = 0.0f;
        hasLaunchedBall = false;
        InstantiateBubbles();
        lastShotTimeStamp = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        rotation = 0.0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.rotation.eulerAngles.y < 60 || transform.rotation.eulerAngles.y > 270)
            {
                rotation += 150 * Time.deltaTime;
                if (!turningGears.isPlaying)
                    turningGears.Play();
            }else
            {
                if (turningGears.isPlaying)
                    turningGears.Stop();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.rotation.eulerAngles.y < 90 || transform.rotation.eulerAngles.y > 300)
            {
                rotation -= 150 * Time.deltaTime;
                if(!turningGears.isPlaying)
                    turningGears.Play();
            }else
            {
                if (turningGears.isPlaying)
                    turningGears.Stop();
            }
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) && !hasLaunchedBall || (Time.time - lastShotTimeStamp > 5))
        {
            hasLaunchedBall = true;
            lastShotTimeStamp = Time.time;
            nextBubble.GetComponent<Bubble>().launch(transform.rotation.eulerAngles.y);
            launchSound.Play();
        }else {
            if (turningGears.isPlaying)
                turningGears.Stop();
        }

        transform.Rotate(Vector3.up, rotation);
    }

    private void InstantiateBubbles()
    {
        nextBubble = Instantiate(bubblePrefab);
        nextBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
        nextBubble.transform.position = transform.position;

        queueBubble = Instantiate(bubblePrefab);
        queueBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
        queueBubble.transform.position = spawnPosition;
    }

    private void updateQueue()
    {
        if (nextBubble.GetComponent<Bubble>().hitTarget())
        {
            nextBubble = queueBubble;
            nextBubble.transform.position = transform.position;

            queueBubble = Instantiate(bubblePrefab);
            queueBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
            queueBubble.transform.position = spawnPosition;

            hasLaunchedBall = false;
        }
    }
}
