using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementScriptBackup : MonoBehaviour
{
    public float enemySpeed = 3.0f;
    public float changeDirectionTime = 3.0f;
    public float rotationSpeed = 3.0f;

    public float[,] directionSettings =
    {
        {1.0f, 0.0f},
        {0.0f, -1.0f},
        {-1.0f, 0.0f},
        {0.0f, 1.0f}
    };

    public int currDir = 0;

    private float directionTimer = 0.0f;

    public bool isRotating = false;

    private float originalRotation = 0.0f;

    //vert movement
    //hor movement
    private float horizontalDir = 1.0f;
    private float verticalDir = 0.0f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(directionSettings[currDir , 0], 0, directionSettings[currDir , 1]) * enemySpeed;

        if (directionTimer >= changeDirectionTime)
        {
            directionTimer = 0;
            Debug.Log("add code here for changing direction");
            SetDirection();
        }

        directionTimer += Time.deltaTime;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(originalRotation + 90, Vector3.up), Time.deltaTime * rotationSpeed);
    }

    void FixedUpdate()
    {
       
    }

    private void SetDirection()
    {
        currDir++;

        if(currDir >= directionSettings.GetLength(0))
        {
            currDir = 0;
        }
    }
}