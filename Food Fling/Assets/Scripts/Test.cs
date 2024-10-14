using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // turn off the stack trace in the debug messages to make it easier to read
        Application.SetStackTraceLogType(LogType.Log,StackTraceLogType.ScriptOnly);
        
        var gameState = new GameState(GameStates.Progression);
        gameState.SwitchState(GameStates.Delivery);
        gameState.SwitchState(GameStates.FoodPrep);
    }


}
