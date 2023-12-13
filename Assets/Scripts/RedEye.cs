using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEye : MonoBehaviour
{
    public float initialZ = -1.0f;
    public float speed = 1.0f;
    private float cylinderRadius = 8.0f; // white floor

    // waiting black eye translation
    private bool waiting = true;
    private float waitTime = 4.0f;

    // Spheres 
    public float lowZ = 0.8f;
    public float collisionZ = 1.35f;
    private float targetZ;
    public GameObject spherePrefab;
    [SerializeField] List<GameObject> spheres_list = new List<GameObject>();
    private GameObject sphereHierarchy;
    public List<GameObject> SpheresList{
        get { return spheres_list; }
        set { spheres_list = value; }
    }
    private bool movingTowardsCollision = false;
    private bool oscillating = false;
    public float oscillationTime = 15.0f;

    // Lines
    public GameObject linePrefab;
    [SerializeField] List<GameObject> lines_list = new List<GameObject>();
    private GameObject lineHierarchy;
    public List<GameObject> LinesList{
        get { return lines_list; }
        set { lines_list = value; }
    }
    private bool scaling = false;
    public float scalingTime = 1.0f;
    public int scalingTimes = 18;

    // Start is called before the first frame update
    void Start()
    {
        sphereHierarchy = new GameObject();
        sphereHierarchy.name = "Secondary spheres";

        lineHierarchy = new GameObject();
        lineHierarchy.name = "Lines";
    }

    // Update is called once per frame
    void Update()
    {
        if(waiting){
            waitTime -= Time.deltaTime;
            if(waitTime <= 0){
                waiting = false;
                scaling = true;
            }
        }
        else if(scaling){
            scalingTime -= Time.deltaTime;
            if(scalingTime <= 0){
                scalingTime = 1.0f;
                --scalingTimes;
                if(scalingTimes == 0){
                    scaling = false;
                    movingTowardsCollision = true;
                }

                float multiplier = (scalingTimes & 1) == 1 ? 2 : 0.5f; 
                transform.localScale = new Vector3(
                                        transform.localScale.x * multiplier, 
                                        transform.localScale.y * multiplier, 
                                        transform.localScale.z * multiplier);
                if(multiplier == 2){
                    InstantiateLine(scalingTimes/2);
                }
            } 
        }

        else if (movingTowardsCollision)
        {
            float step = speed * Time.deltaTime;            
            transform.position = new Vector3(transform.position.x, transform.position.y, 
                                             Mathf.MoveTowards(transform.position.z, collisionZ, step));

            // Check if reached collisionZ
            if (transform.position.z >= collisionZ - 0.001)
            {
                movingTowardsCollision = false;
                oscillating = true;
                InstantiateSphere();
                targetZ = lowZ;
            }
        }

        // Oscillate between lowZ and collisionZ
        else if (oscillating)
        {
            float step = speed * Time.deltaTime;
            if( Mathf.Abs(transform.position.z - targetZ) <= 0.01){
                if(targetZ == collisionZ)
                    InstantiateSphere();
                targetZ = targetZ == collisionZ? lowZ : collisionZ;
            }           
            transform.position = new Vector3(transform.position.x, transform.position.y, 
                                             Mathf.MoveTowards(transform.position.z, targetZ, step));

            oscillationTime -= Time.deltaTime;
            if(oscillationTime <= 0){
                oscillating = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Z)){
            InstantiateSphere();
        }
        if(Input.GetKeyUp(KeyCode.X)){
            InstantiateLine(Random.Range(0, 3));
        }
        eliminateInvalid();

    }

    void InstantiateSphere()
    {   
        float localCylinderRadius = 0.7f * cylinderRadius;
        float x = Random.Range(-localCylinderRadius, localCylinderRadius);
        float z = Random.Range(-localCylinderRadius, localCylinderRadius);
        float distanceToCenter = Mathf.Sqrt(x*x + z*z);
        float maxRadius = (localCylinderRadius*1.01f- distanceToCenter);
        float biasedRandom = 1.0f - Mathf.Pow(Random.Range(0.3f, 0.85f), 2);
        float diameter = 2*maxRadius*biasedRandom;

        GameObject sphereInst = Instantiate(spherePrefab, new Vector3(x,5,z), Quaternion.identity);
        sphereInst.transform.localScale = new Vector3(diameter, diameter, diameter);

        spheres_list.Add(sphereInst.gameObject);
        sphereInst.transform.parent = sphereHierarchy.gameObject.transform;
    }

    void InstantiateLine(int time)
    {
        // Generate two random points on a circles
        int quadrant1, quadrant2;
        if(time % 4 == 2){
            quadrant1 = 0; 
            quadrant2 = 2;
        }
        else if(time % 4 == 1){
            quadrant1 = 1; 
            quadrant2 = 3;
        }
        else if(time % 4 == 0){
            quadrant1 = 0; 
            quadrant2 = 1;
        }
        else{
            quadrant1 = 2; 
            quadrant2 = 3;
        }

        Vector3 pointA = RandomPointOnCircle(cylinderRadius, quadrant1);
        Vector3 pointB = RandomPointOnCircle(cylinderRadius, quadrant2);

        // Calculate the midpoint
        Vector3 midpoint = (pointA + pointB) / 2;

        // Instantiate the cube at the midpoint
        GameObject cubeInst = Instantiate(linePrefab, midpoint, Quaternion.identity);

        // Scale the cube in Y and Z
        float distance = Vector3.Distance(pointA, pointB);
        cubeInst.transform.localScale = new Vector3(distance, 0.1f, 0.1f);

        // Rotate the cube to align with the two points
        // Calculate direction vector from pointA to pointB
        Vector3 direction = pointB - pointA;
        // Calculate the rotation needed to align the cube along this direction
        cubeInst.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Adjust the cube's rotation so that it's length aligns with the line between points
        cubeInst.transform.Rotate(0, -90, 0, Space.Self);

        lines_list.Add(cubeInst.gameObject);
        cubeInst.transform.parent = lineHierarchy.gameObject.transform;
    }

    Vector3 RandomPointOnCircle(float radius, int quadrant)
    {
        float angle = Random.Range(0.0f, Mathf.PI/2) + quadrant * Mathf.PI/2;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(x, 5, z);
    }

    void eliminateInvalid(){
        for(int i = 0; i < spheres_list.Count;){
            if(spheres_list[i].transform.position.y < -4){
                Destroy(spheres_list[i]);
                spheres_list.RemoveAt(i);
            }
            else{
                ++i;
            }
        }
        for(int i = 0; i < lines_list.Count;){
            if(lines_list[i].transform.position.y < -4){
                Destroy(lines_list[i]);
                lines_list.RemoveAt(i);
            }
            else{
                ++i;
            }
        }
    }
}
