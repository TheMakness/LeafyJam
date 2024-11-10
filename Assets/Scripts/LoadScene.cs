using UnityEngine;

public class LoadScene : MonoBehaviour
{
    private void Awake()
    {
        // Load the gameplay scene additively
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay", UnityEngine.SceneManagement.LoadSceneMode.Additive);


        // Load the art scene additively
        UnityEngine.SceneManagement.SceneManager.LoadScene("Art", UnityEngine.SceneManagement.LoadSceneMode.Additive);

    }


}
