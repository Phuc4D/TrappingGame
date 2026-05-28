using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] float swingSpeed = 2f;
    [SerializeField] float swingAngle = 60f;
    [SerializeField] GameObject chainPrefab;
    [SerializeField] int chainLength = 5;
    void Start(){
    Vector3 pivot = transform.parent.position;
    Vector3 ball = transform.position;
    for(int i = 1; i < chainLength; i++){
        float t = (float)i / chainLength;
        Vector3 pos= Vector3.Lerp(pivot,ball,t);
        Instantiate(chainPrefab,pos, Quaternion.identity, transform.parent);
    }
    }
   void Update()
    {   
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.parent.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
