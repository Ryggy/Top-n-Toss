using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterSlingshot : MonoBehaviour
{
    public float power = 10f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    public TrajectoryLine tl;

    Camera cam;
    Vector2 force;
    Vector3 startpoint;
    Vector3 endpoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;   
        tl = GetComponent<TrajectoryLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startpoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startpoint.z = 15;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            currentPoint.z = 15;
            tl.RenderLine(startpoint, currentPoint); 
        }

        if(Input.GetMouseButtonUp(0))
        {
            endpoint = cam.ScreenToViewportPoint(Input.mousePosition);
            endpoint.z = 15;

            force = new Vector2(Mathf.Clamp(startpoint.x - endpoint.x, minPower.x, maxPower.x), Mathf.Clamp(startpoint.y - endpoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power, ForceMode2D.Impulse);
        }
    }
}
