using UnityEngine.Audio;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public enum SoundType
    {
        Music,
        Ambient,
        SoundFX
    }

    public SoundType soundType;
    public AudioMixerGroup mixerGroup;
    [Range(0f, 1f)]
    private float volume = 1f;
    [Range(0.1f, 3f)]
    private float pitch = 1f;
    //[Header("Set to true if you want this clip too loop indefinitely")]
    private bool loop;
    public AudioClip[] clips;
    [HideInInspector]
    public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            if (gameObject.TryGetComponent(out audioSource) == false)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        if (clips.Length <= 0)
            Debug.LogError("No AudioClips added to " + gameObject.name);
        else
            InitAudioSource();
    }

    private void Start()
    {
        
    }

    public AudioClip GetClip(string clipName)
    {
        for(int i = 0; i < clips.Length; i++)
        {
            if(clips[i].name.CompareTo(clipName) == 0)
            {
                return clips[i];
            }
        }
        Debug.LogWarning("Audio clip not found in " + gameObject.name);
        return null;
    }

    private void InitAudioSource()
    {
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip = clips[0];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
    }
}

