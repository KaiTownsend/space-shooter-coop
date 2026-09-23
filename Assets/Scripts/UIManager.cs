using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    // [SerializeField] 

    public void UpdateTimer(float currentTime)
    {
        float minutes = (int)currentTime/60;
        float seconds = (int)currentTime%60;

        _timerText.text = $"{minutes}m {seconds}s";
    }
}
