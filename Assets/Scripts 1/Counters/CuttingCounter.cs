using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeInteract[] cuttingRecipeInteracts;


    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is NO KitchenObject
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectInteract()))
                {
                    // Player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeInteract cuttingRecipeInteract = GetCuttingRecipeInteractWithInput(GetKitchenObject().GetKitchenObjectInteract());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeInteract.cuttingProgressFull
                    });
                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            //There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player has a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectInteract()))
                    {
                        GetKitchenObject().SelfDestroy();
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectInteract())) 
        {
            // There is a KitchenObject AND that can be cut
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeInteract cuttingRecipeInteract = GetCuttingRecipeInteractWithInput(GetKitchenObject().GetKitchenObjectInteract());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{progressNormalized = (float)cuttingProgress / cuttingRecipeInteract.cuttingProgressFull});

            if (cuttingProgress >= cuttingRecipeInteract.cuttingProgressFull)
            { 
                KitchenObjectInteract outputKitchenObjectInteract = GetOutputForInput(GetKitchenObject().GetKitchenObjectInteract());

                GetKitchenObject().SelfDestroy();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectInteract, this); 
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        CuttingRecipeInteract cuttingRecipeInteract = GetCuttingRecipeInteractWithInput(inputKitchenObjectInteract);
        return cuttingRecipeInteract != null;
    }

    private KitchenObjectInteract GetOutputForInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        CuttingRecipeInteract cuttingRecipeInteract = GetCuttingRecipeInteractWithInput(inputKitchenObjectInteract);

        if (cuttingRecipeInteract != null)
        {
            return cuttingRecipeInteract.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeInteract GetCuttingRecipeInteractWithInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        foreach (CuttingRecipeInteract cuttingRecipeInteract in cuttingRecipeInteracts)
        {
            if (cuttingRecipeInteract.input == inputKitchenObjectInteract)
            {
                return cuttingRecipeInteract;
            }
        }
        return null;
    }
}
