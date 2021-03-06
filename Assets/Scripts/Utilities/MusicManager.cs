using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public bool PickRandom = false;

    public AudioClip[] AudioClips;

    public AudioSource AudioSource;

    private int _counter = 0;

    private List<int> _alreadyPlayed = new List<int>();

    private float audioVolume = 0.075f;
    
    [SerializeField] private SettingsManager _settingsManager;

    void ReloadSetings()
    {
        if (PlayerPrefs.GetInt("ToggleAudio", 1) != 0)
        {
            AudioSource.volume = audioVolume;
        }
        else
        {
            AudioSource.volume = 0;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ReloadSetings();
        _settingsManager.addSettingsEventListener(ReloadSetings);
        
        int r;

        if (PickRandom == false && AudioClips.Length > 0)
        {
            r = _counter;
            _counter++;
        }
        else
        {
            r = Random.Range(0, AudioClips.Length);
        }

        AudioSource.clip = AudioClips[r];

        AudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AudioSource.isPlaying)
        {
            int r;

            if (_counter >= AudioClips.Length )
            {
                _counter = 0;
            }

            if (PickRandom == false)
            {
                r = _counter;
                _counter++;
            }
            else
            {
                r = Random.Range(0, AudioClips.Length);
            }


            AudioSource.clip = AudioClips[r];

            AudioSource.Play();
        }
    }
}
