using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatSpot : MonoBehaviour
{
    [SerializeField] private GameObject hintArrow;

    private Button catButton;
    private Image catImage;

    internal bool isClicked = false;
    internal Action<CatSpot> catClicked;

    private void Awake()
    {
        catButton = GetComponent<Button>();
        catImage = GetComponent<Image>();
        catButton.onClick.AddListener(OnCatClicked);
    }

    private void OnCatClicked()
    {
        Debug.Log("Cat Clicked!");
        catImage.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        catClicked?.Invoke(this);

        isClicked = true;
    }

    public void ShowHintArrow()
    {
        if (hintArrow == null)
        {
            return;
        }

        StartCoroutine(ShowHintRoutine(2));

        IEnumerator ShowHintRoutine(float seconds)
        {
            hintArrow.SetActive(true);
            yield return new WaitForSeconds(seconds);
            hintArrow.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        catButton.onClick.RemoveAllListeners();
    }
}
