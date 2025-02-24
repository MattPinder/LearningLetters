using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public GameObject[] letterPrefabs;                         // Array of letter prefabs (defined in Unity Editor)
    public TextMeshProUGUI scoreText;                          // Text displaying the remaining orbs during gameplay
    public GameObject titleScreen;                             // GameObject containing title screen text
    private PlayerTrail player;                                // GameObject representing the player

    public bool isGameActive = false;                          // Determine whether the game is active or not
    public int orbsRemaining;                                  // How many orbs left to collect on the current letter
    public List<Vector3> letterShape = new List<Vector3>();    // List containing the Vector3 of all of a letter's orbs
    // Find a way to convert letterShape to a property!

    // Start on menu screen, and find the Player object
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerTrail>();
    }

    // Check each frame whether any orbs remain; if not, spawn a new letter
    void Update()
    {
        if (isGameActive && orbsRemaining == 0)
        {
            orbsRemaining = 1;                      // Add 1 to orbsRemaining to break the if-statement and prevent over-spawning
            Invoke("SpawnRandomLetter", 1.0f);      // Spawn a new letter after a one second delay
        }
    }

    public void StartGame()
    {
        isGameActive = true;                        // Activate the game
        scoreText.gameObject.SetActive(true);       // Show score text
        titleScreen.gameObject.SetActive(false);    // Hide title screen text

        SpawnRandomLetter();                        // Spawn first letter
    }

    void SpawnRandomLetter()
    {
        letterShape = new List<Vector3>();                          // Purge previous letterShape
        orbsRemaining = 0;                                          // Reset orbsRemaining (set to 1 in Update() to stop extra spawns)
        int letterIndex = Random.Range(0, letterPrefabs.Length);    // Get a random index from the letterPrefabs list

        // Instantiate the random letter
        Instantiate(letterPrefabs[letterIndex],
                    letterPrefabs[letterIndex].transform.position,
                    letterPrefabs[letterIndex].transform.rotation);

        GameObject[] ArrayOfOrbs = GameObject.FindGameObjectsWithTag("Orb");        // Get an array containing all of the newly-spawned orbs

        // Get the coordinates of all of the orbs, and increment orbsRemaining
        // FUNCTION TO ADD: SPAWN THE ORBS DEACTIVATED, THEN ACTIVATE EACH IN TURN AFTER A SHORT (0.5s?) DELAY
        foreach (GameObject orb in ArrayOfOrbs)        
        {
            letterShape.Add(orb.transform.position);
            orbsRemaining++;
        }

        scoreText.text = "Orbs remaining: " + orbsRemaining;        // Update score text
        player.destroyNext = 0;                                     // Reset the player's destroyNext variable
    }
}