using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetAltGameMode : MonoBehaviour
{
    private Button button;              // The attached button
    private GameManager gameManager;    // The scene's Game manager

    public enum GameMode { Random, Lower, Upper, All };
    public GameMode gameMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();    // Define the attached button
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();  // Define the scene's Game Manager

        button.onClick.AddListener(SetGameMode);
    }

    void SetGameMode()
    {
        gameManager.altGameMode = gameMode.ToString();
        gameManager.StartGame();
    }
}