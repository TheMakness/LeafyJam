using UnityEngine;

public class S_SwitchCamera : MonoBehaviour
{

    public float transitionSpeed = 2f;

    private bool isTransitioning = false;
    private float transitionProgress = 0f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Update()
    {
        if (isTransitioning)
        {
            TransitionCamera();
        }
    }


    public void MoveCameraToPosition(Vector3 fromPosition, Vector3 toPosition)
    {
        startPosition = fromPosition;
        endPosition = toPosition;
        transitionProgress = 0f;
        isTransitioning = true;
    }

    private void TransitionCamera()
    {

        transitionProgress += Time.deltaTime * transitionSpeed;


        Camera.main.transform.position = Vector3.Lerp(startPosition, endPosition, transitionProgress);

        if (transitionProgress >= 1f)
        {
            isTransitioning = false;
        }

    }

}
