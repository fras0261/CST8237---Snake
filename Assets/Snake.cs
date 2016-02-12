using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/*Largely based on http://coffeebreakcodes.com/sample-projects/2d-snake-game-unity3d-c/*/
public class Snake : MonoBehaviour {

    public GameObject foodPellet;
    public GameObject tailSegment;

    public GUIText livesText;
    public GUIText scoreText;

    public Transform topBorder;
    public Transform bottomBorder;
    public Transform leftBorder;
    public Transform rightBorder;

    private float speed = 0.1f;
    Vector2 directionVector = Vector2.up;
    Vector2 movementVector;

    List<GameObject> tail = new List<GameObject>();

    private bool eat = false;
    private bool isHorizontal = false;
    private bool isVertical = true;

    private int lives = 3;
    private int totalPoints = 0;

	// Use this for initialization
	void Start () {
        SpawnFood();
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + totalPoints;
        InvokeRepeating("MoveSnake", 0.3f, speed);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow) && isHorizontal == false)
        {
            directionVector = Vector2.right;
            isVertical = false;
            isHorizontal = true;

        }            
        else if (Input.GetKey(KeyCode.DownArrow) && isVertical == false)
        {
            directionVector = Vector2.down;
            isVertical = true;
            isHorizontal = false;

        }            
        else if (Input.GetKey(KeyCode.LeftArrow) && isHorizontal == false)
        {
            directionVector = Vector2.left;
            isVertical = false;
            isHorizontal = true;
        }            
        else if (Input.GetKey(KeyCode.UpArrow) && isVertical == false)
        {
            directionVector = Vector2.up;
            isVertical = true;
            isHorizontal = false;
        }
            
        movementVector = directionVector;
	}

    void MoveSnake()
    {
        Vector2 currentPosition = transform.position;
        Debug.Log(tail.Count);

        if (eat)
        {
            GameObject ts = (GameObject)Instantiate(tailSegment, currentPosition, Quaternion.identity);
            tail.Insert(0, ts);
            eat = false;
        }
        else if(tail.Count > 0)
        {
            tail.Last().transform.position = currentPosition;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }

        transform.Translate(movementVector);
    }

    void SpawnFood()
    {
        int xFood = (int)Random.Range((leftBorder.position.x + 1.0f), (rightBorder.position.x - 1.0f));
        int yFood = (int)Random.Range((bottomBorder.position.y + 1.0f), (topBorder.position.y - 1.0f));

        Instantiate(foodPellet, new Vector2(xFood, yFood), Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.name.Contains("FoodPrefab"))
        {
            eat = true;
            Destroy(obj.gameObject);
            totalPoints += (5 * (tail.Count + 1));
            scoreText.text = "Score: " + totalPoints;
            SpawnFood();
        }
        else
        {
            lives--;

            if (lives > 0)
            {
                livesText.text = "Lives: " + lives;

                foreach (GameObject tailBlock in tail)
                    GameObject.Destroy(tailBlock);

                tail.Clear();                               
                this.transform.position = new Vector3(-5, -5, 0);

            }
            else
            {
                Application.LoadLevel("SnakeMain");
            }

        }
           
    }

}
