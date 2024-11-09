using UnityEngine;
using UnityEngine.Rendering;

public class DeskManager : MonoBehaviour
{
    [SerializeField] private GameObject m_deskAnchor;
    [SerializeField] private Collider m_deskCollider;
    private SelectableObject m_selectableObject;
    

    public void SpawnObject(GameObject go)
    {

        if (!m_selectableObject)
        {
            m_selectableObject = Instantiate(go, m_deskAnchor.transform).GetComponent<SelectableObject>();
            m_selectableObject.EndEvent.AddListener(UnSelect);
        }
        else
        {
            Destroy(m_selectableObject.gameObject);
            m_selectableObject = Instantiate(go, m_deskAnchor.transform).GetComponent<SelectableObject>();
            m_selectableObject.EndEvent.AddListener(UnSelect);
        }
        m_deskCollider.enabled = false;
    }

    public void SelectObject(GameObject go)
    {
        if (!m_selectableObject)
        {
            m_deskCollider.enabled = false;
            m_selectableObject = go.GetComponent<SelectableObject>();
            m_selectableObject.EndEvent.AddListener(UnSelect);
            m_selectableObject.Move(m_deskAnchor.transform.position);
            m_selectableObject.gameObject.GetComponent<Collider>().enabled = true;
        }

    }

    private void UnSelect() 
    { 
            m_deskCollider.enabled = true;
            m_selectableObject.EndEvent.RemoveAllListeners();
            m_selectableObject = null; 
    }
    
}
