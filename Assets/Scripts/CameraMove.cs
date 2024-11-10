using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 startRotation;
    private bool isRotating;
    public MouseInteraction mouseInteraction;
    public DeskManager deskManager;
    [SerializeField] private Vector3 endRotation = new Vector3(-2,0,0);
    [SerializeField] private float rotationSpeed = 1f;


    private void Start()
    {
        mouseInteraction.SwitchPhaseEvent.AddListener(switchRotation);
        startRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (isRotating)
        {
            RotateCamera();
        }
    }

    //Function to slerp beetween two rotation values and when is done stop rotating
    private void RotateCamera()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(endRotation), Time.deltaTime * rotationSpeed);
        if (transform.rotation == Quaternion.Euler(endRotation))
        {
            isRotating = false;
            Vector3 temp = startRotation;
            startRotation = endRotation;
            endRotation = temp;
        }
    }

   

    private void switchRotation()
    {
        isRotating = true;
    }
}
