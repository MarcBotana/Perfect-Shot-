using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public UIManagerVictory uIManager;
    public GameManager.Mode mode;

    public GameManager.Difficulty difficulty;

    public int characterPlayed = 0;

    public Transform[] cameraPositions;

    public Camera mainCamera;

    public Transform players;

    /**
        Carregar text de dificultat i mode.
    **/
    void Start()
    {
        int playerDiff = PlayerPrefs.GetInt("Difficulty");
        int playerMode = PlayerPrefs.GetInt("Mode");
        characterPlayed = PlayerPrefs.GetInt("CharacterSelected", 0);

        mode = (GameManager.Mode)playerMode;

        if (mode == GameManager.Mode.Normal)
        {
            difficulty = (GameManager.Difficulty)playerDiff;
        }
        else if (mode == GameManager.Mode.Champion)
        {
            difficulty = GameManager.Difficulty.Easy;
        }

        uIManager.UpdateUI();
        
        players.GetChild(characterPlayed).gameObject.SetActive(true);

        Animator animator = players.GetChild(characterPlayed).GetChild(0).GetComponent<Animator>();

        MoveCameraToPlayer();

        PlayRandomDance(animator);
        
    }

    /**
         Mètode per posicionar la camera en el Player seleccionat.
    **/
    private void MoveCameraToPlayer()
    {
        Transform targetPos = cameraPositions[characterPlayed];

        mainCamera.transform.position = targetPos.position;
        mainCamera.transform.rotation = targetPos.rotation;
    }

    /**
         Mètode per fer ballar al jugador un ball aleatori.
    **/
    private void PlayRandomDance(Animator animator)
    {
        int danceID = Random.Range(1, 6);
        Debug.Log(danceID);
        animator.SetInteger("Dance", danceID);
    }

    /**
         Mètode per tornar al Menú Principal.
    **/
    public void ExitToMenu()
    {
        players.GetChild(characterPlayed).gameObject.SetActive(false);
        SceneManager.LoadScene("MenuScene");


    }


    
}
