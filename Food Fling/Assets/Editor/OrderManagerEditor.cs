using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OrderManager))]
public class OrderManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector to keep other properties visible
        DrawDefaultInspector();

        // Get a reference to the order manager script
        OrderManager orderManager = (OrderManager)target;

        // Add a button to check ingredient compatibility
        if (GUILayout.Button("Take Customer Order"))
        {
            orderManager.TakeCustomerOrder(OrderManager.Instance.debugCustomer);
            Debug.Log($"Taking {OrderManager.Instance.debugCustomer.name}'s order");
        }
    }
}
