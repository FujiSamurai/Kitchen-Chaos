using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeInteract[] fryingRecipeInteracts;
    [SerializeField] private BurningRecipeInteract[] burningRecipeInteracts;

    private State state;
    private float fryingTimer;
    private FryingRecipeInteract fryingRecipeInteract;
    private float burningTimer;
    private BurningRecipeInteract burningRecipeInteract;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeInteract.fryingTimerFull });

                    if (fryingTimer > fryingRecipeInteract.fryingTimerFull)
                    {
                        //fried
                        GetKitchenObject().SelfDestroy();

                        KitchenObject.SpawnKitchenObject(fryingRecipeInteract.output, this);

                        state = State.Fried;

                        burningTimer = 0f;

                        burningRecipeInteract = GetBurningRecipeInteractWithInput(GetKitchenObject().GetKitchenObjectInteract());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeInteract.burningTimerFull });

                    if (burningTimer > burningRecipeInteract.burningTimerFull)
                    {
                        //fried
                        GetKitchenObject().SelfDestroy();

                        KitchenObject.SpawnKitchenObject(burningRecipeInteract.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

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
                    // Player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeInteract = GetFryingRecipeInteractWithInput(GetKitchenObject().GetKitchenObjectInteract());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeInteract.fryingTimerFull });
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        FryingRecipeInteract fryingRecipeInteract = GetFryingRecipeInteractWithInput(inputKitchenObjectInteract);
        return fryingRecipeInteract != null;
    }

    private KitchenObjectInteract GetOutputForInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        FryingRecipeInteract fryingRecipeInteract = GetFryingRecipeInteractWithInput(inputKitchenObjectInteract);

        if (fryingRecipeInteract != null)
        {
            return fryingRecipeInteract.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeInteract GetFryingRecipeInteractWithInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        foreach (FryingRecipeInteract fryingRecipeInteract in fryingRecipeInteracts)
        {
            if (fryingRecipeInteract.input == inputKitchenObjectInteract)
            {
                return fryingRecipeInteract;
            }
        }
        return null;
    }

    private BurningRecipeInteract GetBurningRecipeInteractWithInput(KitchenObjectInteract inputKitchenObjectInteract)
    {
        foreach (BurningRecipeInteract burningRecipeInteract in burningRecipeInteracts)
        {
            if (burningRecipeInteract.input == inputKitchenObjectInteract)
            {
                return burningRecipeInteract;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
