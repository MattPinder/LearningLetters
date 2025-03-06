using TMPro;
using UnityEditor;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    // Variables for Menu and Instructions screens
    [Header("Menu and Instruction Screens")]
    public GameObject titleScreen;         // Game Object representing the title screen
    public GameObject infoScreen;          // Game Object representing the instruction screen

    // Variables for in-game HUD
    [Header("In-game HUD")]
    public TextMeshProUGUI orbsCollectedText;   // Text displaying the orbs collected during gameplay
    public TextMeshProUGUI lettersCompleteText;     // Text displaying the number of completed letters during gameplay

    // Variables for upper/lower case slider
    [Header("Upper/Lower Case Slider")]
    private bool letterStyle = false;       // Start with lower case letters
    public GameObject upperCase;            // Upper case buttons
    public GameObject lowerCase;            // Lower case buttons

    // Other
    [Header("Other")]
    public GameManager gameManager;         // Game Manager
    public TextMeshProUGUI winText;         // Text displaying when the player wins a round

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Toggle between Menu, Instructions, and Game UI
    public void ToInstructions()
    {
        titleScreen.gameObject.SetActive(false);
        infoScreen.gameObject.SetActive(true);
    }

    public void ToMenu()
    {
        titleScreen.gameObject.SetActive(true);             // Show title screen
        infoScreen.gameObject.SetActive(false);             // Hide instruction screen
        orbsCollectedText.gameObject.SetActive(false);      // Hide treasure collected text
        lettersCompleteText.gameObject.SetActive(false);    // Hide letters complete text
        winText.gameObject.SetActive(false);                // Hide win screen text
    }

    public void ToGame()
    {
        orbsCollectedText.text = "Treasure collected: " + gameManager.treasureCollected; // Set orbs collected to zero
        orbsCollectedText.gameObject.SetActive(true);                                    // Show treasure collected text
        lettersCompleteText.text = "Letters remaining: " + gameManager.lettersToWin;     // Set initial letters complete text
        lettersCompleteText.gameObject.SetActive(true);                                  // Show letters complete text
        titleScreen.gameObject.SetActive(false);                                         // Hide title screen text
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
