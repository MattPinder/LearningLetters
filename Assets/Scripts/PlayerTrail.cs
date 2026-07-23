using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Adding this component will also add a Box Collider component, if missing
[RequireComponent(typeof(BoxCollider))]

public class PlayerTrail : MonoBehaviour
{
    private GameManager gameManager;    // Scene's Game Manager
    private Camera cam;                 // Scene's main camera
    private Vector3 playerPos;          // Player's mouse/finger/stylus position
    public BoxCollider col;            // This object's Box Collider component
    public ParticleSystem particles;    // This object's child Particle system
    private bool drawing = false;       // Whether the player is drawing or not

    public int destroyNext;            // Track index number of the next collectible required

    // Components for the 'painting' system
    public GameObject trailPrefab = null;
    private GameObject currentTrail = null;
    private float width = 5f;
    private Color color = Color.magenta;

    void Awake()
    {
        cam = Camera.main;
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        particles.gameObject.SetActive(false);
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
                StartTrail();
                UpdateComponents();
            }
            // ... deactivate 'drawing mode' if the mouse button etc. is released
            else if (Input.GetMouseButtonUp(0))
            {
                drawing = false;
                EndTrail();
                UpdateComponents();
                if (particles.isPlaying)
                {
                    particles.Stop();
                }

                
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

    // Enable/disable Box Collider and Particle System
    // Enable Particle System only if the user hasn't disabled it
    void UpdateComponents()
    {
        col.enabled = drawing;
        if (GameManager.instance.drawParticles)
        {
            particles.gameObject.SetActive(drawing);
        }
    }

    // Copied from Unity VR Tutorial
    public void StartTrail()
    {
        if (!currentTrail)
        {
            currentTrail = Instantiate(trailPrefab, transform.position, transform.rotation, transform);
            ApplySettings(currentTrail);
        }
    }

    // Copied from Unity VR Tutorial (change size/colour as needed)
    private void ApplySettings(GameObject trailObject)
    {
        TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();
        trailRenderer.widthMultiplier = width;
        trailRenderer.startColor = color;
        trailRenderer.endColor = color;
    }

    // Copied from Unity VR Tutorial
    public void EndTrail()
    {
        if (currentTrail)
        {
            currentTrail.transform.parent = null;
            currentTrail = null;
        }
    }

    public void ClearTrail()
    {
        GameObject[] oldTrails = GameObject.FindGameObjectsWithTag("Trail");
        foreach (GameObject eachTrail in oldTrails)
        {
            Destroy(eachTrail);
        }
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

                gameManager.IncreaseTreasure(1);    // Add one to the player's Treasure score
                destroyNext++;                      // Increment index of next orb
            }
            else
            {
                other.gameObject.GetComponent<CollectibleController>().WrongColour();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CollectibleController>())
        {
            other.gameObject.GetComponent<CollectibleController>().RevertColour();
        }
    }
}
