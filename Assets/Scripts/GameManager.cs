using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Variables for letter spawning
    public GameObject[] letterPrefabs;                          // Array of letter prefabs (defined in Unity Editor)
    public GameObject selectedLetter;                           // Letter defined by the button press
    public int orbsRemaining;                                   // How many orbs left to collect on the current letter
    public int treasureCollected;                              // How many orbs the player has collected this game
    private int lettersComplete;                                // How many letters have been completed in the current session
    // Find a way to convert letterShape to a property!
    public List<Vector3> letterShape = new List<Vector3>();     // List containing the Vector3 of all of a letter's orbs
    private float spawnDelay = 0.5f;                            // Seconds to wait before spawning a new letter
    public int lettersToWin { get; private set; }               // Number of letters to complete to win    

    // Variables for sound effects
    private AudioSource gameAudio;          // Game audio source
    public AudioClip orbCollectedSound;     // Sound played when orb collected
    public AudioClip letterCompletedSound;  // Sound played when letter completed
    public AudioClip winSound;              // Plays when the game is won

    // Other
    public MenuUIController menuUIController;                   // Canvas's menu UI controller script
    private PlayerTrail player;                                 // GameObject representing the player
    public bool isGameActive = false;                           // Determine whether the game is active or not
    public string altGameMode = null;                           // Any alternate game mode selected (Upper/Lower/All)

    // Start on menu screen, and find the Player object
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerTrail>();
        gameAudio = GetComponent<AudioSource>();
        menuUIController = GameObject.Find("Canvas").GetComponent<MenuUIController>();
    }

    // Check each frame whether any orbs remain; if not, spawn a new letter
    void Update()
    {
        if (isGameActive && orbsRemaining == 0)
        {
            orbsRemaining = 1;                      // Temporarily add 1 to orbsRemaining to break the if-statement and prevent over-spawning
            IncreaseScore(1);                       // Add 1 to the score

            if (lettersComplete == lettersToWin)
            {
                gameAudio.PlayOneShot(winSound);
                menuUIController.VictoryScreen();
                Invoke("WinGame", 1.0f);                      // End game if the player has completed enough letters
            } else {
                gameAudio.PlayOneShot(letterCompletedSound);
                Invoke("SpawnLetter", spawnDelay);            // Spawn a new letter after a one second delay
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;                                                    // Activate the game
        lettersComplete = 0;                                                    // Set completed letters to zero
        treasureCollected = 0;                                                  // Set collected orbs to zero
        WinCondition();                                                         // Set the win condition
        menuUIController.ToGame();                                              // Set UI to game screen
        SpawnLetter();                                                          // Spawn letter to start the game
    }

    // Once the game is won, return to main menu
    void WinGame()
    {
        isGameActive = false;                               // Deactivate the game state
        selectedLetter = null;                              // Unload the selected letter
        player.ClearTrail();                                // Remove the last set of painted trails
        altGameMode = null;                                 // Reset altGameMode
        menuUIController.ToMenu();                          // Set UI to menu screen
    }

    // Increment orbs collected count
    public void IncreaseTreasure(int AddToScore)
    {
        gameAudio.PlayOneShot(orbCollectedSound);
        treasureCollected += AddToScore;
        menuUIController.orbsCollectedText.text = "Treasure collected: " + treasureCollected;
    }

    // Increment letters completed count
    void IncreaseScore(int AddToScore)
    {
        lettersComplete += AddToScore;
        menuUIController.lettersCompleteText.text = "Letters remaining: " + (lettersToWin - lettersComplete);
    }

    // Set number of letters needed to win
    void WinCondition()
    {
        if (altGameMode == "Upper" || altGameMode == "Lower")
        {
            lettersToWin = 26;
        }
        else if (altGameMode == "All")
        {
            lettersToWin = 52;
        }
        else
        {
            lettersToWin = 3;
        }
    }

    void SpawnLetter()
    {
        player.ClearTrail();                // Clear the player's trail from the previous letter
        letterShape = new List<Vector3>();  // Purge previous letterShape
        orbsRemaining = 0;                  // Reset orbsRemaining (set to 1 in Update() to stop extra spawns)
        player.destroyNext = 0;             // Reset the player's destroyNext variable

        if (selectedLetter == null && altGameMode == null)  // Random letters
        {
            // Get a random index from the letterPrefabs list, and instantiate the random letter
            int letterIndex = Random.Range(0, letterPrefabs.Length);
            Instantiate(letterPrefabs[letterIndex],
                        letterPrefabs[letterIndex].transform.position,
                        letterPrefabs[letterIndex].transform.rotation);
        }
        else if (altGameMode == "Upper" || altGameMode == "All") // All upper case letters, or all letters
        {
            Instantiate(letterPrefabs[lettersComplete],
                        letterPrefabs[lettersComplete].transform.position,
                        letterPrefabs[lettersComplete].transform.rotation);
        }
        else if (altGameMode == "Lower") // All lower case letters
        {
            Instantiate(letterPrefabs[lettersComplete + 26],
                        letterPrefabs[lettersComplete + 26].transform.position,
                        letterPrefabs[lettersComplete + 26].transform.rotation);
        }
        else // Single letters
        {
            // Instantiate the selected letter
            Instantiate(selectedLetter, selectedLetter.transform.position, selectedLetter.transform.rotation);
        }
        GameObject[] arrayOfOrbs = GameObject.FindGameObjectsWithTag("Orb");        // Get an array containing all of the newly-spawned orbs

        // Get the coordinates of all of the orbs, and increment orbsRemaining
        // FUNCTION TO ADD: SPAWN THE ORBS DEACTIVATED, THEN ACTIVATE EACH IN TURN AFTER A SHORT (0.5s?) DELAY
        foreach (GameObject orb in arrayOfOrbs)
        {
            letterShape.Add(orb.transform.position);
            orbsRemaining++;
        }
    }
}