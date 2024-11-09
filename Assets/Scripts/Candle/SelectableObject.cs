using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private GameObject rotationAnchor;

    public void Move(Vector3 position)
    {
        transform.position = position;
    }

    public void RotateObject(Vector3 delta)
    {
        if (Vector3.Dot(transform.position, Vector3.up) >= 0)
        {
            rotationAnchor.transform.Rotate(transform.up,Vector3.Dot(delta,Camera.main.transform.right),Space.World);
        }
        else
        {
            rotationAnchor.transform.Rotate(transform.up, -Vector3.Dot(delta, Camera.main.transform.right),Space.World);
        }

        rotationAnchor.transform.Rotate(Camera.main.transform.right,Vector3.Dot(delta, Camera.main.transform.up),Space.World);
    }

    public void ResetRotation()
    {
        rotationAnchor.transform.rotation = Quaternion.identity;
    }
}
