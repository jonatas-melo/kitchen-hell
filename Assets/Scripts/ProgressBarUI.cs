using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " doesn't have a component that implements IHasProgress");
        }
        
        hasProgress.OnProgressChanged += HasProgressOnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgressOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs args)
    {
        barImage.fillAmount = args.ProgressNormalized;

        if (args.ProgressNormalized is <= 0f or >= 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
