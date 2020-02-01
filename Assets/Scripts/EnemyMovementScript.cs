using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class EnemyMovementScript : MonoBehaviour
{
    public float enemySpeed = 3.0f;
    public float changeDirectionTime = 3.0f;

    //use for controlling enemy movement direction
    private float enemyVertDirection = 0.0f;
    private float enemyHoriDirection = 0.0f;

    private Rigidbody rb;

    private float directionTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        directionTimer += Time.deltaTime;

        if(directionTimer >= changeDirectionTime)
        {
            directionTimer = 0;
            Debug.Log("add code here for changing direction");
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * enemySpeed, 0, Input.GetAxis("Vertical") * enemySpeed);
    }

    private void ChangeDirection()
    {
        //code to change enemy movement direction
    }
}
