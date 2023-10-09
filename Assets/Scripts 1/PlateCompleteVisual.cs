using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectInteract_GameObject
    {
        public KitchenObjectInteract kitchenObjectInteract;
        public GameObject gameObject;
    }

    [SerializeField] private List<KitchenObjectInteract_GameObject> kitchenObjectInteractGameObjectList;
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectInteract_GameObject kitchenObjectInteractGameObject in kitchenObjectInteractGameObjectList)
        {
            kitchenObjectInteractGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectInteract_GameObject kitchenObjectInteractGameObject in kitchenObjectInteractGameObjectList)
        {
            if (kitchenObjectInteractGameObject.kitchenObjectInteract == e.kitchenObjectInteract) 
            {
                kitchenObjectInteractGameObject.gameObject.SetActive(true);
            }
        }
    }
}
