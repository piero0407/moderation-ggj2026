using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private float restartDelay = 8f;
    [SerializeField] private FloatVariable sanity;
    private float restartTimer = 0f;

    void Update()
    {
        restartTimer += Time.deltaTime;
        if (restartTimer >= restartDelay)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        sanity.Value = 1.0f;
    }
}
