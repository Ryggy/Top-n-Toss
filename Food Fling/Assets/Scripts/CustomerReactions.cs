using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerReactions : MonoBehaviour
{
    public List<string> horribleReactions = new List<string>()
    {
        "Bleh",
        "Horrible",
        "It's awful"
    };
    public List<string> badReactions = new List<string>()
    {
        "No good",
        "Not Paying",
        "Not what I ordered"
    };
    public List<string> okayReactions = new List<string>()
    {
        "I guess I'll take it",
        "It's okay",
        "Not the best"
    };
    public List<string> goodReactions = new List<string>()
    {
        "I like it",
        "Mnn this is good",
        "Nice"
    };
    public List<string> perfectReactions = new List<string>()
    {
        "Best pizza ever",
        "I love it",
        "Its brilliant"
    };
}