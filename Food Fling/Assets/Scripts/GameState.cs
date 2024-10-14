using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    // A reference to the current state 
    private State _currentState = null;
    private readonly Dictionary<GameStates, State> _stateDictionary = new Dictionary<GameStates, State>();
    public GameState(GameStates state)
    {
        _stateDictionary.Add(GameStates.Progression, new ProgressionState());
        _stateDictionary.Add(GameStates.FoodPrep, new FoodPrepState());
        _stateDictionary.Add(GameStates.Delivery, new DeliveryState());
        
        SwitchState(state);
    }

    public void Update()
    {
        _currentState.UpdateState(this);
    }

    private void SwitchState(State newState)
    {
        if (newState == _currentState || newState == null)
        {
            return;
        }
        Debug.Log($"GameState: Transition to {newState.GetType().Name}.");
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void SwitchState(GameStates newState)
    {
        var state = _stateDictionary[newState];
        
        SwitchState(state);
    }
    
    // Example of methods to switch input maps and HUDs (placeholder methods)
    public void SwitchInputMap(string inputMapName)
    {
        // Switches the input system to a new input map (e.g., Food Prep vs Delivery)
        Debug.Log($"Switching input map to {inputMapName}");
    }

    public void SwitchHUD(string hudType)
    {
        // Switches the HUD based on game state (e.g., Food Prep HUD or Delivery HUD)
        Debug.Log($"Switching HUD to {hudType}");
    }
}

public abstract class State
{ 
  protected GameState _context;

  protected void SetContext(GameState context)
  {
      _context = context;
  }

  public abstract void EnterState(GameState context);
  public abstract void ExitState(GameState context);
  public abstract void UpdateState(GameState context);
}

public enum GameStates
{
    Progression,
    FoodPrep,
    Delivery
}
public class FoodPrepState : State
{
    public override void EnterState(GameState context)
    {
        SetContext(context);
        
        // Switch to Food Prep input map
        _context.SwitchInputMap("FoodPrepInput");

        // Update HUD to Food Prep UI
        _context.SwitchHUD("FoodPrepHUD");
    }

    public override void ExitState(GameState context)
    {
        Debug.Log("Exiting Food Prep State");
        // Any necessary cleanup when leaving the state
    }
    
    public override void UpdateState(GameState context)
    {
        
    }
}

public class DeliveryState : State
{
    public override void EnterState(GameState context)
    {
        SetContext(context);
        
        // Switch to Delivery input map
        _context.SwitchInputMap("DeliveryInput");

        // Update HUD to Delivery UI
        _context.SwitchHUD("DeliveryHUD");
    }

    public override void ExitState(GameState context)
    {
        Debug.Log("Exiting Delivery State");
        // Any necessary cleanup when leaving the state
    }
    
    public override void UpdateState(GameState context)
    {
        
    }
}

public class ProgressionState : State
{
    public override void EnterState(GameState context)
    {
        SetContext(context);
        
        // Switch to Progression input map
        _context.SwitchInputMap("ProgressionInput");

        // Update HUD to Progression UI (e.g., menu for unlocks, level selection)
        _context.SwitchHUD("ProgressionHUD");
    }

    public override void ExitState(GameState context)
    {
        Debug.Log("Exiting Progression State");
        // Any necessary cleanup when leaving the state
    }
    public override void UpdateState(GameState context)
    {
        
    }
}