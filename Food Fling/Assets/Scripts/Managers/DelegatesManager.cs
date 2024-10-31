using System;
using UnityEngine;

public class DelegatesManager : MonoBehaviour
{
    public static DelegatesManager Instance;

    public ICustomerEventHandler CustomerEventHandler { get; private set; }
    public IOrderEventHandler OrderEventHandler { get; private set; }
    public IPizzaEventHandler PizzaEventHandler { get; private set; }
    public IProgressionEventHandler ProgressionEventHandler { get; private set; } = new ProgressionEventHandler();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize the handlers
            CustomerEventHandler = new CustomerEventHandler();
            OrderEventHandler = new OrderEventHandler();
            ProgressionEventHandler = new ProgressionEventHandler();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public event Action OnGameStart;
    public event Action OnGameEnd;
    
    // wrapper for calling game events
    
    // Customer-related events
    public void TriggerOnCustomerArrives(CustomerData customer) => CustomerEventHandler.CustomerArrives(customer);
    public void TriggerOnCustomerLeaves(CustomerData customer) => CustomerEventHandler.CustomerLeaves(customer);
    public void TriggerOnCustomerSatisfactionChange(CustomerData customer) => CustomerEventHandler.CustomerSatisfactionChange(customer);
    public void TriggerOnCustomerPatienceChange(CustomerData customer) => CustomerEventHandler.CustomerPatienceChange(customer);
   
    // Order-related events
    public void TriggerOnOrderGenerated(OrderData order) => OrderEventHandler.OrderGenerated(order);
    public void TriggerOnOrderSubmitted(OrderData order, Pizza pizza) => OrderEventHandler.OrderSubmitted(order, pizza);
    public void TriggerOnOrderCompleted(OrderData order, bool success) => OrderEventHandler.OrderCompleted(order, success);
    
    // Progress-related events
    public void TriggerOnMoneyUpdated(int moneyIncrement) => ProgressionEventHandler.MoneyUpdated(moneyIncrement);
    public void TriggerOnLevelUp(int level) => ProgressionEventHandler.LevelUp(level);
    public void TriggerOnUpgradeUnlocked(string upgrade) =>ProgressionEventHandler.UpgradeUnlocked(upgrade);
    public void TriggerOnIngredientUnlocked(string ingredient) =>ProgressionEventHandler.IngredientUnlocked(ingredient);
    
    // Serialization-related events
    public void TriggerOnGameStart() => OnGameStart?.Invoke();
    public void TriggerOnGameEnd() => OnGameEnd?.Invoke();
    
}
