using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CatSpotManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmpCounter;

    private List<CatSpot> allCats;
    private int catsClickedCount = 0;
    private int totalCatsCount;

    void Start()
    {
        allCats = new List<CatSpot>();
        allCats.AddRange(FindObjectsByType<CatSpot>(FindObjectsSortMode.None).ToList());
        allCats.ForEach(cat => cat.catClicked += OnCatClicked);

        totalCatsCount = allCats.Count;
        SetCounterText();
    }

    private void OnCatClicked(CatSpot catspot)
    {
        if (catspot.isClicked)
        {
            return;
        }

        catsClickedCount++;

        SetCounterText();
    }

    private void SetCounterText()
    {
        tmpCounter.text = $"{catsClickedCount}/{totalCatsCount}";
    }

    private void OnDisable()
    {
        allCats.ForEach(cat => cat.catClicked -= OnCatClicked);
    }
}
