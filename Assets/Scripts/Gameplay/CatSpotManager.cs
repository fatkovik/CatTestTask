using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatSpotManager : MonoBehaviour
{
    [SerializeField] TMP_Text foundCatsCounter;
    [SerializeField] Button mapBackground;
    [SerializeField] ParticleSystem correctClickVFX;
    [SerializeField] ParticleSystem wrongClickVFX;
    [SerializeField] Camera uiCamera;

    internal Action onWrongSpotClicked;

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

        mapBackground.onClick.AddListener(OnWrongSpotClicked);
    }

    private void OnCatClicked(CatSpot catspot)
    {
        if (catspot.isClicked)
        {
            return;
        }

        catsClickedCount++;
        SpawnVFX(correctClickVFX);
        SetCounterText();
    }

    private void OnWrongSpotClicked()
    {
        SpawnVFX(wrongClickVFX);
        onWrongSpotClicked?.Invoke();
    }

    private void SpawnVFX(ParticleSystem prefab)
    {
        if (prefab == null) return;

        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10f;
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(screenPos);

        ParticleSystem vfx = Instantiate(prefab, worldPos, Quaternion.identity);
        Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
    }

    private void SetCounterText()
    {
        foundCatsCounter.text = $"{catsClickedCount}/{totalCatsCount}";
    }

    private void OnDisable()
    {
        allCats.ForEach(cat => cat.catClicked -= OnCatClicked);
        mapBackground.onClick.RemoveListener(OnWrongSpotClicked);
    }
}
