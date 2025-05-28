using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public UIManagerMenu uIManager;

    public Animator animator;

    private string[] difficulties = { "EASY", "MEDIUM", "HARD" };
    private string[] modes = { "NORMAL", "CHAMPION" };

    private int currentDifficulty = 0;

    private int currentMode = 0;

    public Transform[] cameraPositions;

    public Camera mainCamera;

    public float cameraMoveDuration = 1f;

    private Coroutine moveCameraCoroutine;

    private int characterSelected = 0;

    /**
        Aplicar les dificultats per defecte i fer animació de Fade per mostrar el Menú.
    **/
    void Start()
    {
        PlayerPrefs.SetInt("Difficulty", currentDifficulty);
        PlayerPrefs.SetInt("Mode", currentMode);
        PlayerPrefs.Save();

        UpdateUI();

        animator.SetTrigger("Fade");
    }   

    /**
         Començar el joc amb el personatge seleccionat.
    **/
    public void PlayGame()
    {
        PlayerPrefs.SetInt("CharacterSelected", characterSelected);
        SceneManager.LoadScene("GameplayScene");
    }

    /**
         Sortir del joc.
    **/
    public void ExitGame()
    {
        Application.Quit();
    }

    /**
         Modificar a la següent dificultat.
    **/
    public void NextDifficulty()
    {
        if (currentDifficulty < difficulties.Length - 1)
        {
            currentDifficulty++;
            UpdateUI();
        }
    }

    /**
         Modificar a l'anterior dificultat.
    **/
    public void PreviousDifficulty()
    {
        if (currentDifficulty > 0)
        {
            currentDifficulty--;
            UpdateUI();
        }
    }

    /**
         Modificar al següent mode.
    **/
    public void NextMode()
    {
        if (currentMode < modes.Length - 1)
        {
            currentMode++;
            UpdateUI();
        }
    }

    /**
         Modificar a l'anterior mode.
    **/
    public void PreviousMode()
    {
        if (currentMode > 0)
        {
            currentMode--;
            UpdateUI();
        }
    }

    /**
         Modificar la camera i el jugador seleccionat.
    **/
    public void PlayerZlatan()
    {
        characterSelected = 0;
        uIManager.UpdatePlayerText(characterSelected);
        MoveCameraToPlayer();

    }

    /**
         Modificar la camera i el jugador seleccionat.
    **/
    public void PlayerMessi()
    {
        characterSelected = 1;
        uIManager.UpdatePlayerText(characterSelected);
        MoveCameraToPlayer();
    }

    /**
        Posicionar la camera en el personatge seleccionat.
    **/
    private void MoveCameraToPlayer()
    {
        Transform targetPos = cameraPositions[characterSelected];

        if (moveCameraCoroutine != null)
            StopCoroutine(moveCameraCoroutine);

        moveCameraCoroutine = StartCoroutine(SmoothMoveCamera(targetPos));
    }

    /**
         Aplicar animació suau a la camera.
    **/
    private IEnumerator SmoothMoveCamera(Transform target)
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            float t = elapsed / cameraMoveDuration;

            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = endPos;
        mainCamera.transform.rotation = endRot;
    }

    /**
         Modificar els text de dificultat i mode.
    **/
    private void UpdateUI()
    {
        uIManager.UpdateDifficultyText(difficulties[currentDifficulty]);
        uIManager.UpdateModeText(modes[currentMode]);
    }

    /**
         Tancar opcions amb valors per defecte.
    **/
    public void CloseOptions()
    {
        currentDifficulty = 0;
        currentMode = 0;

        PlayerPrefs.SetInt("Difficulty", currentDifficulty);
        PlayerPrefs.SetInt("Mode", currentMode);

        UpdateUI();

        uIManager.BackToMenu();
    }

    /**
         Tancar opcions amb valors seleccionats.
    **/
    public void SaveOptionsPreferences()
    {
        PlayerPrefs.SetInt("Difficulty", currentDifficulty);
        PlayerPrefs.SetInt("Mode", currentMode);

        UpdateUI();

        uIManager.BackToMenu();
    }
}
