using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MouseInteraction : MonoBehaviour
{
    public UnityEvent SwitchPhaseEvent;

    public enum Phase
    {
        Phase1,
        Phase2,
    }
    [SerializeField] private Phase m_CurrentPhase;
    [SerializeField] private SelectableObject m_selectedObject;
    [SerializeField] private SelectableObject m_rotatingObject;
    [SerializeField] private GameObject m_ValidationPrefab;
    private Vector3 mousePosition;
    private Vector3 mouseDirection;

    private bool isObjectSelected;
    private bool isRotating;
    private bool isDecorationSelected;
    private bool isMatchSelected;

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

    public void SwitchPhase(SelectableObject currentselectedObject)
    {
        
        if (currentselectedObject)
        {
            Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(currentselectedObject.transform.position));
            SelectObject(currentselectedObject);
            if (m_CurrentPhase == Phase.Phase1)
            {
                Instantiate(m_ValidationPrefab, m_selectedObject.transform.position, Quaternion.identity);
            }
        }
        
        

        m_CurrentPhase = (Phase)((((int)m_CurrentPhase) + 1) % 2);
        SwitchPhaseEvent.Invoke();
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
        isMatchSelected = false;
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
            {
                if (!isDecorationSelected) //C'est une bougie
                {
                    selectObject.gameObject.GetComponent<CandleInteraction>()?.ChangeWick();
                }

            }

            else
            {
                ShelfCase shelfCase = hit.transform.gameObject.GetComponent<ShelfCase>();
                if (shelfCase && m_CurrentPhase == Phase.Phase2)
                {
                    if (isMatchSelected)
                    {
                        shelfCase.m_ContainingObject.gameObject.GetComponent<CandleInteraction>().Ignite();
                        return;
                    }

                    if (isObjectSelected && !m_selectedObject.GetComponent<CandleInteraction>() && !isMatchSelected )
                    {
                        return;
                    }

                    
                    SelectableObject returnObject = shelfCase.Interact(m_selectedObject);
                    if (returnObject)
                    {
                        returnObject.gameObject.GetComponent<CandleInteraction>().Extinguish();
                        if (isObjectSelected)
                        {
                            ReleaseObject();
                        }
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
                    if (container && m_CurrentPhase == Phase.Phase1)
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
                            if (deskManager && isObjectSelected && m_CurrentPhase == Phase.Phase1)
                            {
                                deskManager.SelectObject(m_selectedObject.gameObject);
                                ReleaseObject();
                            }
                            else
                            {
                                DecorationSpawner decorationSpawner;
                                decorationSpawner = hit.transform.gameObject.GetComponent<DecorationSpawner>();
                                if (decorationSpawner && !isObjectSelected && m_CurrentPhase == Phase.Phase1)
                                {
                                    SelectObject(Instantiate(decorationSpawner.Prefab, mousePosition, Quaternion.identity).GetComponent<SelectableObject>());
                                }

                                else
                                {
                                    ColorSelector colorSelector;
                                    colorSelector = hit.transform.gameObject.GetComponent<ColorSelector>();
                                    if (colorSelector && m_CurrentPhase == Phase.Phase1)
                                    {
                                        colorSelector.Interact();
                                    }

                                    else
                                    {
                                        MatchJar matchJar;
                                        matchJar = hit.transform.gameObject.GetComponent<MatchJar>();
                                        if (matchJar && m_CurrentPhase == Phase.Phase2 && !isObjectSelected)
                                        {
                                            SelectObject(Instantiate(matchJar.Prefab, mousePosition, Quaternion.identity ).GetComponent<SelectableObject>());
                                            isMatchSelected = true;
                                        }
                                    }
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
