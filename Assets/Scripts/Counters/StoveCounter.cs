using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    [SerializeField] private BurningRecipeSO[] burningRecipes;

    private State _state;
    private float _fryingTimer;
    private FryingRecipeSO _selectedFryingRecipe;
    private float _burningTimer;
    private BurningRecipeSO _selectedBurningRecipe;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State State;
    }

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;

        switch (_state)
        {
            case State.Idle:
                break;
            case State.Frying:
                _fryingTimer += Time.deltaTime;
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    ProgressNormalized = _fryingTimer / _selectedFryingRecipe.fryingTimerMax
                });

                if (_fryingTimer > _selectedFryingRecipe.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(_selectedFryingRecipe.output, this);

                    _state = State.Fried;
                    _burningTimer = 0f;
                    _selectedBurningRecipe = GetBurningRecipeFromInput(GetKitchenObject().GetKitchenObjectSO());
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        State = _state
                    });
                }

                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    ProgressNormalized = _burningTimer / _selectedBurningRecipe.burningTimerMax
                });

                if (_burningTimer > _selectedBurningRecipe.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(_selectedBurningRecipe.output, this);

                    _state = State.Burned;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        State = _state
                    });
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        ProgressNormalized = 0f
                    });
                }

                break;
            case State.Burned:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasFryingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                _selectedFryingRecipe = GetRecipeFromInput(GetKitchenObject().GetKitchenObjectSO());
                _state = State.Frying;
                _fryingTimer = 0f;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    State = _state
                });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    ProgressNormalized = _fryingTimer / _selectedFryingRecipe.fryingTimerMax
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
                _state = State.Idle;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    State = _state
                });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    ProgressNormalized = 0f
                });
            }
        }
    }

    private bool HasFryingRecipeWithInput(KitchenObjectSO input)
    {
        return GetRecipeFromInput(input) != null;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO input)
    {
        var recipe = GetRecipeFromInput(input);
        return recipe == null ? null : recipe.output;
    }

    private FryingRecipeSO GetRecipeFromInput(KitchenObjectSO input)
    {
        foreach (var recipe in fryingRecipes)
        {
            if (recipe.input == input)
            {
                return recipe;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeFromInput(KitchenObjectSO input)
    {
        foreach (var recipe in burningRecipes)
        {
            if (recipe.input == input)
            {
                return recipe;
            }
        }

        return null;
    }
}