using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileGenerator : MonoBehaviour
{
    [SerializeField] private Image iconCanvas;
    [SerializeField] private TMP_Text itemDescription;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!iconCanvas) iconCanvas = gameObject.GetComponent<Image>();
        if (!itemDescription) itemDescription = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public void RefreshIcon(Sprite newImage = null, String newDescription = null)
    {
        iconCanvas.sprite = newImage;
        itemDescription.text = newDescription;
    }
}
