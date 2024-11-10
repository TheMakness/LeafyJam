using UnityEngine;

[ExecuteInEditMode]

public class ShelfCase : MonoBehaviour
{
    [SerializeField] public SelectableObject m_ContainingObject;
    [SerializeField] private float m_size = 1;
    [SerializeField] private GameObject m_anchor;
    private Vector3 m_spawnPosition;
    private BoxCollider m_collider;


    private void OnValidate()
    {
        m_collider = GetComponent<BoxCollider>();
        m_collider.size = new Vector3(m_size, m_size, m_size);
        m_spawnPosition = m_anchor.transform.position;
    }

    public SelectableObject Interact(SelectableObject interactObject)
    {
        SelectableObject resultObject = null;

        bool isContainingObject = m_ContainingObject;
        bool isInteractObject = interactObject;


        if (isContainingObject && !interactObject) //Si il y a une bougie et que le joueur n'en n'a pas on return la bougie et on l'unparent de la case
        {
            resultObject = m_ContainingObject;
            m_ContainingObject = null;
        }

        if (isInteractObject && !isContainingObject) // Si le joueur à une bougie et qu'il n'y en à pas dans l'étagère le joueur dépose sa bougie et on renvoie null
        {
            m_ContainingObject = interactObject;
            DropObjectIntoCase(m_ContainingObject);
            resultObject = null;
        }

        if (isContainingObject && isInteractObject) // Si le joueur à une bougie et que l'étagere en contient une on swap et on renvoi la nvl bougie
        {
            resultObject = m_ContainingObject;
            m_ContainingObject = interactObject;
            DropObjectIntoCase (m_ContainingObject);
        }

        return resultObject;
    }

    public void DropObjectIntoCase(SelectableObject interactObject)
    {
        interactObject.Move(m_anchor.transform.position);
    }
}
