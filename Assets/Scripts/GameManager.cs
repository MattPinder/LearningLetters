using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject letterPrefab;

    public bool isGameActive;

    public GameObject orbPrefab;

    public TextMeshProUGUI scoreText;
    public int orbsRemaining;

    private PlayerTrail player;

    //    public Vector3[] letterShape { get; private set; }
    public List<Vector3> letterShape = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerTrail>();

        isGameActive = true;

        // WHERE IS THE BEST PLACE TO SET ALL LETTER SHAPES?

        //// Letter A
        //letterShape = new[] { new Vector3(0.0f, 3.7f, 0.0f), new Vector3(-0.9f, 1.7f, 0.0f), new Vector3(-2.5f, -0.7f, 0.0f),
        //                      new Vector3(-4.0f, -3.0f, 0.0f), new Vector3(0.9f, 1.7f, 0.0f), new Vector3(2.5f, -0.7f, 0.0f),
        //                      new Vector3(4.0f, -3.0f, 0.0f), new Vector3(-0.8f, -0.7f, 0.0f), new Vector3(0.8f, -0.7f, 0.0f) };

        //// Letter C
        //letterShape = new[] { new Vector3(3.0f, 2.5f, 0.0f), new Vector3(0.0f, 3.5f, 0.0f), new Vector3(-2.5f, 1.5f, 0.0f),
        //                      new Vector3(-2.5f, -1.5f, 0.0f), new Vector3(0.0f, -3.5f, 0.0f), new Vector3(3.0f, -2.5f, 0f) };

        //        SpawnLetter(letterShape);
        Instantiate(letterPrefab, letterPrefab.transform.position, letterPrefab.transform.rotation);
        GameObject[] ArrayOfOrbs = GameObject.FindGameObjectsWithTag("Orb");
        Debug.Log("Length of array: " + ArrayOfOrbs.Length);
        
        foreach (GameObject orb in ArrayOfOrbs)
        {
            letterShape.Add(orb.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // IF ALL ORBS ARE GONE, SPAWN ANOTHER RANDOM LETTER FROM A LIST
        if (orbsRemaining == 0)
        {
            player.destroyNext = 0;
//            SpawnLetter(letterShape);
        }
    }

    // Spawn letter based off of a Vector3 array
//    void SpawnLetter(Vector3[] posList)
//    {
//        for (int i = 0; i < posList.Length; i++)
//        {
//            Instantiate(orbPrefab, posList[i], orbPrefab.transform.rotation);
//        }
//        orbsRemaining = posList.Length;
//        scoreText.text = "Orbs remaining: " + posList.Length;
//    }
}
