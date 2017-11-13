using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour {

    [SerializeField]
    public GameObject bubble;
    [SerializeField]
    public GameObject cannonPoiter;
    [SerializeField]
    public AudioSource popSound;
    [SerializeField]
    public AudioSource rowBlockedSound;
    [SerializeField]
    public GameObject RowPrefab;
    [SerializeField]
    public Transform deadLinePosition;
    [SerializeField]
    public bool offsetWallEnable;

    private static float bubbleOffset1 = 1.05f;
    private static float bubbleOffset2 = bubbleOffset1 * 2;

    private int timesFired;
    private GameObject lastCeilRow;
    private int NumberOfColumns = 10; // number of columns for the grid
    private int NumberOfRows = 16; // number of rows for the grid
    [SerializeField]
    public int[,] level1grid =
    {
        {0,0,1,2,1,1,4,4,5,5 },
        {0,1,1,2,3,3,4,5,5,-1 },
        {0,0,1,2,1,1,4,4,5,5 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 }
    };
    private int[,] level2grid = 
    {
        {-1,-1,-1,0,0,-1,-1,-1 },
        {-1,-1,3,0,4,-1,-1,-1 },
        {-1,-1,5,3,4,1,-1,-1 },
        {-1,0,5,-1,1,3,-1,-1 },
        {-1,2,0,-1,-1,3,2,-1 },
        {2,4,-1,-1,-1,2,1,-1 },
        {5,4,-1,-1,-1,-1,1,1 },
        {-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1 },
        {-1,-1,-1,-1,-1,-1,-1,-1 }
    };

    private GameObject[,] bubbleGrid;

    void Start () {
        bubbleGrid = new GameObject[NumberOfRows, NumberOfColumns];
        createGrid();
        timesFired = 0;
    }
	
	void Update () {
		
	}

    private void createGrid()
    {
        /* output each array element's value */
        Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        int[,] levelGrid = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "level2") ? level2grid : level1grid; 
        for (int i = 0; i < NumberOfRows; i++)
        {
            for ( int j = 0; j < NumberOfColumns; j++)
            {
                if(levelGrid[i,j] != -1)
                {
                    GameObject newBubble = GameObject.Instantiate(bubble, transform);
                    newBubble.transform.SetParent(transform);
                    newBubble.GetComponent<Bubble>().setColor(levelGrid[i, j]);
                    newBubble.transform.position = transform.position;
                    newBubble.transform.localPosition += GetGridWordlPostion(i, j);
                    bubbleGrid[i, j] = newBubble;
                }
            }
        }
    }

    public void SetInGrid(GameObject bubble)
    {
        float oldY = bubble.transform.localPosition.y;
        bubble.transform.parent = transform;
        Vector2 gridPos = FindGridCoodinates(bubble.transform.localPosition);
        int row = (int)gridPos.x, col = (int)gridPos.y;
        Debug.Log("Set in grid: (" + bubble.transform.localPosition.x + ", " + bubble.transform.localPosition.z + ") => (" + row + ", " + col + ")");
        bubble.transform.localPosition = GetGridWordlPostion(row, col);
        bubble.transform.localPosition.Set(bubble.transform.localPosition.x,oldY,bubble.transform.localPosition.z);
        bubbleGrid[row, col] = bubble;
        cannonPoiter.SendMessage("updateQueue");
        FindAndClearCluster(row, col);
        timesFired++;
        if (timesFired == 5)
        {
            timesFired = 0;
            addRow();
            if(offsetWallEnable)
                OffsetWall();
        }
        int bubblesLeft = FindBubblesLeft();
        Debug.Log("Bubbles Left:" + bubblesLeft);
        if(bubblesLeft == 0)
        {
            // Win Game
            Debug.Log("Won Game");
            LevelManager.Instance.gameWon(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }else
        {
            GameObject lowestBubble = FindLowestBubble();
            if (lowestBubble.transform.position.z - bubbleOffset1 <= deadLinePosition.position.z)
            {
                // Lose Game
                Debug.Log("Lost Game");
                LevelManager.Instance.gameLost();
            }
        }
        
    }

    private void OffsetWall()
    {
        FindObjectOfType<WallMesh>().OffsetWall() ;
    }

    private int FindBubblesLeft()
    {
        int counter = 0;
        for (int i = 0; i < NumberOfRows; i++)
        {
            GameObject lowestInRow = null;
            for (int j = 0; j < NumberOfColumns; j++)
            {
                if (bubbleGrid[i, j] != null)
                {
                    lowestInRow = bubbleGrid[i, j];
                    counter++;
                }
            }
            if (lowestInRow == null)
                break;
        }
        return counter;
    }

    private GameObject FindLowestBubble()
    {
        GameObject lowest = null;
        for(int i = 0; i < NumberOfRows; i++)
        {
            GameObject lowestInRow = null;
            for(int j = 0; j < NumberOfColumns; j++)
            {
                if (bubbleGrid[i, j] != null)
                {
                    lowestInRow = bubbleGrid[i, j];
                    break;
                }
            }
            if (lowestInRow != null)
                lowest = lowestInRow;
            else
                break;
        }
        return lowest;
    }

    private void FindAndClearCluster(int row, int col)
    {
        List<Vector2> toBeAnalysed = new List<Vector2>();
        List<Vector2> done = new List<Vector2>();
        toBeAnalysed.Add(new Vector2(row, col));
        FindCluster(ref toBeAnalysed, ref done);
        //string debugClust = "";
        /*foreach (Vector2 c in done)
        {
            debugClust += "(" + c.x + ", " + c.y + ")\t";
        }*/
        //Debug.Log(debugClust);
        if (done.Count > 2)
        {
            addPoints(done.Count * 10);
            ClearCluster(ref done);
            CheckHangingBubbles(ref done);
        }
    }

    private List<Vector2> FindAllConnectedBubbles(Vector2 b)
    {
        List<Vector2> connected = GetAdjancentBubbles(b);
        connected.Add(b);
        for (int i =0; i < connected.Count; i++)
        {
            List<Vector2> neighbors = GetAdjancentBubbles(connected[i]);
            foreach(Vector2 v in neighbors)
            {
                if (!connected.Contains(v))
                    connected.Add(v);
            }
        }
        return connected;
    }

    private void CheckHangingBubbles(ref List<Vector2> done)
    {
        //Debug.Log("Check hanging");
        // Get connected bubble to the popping ones
        List<Vector2> hanging = new List<Vector2>();
        foreach (Vector2 v in done)
        {
            List<Vector2> neighbors = GetAdjancentBubbles(v);
            //Debug.Log("Neighbors from " + v + ": " + neighbors.Count);
            foreach(Vector2 v2 in neighbors)
            {
                if (v2.x != 0)
                {
                    List<Vector2> connected = FindAllConnectedBubbles(v2);
                    //Debug.Log("Connected to " + v2 + ": " + connected.Count);
                    if (connected.FindIndex(x => x.x == 0.0f) == -1)
                    {
                        hanging = hanging.Union(connected).ToList();
                       // Debug.Log("Hanging: " + hanging.Count);
                    }
                }
            }
        }
        DropCluster(ref hanging);
    }

    private void ClearCluster(ref List<Vector2> done)
    {
        //Debug.Log("Playing bubble burst sound");
        //popSound.Play();
        foreach (Vector2 bubble in done)
        {
            int row = (int)bubble.x, col = (int)bubble.y;
            GameObject go = bubbleGrid[row, col];
            bubbleGrid[row, col] = null;
            go.SendMessage("DestroyBubble");
        }
    }

    private void DropCluster(ref List<Vector2> hanging)
    {
        addPoints((int)Math.Pow(2.0, hanging.Count) * 10);
        foreach (Vector2 bubble in hanging)
        {
            int row = (int)bubble.x, col = (int)bubble.y;
            GameObject go = bubbleGrid[row, col];
            bubbleGrid[row, col] = null;
            go.SendMessage("DropBubble");
        }
    }

    private List<Vector2> GetAdjancentBubbles(Vector2 b)
    {
        List<Vector2> bubbles = new List<Vector2>();
        int row = (int)b.x, col = (int)b.y;
        Vector2[] adjs = {
                new Vector2(row,col-1),
                new Vector2(row, col + 1),
                new Vector2(row - 1, col - (row % 2 == 0 ? 1 : 0)),
                new Vector2(row - 1, col + (row % 2 == 0 ? 0 : 1)),
                new Vector2(row + 1, col - (row % 2 == 0 ? 1 : 0)),
                new Vector2(row + 1, col + (row % 2 == 0 ? 0 : 1))
            };
        for (int j = 0; j < 6; j++)
        {
            int bubbleRow = (int)adjs[j].x, bubbleCol = (int)adjs[j].y;
            //Debug.Log("Checking neighbor: (" + bubbleRow + ", " + bubbleCol + ")");
            if (bubbleRow >= 0 && bubbleCol >= 0 && bubbleRow < NumberOfRows && bubbleCol < NumberOfColumns && bubbleGrid[bubbleRow, bubbleCol] != null)
            {
                bubbles.Add(new Vector2(bubbleRow, bubbleCol));
            }
        }
        return bubbles;
    }

    private List<Vector2> GetSameColorAdjancentBubbles(Vector2 b)
    {
        int color = bubbleGrid[(int)b.x, (int)b.y].GetComponent<Bubble>().GetColor();
        List<Vector2> bubbles = GetAdjancentBubbles(b);
        if (bubbles.Count > 0)
            return bubbles.FindAll(x => bubbleGrid[(int)x.x, (int)x.y].GetComponent<Bubble>().GetColor() == color);
        else return bubbles;
    }

    private void FindCluster(ref List<Vector2> toAnalyse, ref List<Vector2> done)
    {
        for (int i = 0; i < toAnalyse.Count; i++)
        {
            List<Vector2> sameColorNeighbors = GetSameColorAdjancentBubbles(toAnalyse[i]);
            foreach(Vector2 bubble in sameColorNeighbors)
            {
                //Debug.Log(bubble.x + ", " + bubble.y);
                if (!done.Contains(bubble) && !toAnalyse.Contains(bubble))
                    toAnalyse.Add(bubble);
                //Debug.Log(bubble);
            }
            done.Add(toAnalyse[i]);
        }
    }

    private Vector3 GetGridWordlPostion(int row, int col)
    {
        return new Vector3(+bubbleOffset1 + ((col) * bubbleOffset2) + (row % 2 != 0 ? bubbleOffset1 : 0.0f), bubbleOffset1, - bubbleOffset1 - ((row) * bubbleOffset2));
    }

    private Vector2 FindGridCoodinates(Vector3 pos)
    {
        int row = (int)Math.Round(Math.Abs((pos.z+ bubbleOffset1) / bubbleOffset2));
        float col = 0.0f;
        if (pos.x < bubbleOffset1)
            col = 0;
        else
        {
            col = (int)Math.Round(Math.Abs((pos.x - (row % 2 != 0 ? bubbleOffset1 : 0.0f) - bubbleOffset1) / bubbleOffset2));
            if (row % 2 == 0 && col >= NumberOfColumns)
                col = NumberOfColumns-1;
            else if (row % 2 != 0 && col >= NumberOfColumns-1)
                col = NumberOfColumns-2;
        }
        return new Vector3(row, col);
    }

    private void addPoints(int points)
    {
        GameScore.Instance.addToScore(points);
    }

    public void addRow()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z -bubbleOffset1);
        lastCeilRow = GameObject.Instantiate(RowPrefab);
        lastCeilRow.transform.SetParent(transform);
        lastCeilRow.transform.position = transform.position;
        lastCeilRow.transform.localPosition = new Vector3(-0.03f,0.0f, +bubbleOffset1);
        lastCeilRow.transform.SetParent(null);
        //rowBlockedSound.Play();
    }
}
