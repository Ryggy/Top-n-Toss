using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CustomerManager : MonoBehaviour
{
    // Singleton
    private static CustomerManager _instance = null;
    public static CustomerManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // Try to find an existing instance in the scene
            _instance = FindObjectOfType<CustomerManager>();

            if (_instance == null)
            {
                Debug.LogError("No CustomerManager found in the scene!");
            }
            return _instance;
        }
    }
    
    [Header("Customer Pool Settings")]
    [SerializeField] private int poolSize = 3;
    [SerializeField] private Vector3 offScreenPosition = new Vector3(-5,2,0);
    [SerializeField] private Vector2 arrivalDelayRange = new Vector2(2f, 5f);
    public GameObject[] customers;
    
    private Queue<Customer> customerPool = new Queue<Customer>();
    private Dictionary<int, Customer> activeCustomers = new Dictionary<int, Customer>(); // Customers currently on-screen
    private Dictionary<int, Customer> allCustomers = new Dictionary<int, Customer>();   // All customers ever created
    public Dictionary<int, Customer> Customers => allCustomers;

    public int maxCustomers = 3;
    [SerializeField] private List<Transform> onScreenPositions = new List<Transform>(); // List of available positions
    private List<Transform> availablePositions = new List<Transform>();
    private Dictionary<Customer, Transform> customerPositions = new Dictionary<Customer, Transform>();
    private Coroutine spawnCoroutine;
    private void Awake()
    {
        if (onScreenPositions.Count < maxCustomers)
        {
            Debug.LogError("Not enough on-screen positions for the specified maximum customers!");
        }
        
        InitialiseAllCustomers();
        ResetAvailablePositions();
    }

    private void InitialiseAllCustomers()
    {
        foreach (GameObject obj in customers)
        {
            Customer customer = obj.GetComponent<Customer>();
            if (customer != null)
            {
                int customerID = customer.Data.CustomerID;
                if (!allCustomers.ContainsKey(customerID))
                {
                    allCustomers.Add(customerID, customer);
                    customerPool.Enqueue(customer);
                    obj.SetActive(false); // Ensure they are disabled initially
                }
                else
                {
                    Debug.LogWarning($"Duplicate Customer ID detected: {customerID}. Skipping.");
                }
            }
            else
            {
                Debug.LogError($"GameObject {obj.name} does not have a Customer component attached!");
            }
        }
    }

    private void Start()
    {
        // Start by spawning customers to fill the initial on-screen capacity
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(CheckAndSpawnCustomers());
        }
    }
    
    private void ResetAvailablePositions()
    {
        availablePositions.Clear();
        availablePositions.AddRange(onScreenPositions);
    }
    
    /// <summary>
    /// Spawns a customer from the pool and moves them on-screen.
    /// </summary>
    public void SpawnCustomer()
    {
        if (activeCustomers.Count < maxCustomers && customerPool.Count > 0 && availablePositions.Count > 0)
        {
            Customer customer = customerPool.Dequeue();
            customer.gameObject.SetActive(true);

            // Choose a random available position
            Transform targetPosition = availablePositions[Random.Range(0, availablePositions.Count)];
            availablePositions.Remove(targetPosition);
            
            // Track assigned position
            customerPositions[customer] = targetPosition;
            
            // Add to activeCustomers
            activeCustomers[customer.Data.CustomerID] = customer;

            StartCoroutine(customer.MoveToPosition(targetPosition.position, () =>
            {
                OrderManager.Instance.TakeCustomerOrder(customer);
            }));
        }
    }
    
    /// <summary>
    /// Checks and spawns customers to maintain the desired number on-screen.
    /// </summary>
    private IEnumerator CheckAndSpawnCustomers()
    {
        while (activeCustomers.Count < maxCustomers && customerPool.Count > 0 && availablePositions.Count > 0)
        {
            float delay = Random.Range(arrivalDelayRange.x, arrivalDelayRange.y);
            yield return new WaitForSeconds(delay);

            SpawnCustomer();
        }

        // Reset the coroutine reference when finished
        spawnCoroutine = null;
    }
    
    /// <summary>
    /// Handles the customer leaving after completing an order.
    /// </summary>
    /// <param name="customer">The customer to remove.</param>
    public void RemoveCustomer(Customer customer)
    {
        if (!activeCustomers.ContainsKey(customer.Data.CustomerID)) return;

        StartCoroutine(customer.MoveToPosition(offScreenPosition, () =>
        {
            // Remove from activeCustomers and return to pool
            activeCustomers.Remove(customer.Data.CustomerID);
            customer.gameObject.SetActive(false);
            customerPool.Enqueue(customer);
            
            // Free the assigned position
            if (customerPositions.TryGetValue(customer, out Transform positionToFree))
            {
                availablePositions.Add(positionToFree);
                customerPositions.Remove(customer);
            }
            
            // Start the spawn coroutine to check conditions
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(CheckAndSpawnCustomers());
            }
        }));
    }
    
    public Customer GetCustomerByID(int customerID)
    {
        if (allCustomers.TryGetValue(customerID, out var customer))
        {
            return customer;
        }

        Debug.LogWarning($"Customer with ID {customerID} not found in CustomerManager.");
        return null;
    }
}
