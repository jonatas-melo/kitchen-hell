using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private int _cuttingProcess;

    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasCuttingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                _cuttingProcess = 0;

                var recipe = GetRecipeFromInput(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    ProgressNormalized = (float)_cuttingProcess / recipe.cuttingProgressMax
                });
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject()) return;

        var recipe = GetRecipeFromInput(GetKitchenObject().GetKitchenObjectSO());
        if (recipe == null) return;

        _cuttingProcess++;
        OnCut?.Invoke(this, EventArgs.Empty);
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
        {
            ProgressNormalized = (float)_cuttingProcess / recipe.cuttingProgressMax
        });
        
        if (_cuttingProcess < recipe.cuttingProgressMax) return;

        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(recipe.output, this);
    }

    private bool HasCuttingRecipeWithInput(KitchenObjectSO input)
    {
        return GetRecipeFromInput(input) != null;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO input)
    {
        var recipe = GetRecipeFromInput(input);
        return recipe == null ? null : recipe.output;
    }

    private CuttingRecipeSO GetRecipeFromInput(KitchenObjectSO input)
    {
        foreach (var recipe in cuttingRecipes)
        {
            if (recipe.input == input)
            {
                return recipe;
            }
        }

        return null;
    }
}