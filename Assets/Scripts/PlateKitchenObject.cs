using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validIngredientList;
    private List<KitchenObjectSO> _ingredientList;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSo;
    }
        
    private void Awake()
    {
        _ingredientList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO ingredient)
    {
        if (!validIngredientList.Contains(ingredient) || _ingredientList.Contains(ingredient)) return false;
        _ingredientList.Add(ingredient);
        
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            KitchenObjectSo = ingredient
        });
        
        return true;
    }
}
