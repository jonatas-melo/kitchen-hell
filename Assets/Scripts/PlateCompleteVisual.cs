using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlateCompleteVisual : MonoBehaviour
{
    
    [Serializable]
    public struct KitchenObjectSOGameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject GameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSOGameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
        
        foreach (var kitchenObjectSoGameObject in kitchenObjectSOGameObjectList)
        {
            kitchenObjectSoGameObject.GameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectSoGameObject in kitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSoGameObject.KitchenObjectSO == e.KitchenObjectSo)
            {
                kitchenObjectSoGameObject.GameObject.SetActive(true);
            }
        }
    }
}
