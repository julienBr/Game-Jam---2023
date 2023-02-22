using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer _timer;
    public static float time;

    private void Awake()
    {
        if (_timer != null && _timer != this)
        {
            Destroy(gameObject);
            return;
        }
        _timer = this;
        DontDestroyOnLoad(this);
        time = 0f;
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        //if (PauseMenu.returnToMainMenu || PauseMenu.isRestart) time = 0f;
    }
}