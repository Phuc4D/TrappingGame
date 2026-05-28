using UnityEngine;

public class CollectedEffect : MonoBehaviour
{
    [SerializeField] float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}