using UnityEngine;

public class TitleSounds : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private int musicIndexToPlay = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (soundManager != null)
        {
            soundManager.PlayMusicTrack(musicIndexToPlay);
        }
        else
        {
            SoundManager globalSoundManager = Object.FindFirstObjectByType<SoundManager>();
            if (globalSoundManager != null)
            {
                globalSoundManager.PlayMusicTrack(musicIndexToPlay);
            }
            else
            {
                Debug.LogWarning("No SoundManager found in the scene to play title music.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
