using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private GameObject _looseWindow;
    [SerializeField] private GameObject _winWindow;

    private void OnEnable()
    {
        CameraController.PlayerDeath += DisplayLooseWindow;
        CameraController.PlayerWin += DisplayWinWindow;
    }

    private void OnDisable()
    {
        CameraController.PlayerDeath -= DisplayLooseWindow;
        CameraController.PlayerWin -= DisplayWinWindow;
    }

    private void DisplayLooseWindow()
    {
        _looseWindow.SetActive(true);
        Cursor.visible = true;
    }

    private void DisplayWinWindow()
    {
        _winWindow.SetActive(true);
        Cursor.visible = true;
        _score.text += _timer.text;
    }
    
    private void Update()
    {
        _timer.text = $"{Mathf.Floor(Timer.time / 60):0}:{Timer.time % 60:00}";
    }
}