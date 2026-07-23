using Benjathemaker;
using NUnit.Framework;
using UnityEngine;

public class LetterController : MonoBehaviour
{
    public Vector3[] orbLocations;
    public GameObject orbPrefab;

    void Awake()
    {
        SpawnOrb();
    }

    void SpawnOrb()
    {
        foreach (Vector3 location in orbLocations)
        {
//            // Instantiate each orb as a child of the letter's main GameObject
//            Instantiate(orbPrefab, location, orbPrefab.transform.rotation, gameObject.transform);

            // Try this to reduce lag on old devices, revert to the above if it doesn't work
            GameObject newOrb = Instantiate(orbPrefab, location, orbPrefab.transform.rotation, gameObject.transform);
            if (!GameManager.instance.gemsSpinning)
            {
                newOrb.GetComponent<SimpleGemsAnim>().enabled = false;
            }
        }
    }
}
