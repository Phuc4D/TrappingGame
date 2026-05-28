using UnityEngine;

public class EndFlag : MonoBehaviour
{
    Animator anim;
    void Awake(){
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            anim.Play("EndPressed");
        GameManager.Instance.LevelComplete(); 
        }
    }
}
