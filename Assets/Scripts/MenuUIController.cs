using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class MenuUIController : MonoBehaviour
{
    // Variables for Menu and Instructions screens
    [Header("Menu and Instruction Screens")]
    public GameObject titleScreen;         // Game Object representing the title screen
    public GameObject infoScreen;          // Game Object representing the instruction screen

    // Variables for in-game HUD
    [Header("In-game HUD")]
    public TextMeshProUGUI orbsCollectedText;    // Text displaying the orbs collected during gameplay
    public TextMeshProUGUI lettersCompleteText;  // Text displaying the number of completed letters during gameplay
    public Button quitCurrentGameButton;         // Button that quits the current game

    // Variables for upper/lower case slider
    [Header("Upper/Lower Case Slider")]
    private bool letterStyle = false;       // Start with lower case letters
    public GameObject upperCase;            // Upper case buttons
    public GameObject lowerCase;            // Lower case buttons

    // Other
    [Header("Other")]
    private GameManager gameManager;        // Game Manager
    private PlayerTrail player;             // GameObject representing the player
    public TextMeshProUGUI winText;         // Text displaying when the player wins a round

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerTrail>();
    }

    // Toggle between Menu, Instructions, and Game UI
    public void ToInstructions()
    {
        titleScreen.gameObject.SetActive(false);
        infoScreen.gameObject.SetActive(true);
    }

    public void ToMenu()
    {
        // Unload any remaining letters
        GameObject[] leftoverLetters = GameObject.FindGameObjectsWithTag("LetterPrefab");
        foreach (GameObject leftover in leftoverLetters)
        {
            Destroy(leftover);
        }

        player.ClearTrail();                                // Remove the last set of painted trails
        player.col.enabled = false;                         // Disable player box collider (just in case)
        player.particles.gameObject.SetActive(false);       // Disable player particle effects
        gameManager.isGameActive = false;                   // Deactivate the game state
        gameManager.selectedLetter = null;                  // Unload the selected letter
        gameManager.altGameMode = null;                     // Reset altGameMode
        
        // Show menu, and hide non-menu UI elements
        titleScreen.gameObject.SetActive(true);
        infoScreen.gameObject.SetActive(false);
        orbsCollectedText.gameObject.SetActive(false);
        lettersCompleteText.gameObject.SetActive(false);
        quitCurrentGameButton.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
    }

    public void ToGame()
    {
        // Set relevant text for the new game
        orbsCollectedText.text = "Treasure collected: " + gameManager.treasureCollected;
        lettersCompleteText.text = "Letters remaining: " + gameManager.lettersToWin;
        // Set up the relevant HUD elements
        orbsCollectedText.gameObject.SetActive(true);
        lettersCompleteText.gameObject.SetActive(true);
        quitCurrentGameButton.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);
    }

    // Switch between upper- and lower-case letters in the menu based on a toggle
    public void SwitchLetterStyle()
    {
        letterStyle = !letterStyle;

        if (letterStyle)
        {
            upperCase.gameObject.SetActive(true);
            lowerCase.gameObject.SetActive(false);
        }
        else if (!letterStyle)
        {
            upperCase.gameObject.SetActive(false);
            lowerCase.gameObject.SetActive(true);
        }
    }

    // Show the victory screen, then return to main menu after a delay
    public void VictoryScreen()
    {
        winText.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
	    Application.Quit();
    #endif
    }
}
