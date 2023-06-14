using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;

    private static readonly int Cut = Animator.StringToHash("Cut");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingContainerOnCut;
    }

    private void CuttingContainerOnCut(object sender, EventArgs e)
    {
        _animator.SetTrigger(Cut);
    }
}
