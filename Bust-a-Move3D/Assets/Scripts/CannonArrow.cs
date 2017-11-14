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
    public Transform spawnPosition;

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
            if (transform.rotation.eulerAngles.y < 50 || transform.rotation.eulerAngles.y > 280)
            {
                rotation += 100 * Time.deltaTime;
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
            if (transform.rotation.eulerAngles.y < 80 || transform.rotation.eulerAngles.y > 310)
            {
                rotation -= 100 * Time.deltaTime;
                if(!turningGears.isPlaying)
                    turningGears.Play();
            }else
            {
                if (turningGears.isPlaying)
                    turningGears.Stop();
            }
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) && !hasLaunchedBall && IsGridClear() &&!updateQueueAnimationSwitch || Time.time - lastShotTimeStamp > 10)
        {
            hasLaunchedBall = true;
            lastShotTimeStamp = Time.time;
            nextBubble.GetComponent<Bubble>().launch(transform.rotation.eulerAngles.y);
            launchSound.Play();
        }else {
            if (turningGears.isPlaying)
                turningGears.Stop();
        }

        if(updateQueueAnimation){
			
            queueBubble.transform.position = spawnPosition.position;
			float scale = 10f * Time.deltaTime;
            if (queueBubble.transform.localScale.x < 2.2)
            {
                if (nextBubble.transform.localScale.x <= Vector3.zero.x || updateQueueAnimationSwitch)
                {
                    updateQueueAnimationSwitch = true;
                    nextBubble.transform.position = transform.position;
                    nextBubble.transform.localScale += new Vector3(scale, scale, scale);
                    queueBubble.SetActive(true);
                    queueBubble.transform.localScale += new Vector3(scale, scale, scale);
                }
                else
                {

                    nextBubble.transform.localScale -= new Vector3(scale, scale, scale);
                }
            }else{
                updateQueueAnimationSwitch = false;
				nextBubble.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                updateQueueAnimation = false;
				queueBubble.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
            }

        }

        transform.Rotate(Vector3.up, rotation);
    }
    private bool isGridClear;
    public void SetGridClear(bool canPlay){
        isGridClear = canPlay;
    }

    public bool IsGridClear(){
        return isGridClear;
    }

    private void InstantiateBubbles()
    {
        nextBubble = Instantiate(bubblePrefab);
        nextBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
        nextBubble.transform.position = transform.position;

        queueBubble = Instantiate(bubblePrefab);
        queueBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
        queueBubble.transform.position = spawnPosition.position;
    }

    public void updateQueue()
    {
        updateQueue(false);
    }
    private bool updateQueueAnimation;
    private bool updateQueueAnimationSwitch;
	public void updateQueue(bool force)
	{
		print("Cannon: update queue,force="+force+", " + nextBubble);
        if (force)
            Destroy(nextBubble);
        if (force || nextBubble.GetComponent<Bubble>().hitTarget())
		{
            updateQueueAnimation = true;
			nextBubble = queueBubble;

			queueBubble = Instantiate(bubblePrefab);
			queueBubble.GetComponent<Bubble>().setColor(Random.Range(0, 6));
            queueBubble.transform.localScale = Vector3.zero;
            queueBubble.SetActive(false);
			hasLaunchedBall = false;
		}
	}
}
