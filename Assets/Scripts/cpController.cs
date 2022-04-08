using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpController : MonoBehaviour
{
    public GameObject pb;
    public int selfindex;
    public bool alive;
    private GameObject effec;
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
            effec=Instantiate(pb, this.gameObject.transform.position, Quaternion.identity);
            Destroy(effec,2000.0f);
            alive=true;
            GetComponentInParent<CheckpointController>().Checkpoints[selfindex].gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider collision){
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Player"){
            alive=false;
            if(selfindex+1<GetComponentInParent<CheckpointController>().Checkpoints.Length){
                GetComponentInParent<CheckpointController>().Checkpoints[selfindex+1].SetActive(true);
            }
            else{
                GetComponentInParent<CheckpointController>().Checkpoints[0].SetActive(true);
            }
        }
    }
}
