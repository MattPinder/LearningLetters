using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Adding this component will also add Trail Renderer and Box Collider components, if missing
[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class PlayerTrail : MonoBehaviour
{
    private GameManager gameManager;    // Scene's Game Manager
    private Camera cam;                 // Scene's main camera
    private Vector3 playerPos;          // Player's mouse/finger/stylus position
    private TrailRenderer trail;        // This object's Trail Renderer component
    private BoxCollider col;            // This object's Box Collider component
    private bool drawing = false;       // Whether the player is drawing or not

    public int destroyNext;            // Track index number of the next collectible required

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        destroyNext = 0;
    }

    void Update()
    {
        // If the game is running...
        if (gameManager.isGameActive)
        {
            // ... activate 'drawing mode' if the mouse button etc. is down
            if (Input.GetMouseButtonDown(0))
            {
                drawing = true;
                UpdateComponents();
            }
            // ... deactivate 'drawing mode' if the mouse button etc. is released
            else if (Input.GetMouseButtonUp(0))
            {
                drawing = false;
                UpdateComponents();
            }
            // ... move the player position if drawing mode is active
            if (drawing)
            {
                UpdateMousePosition();
            }
        }
    }

    // Move 'player' based on mouse/finger/stylus input
    void UpdateMousePosition()
    {
        playerPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, (cam.transform.position.z * -1)));
        transform.position = playerPos;
    }

    // Enable/disable Trail Renderer and Box collider
    void UpdateComponents()
    {
        trail.enabled = drawing;
        col.enabled = drawing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollectibleController>())
        {
            if (other.transform.position == gameManager.letterShape[destroyNext])
            {
                other.gameObject.GetComponent<CollectibleController>().DestroyCollectible();    // Destroy collectible
                gameManager.orbsRemaining--;    // Decrement remaining

                // If no more orbs exist, delete the parent object
                if (gameManager.orbsRemaining == 0)
                {
                    Destroy(other.transform.parent.gameObject);
                }

                scoreText.text = "Orbs remaining: " + gameManager.orbsRemaining;    // Update score text
                destroyNext++;  // Increment index of next orb
            }
            else
            {
                other.gameObject.GetComponent<CollectibleController>().WrongColour();
            }
        }
    }

    // THIS SHOULD ALSO TRIGGER IF THE BUTTON IS RELEASED WHILE TOUCHING A WRONG ORB,
    // OTHERWISE IT COULD STAY WRONG-COLOURED FOREVER
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CollectibleController>())
        {
            other.gameObject.GetComponent<CollectibleController>().RevertColour();
        }
    }
}
