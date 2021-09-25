using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorRays : MonoBehaviour
{
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

    // public float[] sensorsNormalized; // all the normalized values of distance to X, starting with goal and front and going clockwise
    // public float[] nnInputs; //the same as sensorsNormalized
    //ah, arrays can't be resized... copy needed? basically redundant...
    
    
    //goal
    // [SerializeField] GameObject goal;
    // private Transform goalPos = goal.transform;
    public Transform goal;
    private float startGoalDistance;
    

    // Start is called before the first frame update
    void Start()
    {
        startGoalDistance = Vector3.Distance(transform.position, goal.position);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckSensors();
    }

    private void CheckSensors()
    {
        //to avoid rotation issue, changing all transform.forward/up/right to Vector3.etc
        //now issue is that it's always facing the z direction... should change based on input forward dir...
        //trying to replace with inputForce now.
        //nope, okay, nvm, just gonna surround the ball with sensors lol.
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        // Vector3 sensorDirection = inputForce;

        // sensorStartPos += Vector3.forward * frontSensorPos.z;
        // sensorStartPos += Vector3.up * frontSensorPos.y;
        // Debug.Log(sensorStartPos);

        //reset sensor array
        float[] sensorsNormalized = new float[9];

        //ray to goal
        float goalDistance = Vector3.Distance(transform.position, goal.position);
        float normalizedGoalDistance = goalDistance / startGoalDistance;
        Vector3 goalDirection = (goal.position - transform.position);
        if(Physics.Raycast(sensorStartPos, goalDirection, out hit, 500f)){
            Debug.DrawLine(sensorStartPos, hit.point);
            // Debug.Log(normalizedGoalDistance);
            sensorsNormalized[0] = normalizedGoalDistance;
        } else {
            Debug.Log("where the hell are you? Did you fall off?");
        }

        //front center fall sensor
        Vector3 frontStartPos = transform.position;
        frontStartPos += Vector3.forward * frontSensorPos.z;
        frontStartPos += Vector3.up * frontSensorPos.y;
        if (Physics.Raycast(frontStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontStartPos, hit.point);
            sensorsNormalized[1] = hit.distance / fallSensorLength;
        } else {
            sensorsNormalized[1] = 1;
        }
        //front right fall sensor
        Vector3 frontRightStartPos = transform.position;
        frontRightStartPos += Vector3.forward * frontRightSensorPos.z;
        frontRightStartPos += Vector3.up * frontRightSensorPos.y;
        frontRightStartPos += Vector3.right * frontRightSensorPos.x;
        if (Physics.Raycast(frontRightStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(sideSensorAngle, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontRightStartPos, hit.point);
            sensorsNormalized[2] = hit.distance / fallSensorLength;
        } else {
            sensorsNormalized[2] = 1;
        }
        //right fall sensor
        Vector3 rightStartPos = transform.position;
        rightStartPos += Vector3.up * rightSensorPos.y;
        rightStartPos += Vector3.right * rightSensorPos.x;
        if (Physics.Raycast(rightStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.forward) * Quaternion.AngleAxis(sideSensorAngle * 2f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(rightStartPos, hit.point);
            sensorsNormalized[3] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[3] = 1;
        }
        //back right fall sensor
        Vector3 backRightStartPos = transform.position;
        backRightStartPos += Vector3.forward * backRightSensorPos.z;
        backRightStartPos += Vector3.up * backRightSensorPos.y;
        backRightStartPos += Vector3.right * backRightSensorPos.x;
        if (Physics.Raycast(backRightStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(sideSensorAngle * 3f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(backRightStartPos, hit.point);
            sensorsNormalized[4] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[4] = 1;
        }
        //back fall sensor
        Vector3 backStartPos = transform.position;
        backStartPos += Vector3.forward * backSensorPos.z;
        backStartPos += Vector3.up * backSensorPos.y;
        if (Physics.Raycast(backStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Vector3.back, out hit, fallSensorLength)){
            Debug.DrawLine(backStartPos, hit.point);
            sensorsNormalized[5] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[5] = 1;
        }
        //back left fall sensor
        Vector3 backLeftStartPos = transform.position;
        backLeftStartPos += Vector3.forward * backLeftSensorPos.z;
        backLeftStartPos += Vector3.up * backLeftSensorPos.y;
        backLeftStartPos += Vector3.right * backLeftSensorPos.x;
        if (Physics.Raycast(backLeftStartPos, Quaternion.AngleAxis(-fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(-sideSensorAngle * 3f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(backLeftStartPos, hit.point);
            sensorsNormalized[6] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[6] = 1;
        }
        //left fall sensor
        Vector3 leftStartPos = transform.position;
        leftStartPos += Vector3.up * leftSensorPos.y;
        leftStartPos += Vector3.right * leftSensorPos.x;
        if (Physics.Raycast(leftStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.forward) * Quaternion.AngleAxis(-sideSensorAngle * 2f, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(leftStartPos, hit.point);
            sensorsNormalized[7] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[7] = 1;
        }
        //front left fall sensor
        Vector3 frontLeftStartPos = transform.position;
        frontLeftStartPos += Vector3.forward * frontLeftSensorPos.z;
        frontLeftStartPos += Vector3.up * frontLeftSensorPos.y;
        frontLeftStartPos += Vector3.right * frontLeftSensorPos.x;
        if (Physics.Raycast(frontLeftStartPos, Quaternion.AngleAxis(fallSensorAngle, Vector3.right) * Quaternion.AngleAxis(-sideSensorAngle, Vector3.up) * Vector3.forward, out hit, fallSensorLength)){
            Debug.DrawLine(frontLeftStartPos, hit.point);
            sensorsNormalized[8] = hit.distance / fallSensorLength;
        }else {
            sensorsNormalized[8] = 1;
        }

        Debug.Log("0: " + sensorsNormalized[0]);
        Debug.Log("1: " + sensorsNormalized[1]);
        Debug.Log("2: " + sensorsNormalized[2]);
        Debug.Log("3: " + sensorsNormalized[3]);
        Debug.Log("4: " + sensorsNormalized[4]);
        Debug.Log("5: " + sensorsNormalized[5]);
        Debug.Log("6: " + sensorsNormalized[6]);
        Debug.Log("7: " + sensorsNormalized[7]);
        Debug.Log("8: " + sensorsNormalized[8]);


    }
}
