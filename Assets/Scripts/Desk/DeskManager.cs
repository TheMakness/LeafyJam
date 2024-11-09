using UnityEngine;
using UnityEngine.Rendering;

public class DeskManager : MonoBehaviour
{
    [SerializeField] private GameObject m_deskAnchor;
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
    }

    private void UnSelect() 
    { 
            m_selectableObject.EndEvent.RemoveAllListeners();
            m_selectableObject = null; 
    }
    
}
