using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blackFloor;
    public GameObject whiteFloor;
    public GameObject car;
    public GameObject redEye;
    public GameObject colorfulSphere;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects(){
        yield return new WaitForSeconds(1);

        GameObject blackFloorInstance = Instantiate(blackFloor, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(whiteFloor, new Vector3(0, 0.52f, 0), Quaternion.identity);
        GameObject carInstance = Instantiate(car, new Vector3(-1.853f, 0.3976f,10.04046f),  Quaternion.Euler(-90, 0, -99.154f));
        carInstance.transform.SetParent(blackFloorInstance.transform, true);

        GameObject redEyeInstance = Instantiate(redEye, new Vector3(-1.0f, 10.0f, -1.0f), Quaternion.identity);
        Transform innerBlackEye = redEyeInstance.transform.Find("BlackEye");
        if (innerBlackEye != null)
        {
            BlackEye childScript = innerBlackEye.GetComponent<BlackEye>();
            if (childScript != null)
            {
                // Modify the component as needed
                childScript.RedEye = redEyeInstance;
            }
            else{
                Debug.LogError("Script Child not found ");
            }
        }
        else{
            Debug.LogError("Child not found in red eye");
        }

        Instantiate(colorfulSphere, new Vector3(-1.0f, 10.0f, 2.3f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
