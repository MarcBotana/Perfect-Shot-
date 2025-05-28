using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas gameplayUI;
    public Canvas kickUI;
    public Canvas endGameUI;
    public Canvas pauseUI;

    public Slider kickForceSlider;
    public Texture2D crosshairTexture;
    public Vector2 hotspot = Vector2.zero;

    public GameManager gameManager;

    public TextMeshProUGUI goalsText;

    public RawImage[] goalsPanels;

    public TextMeshProUGUI levelText;

    public TextMeshProUGUI goalAnnouncement;

    public TextMeshProUGUI failAnnouncement;

    public TextMeshProUGUI countDown;

    public GameObject nextDifficultyPanel;

    public TextMeshProUGUI nextDifficultyText;

    public TextMeshProUGUI totalGoals;

    public TextMeshProUGUI maxLevel;

    public TextMeshProUGUI maxDiff;

    public TextMeshProUGUI difficultyText;

    public TextMeshProUGUI modeText;

    public Slider musicSlider;

    public Slider effectsSlider;

    public bool isPaused = false;

    /**
         Preparació de les pantalles del Menú i modificació del Só.
    **/
    private void Start()
    {
        UpdateGoalsText();
        kickForceSlider.value = 0f;
        gameplayUI.gameObject.SetActive(true);
        kickUI.gameObject.SetActive(false);
        endGameUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);

        goalAnnouncement.gameObject.SetActive(false);
        failAnnouncement.gameObject.SetActive(false);

        nextDifficultyPanel.gameObject.SetActive(false);

        countDown.gameObject.SetActive(false);

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
         Comprovar si el Joc esta pausat o mostrar la UI de KickMode i modificar el cursor.
    **/
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isInKickMode)
        {
            PauseGame();
            kickUI.gameObject.SetActive(false);
            return;
        }
        else if(!gameManager.isInKickMode)
        {
            kickUI.gameObject.SetActive(false);
            return;
        }

        kickUI.gameObject.SetActive(true);

        Cursor.SetCursor(crosshairTexture, hotspot, CursorMode.Auto);

        kickForceSlider.value = gameManager.currentKickForce;

    }

    /**
         Actualitzar el text de Gols.
    **/
    public void UpdateGoalsText()
    {
        goalsText.text = $"GOALS: {gameManager.goals}";

    }

    /**
         Actualitzar el text de dificultat.
    **/
    public void UpdateDifficultyText(GameManager.Difficulty difficulty)
    {
        difficultyText.text = "Difficulty: " + difficulty;
    }

    /**
         Actualitzar el text de mode.
    **/
    public void UpdateModeText(GameManager.Mode mode)
    {
        modeText.text = "Mode: " + mode;
    }

    /**
         Actualitzar el color dels panells de gol.
    **/
    public void UpdatePanel(bool Goal)
    {
        if (Goal)
        {
            goalsPanels[gameManager.currentShoots - 1].color = Color.green;
        }
        else
        {
            goalsPanels[gameManager.currentShoots - 1].color = Color.red;
        }
    }

    /**
         Restablir el color dels panells de gol.
    **/
    public void ResetPanel()
    {
        for (int i = 0; i < 5; i++)
        {
            goalsPanels[i].color = Color.gray;
        }
    }

    /**
        Pausar el joc i mostrar el menu de pausa.
    **/
    public void PauseGame()
    {
        isPaused = !isPaused;

        pauseUI.gameObject.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

    }

    /**
         Actualitzar el text de Nivell.
    **/
    public void UpdateLevel()
    {
        levelText.text = $"Level: {gameManager.level}";
    }

    public void ShowCountdown(bool isGoal, int countDownSeconds)
    {
        if (isGoal)
        {
            countDown.gameObject.SetActive(true);
            countDown.text = $"Next Level in {countDownSeconds}s";

        }
        else
        {
            countDown.gameObject.SetActive(true);
            countDown.text = $"Restart Level in {countDownSeconds}s";
        }

    }

    /**
         Mostrar panell de següent dificultat (només mode Champion).
    **/
    public void ShowDifficultyPanel(GameManager.Difficulty difficulty)
    {
        goalAnnouncement.gameObject.SetActive(false);
        nextDifficultyPanel.SetActive(true);
        nextDifficultyText.gameObject.SetActive(true);
        nextDifficultyText.text = $"Next: {difficulty}";


    }

    /**
         Ocultar els panells de següent nivell/dificultat.
    **/
    public void HideCountDown()
    {
        nextDifficultyPanel.SetActive(false);
        countDown.gameObject.SetActive(false);
        goalAnnouncement.gameObject.SetActive(false);
        failAnnouncement.gameObject.SetActive(false);
    }

    /**
         Mostrar text de Gol.
    **/
    public void ShowGoalText()
    {
        goalAnnouncement.gameObject.SetActive(true);
    }

    /**
         Mostrar text de Fail.
    **/
    public void ShowFailText()
    {
        failAnnouncement.gameObject.SetActive(true);
    }

    /**
         Mostrar panell de GameOver.
    **/
    public void ShowEndGameUI(GameManager.Difficulty difficulty)
    {
        gameplayUI.gameObject.SetActive(false);
        kickUI.gameObject.SetActive(false);
        endGameUI.gameObject.SetActive(true);

        Debug.Log("ENDGAME UI");

        totalGoals.text = $"Total Goals: {gameManager.totalGoals}";
        maxLevel.text = $"Max Level: {gameManager.level}";
        maxDiff.text = $"Max Difficulty: {difficulty}";

    }




}
