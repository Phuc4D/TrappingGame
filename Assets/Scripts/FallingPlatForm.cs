using UnityEngine;
public class FallingPlatForm : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float resetDelay = 3f;

    Rigidbody2D rb;
    Vector3 startPosition;
    bool triggered;
    Animator anim;
    void Awake()
    {
        // lấy reference rb, lưu startPosition
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player") || triggered) return;
        
        bool playerAbove = collision.transform.position.y > transform.position.y;
        if(!playerAbove) return;
        
        triggered = true;
        anim.SetTrigger("Fall");
        Invoke(nameof(Fall), shakeDuration);
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Invoke(nameof(ResetPlatform), resetDelay);

    }

    void ResetPlatform()
    {
        transform.position = startPosition ;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        triggered = false;
        anim.Play("Platforms");
    }
}