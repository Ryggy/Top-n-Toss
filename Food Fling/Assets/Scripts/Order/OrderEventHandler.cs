using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEventHandler : IOrderEventHandler
{
    public event Action<OrderData> OnOrderGenerated;
    public event Action<OrderData, Pizza> OnOrderSubmitted;
    public event Action<OrderData, bool> OnOrderCompleted;
    
    public void OrderGenerated(OrderData order)
    {
        OnOrderGenerated?.Invoke(order);
    }

    public void OrderSubmitted(OrderData order, Pizza pizza)
    {
        OnOrderSubmitted?.Invoke(order, pizza);
    }

    public void OrderCompleted(OrderData order, bool success)
    {
        OnOrderCompleted?.Invoke(order, success);
    }
}

public interface IOrderEventHandler
{
    event Action<OrderData> OnOrderGenerated;
    event Action<OrderData, Pizza> OnOrderSubmitted;
    event Action<OrderData, bool> OnOrderCompleted;
    void OrderGenerated(OrderData order);
    void OrderSubmitted(OrderData order, Pizza pizza);
    void OrderCompleted(OrderData order, bool success);
}