using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class CuttingRecipeInteract : ScriptableObject
{
    public KitchenObjectInteract input;
    public KitchenObjectInteract output;
    public int cuttingProgressFull;
}
