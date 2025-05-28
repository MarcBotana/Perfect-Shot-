using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;

    public float maxMusicVolume = 0.5f;

    public float maxEffectsVolume = 0.2f;

    public float musicVolume = 0.5f;

    public float effectsVolume = 0.2f;

    public AudioSource musicSource;

    public AudioSource effectsSource;


    /**
         Instanciar el MusicManager per a la resta del joc.
    **/
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.volume = maxMusicVolume;
        effectsSource.volume = maxEffectsVolume;

    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = Mathf.Clamp01(newVolume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public float GetMaxMusicVol()
    {
        return maxMusicVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetEffectsVolume(float newVolume)
    {
        effectsVolume = Mathf.Clamp01(newVolume);
        effectsSource.volume = effectsVolume;
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
        PlayerPrefs.Save();
    }

    public float GetMaxEffectsVol()
    {
        return maxEffectsVolume;
    }

    public float GetEffectsVolume()
    {
        return effectsVolume;
    }

    /**
         Mètode per fer sonar un só sense Loop (només un cop).
    **/
    public void PlayEffect(AudioClip clip)
    {
        if (clip == null) return;
        effectsSource.PlayOneShot(clip, effectsVolume);
    }

    /**
         Mètode per fer sonar un só en Loop.
    **/
    public void PlayLoopingEffect(AudioClip clip)
    {
        if (clip == null) return;

        effectsSource.clip = clip;
        effectsSource.volume = effectsVolume;
        effectsSource.loop = true;
        effectsSource.Play();
    }

    /**
         Mètode per aturar el só que estigui en Loop.
    **/
    public void StopLoopingEffect()
    {
        effectsSource.Stop();
    }

}
