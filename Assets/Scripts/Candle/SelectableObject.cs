using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public void Move(Vector3 position)
    {
        transform.position = position;
    }
}
