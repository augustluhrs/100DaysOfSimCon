using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    private bool initialized = false;
    public GameObject goal;
    private float startGoalDistance;
    [SerializeField] float goalReward = 500;
    [SerializeField] float rewardBoost = 5;
    [SerializeField] GameObject startMarble;
    Vector3 startPos;

    private NeuralNetwork net;
    // private Rigidbody rBody; //think this is only for mats
    // private Material[] mats;

    [SerializeField] float speedFactor = 2;
    public Vector3 inputForce;

    //based on EYEmaginary's "Car AI Tutorial" Series, thanks!!
    //Sensors
    //TODO: if time, make these mutable also?
    public float fallSensorLength = 6f;
    public Vector3 frontSensorPos = new Vector3 (0f, 0.2f, 0.5f);
    public Vector3 rightSensorPos = new Vector3 (0.5f, 0.2f, 0f);
    public Vector3 backSensorPos = new Vector3 (0f, 0.2f, -0.5f);
    public Vector3 leftSensorPos = new Vector3 (-0.5f, 0.2f, 0f);
    public Vector3 frontRightSensorPos = new Vector3 (0.3f, 0.2f, 0.3f);
    public Vector3 backRightSensorPos = new Vector3 (0.3f, 0.2f, -0.3f);
    public Vector3 backLeftSensorPos = new Vector3 (-0.3f, 0.2f, -0.3f);
    public Vector3 frontLeftSensorPos = new Vector3 (-0.3f, 0.2f, 0.3f);
    // public float frontSideSensorPosition = 0.2f;
    public float sideSensorAngle = 45f; //for the corner angles
    public float fallSensorAngle = 20f; //towards the ground
    
    void Start()
    {
       //start pos and goal distance
        startPos = startMarble.transform.position;
        startGoalDistance = Vector3.Distance(transform.position, goal.transform.position);

        //mats stuff goes here
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (initialized == true)
        {
            //sense surroundings, send data to NN, receive movement forces
            CheckSensorsAndMove();
        }
    }

    private void CheckSensorsAndMove()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;

        //reset sensor array
        float[] sensorsNormalized = new float[9];

        //ray to goal
        float goalDistance = Vector3.Distance(transform.position, goal.transform.position);
        float normalizedGoalDistance = goalDistance / startGoalDistance;
        Vector3 goalDirection = (goal.transform.position - transform.position);
        if(Physics.Raycast(sensorStartPos, goalDirection, out hit, 100f)){
            Debug.DrawLine(sensorStartPos, hit.point);
            // Debug.Log(normalizedGoalDistance);
            sensorsNormalized[0] = 1f - normalizedGoalDistance;
            //sensorsNormalized[0] = goalDistance;
        }
        else {
            Debug.Log("where the hell are you? Did you fall off?");
            sensorsNormalized[0] = -1f;
        }
        //front center fall sensor
        Vector3 frontStartPos = transform.position;
        frontStartPos += Vector3.forward * frontSensorPos.z;
        frontStartPos += Vector3.up * frontSensorPos.y;
        if (Physics.Raycast(frontStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontStartPos, hit.point);
            sensorsNormalized[1] = 1f - (hit.distance / fallSensorLength);
        } else {
            sensorsNormalized[1] = -1f;
        }
        //front right fall sensor
        Vector3 frontRightStartPos = transform.position;
        frontRightStartPos += Vector3.forward * frontRightSensorPos.z;
        frontRightStartPos += Vector3.up * frontRightSensorPos.y;
        frontRightStartPos += Vector3.right * frontRightSensorPos.x;
        if (Physics.Raycast(frontRightStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(sideSensorAngle, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontRightStartPos, hit.point);
            sensorsNormalized[2] = 1f - (hit.distance / fallSensorLength);
        } else {
            sensorsNormalized[2] =-1f;
        }
        //right fall sensor
        Vector3 rightStartPos = transform.position;
        rightStartPos += Vector3.up * rightSensorPos.y;
        rightStartPos += Vector3.right * rightSensorPos.x;
        if (Physics.Raycast(rightStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.forward) * Quaternion.AngleAxis(sideSensorAngle * 2f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(rightStartPos, hit.point);
            sensorsNormalized[3] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[3] = -1f;
        }
        //back right fall sensor
        Vector3 backRightStartPos = transform.position;
        backRightStartPos += Vector3.forward * backRightSensorPos.z;
        backRightStartPos += Vector3.up * backRightSensorPos.y;
        backRightStartPos += Vector3.right * backRightSensorPos.x;
        if (Physics.Raycast(backRightStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(sideSensorAngle * 3f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(backRightStartPos, hit.point);
            sensorsNormalized[4] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[4] = -1f;
        }
        //back fall sensor
        Vector3 backStartPos = transform.position;
        backStartPos += Vector3.forward * backSensorPos.z;
        backStartPos += Vector3.up * backSensorPos.y;
        if (Physics.Raycast(backStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Vector3.back, out hit, fallSensorLength)){
            Debug.DrawLine(backStartPos, hit.point);
            sensorsNormalized[5] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[5] =-1f;
        }
        //back left fall sensor
        Vector3 backLeftStartPos = transform.position;
        backLeftStartPos += Vector3.forward * backLeftSensorPos.z;
        backLeftStartPos += Vector3.up * backLeftSensorPos.y;
        backLeftStartPos += Vector3.right * backLeftSensorPos.x;
        if (Physics.Raycast(backLeftStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(-sideSensorAngle * 3f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(backLeftStartPos, hit.point);
            sensorsNormalized[6] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[6] =-1f;
        }
        //left fall sensor
        Vector3 leftStartPos = transform.position;
        leftStartPos += Vector3.up * leftSensorPos.y;
        leftStartPos += Vector3.right * leftSensorPos.x;
        if (Physics.Raycast(leftStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.forward) * Quaternion.AngleAxis(-sideSensorAngle * 2f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(leftStartPos, hit.point);
            sensorsNormalized[7] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[7] =-1f;
        }
        //front left fall sensor
        Vector3 frontLeftStartPos = transform.position;
        frontLeftStartPos += Vector3.forward * frontLeftSensorPos.z;
        frontLeftStartPos += Vector3.up * frontLeftSensorPos.y;
        frontLeftStartPos += Vector3.right * frontLeftSensorPos.x;
        if (Physics.Raycast(frontLeftStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(-sideSensorAngle, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontLeftStartPos, hit.point);
            sensorsNormalized[8] = 1f - (hit.distance / fallSensorLength);
        }else {
            sensorsNormalized[8] =-1f;
        }

        //send the sensor inputs to the NN
        float[] outputs = net.FeedForward(sensorsNormalized);

        //move based on NN outputs
        Vector3 w = transform.forward * outputs[0]; //hope this works
        Vector3 d = transform.right * outputs[1];
        Vector3 s = -transform.forward * outputs[2];
        Vector3 a = -transform.right * outputs[3];

        //get sum of all movements
        inputForce = w + d + s + a;  //no idea if this is how to do this lol
        
        //apply speedFactor and create force 
        GetComponent<Rigidbody>().AddForce(inputForce * speedFactor);

        //update their fitness
        //have to prevent them from getting penalized when they fall off, else they all just play it safe.
        if (normalizedGoalDistance > 1){
            normalizedGoalDistance = 1;
        }

        net.AddFitness((1f-normalizedGoalDistance)* rewardBoost); //so closer to goal, more points they get, relative
        //net.AddFitness((1/goalDistance) * rewardBoost); //so closer to goal, more points they get, relative
    }
    public void Init(NeuralNetwork net, GameObject goal)
    {
        this.goal = goal;
        this.net = net;
        initialized = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "goal")
        {
            //no idea why this isn't working
            transform.position = startPos;
            // Debug.Log("x: " + startPos.x);
            // Debug.Log("y: " + startPos.y);
            // Debug.Log("z: " + startPos.z);
            Debug.Log(net + " made it to goal");
            //add to fitness
            net.AddFitness(goalReward); //idk, should test
        }
    }
    
}
