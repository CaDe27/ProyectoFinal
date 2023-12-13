using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEye : MonoBehaviour
{
    public GameObject RedEye;
    public float radius;
    public float speed = 3.141516954f; // Speed of the translation
    private float angle = 0.0f; // Current angle of rotation
    public bool activateMotion = false; // Flag to activate/deactivate the motion
    public float motionDuration = 4.0f; // Duration for which the motion is active
    private float motionTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        radius = Vector3.Distance(transform.position, RedEye.transform.position)/2;
        StartMotion();   
    }

    // Update is called once per frame
    void Update()
    {
        if(activateMotion){
            motionTimer += Time.deltaTime;
            if(motionTimer >= motionDuration){
                activateMotion = false;
                motionTimer = 0.0f;
                return;
            }
            TranslateAround();
        }
        else{
            radius = Vector3.Distance(transform.position, RedEye.transform.position);
        }
    }

    
    void TranslateAround(){
        angle += speed * Time.deltaTime;

        // Calculate the new position of the cylinder
        float x = RedEye.transform.position.x + radius * Mathf.Cos(angle);
        float z = RedEye.transform.position.z + radius * Mathf.Sin(angle);
        float y = transform.position.y; // Keep the Y-coordinate constant

        transform.position = new Vector3(x, y, z);
    }

    public void StartMotion()
    {
        activateMotion = true;
        motionTimer = 0.0f;
    }
}
