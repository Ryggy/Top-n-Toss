using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        pizzaVector = new Vector3(collision.transform.position.x, collision.transform.position.y);
        if (collision.gameObject.CompareTag("Pizza"))
        {
            Debug.Log("Pizza bounced");
            multiplierManager.multTotal = multiplierManager.multTotal + multiplierManager.multPerBounce;
            GameObject points = Instantiate(floatingPoints, pizzaVector, Quaternion.identity) as GameObject;
            points.transform.GetChild(0).GetComponent<TextMeshPro>().text = "x" + multiplierManager.multTotal.ToString();
        }
    }
}
