using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    private Vector3 pizzaVector;

    public MultiplierManager multiplierManager;

    public GameObject floatingPoints;

    // Start is called before the first frame update
    void Start()
    {
        multiplierManager = FindObjectOfType<MultiplierManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        pizzaVector = new Vector3(collision.transform.position.x, collision.transform.position.y);

        if (collision.gameObject.name == "Pizza")
        {
            Debug.Log("Pizza bounced");
            multiplierManager.multTotal = multiplierManager.multTotal + multiplierManager.multPerBounce;
            Instantiate(floatingPoints, transform.position, Quaternion.identity);
        }
    }
}
