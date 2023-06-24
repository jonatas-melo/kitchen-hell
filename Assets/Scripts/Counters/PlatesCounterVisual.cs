using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjects;

    private void Awake()
    {
        _plateVisualGameObjects = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounterOnOnPlateRemoved;
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e)
    {
        var plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        
        const float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0f, plateOffsetY * _plateVisualGameObjects.Count, 0f);
        _plateVisualGameObjects.Add(plateVisualTransform.gameObject);
    }

    private void PlatesCounterOnOnPlateRemoved(object sender, EventArgs e)
    {
        var plateVisualGameObject = _plateVisualGameObjects[^1];
        _plateVisualGameObjects.Remove(plateVisualGameObject);
        Destroy(plateVisualGameObject);
    }
}
