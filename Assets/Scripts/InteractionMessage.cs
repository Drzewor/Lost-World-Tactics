using UnityEngine;

public class InteractionMessage : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 5;
    [SerializeField] private float speed = 2;
    void Start()
    {
        Destroy(gameObject,timeToDestroy);
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
