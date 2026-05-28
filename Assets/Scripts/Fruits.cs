using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour
{
    [SerializeField] int scoreValue = 1;
    [SerializeField] GameObject collectedEffectPrefab;
    Animator anim;
    void Start(){
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreValue);
            StartCoroutine(CollectEffect());
        }
    }
    IEnumerator CollectEffect()
    {
    GetComponent<Collider2D>().enabled = false;
    anim.Play("Collected");
    yield return new WaitForSeconds(0.5f);       // chờ animation xong
    Destroy(gameObject);
}
}