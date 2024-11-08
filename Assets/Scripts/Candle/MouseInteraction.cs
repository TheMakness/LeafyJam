using Unity.VisualScripting;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    private MoveableObject selectedObject;
    private Vector3 mousePosition;
    private Vector3 mouseDirection;
    private bool mouseLock;
        

    private void Update()
    {
        mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectedObject = hit.transform.gameObject.GetComponent<MoveableObject>();
                if (selectedObject != null)
                    mouseLock = true;
            }
        }

        if (!Input.GetMouseButton(0) && mouseLock)
        {
            mouseLock = false;
            selectedObject = null;
        }


        if (mouseLock)
        {
            Vector3 screenPos = new Vector3(mousePosition.x, mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 newLocation = Camera.main.ScreenToWorldPoint(screenPos);
            selectedObject.transform.position = newLocation;
        }
    }

 

}
