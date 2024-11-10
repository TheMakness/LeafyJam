using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CandleInteraction : MonoBehaviour
{
    [SerializeField] private Wax wax;
    [SerializeField] private GameObject rotatingAnchor;
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
}
