using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hourhand : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        mouseRotation();
    }

    void mouseRotation()
    {
        //sets the mouse position as a vector 2
        Vector2 mouse_pos = Input.mousePosition;

        //sets the camera position as a vector 2
        Vector2 object_pos = Camera.main.WorldToScreenPoint(transform.localPosition);

        //gets the offset of the mouse from the camera
        Vector2 offset = new Vector2(mouse_pos.x - object_pos.x, mouse_pos.y - object_pos.y);

        //gets the tan of the offset and sets it to degrees
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        //sets the rotation of the object to the angle on the z axis
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
