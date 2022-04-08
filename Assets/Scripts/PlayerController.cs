using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    [SerializeField] float currentSpeed;
    float speed;
    float rotate;
    [SerializeField] float currentRotate;
    int driftDirection;
    float driftPower;
    public float driftSpeed;
    int driftMode = 0;
    public Transform cam;
    public Transform playerModel;
    public Transform Normal;
    public Rigidbody rb;
    [Header("Bools")]
    public bool drifting;
    [Header("Parameters")]
    public float acceleration = 10f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    void Start ()
    {
    }
    void Update()
    {
        transform.position = rb.transform.position + new Vector3(0, 0.4f, 0);
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(inputDirection.sqrMagnitude > 1)
        {
            inputDirection = inputDirection.normalized;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            speed = Mathf.Abs(inputDirection.y)*acceleration;
        }
        if (driftPower > 0)
            {
                speed+=currentSpeed*driftSpeed/3;
                driftPower-=1;
            }
        
        //drift?
        if (Input.GetButtonDown("Jump") && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;

            //playerModel.parent.DOComplete();psa
            //playerModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);psa

        }
        //steer?
        if (Input.GetAxis("Horizontal") != 0&&!drifting)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(inputDirection.x);
            Steer(dir, amount*2);
        }
        
        if (drifting)
        {
            float control = (driftDirection == 1) ? Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? Remap(Input.GetAxis("Horizontal"), -1, 1, .2f, 1) : Remap(Input.GetAxis("Horizontal"), -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl;

            //ColorDrift();
        }
        if (Input.GetButtonUp("Jump") && drifting)
        {
            //Boost();
            drifting=false;
        }
        
        //Animations    

        //a) Kart
        if (!drifting)
        {
            playerModel.localEulerAngles = Vector3.Lerp(playerModel.localEulerAngles, new Vector3(0, 90 + (Input.GetAxis("Horizontal") * 15), playerModel.localEulerAngles.z), 10f/Time.deltaTime);
        }
        else
        {
            float control = (driftDirection == 1) ? Remap(Input.GetAxis("Horizontal"), -1, 1, .5f, 2) : Remap(Input.GetAxis("Horizontal"), -1, 1, 2, .5f);
            //playerModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(playerModel.parent.localEulerAngles.y,(control * 15) * driftDirection, .2f), 0);
        }
        /*
        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);
        backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((Input.GetAxis("Horizontal") * 45)));
        */
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
        
        //rb.AddForce(movement * speed);
        
        //Forward Acceleration
        if(!drifting){
            rb.AddForce(movement * currentSpeed, ForceMode.Acceleration);
        }
        else{
            rb.AddForce(transform.forward * Input.GetAxis("Vertical") * currentSpeed, ForceMode.Acceleration);
        }
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, cam.eulerAngles.y+ currentRotate, transform.eulerAngles.z), 10f/Time.deltaTime);
        //Gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        currentSpeed = Mathf.SmoothStep(currentSpeed, speed,12f/Time.deltaTime); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate,4f/Time.deltaTime); rotate = 0f;

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up*.1f), Vector3.down, out hitOn, 1.1f,layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f)   , Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        Normal.up = Vector3.Lerp(Normal.up, hitNear.normal, Time.deltaTime * 8.0f);
        Normal.Rotate(0, transform.eulerAngles.y, 0);
    }
    public void Boost()
    {
        drifting = false;

        if (driftMode > 0)
        {
            //DOVirtual.Float(currentSpeed * 3, currentSpeed, .3f * driftMode, Speed);psa
            //DOVirtual.Float(0, 1, .5f, ChromaticAmount).OnComplete(() => DOVirtual.Float(1, 0, .5f, ChromaticAmount));
            //playerModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            //playerModel.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();
        }

        driftPower = 0;
        driftMode = 0;

        /*foreach (ParticleSystem p in primaryParticles)
        {
            p.startColor = Color.clear;
            p.Stop();
        }
        */
        //playerModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);psa
    }
    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount*3;
    }
    private void Speed(float x)
    {
        speed = x;
    }
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
