using UnityEngine;

public class SpawnN : MonoBehaviour
{

    [SerializeField] private GameObject N;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        Instantiate(N, transform.position, transform.rotation);
        spriteRenderer.sprite = null;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
