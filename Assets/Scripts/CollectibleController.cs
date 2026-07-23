using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerTrail player;
    public ParticleSystem explosionParticle;

    [Header("Orb Colours")]
    private Material orbColour;
    public Material nextColour;
    public Material wrongColour;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerTrail>();
        orbColour = gameObject.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (player.col.enabled == false)
        {
            RevertColour();
        }

        // Change the colour of the orb if it's next in sequence
        if (gameManager.letterShape[player.destroyNext] == gameObject.transform.position)
        {
            NextColour();
        }
    }

    public void DestroyCollectible()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            if (GameManager.instance.explosions)
            {
                Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            }
        }
    }

    // Change collectible colour if it's the next to be collected
    public void NextColour()
    {
        gameObject.GetComponent<MeshRenderer>().material = nextColour;
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