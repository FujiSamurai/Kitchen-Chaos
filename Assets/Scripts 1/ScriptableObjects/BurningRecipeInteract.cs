using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class BurningRecipeInteract : ScriptableObject
{
    public KitchenObjectInteract input;
    public KitchenObjectInteract output;
    public float burningTimerFull;
}
