using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    public event EventHandler OnPlayerGrabbedObject; 

    public override void Interact(Player player)
    {
        Transform kitchenObjectSoTransform = Instantiate(kitchenObjectSo.prefab);
        kitchenObjectSoTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}