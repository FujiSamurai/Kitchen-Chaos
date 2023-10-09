using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class RecipeInteract : ScriptableObject
{
    public List<KitchenObjectInteract> kitchenObjectInteracts;
    public string recipeName;
}
