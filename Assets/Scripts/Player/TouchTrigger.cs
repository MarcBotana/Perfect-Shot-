using System.Collections;
using UnityEngine;

public class TouchTrigger : MonoBehaviour
{
    public GameManager gameManager;

    public Animator playerAnimator;

    /**
         Comprovar col·lisió entre el Player i els enemics.
    **/
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        playerAnimator.SetTrigger("Fall");

        StartCoroutine(TriggerFallSequence());
    }

    /**
         Esperar a acabar la animació per presentar la pantalla de GameOver.
    **/
    private IEnumerator TriggerFallSequence()
    {

        yield return new WaitForSeconds(0.5f);

        gameManager.isGameOver = true;

        yield return new WaitForSeconds(1.5f);

        gameManager.EndGame(true);
    }

}
