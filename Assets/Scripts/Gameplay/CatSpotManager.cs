using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatSpotManager : MonoBehaviour
{
    [SerializeField] private TMP_Text foundCatsCounter;
    [SerializeField] private Button mapBackground;
    [SerializeField] private ParticleSystem correctClickVFX;
    [SerializeField] private ParticleSystem wrongClickVFX;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private MapDrag mapDrag;
    [SerializeField] private HintManager hintManager;

    internal Action onWrongSpotClicked;
    internal Action onAllCatsClicked;

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
        hintManager.onHintUsed += OnHintUsed;
    }

    private void OnHintUsed()
    {
        var availableCats = allCats.Where(cat => !cat.isClicked).ToList();
        if (availableCats.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, availableCats.Count);
        CatSpot catToReveal = availableCats[randomIndex];
        catToReveal.ShowHintArrow();
    }

    private void OnCatClicked(CatSpot catspot)
    {
        if (catspot.isClicked) return;

        catsClickedCount++;
        SpawnVFX(correctClickVFX);
        SetCounterText();

        if (catsClickedCount == totalCatsCount)
        {
            onAllCatsClicked?.Invoke();
        }
    }

    private void OnWrongSpotClicked()
    {
        if (mapDrag != null && mapDrag.WasDragging) return;

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
