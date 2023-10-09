using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectInteract kitchenObjectInteract;
    /*[SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;*/



    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectInteract, player);
            //kitchenObjectTransform.localPosition = Vector3.zero;

            //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectInteract().objectName);

            /*kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(this);*/

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }


    }
   
}
