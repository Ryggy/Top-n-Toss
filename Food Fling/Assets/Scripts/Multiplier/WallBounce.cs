using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{

    public MultiplierManager multiplierManager;

    // Start is called before the first frame update
    void Start()
    {
        multiplierManager = FindObjectOfType<MultiplierManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Pizza")
        {
            Debug.Log("Pizza bounced");
            multiplierManager.multTotal = multiplierManager.multTotal + multiplierManager.multPerBounce;
        }
    }
}
