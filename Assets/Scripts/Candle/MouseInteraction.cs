using Unity.VisualScripting;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] private SelectableObject m_selectedObject;
    [SerializeField] private SelectableObject m_rotatingObject;
    private Vector3 mousePosition;
    private Vector3 mouseDirection;
    private bool isObjectSelected;
    private bool isRotating;

    private Vector3 m_prevPos;
    private Vector3 m_posDelta;
        

    private void Update()
    {
        mousePosition = Input.mousePosition;

        if(Input.GetMouseButtonDown(0))
        {
            OnLeftMouseClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseClick();
        }

        if (!Input.GetMouseButton(1))
            StopRotatingObject();

        if (isObjectSelected)
        {
            FollowMouse();
        }

        if (isRotating) 
        {
            m_posDelta = mousePosition - m_prevPos;
            m_rotatingObject.RotateObject(m_posDelta);
        }

        m_prevPos = mousePosition;
    }

    private void FollowMouse()
    {
        Vector3 screenPos = new Vector3(mousePosition.x, mousePosition.y, Camera.main.WorldToScreenPoint(m_selectedObject.transform.position).z);
        Vector3 newLocation = Camera.main.ScreenToWorldPoint(screenPos);
        newLocation.z = m_selectedObject.transform.position.z;
        m_selectedObject.Move(newLocation);
    }

    private void SelectObject(SelectableObject obj)
    {
        m_selectedObject = obj;
        m_selectedObject.NewSelection();
        obj.gameObject.GetComponent<Collider>().enabled = false;
        isObjectSelected = true;
        m_selectedObject.ResetRotation();
    }

    private void StartRotateObject(SelectableObject obj)
    {
        m_rotatingObject = obj;
        isRotating = true;
    }

    private void StopRotatingObject()
    {
        isRotating = false;
        m_rotatingObject = null;
        m_posDelta = Vector3.zero;
        m_prevPos = Vector3.zero;
    }

    private void ReleaseObject()
    {
        isObjectSelected = false;
        m_selectedObject = null;
    }

    private void OnLeftMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        if (raycastResult)
        {
            SelectableObject selectObject = hit.transform.gameObject.GetComponent<SelectableObject>();
            if (selectObject && !isObjectSelected)
                SelectObject(selectObject);
            else
            {
                ShelfCase shelfCase;
                if (shelfCase = hit.transform.gameObject.GetComponent<ShelfCase>())
                {
                    SelectableObject returnObject = shelfCase.Interact(m_selectedObject);
                    if (returnObject)
                    {
                        if (isObjectSelected)
                            ReleaseObject();
                        SelectObject(returnObject);
                        
                    }
                    else
                    {
                       ReleaseObject();
                    }
                }
                else
                {
                    Container container;
                    container = hit.transform.gameObject.GetComponent<Container>();
                    if (container)
                    container.Interact();

                    else
                    {
                        Trashbin bin;
                        bin = hit.transform.gameObject.GetComponent<Trashbin>();
                        if (bin)
                        {
                            bin.Delete(m_selectedObject.gameObject);
                            ReleaseObject();
                        }
                    }
                }
            }
            
        }
            
       
    }

    private void OnRightMouseClick()
    {

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        if (raycastResult)
        {
            SelectableObject selectObject = hit.transform.gameObject.GetComponent<SelectableObject>();
            if (selectObject && !isRotating)
            {
                StartRotateObject(selectObject);
            }
        }

       
    }

}
