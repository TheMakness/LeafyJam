using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CandleInteraction : MonoBehaviour
{
    [SerializeField] private Wax wax;
    [SerializeField] private GameObject rotatingAnchor;
    [SerializeField] private GameObject[] wick;
    [SerializeField] private int currentWick;

    [SerializeField] List<Decoration> decorations = new List<Decoration>();


    public void AddDecoration(Decoration decoration)
    {
        decorations.Add(decoration);
        decoration.gameObject.transform.SetParent(rotatingAnchor.transform, true);
    }

    public void RemoveDecoration(Decoration decoration)
    {
        decorations.Remove(decoration);
        decoration.transform.SetParent(null, true);
    }

    public Vector3 GetPositionOnWax(Vector3 currentPos)
    {
        return wax.GetComponent<MeshCollider>().ClosestPoint(currentPos);
    }

    public void ChangeWaxColor(Color color)
    {
        Renderer r = wax.gameObject.GetComponent<Renderer>();
        r.material.color = color;
    }

    public void ChangeWick()
    {
        wick[currentWick].SetActive(false);
        currentWick = (currentWick + 1) % wick.Length;
        wick[currentWick].SetActive(true);
    }

    public void Ignite()
    {
        wick[currentWick].GetComponentInChildren<ParticleSystem>().Play();
    }

    public void Extinguish()
    {
        wick[currentWick].GetComponentInChildren<ParticleSystem>().Stop();
    }
}
