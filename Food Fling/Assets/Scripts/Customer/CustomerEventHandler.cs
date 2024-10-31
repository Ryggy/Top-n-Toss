using System;
using UnityEngine;

public class CustomerEventHandler :  ICustomerEventHandler
{
    public event Action<CustomerData> OnCustomerArrives; 
    public event Action<CustomerData> OnCustomerLeaves; 
    public event Action<CustomerData> OnCustomerSatisfactionChange; 
    public event Action<CustomerData> OnCustomerPatienceChange;
    
    public void CustomerArrives(CustomerData customer)
    {
        OnCustomerArrives?.Invoke(customer);
    }

    public void CustomerLeaves(CustomerData customer)
    {
        OnCustomerLeaves?.Invoke(customer);
    }
    
    public void CustomerSatisfactionChange(CustomerData customer)
    {
        OnCustomerSatisfactionChange?.Invoke(customer);
    }

    public void CustomerPatienceChange(CustomerData customer)
    {
        OnCustomerPatienceChange?.Invoke(customer);
    }
}

public interface ICustomerEventHandler
{
    event Action<CustomerData> OnCustomerArrives; 
    event Action<CustomerData> OnCustomerLeaves; 
    event Action<CustomerData> OnCustomerSatisfactionChange; 
    event Action<CustomerData> OnCustomerPatienceChange; 
    void CustomerArrives(CustomerData customer);
    void CustomerLeaves(CustomerData customer);
    void CustomerSatisfactionChange(CustomerData customer);
    void CustomerPatienceChange(CustomerData customer);
}
