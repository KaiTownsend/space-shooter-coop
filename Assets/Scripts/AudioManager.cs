using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private AudioSource[] _audioSourceArray;

    private void Awake()
    {
        _audioSourceArray = new AudioSource[30];

        for (int i = 0; i < _audioSourceArray.Length; i++)
        {
            _audioSourceArray[i] = gameObject.AddComponent<AudioSource>();
            _audioSourceArray[i].clip = _audioClip;
        }
    }

    public void PlayAudioOnHit()
    {
        foreach (AudioSource audioSource in _audioSourceArray)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                break;
            } 
        }
    }
}
