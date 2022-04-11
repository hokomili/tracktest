using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject timer;
    public GameObject row;
    public GameObject cp;
    public GameObject minimap;
    public GameObject player;
    public GameObject cursor;
    public float time;
    [Range(0.1f,10f)]
    public float mprescaleamount;
    public Vector3 mpoffset;
    public int rowcount=0;
    public int lastcp;
    [Range(1,60)]
    public int timerfps=25;
    float tiem;
    bool nextrow=true;
    void Start()
    {
        timer.GetComponent<TextMeshProUGUI>().text="00:00:00";
        time=0f;
        tiem=0f;
        row.GetComponent<TextMeshProUGUI>().text="0";
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        if(Mathf.Abs(time-tiem)>1f/timerfps){
            tiem=time;
            string a="00",b="00",c="00";
            if(time<600){
                a="0"+Mathf.FloorToInt(time/60).ToString();
            }
            else{
                a=Mathf.FloorToInt(time/60).ToString();
            }
            if(time%60<10){
                b="0"+Mathf.FloorToInt(time%60).ToString();
            }
            else{
                b=Mathf.FloorToInt(time%60).ToString();
            }
            if((time*100)%100<10){
                c="0"+(Mathf.FloorToInt(time*100)%100).ToString();
            }
            else{
                c=(Mathf.FloorToInt(time*100)%100).ToString();
            }
            timer.GetComponent<TextMeshProUGUI>().text=a+":"+b+":"+c;
        }
        if(!nextrow&&cp.GetComponent<CheckpointController>().Checkpoints[lastcp].gameObject.activeSelf==false){
            nextrow=true;
        }
        if(nextrow&&cp.GetComponent<CheckpointController>().Checkpoints[lastcp].gameObject.activeSelf==true){
            nextrow=false;
            rowcount++;
            row.GetComponent<TextMeshProUGUI>().text=rowcount.ToString();
        }
        Vector3 playerpos=player.transform.position;
        Vector2 mapsize=minimap.GetComponent<RectTransform>().sizeDelta;
        Vector3 newpos;
        newpos.x=mapsize.y/2-playerpos.x/1000*mapsize.y;
        newpos.y=mapsize.x/2-playerpos.z/1000*mapsize.x;
        newpos.z=0;
        cursor.GetComponent<RectTransform>().localPosition=newpos*mprescaleamount+mpoffset;
    }
}
