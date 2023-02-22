using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;

    private void Update()
    {
        timer.text = $"{Mathf.Floor(Timer.time / 60):0}:{Timer.time % 60:00}";
    }
}