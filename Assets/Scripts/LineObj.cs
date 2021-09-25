using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObj : MonoBehaviour
{
    private LineRenderer line;

    public float lineDrawSpeed = 6f;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        //lineRenderer.SetPosition(0, new Vector3(-1f, 0.3f, -0.7f));
        // //lineRenderer.SetWidth(0.45f, 0.45f);
        // lineRenderer.startWidth =0.1f;
        // lineRenderer.endWidth = 0.1f;

        // lineRenderer.positionCount = 3;
        // lineRenderer.useWorldSpace = false;
        // //dist = Vector3.Distance(origin.position, destination.position);
        
        // Vector3 pointZero = new Vector3( origin.position.x, origin.position.y, origin.position.z ); //transform.position.x, transform.position.y, transform.z);
        // Debug.Log(origin.position.x + origin.position.y + " " + origin.position.z);
        // Vector3 pointA = new Vector3(-1f, -0.1f, -0.7f);
        // Vector3 pointB = new Vector3(1f, -0.1f, -0.7f);
        // lineRenderer.SetPosition(0, pointA);
        // lineRenderer.SetPosition(1, pointB);
        // //lineRenderer.SetPosition(2, pointA);
        //drawCircleX(0.1f,0.012f); //, transform);
        //Debug.Log(transform.position.x + " " + transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        // if (counter <dist)
        // {
        //     counter += .1f/lineDrawSpeed;
        //     float x = Mathf.Lerp(0,dist,counter)
        // }
        
    }

   // public void DrawCircle(this GameObject container, float radius, float lineWidth)
    public void drawCircleX(float radius, float lineWidth) //, Transform origin)
    {
        Debug.Log("draw");
        var segments = 60;
        //var line = container.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius  ,  Mathf.Cos(rad) * radius, -0.43f);
        }

        line.SetPositions(points);
    }
}
