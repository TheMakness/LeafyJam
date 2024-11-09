using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private DeskManager m_manager;
    [SerializeField] private GameObject m_prefab;

    public void Interact()
    {
        if (m_manager != null)
            m_manager.SpawnObject(m_prefab);
    }
}
