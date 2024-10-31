using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerUI : MonoBehaviour
{
    private void OnEnable()
    {
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerPatienceChange += HandlePatienceChange;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerSatisfactionChange += HandleSatisfactionChange;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerPatienceChange -= HandlePatienceChange;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerSatisfactionChange -= HandleSatisfactionChange;
    }
    
    private void HandlePatienceChange(CustomerData obj)
    {
       
    }

    private void HandleSatisfactionChange(CustomerData obj)
    {
       
    }
}
