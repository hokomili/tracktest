using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletpProjectile : MonoBehaviour
{
    private Rigidbody bulletrigid;
    private float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        lifetime=3;
        bulletrigid=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed=100f;
        bulletrigid.velocity=transform.forward*speed;
        if(lifetime<0){
            Debug.Log("No hit");
            Destroy(gameObject);
        }
        lifetime-=Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer==default){
            Debug.Log("HP-1");
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(gameObject,0.5f);
        }
    }
}
