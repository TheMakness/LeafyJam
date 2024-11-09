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
    private bool isDecorationSelected;

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

        if (isDecorationSelected)
        {
            MoveDecoration();
            if (Input.GetMouseButtonDown(0))
            {
                AddDecorationToCandle();
            }
        }
        m_prevPos = mousePosition;
    }

    private void AddDecorationToCandle()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        if (raycastResult)
        {
            SelectableObject newSelectObject = hit.transform.gameObject.GetComponent<SelectableObject>();
            CandleInteraction candleInteraction = hit.transform.gameObject.GetComponent<CandleInteraction>();

            if (newSelectObject && candleInteraction)
            {
                candleInteraction.AddDecoration(m_selectedObject.GetComponent<Decoration>());
                ReleaseObject();
            } 

        }
    }

    private void MoveDecoration()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        if (raycastResult)
        {
            SelectableObject selectObject = hit.transform.gameObject.GetComponent<SelectableObject>();
            CandleInteraction candleInteraction = hit.transform.gameObject.GetComponent<CandleInteraction>();

            if (selectObject && candleInteraction)
                m_selectedObject.Move(candleInteraction.GetPositionOnWax(hit.point));
                
        }
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
        if (m_selectedObject.GetComponent<Decoration>())
        {
            isDecorationSelected = true;
        }
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
        isDecorationSelected = false;
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
                    if (isObjectSelected && !m_selectedObject.GetComponent<CandleInteraction>())
                    {
                        return;
                    }

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
                        else
                        {
                            DeskManager deskManager;
                            deskManager = hit.transform.gameObject.GetComponent<DeskManager>();
                            if (deskManager && isObjectSelected)
                            {
                                deskManager.SelectObject(m_selectedObject.gameObject);
                                ReleaseObject();
                            }
                            else
                            {
                                DecorationSpawner decorationSpawner;
                                decorationSpawner = hit.transform.gameObject.GetComponent<DecorationSpawner>();
                                if (decorationSpawner && !isObjectSelected)
                                {
                                   SelectObject(Instantiate(decorationSpawner.Prefab,mousePosition,Quaternion.identity).GetComponent<SelectableObject>());
                                }
                            }
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
