using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float bounceForce = 18f;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        if(playerRb == null){
            return;
        }
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
        anim.SetTrigger("Bounce");

    }
}