using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public float distanceFromNode;

    [Header("Sensors")]
    public float sensorLength = 3f;
    public float frontSensorPos = 0.5f;
    public float frontsideSensorPos = 0.2f;

    private List<Transform> nodes;
    private int currentNode = 0;

    private void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if(pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos.z += frontSensorPos;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);


        //front center sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        //front right sensor
        sensorStartPos.x += frontsideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);

        //front left sensor
        sensorStartPos.y -= 2 * frontsideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);


        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point, Color.green);

    }

    private void ApplySteer()
    {
        Vector3 relativeVector = this.transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        wheelFL.motorTorque = 1500f;
        wheelFR.motorTorque = 1500f;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(this.transform.position, nodes[currentNode].position) < distanceFromNode)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

}

