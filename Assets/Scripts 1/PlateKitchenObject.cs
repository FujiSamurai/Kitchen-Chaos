using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectInteract kitchenObjectInteract;
    }

    [SerializeField] private List<KitchenObjectInteract> validKitchenObjectInteracts;

    private List<KitchenObjectInteract> kitchenObjectInteracts;

    private void Awake()
    {
        kitchenObjectInteracts = new List<KitchenObjectInteract>();
    }

    public bool TryAddIngredient(KitchenObjectInteract kitchenObjectInteract) 
    {
        if (!validKitchenObjectInteracts.Contains(kitchenObjectInteract))
        {
            // not valid ingredient
            return false;
        }
        if (kitchenObjectInteracts.Contains(kitchenObjectInteract))
        {
            // Already has same object 
            return false;
        }
        else
        {
            kitchenObjectInteracts.Add(kitchenObjectInteract);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{kitchenObjectInteract = kitchenObjectInteract});

            return true;
        }
    }

    public List<KitchenObjectInteract> GetKitchenObjectInteracts()
    {
        return kitchenObjectInteracts;
    }
    
}
