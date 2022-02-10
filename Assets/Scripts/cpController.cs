using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpController : MonoBehaviour
{
    public GameObject pb;
    public bool alive;
    // Start is called before the first frame update
    void Start()
    {
        alive=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
        {
            Instantiate(pb, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider collision){
    if(collision.gameObject.tag == "Player"){
        alive=false;
    }
 }
}
