using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListInteract recipeListInteract;


    private List<RecipeInteract> waitingRecipeInteracts;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerFull = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;

        waitingRecipeInteracts = new List<RecipeInteract>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerFull;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeInteracts.Count < waitingRecipesMax)
            {
                RecipeInteract waitingRecipeInteract = recipeListInteract.recipeInteracts[UnityEngine.Random.Range(0, recipeListInteract.recipeInteracts.Count)];
                waitingRecipeInteracts.Add(waitingRecipeInteract);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeInteracts.Count; ++i)
        {
            RecipeInteract waitingRecipeInteract = waitingRecipeInteracts[i];

            if (waitingRecipeInteract.kitchenObjectInteracts.Count == plateKitchenObject.GetKitchenObjectInteracts().Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectInteract recipeKitchenObjectInteract in waitingRecipeInteract.kitchenObjectInteracts)
                {
                    // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;

                    foreach (KitchenObjectInteract plateKitchenObjectInteract in plateKitchenObject.GetKitchenObjectInteracts())
                    {
                        // Cycling through all ingredients in the recipe
                        if (plateKitchenObjectInteract == recipeKitchenObjectInteract)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    successfulRecipesAmount++;

                    // Player delivered the correct recipe!
                    waitingRecipeInteracts.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }
        // No matches found!
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    public List<RecipeInteract> GetWaitingRecipeInteracts() 
    { 
        return waitingRecipeInteracts; 
    }

    public int GetsuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
