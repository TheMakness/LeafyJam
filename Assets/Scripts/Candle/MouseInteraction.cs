using Unity.VisualScripting;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] private SelectableObject m_selectedObject;
    private Vector3 mousePosition;
    private Vector3 mouseDirection;
    private bool isObjectSelected;
        

    private void Update()
    {
        mousePosition = Input.mousePosition;

        if(Input.GetMouseButtonDown(0))
        {
            OnClickMouse();
        }


        if (isObjectSelected)
        {
            Vector3 screenPos = new Vector3(mousePosition.x, mousePosition.y, Camera.main.WorldToScreenPoint(m_selectedObject.transform.position).z);
            Vector3 newLocation = Camera.main.ScreenToWorldPoint(screenPos);
            newLocation.z = m_selectedObject.transform.position.z;
            m_selectedObject.Move(newLocation);
        }
    }

 
    private void SelectObject(SelectableObject obj)
    {
        m_selectedObject = obj;
        obj.gameObject.GetComponent<Collider>().enabled = false;
        isObjectSelected = true;
    }

    private void ReleaseObject()
    {
       // m_selectedObject.gameObject.GetComponent<Collider>().enabled = true;
        isObjectSelected = false;
        m_selectedObject = null;
    }

    private void OnClickMouse()
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
            }
        }
            
       
    }

}
