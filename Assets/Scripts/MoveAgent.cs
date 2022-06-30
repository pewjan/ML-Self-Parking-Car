using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveAgent : Agent
{

    [SerializeField] private float moveSpeed = 5000f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] public Transform[] Goals;
    [SerializeField] private Material original;
    [SerializeField] private GameObject[] ParkedCarsArr = new GameObject[5];
    public Vector3[] ParkedCarsPosition = new Vector3[5];
    public Quaternion[] ParkedCarsRotation = new Quaternion[5];
    public Quaternion reset = new Quaternion(0, 0, 0, 0);
    private Vector3 currentActivateParkPosition = new Vector3();
    private GameObject currentActivatePark;
    public Vector3 busPosition = new Vector3();
    public StatScript textScript;

    public Rigidbody car;

    Vector3 m_EulerAngleVelocity;

    public MeshRenderer Floor;
    float positiveDistantReward = 0f;
    private int fixedUpdateCount;
    private int currentAgentMaxStep;
    public bool parkingReached;
    public LayerMask m_LayerMask;
    bool m_Started;
    private int validIndexGlobal = 1;
    GameObject closestSmallGoal;
    public void FixedUpdate()
    {
        AddReward(-1 * (Vector3.Distance(busPosition, currentActivateParkPosition)) / 100);
        fixedUpdateCount++;
        busPosition = gameObject.transform.GetChild(0).transform.position;
        float distantReward = 0.0f;
        textScript.setText(distantReward, CompletedEpisodes, GetCumulativeReward());
        Vector3 dirFromAtoB = (currentActivateParkPosition - busPosition).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, gameObject.transform.GetChild(0).transform.forward);
        if (dotProd > 0.9)
        {
            AddReward(.1f / (Vector3.Distance(busPosition, currentActivateParkPosition)));
        }
    }

    public override void Initialize()
    {
        parkingReached = false;
        currentAgentMaxStep = MaxStep;
        fixedUpdateCount = 0;
        car = GetComponent<Rigidbody>();
        for (int i = 0; i < ParkedCarsArr.Length; i++)
        {
            ParkedCarsPosition[i] = ParkedCarsArr[i].transform.localPosition;
            ParkedCarsRotation[i] = ParkedCarsArr[i].transform.localRotation;
        }
    }
    public override void OnEpisodeBegin()
    {
        parkingReached = false;
        //alidIndexGlobal++;
        if (validIndexGlobal == 4)
        {
            validIndexGlobal = 0;
        }
        car.velocity = Vector3.zero;
        car.angularVelocity = Vector3.zero;


        bool spawnInfrontOfPark = false;

        spawnToRandomLocation(spawnInfrontOfPark);

        for (int i = 0; i < ParkedCarsArr.Length; i++)
        {
            ParkedCarsArr[i].transform.localPosition = ParkedCarsPosition[i];
            ParkedCarsArr[i].transform.localRotation = ParkedCarsRotation[i];
        }


        for (int i = 0; i < Goals.Length; i++)
        {
            if (i != validIndexGlobal)
            {
                Goals[i].gameObject.SetActive(false);
            }
            else
            {
                Goals[i].gameObject.SetActive(true);
                for (int j = 7; j < Goals[i].childCount; j++)
                {
                    Goals[i].GetChild(j).gameObject.SetActive(true);
                }

                currentActivateParkPosition = Goals[i].GetChild(1).transform.position;
                currentActivatePark = Goals[i].gameObject;


            }

        }



    }

    private void spawnToRandomLocation(bool spawnInfrontOfPark)
    {
        if (spawnInfrontOfPark != true)
        {
            transform.localPosition = new Vector3(-25.6f, -18.43606f, -18.98f);
            transform.localRotation = reset;
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {

        Vector3 dirFromAtoB = (currentActivateParkPosition - busPosition).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, gameObject.transform.GetChild(0).transform.forward);
        sensor.AddObservation(dotProd);
        sensor.AddObservation(busPosition.normalized);
        sensor.AddObservation(currentActivateParkPosition.normalized);
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("SmallGoal");

        if (gameObjects.Length == 0)
        {
            sensor.AddObservation(currentActivateParkPosition.normalized);
        }
        else
        {
            closestSmallGoal = FindClosestEnemy();
            sensor.AddObservation(closestSmallGoal.transform.position.normalized);
        }


    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("SmallGoal");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = busPosition;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        Debug.Log(distance);
        return closest;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        int Verticle = actions.DiscreteActions[0];
        int Horizontal = actions.DiscreteActions[1];

        //go forward
        if (Verticle == 1)
        {
            car.AddForce(transform.forward * moveSpeed * Time.deltaTime);
            //transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            //go left
            if (Horizontal == 1)
            {
                m_EulerAngleVelocity = new Vector3(0, -rotateSpeed, 0);
                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);

                car.MoveRotation(car.rotation * deltaRotation);

                //transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
            }
            //go right
            else if (Horizontal == 2)
            {
                m_EulerAngleVelocity = new Vector3(0, rotateSpeed, 0);
                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
                car.MoveRotation(car.rotation * deltaRotation);
                //transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            }

        }
        //go backwards
        else if (Verticle == 2)
        {
            car.AddForce(-transform.forward * moveSpeed * Time.deltaTime);
            //transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            if (Horizontal == 1)
            {
                m_EulerAngleVelocity = new Vector3(0, rotateSpeed, 0);
                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);

                car.MoveRotation(car.rotation * deltaRotation);
                //transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            }
            else if (Horizontal == 2)
            {
                m_EulerAngleVelocity = new Vector3(0, -rotateSpeed, 0);
                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);

                car.MoveRotation(car.rotation * deltaRotation);

                //transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
            }




        }




    }





    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var DiscreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W))
        {
            DiscreteActionsOut[0] = 1;
            if (Input.GetKey(KeyCode.A))
            {
                DiscreteActionsOut[1] = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                DiscreteActionsOut[1] = 2;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DiscreteActionsOut[0] = 2;
            if (Input.GetKey(KeyCode.A))
            {
                DiscreteActionsOut[1] = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                DiscreteActionsOut[1] = 2;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        onCollisionStayAndEnter(collision);
    }

    void OnCollisionStay(Collision collision)
    {

        onCollisionStayAndEnter(collision);
    }
    void onCollisionStayAndEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            AddReward(-30.0f);

        }
        else if (collision.gameObject.name == "Side")
        {
            AddReward(-30.0f);
        }
        else if (collision.gameObject.name == "ParkedCars")
        {
            AddReward(-30.0f);
        }
        else if (collision.gameObject.name == "SmallGoal")
        {
            Destroy(collision.gameObject);
            AddReward(+5f);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SmallGoal")
        {
            other.gameObject.SetActive(false);
            AddReward(+10f);
        }

    }
    public void Parked()
    {
        successParkColor();
        AddReward(+30);


    }

    void successParkColor()
    {
        for (int i = 0; i < 4; i++)
        {
            currentActivatePark.transform.GetChild(i + 2).GetComponent<Renderer>().material.color = Color.green;
        }
        StartCoroutine(WaitColor());

    }
    private IEnumerator WaitColor()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 4; i++)
        {
            currentActivatePark.transform.GetChild(i + 2).GetComponent<Renderer>().material.color = Color.red;
        }
        EndEpisode();

    }
}
