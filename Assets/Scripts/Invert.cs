using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invert : MonoBehaviour
{
    bool inverted = false;
    bool rotated = false;
    Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float z_rotation;
        if (Input.GetKeyDown (KeyCode.A)) {
            if (rotated)
                z_rotation = Mathf.PI; // 180 degrees
            else
                z_rotation = 0; // 0 degrees

            transform.rotation = Quaternion.EulerAngles (0, 0, z_rotation);
            rotated= !rotated;
        }
        float x_invert;
        if (Input.GetKeyDown (KeyCode.S)) {
            if (inverted)
                x_invert = -1.0f; // 180 degrees
            else
                x_invert = 1.0f; // 0 degrees

            inverted= !inverted;
        }
        oldPos = transform.position;
        oldPos[0] = oldPos[0] * -1.0f;

        transform.position = oldPos;

        
    }
}
