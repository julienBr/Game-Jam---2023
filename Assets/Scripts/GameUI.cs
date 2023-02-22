using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject _looseWindow;

    private void OnEnable()
    {
        CameraController.PlayerDeath += DisplayLooseWindow;
    }

    private void OnDisable()
    {
        CameraController.PlayerDeath -= DisplayLooseWindow;
    }

    private void DisplayLooseWindow()
    {
        _looseWindow.SetActive(true);
        Cursor.visible = true;
    }
    
    private void Update()
    {
        timer.text = $"{Mathf.Floor(Timer.time / 60):0}:{Timer.time % 60:00}";
    }
}