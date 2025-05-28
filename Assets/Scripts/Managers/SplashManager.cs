using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public Animator splashAnimator;
    public float delayAnimation = 9f;

    /**
         Començar animació.
    **/
    void Start()
    {
        StartCoroutine(SplashSequence());
    }

    /**
         Animació de logo i fade per carregar la pantalla de menú.
    **/
    IEnumerator SplashSequence()
    {
        splashAnimator.SetTrigger("Fade");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(delayAnimation);

        asyncLoad.allowSceneActivation = true;
    }

}
