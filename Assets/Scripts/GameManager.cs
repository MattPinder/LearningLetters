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
            SpawnRandomLetter();
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        scoreText.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);

        SpawnRandomLetter();
    }

    void SpawnRandomLetter()
    {
        // Purge previous letterShape
        letterShape = new List<Vector3>();

        // Get a random index from the letterPrefabs list
        int letterIndex = Random.Range(0, letterPrefabs.Length);

        // Instantiate the random letter
        Instantiate(letterPrefabs[letterIndex],
                    letterPrefabs[letterIndex].transform.position,
                    letterPrefabs[letterIndex].transform.rotation);

        // Get an array containing all of the newly-spawned orbs
        GameObject[] ArrayOfOrbs = GameObject.FindGameObjectsWithTag("Orb");

        // Get the coordinates of all of the orbs, and increment orbsRemaining
        foreach (GameObject orb in ArrayOfOrbs)
        {
            letterShape.Add(orb.transform.position);
            orbsRemaining++;
        }

        // Update score text
        scoreText.text = "Orbs remaining: " + orbsRemaining;

        // Reset the player's destroyNext variable
        player.destroyNext = 0;
    }
}