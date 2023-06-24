using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _plateSpawnedAmount;
    private int _plateSpawnedAmountMax = 4;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer >= _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (_plateSpawnedAmount < _plateSpawnedAmountMax)
            {
                _plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() || _plateSpawnedAmount <= 0f) return;

        _plateSpawnedAmount--;
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
