using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public float maxdistance;
    public GameObject player;
    public GameObject animateplayer;
    public GameObject wandposition;
    public LayerMask mousecolliderlayermask;
    public GameObject bullet;
    public GameObject bulleteffect;
    public GameObject bulletendeffect;
    public float cooldowntime;
    public float preparetime;
    private float cooldown;
    bool shootprepared=false;
    // Start is called before the first frame update
    void Start()
    {
         Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown>0){
            cooldown-=Time.deltaTime;
        }
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit raycasthit,maxdistance,mousecolliderlayermask)){
            transform.position=raycasthit.point;
        }
        else{
            transform.position=ray.GetPoint(maxdistance);
        }
        if(Input.GetMouseButton(0)&&!shootprepared&&cooldown<=0){
            cooldown=preparetime;
            animateplayer.GetComponent<Animator>().Play("Armature_shoot",0,0f);
            shootprepared=true;
        }
        if(shootprepared&&cooldown<=0){
            shootprepared=false;
            cooldown=cooldowntime;
            Vector3 aimdir=(transform.position-wandposition.transform.position).normalized;
            GameObject newbullet=Instantiate(bullet,wandposition.transform.position,Quaternion.LookRotation(aimdir,Vector3.up));
            GameObject neweffect=Instantiate(bulleteffect,newbullet.transform.position,Quaternion.LookRotation(aimdir,Vector3.up));
            GameObject boomeffect=Instantiate(bulletendeffect,newbullet.transform.position,Quaternion.LookRotation(aimdir,Vector3.up));
            neweffect.transform.parent=wandposition.transform;
            boomeffect.transform.parent=newbullet.transform;
            neweffect.SetActive(true);
        }
    }
}
