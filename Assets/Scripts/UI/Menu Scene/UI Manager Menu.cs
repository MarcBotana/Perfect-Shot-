using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class UIManagerMenu : MonoBehaviour
{
    public Canvas menuUI;

    public Canvas optionsUI;

    public TextMeshProUGUI difficultyText;

    public TextMeshProUGUI modeText;

    public TextMeshProUGUI characterText;

    public Slider musicSlider;

    public Slider effectsSlider;

    /**
         Modificadors de Só.
    **/
    void Start()
    {
        menuUI.gameObject.SetActive(true);
        optionsUI.gameObject.SetActive(false);

        if (MusicManager.Instance != null)
        {

            musicSlider.maxValue = MusicManager.Instance.GetMaxMusicVol();
            effectsSlider.maxValue = MusicManager.Instance.GetMaxEffectsVol();

            musicSlider.value = MusicManager.Instance.GetMusicVolume();
            effectsSlider.value = MusicManager.Instance.GetEffectsVolume();

            musicSlider.onValueChanged.AddListener(value =>
            {
                MusicManager.Instance.SetMusicVolume(value);
                Debug.Log($"[MusicSlider] Nuevo volumen: {value}");
            });

            effectsSlider.onValueChanged.AddListener(value =>
            {
                MusicManager.Instance.SetEffectsVolume(value);
                Debug.Log($"[EffectsSlider] Nuevo volumen: {value}");
            });
        }
    }

    /**
         Mostrar Opcions.
    **/
    public void ShowOptions()
    {
        menuUI.gameObject.SetActive(false);
        optionsUI.gameObject.SetActive(true);
    }

    /**
         Tornar al Menú Principal.
    **/
    public void BackToMenu()
    {
        menuUI.gameObject.SetActive(true);
        optionsUI.gameObject.SetActive(false);
    }

    /**
         Actualitzar el text de dificultat.
    **/
    public void UpdateDifficultyText(string difficulty)
    {
        difficultyText.text = difficulty;
        Debug.Log(difficultyText.text);
    }

    /**
         Actualitzar el text de mode.
    **/
    public void UpdateModeText(string mode)
    {
        modeText.text = mode;
        Debug.Log(difficultyText.text);

    }

    /**
         Actualitzar el text de jugador.
    **/
    public void UpdatePlayerText(int characterSelected)
    {
        switch (characterSelected)
        {
            case 0:
                characterText.text = "Zlatan";
                return;
            case 1:
                characterText.text = "Messi";
                return;
        }
    }
}
