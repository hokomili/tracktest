using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    public float speed;
     
     public Transform cam;
     private Rigidbody rb;
     
     void Start ()
     {
         rb = GetComponent<Rigidbody>();
     }
     
     void FixedUpdate ()
     {
          // CHANGED -- This limits movement speed so you won't move faster when holding a diagonal. It's just a pet peeve of mine
            Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(inputDirection.sqrMagnitude > 1)
            {
                inputDirection = inputDirection.normalized;
            }
            
            // CHANGED -- This takes the camera's facing into account and flattens the controls to a 2-D plane
            Vector3 newRight = Vector3.Cross(Vector3.up, cam.forward);
            Vector3 newForward = Vector3.Cross(newRight, Vector3.up);
            Vector3 movement = (newRight * inputDirection.x) + (newForward * inputDirection.y);
            
            rb.AddForce(movement * speed);
     }
}
  //EOC
