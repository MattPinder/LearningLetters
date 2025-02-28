using UnityEngine;
using UnityEngine.UI;

public class SetLetter : MonoBehaviour
{
    private Button button;              // The attached button
    private GameManager gameManager;    // The scene's Game Manager

    public GameObject letterPrefab;     // The prefab tied to this button

    void Start()
    {
        button = GetComponent<Button>();    // Define the attached button
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();  // Define the scene's Game Manager

        button.onClick.AddListener(TargetLetter);  // When the button is clicked, set the relevant letter
    }

    void TargetLetter()
    {
        gameManager.selectedLetter = letterPrefab;
        gameManager.StartGame();
    }
}
