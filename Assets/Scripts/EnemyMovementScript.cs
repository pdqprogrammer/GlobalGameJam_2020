using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GameObject))]
public class EnemyMovementScript : MonoBehaviour
{
    public float enemySpeed = 3.0f;
    public float changeDirectionTime = 3.0f;
    public float rotationSpeed = 3.0f;

    public bool isRotating = false;

    private float defaultY;

    public GameObject[] MovePoints;

    GameObject MoveTowardObject;

    int currMovePoint;

    // Start is called before the first frame update
    void Start()
    {
        currMovePoint = 0;
        MoveTowardObject = MovePoints[0];
        defaultY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementVector = Vector3.MoveTowards(transform.position, MoveTowardObject.transform.position, Time.deltaTime * enemySpeed);
        movementVector.y = defaultY;

        transform.position = movementVector;
    }

    void FixedUpdate()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == MovePoints[currMovePoint])
        {
            currMovePoint++;

            if (currMovePoint >= MovePoints.Length)
                currMovePoint = 0;

            MoveTowardObject = MovePoints[currMovePoint];
        }
    }
}