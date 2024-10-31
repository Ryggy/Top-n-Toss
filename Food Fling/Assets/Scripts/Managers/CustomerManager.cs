using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private Dictionary<int, Customer> _customers = new Dictionary<int, Customer>();
    public Dictionary<int, Customer> Customers => _customers;
    public void AddCustomer(Customer customer)
    {
        if (!_customers.ContainsKey(customer.Data.CustomerID))
        {
            _customers.Add(customer.Data.CustomerID, customer);
            Debug.Log($"Customer {customer.Data.CustomerName} with ID {customer.Data.CustomerID} added to CustomerManager.");
        }
        else
        {
            Debug.LogWarning($"Customer with ID {customer.Data.CustomerID} already exists in CustomerManager.");
        }
    }
    
    public void RemoveCustomer(int customerID)
    {
        if (_customers.ContainsKey(customerID))
        {
            _customers.Remove(customerID);
            Debug.Log($"Customer with ID {customerID} removed from CustomerManager.");
        }
        else
        {
            Debug.LogWarning($"Customer with ID {customerID} does not exist in CustomerManager.");
        }
    }
    
    public Customer GetCustomerByID(int customerID)
    {
        if (_customers.TryGetValue(customerID, out var customer))
        {
            return customer;
        }

        Debug.LogWarning($"Customer with ID {customerID} not found in CustomerManager.");
        return null;
    }
}
