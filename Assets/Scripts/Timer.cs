using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer _timer;
    public static float time;

    private void Awake()
    {
        time = 0f;
    }
    
    private void Update()
    {
        time += Time.deltaTime;
    }
}