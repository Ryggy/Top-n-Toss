using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slingshot : MonoBehaviour
{

    public Vector3 Initial_Position;
    public int Pizza_Speed;
    public string SceneName;

    public void Awake()
    {
            Initial_Position = transform.position;
    }

    public void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnMouseUp()
    {
        Vector2 Spring_force = Initial_Position - transform.position;
        GetComponent<SpriteRenderer>().color = Color.yellow;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<Rigidbody2D>().AddForce(Pizza_Speed * Spring_force);
    }

    public void OnMouseDrag()
    {
        //pizza drags backward
        // moves direction opposite to drag

        Vector3 DragPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        transform.position = new Vector3(DragPosition.x, DragPosition.y);   
    }

    // Update is called once per frame
    void Update()
    {
       // has pizza hit customer 
       // if yes then add points reference to point system
    }
}
