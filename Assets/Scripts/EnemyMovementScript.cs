using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    public float enemySpeed = 3.0f;
    public float changeDirectionTime = 3.0f;
    public float rotationSpeed = 3.0f;

    private float directionTimer = 0.0f;

    public bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(directionTimer >= changeDirectionTime)
        {
            directionTimer = 0;
            Debug.Log("add code here for changing direction");

        }

        if (!isRotating)
        {
            directionTimer += Time.deltaTime;
            transform.position += Vector3.forward * enemySpeed * Time.deltaTime;
        } 
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(90, Vector3.up), Time.deltaTime * rotationSpeed);
        }
    }

    void FixedUpdate()
    {
       
    }
}
