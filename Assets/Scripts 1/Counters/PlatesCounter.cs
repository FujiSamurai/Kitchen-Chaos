using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateDestroyed;

    [SerializeField] private KitchenObjectInteract plateKitchenObjectInteract;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int spawnPlateCount;
    private int spawnPlateCountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && spawnPlateCount < spawnPlateCountMax)
            {
                spawnPlateCount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            if (spawnPlateCount > 0)
            {
                spawnPlateCount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectInteract, player);

                OnPlateDestroyed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
