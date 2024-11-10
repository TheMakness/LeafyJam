using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    [SerializeField] private DeskManager deskManager;
    [SerializeField] private Color color;
    [SerializeField] private List<GameObject> pelets = new List<GameObject>();

    [ExecuteInEditMode]
    private void OnValidate()
    {
        if (pelets == null)
            return;
        foreach (GameObject pelet in pelets)
        {
            Renderer renderer = pelet.GetComponent<Renderer>();
            if (renderer)
                renderer.sharedMaterial.color = color;
        }
    }

    public void Interact()
    {
        if (deskManager)
        {
           CandleInteraction candle = deskManager.m_selectableObject.gameObject.GetComponent<CandleInteraction>();
            if (candle)
                candle.ChangeWaxColor(color);
        }
    }
}
