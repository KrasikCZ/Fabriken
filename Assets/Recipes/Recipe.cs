using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Recipe")]
public class Recipe : ScriptableObject
{
    public List<BuildReqs> inputs;
    public Item output;
}
