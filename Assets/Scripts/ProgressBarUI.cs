using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounter.OnProgressChanged += CuttingCounterOnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounterOnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs args)
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
