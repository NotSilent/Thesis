using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] float time;

    Vector3 startingScale;

    bool isInitialized = false;

    void Start()
    {
        startingScale = transform.localScale;
    }

    void Update()
    {
        if (isInitialized && transform.localScale.x > 10)
            transform.localScale -= startingScale / time * Time.deltaTime;
    }

    void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.Disable();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

    public void Init()
    {
        isInitialized = true;
    }
}
