using UnityEngine;


public class HurtFlash : MonoBehaviour
{
    //번쩍 해야되
    public float lifetime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //생성되면 바로 삭제 시켜
        Destroy(gameObject, lifetime);    
    }
}
