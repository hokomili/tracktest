using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
     public Transform target;
     public float distance = 10.0f;
     public float height=1.0f;
     public float sensitivity = 3.0f;
     
     private Vector3 offset; 
     
     void Start ()  {
         offset = (transform.position - target.position).normalized * distance+new Vector3(0,height,0);
         transform.position = target.position + offset;
     }
     
     void Update () {
       Quaternion q = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        // Quaternion r = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * sensitivity, Vector3.right);
        offset = q * offset;
        transform.rotation = q  * transform.rotation;
        transform.position = target.position + offset;
     }
}
