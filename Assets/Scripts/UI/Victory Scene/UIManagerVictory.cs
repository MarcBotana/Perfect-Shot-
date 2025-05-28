using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerVictory : MonoBehaviour
{
    public VictoryManager victoryManager;
    public TextMeshProUGUI modeText;

    public TextMeshProUGUI difficultyText;

    /**
         Modificar text dificultat i mode.
    **/
    public void UpdateUI()
    {
        difficultyText.text = $"Difficulty: {victoryManager.difficulty}";
        modeText.text = $"Mode: {victoryManager.mode}";
    }
    
}
