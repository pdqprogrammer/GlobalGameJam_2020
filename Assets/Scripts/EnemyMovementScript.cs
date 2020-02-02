using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GameObject))]
public class EnemyMovementScript : MonoBehaviour
{
    public float enemySpeed = 3.0f;
    public float turnSpeed = 1.0f;
    private float defaultY;

    public bool lockRotate = false;

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

        //transform.LookAt(MoveTowardObject.transform);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveTowardObject.transform.position.normalized), 0.1f);

        Vector3 relativePos = MoveTowardObject.transform.position - transform.position;
        
        if(lockRotate)
            relativePos.y = transform.position.y;

        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("PullableOBject"))
        {
            currMovePoint--;

            if (currMovePoint < 0)
                currMovePoint = MovePoints.Length - 1;

            MoveTowardObject = MovePoints[currMovePoint];
        }
    }
}