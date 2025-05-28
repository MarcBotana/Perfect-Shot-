using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera kickCamera;
    public UIManager uIManager;
    public PlayerSpawner playerSpawner;
    public BallSpawner ballSpawner;
    public EnemySpawner enemySpawner;
    public Transform ball;
    public float kickForceMax = 20f;
    public float chargeSpeed = 10f;
    public bool isInKickMode = false;
    public float currentKickForce = 0f;

    public enum Difficulty { Easy, Medium, Hard }
    public enum Mode { Normal, Champion };
    public Difficulty currentDifficulty = 0;
    public Mode currentMode = 0;
    public int characterSelected = 0;
    public int level = 1;
    public int levelCountDown = 5;
    public bool isGameOver = false;
    public bool isWaitingNextLevel = false;
    public bool isBallKicked = false;
    public int totalGoals = 0;
    public int goals = 0;
    public int fails = 0;
    public int maxShoots = 5;
    public int currentShoots = 0;
    private bool isCharging = false;

    public AudioClip stadiumEffect;
    public AudioClip shotEffect;


    /**
        Mètode per carregar partida, segons Mode i Dificultat seleccionada.
    **/
    private void Start()
    {
        mainCamera.enabled = true;
        kickCamera.enabled = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        MusicManager.Instance.PlayLoopingEffect(stadiumEffect);

        int playerDiff = PlayerPrefs.GetInt("Difficulty");
        int playerMode = PlayerPrefs.GetInt("Mode");
        characterSelected = PlayerPrefs.GetInt("CharacterSelected");

        currentMode = (Mode)playerMode;

        if (currentMode == Mode.Normal)
        {
            currentDifficulty = (Difficulty)playerDiff;
        }
        else if (currentMode == Mode.Champion)
        {
            currentDifficulty = Difficulty.Easy;
        }

        uIManager.UpdateDifficultyText(currentDifficulty);
        uIManager.UpdateModeText(currentMode);
        uIManager.ResetPanel();

        SpawnLevel(currentDifficulty);

    }

    /**
        Mètode per aplicar força i direcció a la pilota si estem en KickMode.
    **/
    void Update()
    {

        if (isGameOver || isWaitingNextLevel) return;

        if (!isInKickMode) return;


        if (Input.GetMouseButtonDown(0))
        {
            currentKickForce = 0f;
            isCharging = true;
        }

        if (isCharging && Input.GetMouseButton(0))
        {
            currentKickForce += chargeSpeed * Time.deltaTime;
            currentKickForce = Mathf.Clamp(currentKickForce, 0f, kickForceMax);
        }

        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;

            Ray ray = kickCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 targetPoint = ray.origin + ray.direction * 100f;

            Vector3 direction = (targetPoint - ball.position).normalized;

            Rigidbody rb = ball.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(direction * currentKickForce, ForceMode.Impulse);

            ball.GetComponentInChildren<BallGoalChecker>().StartWaitingForResult();

            isBallKicked = true;

            MusicManager.Instance.PlayEffect(shotEffect);

            ExitKickMode();
        }
    }

    /**
        Mètode per fer Spawn del player, enemics i pilota segons Dificultat i Nivell.
    **/
    public void SpawnLevel(Difficulty difficulty)
    {
        isBallKicked = false;

        playerSpawner.SpawnPlayer(difficulty);
        ballSpawner.SpawnBall(difficulty, level);
        enemySpawner.SpawnEnemies(difficulty);
    }

    /**
        Mètode per activar la camera de KickMode i pausar moviment del personatge i enemics.
    **/
    public void EnterKickMode()
    {
        isInKickMode = true;
        mainCamera.enabled = false;
        kickCamera.enabled = true;

        ThirdPersonController.isPaused = true;
        EnemyAI.isPaused = true;
    }

    /**
        Mètode per desactivar la camera de KickMode i tornar a la 3ra persona del player.
    **/
    public void ExitKickMode()
    {

        isInKickMode = false;
        currentKickForce = 0f;
        isCharging = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        mainCamera.enabled = true;
        kickCamera.enabled = false;

        ThirdPersonController.isPaused = false;
        EnemyAI.isPaused = false;

    }

    /**
        Mètode per registrar un Goal i comprovar final partida.
    **/
    public void RegisterGoal()
    {
        goals++;
        totalGoals++;
        currentShoots++;
        Debug.Log("GOL!");

        if (level < maxShoots) level++;

        uIManager.UpdateGoalsText();
        uIManager.UpdatePanel(true);

        uIManager.ShowGoalText();

        CheckLevelEnd(true);
    }

    /**
        Mètode per registrar un Fail i comprovar final partida.
    **/
    public void RegisterFail()
    {
        fails++;
        currentShoots++;
        Debug.Log("FAIL!");

        uIManager.UpdatePanel(false);

        uIManager.ShowFailText();

        CheckLevelEnd(false);

    }

    /**
        Mètode per comprovar si la partida esta acabada o encara te intents.
    **/
    private void CheckLevelEnd(bool isGoal)
    {
        if (currentShoots == maxShoots)
        {
            EndGame(false);
        }
        else
        {
            StartCoroutine(NextLevelCountDown(isGoal));
        }

    }

    /**
        Mètode per fer un compte enrere per al següent nivell o per repetir el nivell.
    **/
    private IEnumerator NextLevelCountDown(bool isGoal)
    {
        isWaitingNextLevel = true;

        int secondsLeft = levelCountDown;
        while (secondsLeft > 0)
        {
            uIManager.ShowCountdown(isGoal, secondsLeft);
            yield return new WaitForSeconds(1f);
            secondsLeft--;
        }

        uIManager.HideCountDown();

        isWaitingNextLevel = false;

        uIManager.UpdateLevel();

        SpawnLevel(currentDifficulty);
    }

    /**
        Mètode per mostrar la següent dificultat del nivell (només en mode Champion).
    **/
    private void NextDifficultyPanel()
    {
        isWaitingNextLevel = true;

        uIManager.ShowDifficultyPanel(currentDifficulty + 1);
    }

    /**
         Mètode per ocultar el compte enrere i carregar la següent dificultat (només en mode Champion).
    **/
    public void HideDifficultyPanel()
    {
        uIManager.HideCountDown();

        currentDifficulty++;
        level = 1;
        currentShoots = 0;
        goals = 0;
        fails = 0;

        isWaitingNextLevel = false;

        uIManager.UpdateLevel();
        uIManager.ResetPanel();
        uIManager.UpdateDifficultyText(currentDifficulty);

        isGameOver = false;

        SpawnLevel(currentDifficulty);
    }

    /**
         Mètode per comprovar si es el final de partida i el seu tipus (t'han tocat, has fallat gols o has guanyat el partit).
    **/
    public void EndGame(bool isTouched)
    {
        isGameOver = true;

        if (isTouched)
        {
            Debug.Log("ENDGAME TOUCHED");
            ExitKickMode();
            uIManager.ShowEndGameUI(currentDifficulty);
            MusicManager.Instance.StopLoopingEffect();
            return;
        }

        if (currentMode == Mode.Normal)
        {
            if (fails == 0)
            {
                StartCoroutine(ShowVictoryScene());
                MusicManager.Instance.StopLoopingEffect();
            }
            else
            {
                uIManager.ShowEndGameUI(currentDifficulty);
            }
        }
        else if (currentMode == Mode.Champion)
        {
            if (fails == 0)
            {
                if (currentDifficulty < Difficulty.Hard)
                {
                    NextDifficultyPanel();
                }
                else
                {
                    StartCoroutine(ShowVictoryScene());
                }
            }
            else
            {
                uIManager.ShowEndGameUI(currentDifficulty);
                MusicManager.Instance.StopLoopingEffect();
            }
        }
    }

    /**
         Mètode per mostrar la escena de Victoria.
    **/
    public IEnumerator ShowVictoryScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("VictoryScene");
        MusicManager.Instance.StopLoopingEffect();

    }

    /**
         Mètode per tornar al Menú Principal.
    **/
    public void ExitToMenu()
    {
        MusicManager.Instance.StopLoopingEffect();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");

    }




}