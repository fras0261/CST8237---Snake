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

    private float _speed = 0.1f;
    Vector2 directionVector = Vector2.up;
    Vector2 movementVector;

    List<GameObject> tail = new List<GameObject>();

    private bool _hasEaten = false;
    private bool _isHorizontal = false;
    private bool _isVertical = true;

    private int _lives = 3;
    private int _totalPoints = 0;

	// Use this for initialization
	void Start () {
        SpawnFood();
        livesText.text = "Lives: " + _lives;
        scoreText.text = "Score: " + _totalPoints;
        InvokeRepeating("MoveSnake", 0.3f, _speed);
	}
	
	// Update is called once per frame
	//Controls the direction that the user moves depending on the arrow key pressed by the user
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow) && _isHorizontal == false)
        {
            directionVector = Vector2.right;
            _isVertical = false;
            _isHorizontal = true;

        }            
        else if (Input.GetKey(KeyCode.DownArrow) && _isVertical == false)
        {
            directionVector = Vector2.down;
            _isVertical = true;
            _isHorizontal = false;

        }            
        else if (Input.GetKey(KeyCode.LeftArrow) && _isHorizontal == false)
        {
            directionVector = Vector2.left;
            _isVertical = false;
            _isHorizontal = true;
        }            
        else if (Input.GetKey(KeyCode.UpArrow) && _isVertical == false)
        {
            directionVector = Vector2.up;
            _isVertical = true;
            _isHorizontal = false;
        }
            
        movementVector = directionVector;
	}
		
    void MoveSnake()
    {
        Vector2 currentPosition = transform.position;

		//If the snake has a piece of food then the system will instatiate a new tail prefab and add it the tail
        if (_hasEaten)
        {
            GameObject ts = (GameObject)Instantiate(tailSegment, currentPosition, Quaternion.identity);
            tail.Insert(0, ts);
            _hasEaten = false;
        }
		//If the snake has a tail then the system simulates motion by moving the tail segment at the end to the position just behind the head
        else if(tail.Count > 0)
        {
            tail.Last().transform.position = currentPosition;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }

        transform.Translate(movementVector);
    }

	//The system will spawn a piece of food in a random location within the game's border.
    void SpawnFood()
    {
        int xFood = (int)Random.Range((leftBorder.position.x + 1.0f), (rightBorder.position.x - 1.0f));
        int yFood = (int)Random.Range((bottomBorder.position.y + 1.0f), (topBorder.position.y - 1.0f));

        Instantiate(foodPellet, new Vector2(xFood, yFood), Quaternion.identity);
    }


    void OnTriggerEnter2D(Collider2D obj)
    {
		//If the snake's head collides into a food prefab then the system will destroy the food and spawn it some where new. The score will increase.
        if (obj.name.Contains("FoodPrefab"))
        {
            _hasEaten = true;
            Destroy(obj.gameObject);
            _totalPoints += (5 * (tail.Count + 1));
            scoreText.text = "Score: " + _totalPoints;
            SpawnFood();
        }
		//If the snake's head collides into anything else then a life will be subtracted
        else
        {
            _lives--;

			//If the user still has any lives left then the snake will respawn at the starting position and its tail will be destroyed
            if (_lives > 0)
            {
                livesText.text = "Lives: " + _lives;

                foreach (GameObject tailBlock in tail)
                    GameObject.Destroy(tailBlock);

                tail.Clear();                               
                this.transform.position = new Vector3(-5, -5, 0);

            }
			//If the player losses all of their lifes then the system will reload the game.
            else
            {
                Application.LoadLevel("SnakeMain");
            }

        }
           
    }

}
