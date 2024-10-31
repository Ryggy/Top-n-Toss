using System;
using UnityEngine;

public class DelegatesManager : MonoBehaviour
{
    public static DelegatesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Customer-related events
    public event Action<Customer> OnCustomerArrives;
    public event Action<Customer> OnCustomerLeaves;
    public event Action<Customer, bool> OnOrderCompleted;
    public event Action<Customer, int> OnCustomerSatisfactionChange;
    public event Action<float> OnCustomerPatienceChange;
    // Order-related events
    public event Action<Order> OnOrderGenerated;
    public event Action<int, Pizza> OnOrderSubmitted;

    // Player-related events
    public event Action<int> OnPlayerLevelUp;
    public event Action<int, int> OnPlayerScoreUpdated;

    // Game progression events
    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action<float> OnGameProgressUpdated;
    

    // Methods to trigger events
    public void TriggerOnCustomerArrives(Customer customer) => OnCustomerArrives?.Invoke(customer);
    public void TriggerOnCustomerLeaves(Customer customer) => OnCustomerLeaves?.Invoke(customer);
    public void TriggerOnOrderCompleted(Customer customer, bool success) => OnOrderCompleted?.Invoke(customer, success);
    public void TriggerOnCustomerSatisfactionChange(Customer customer, int satisfaction) => OnCustomerSatisfactionChange?.Invoke(customer, satisfaction);
    public void TriggerOnCustomerPatienceChange(float newPatience) => OnCustomerPatienceChange?.Invoke(newPatience);
    public void TriggerOnOrderGenerated(Order order) => OnOrderGenerated?.Invoke(order);
    public void TriggerOnOrderSubmitted(int orderID, Pizza pizza) => OnOrderSubmitted?.Invoke(orderID, pizza);
    public void TriggerOnPlayerLevelUp(int level) => OnPlayerLevelUp?.Invoke(level);
    public void TriggerOnPlayerScoreUpdated(int newScore, int scoreIncrement) => OnPlayerScoreUpdated?.Invoke(newScore, scoreIncrement);
    public void TriggerOnGameStart() => OnGameStart?.Invoke();
    public void TriggerOnGameEnd() => OnGameEnd?.Invoke();
    public void TriggerOnGameProgressUpdated(float progress) => OnGameProgressUpdated?.Invoke(progress);
}
