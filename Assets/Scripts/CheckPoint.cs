using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    bool activated;
    Animator anim; 

    void Awake(){
        anim = GetComponent<Animator>();

    }

    void OnTriggerEnter2D(Collider2D other){
        if(activated) return;
        if(!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null){
            player.SetSpawnPoint(transform.position  + spawnOffset) ;
            activated = true;
            anim.SetBool("Activated",true);
        }
    }
}
