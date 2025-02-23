using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private GameManager gameManager;

    public ParticleSystem explosionParticle;

    private Material orbColour;
    public Material wrongColour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        orbColour = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyCollectible()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }

    // Change collectible colour if trying to pick up the wrong one
    public void WrongColour()
    {
        gameObject.GetComponent<MeshRenderer>().material = wrongColour;
    }

    public void RevertColour()
    {
        gameObject.GetComponent<MeshRenderer>().material = orbColour;
    }
}
