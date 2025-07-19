using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio; // �߰�


/*
 * 
 * // ����� ����
SoundManager.Instance.PlayBGM(0);

// ��Ʈ ��Ʈ��
SoundManager.Instance.PlayNoteHit(1);

// ��ư Ŭ����
SoundManager.Instance.PlaySFX(2);
*
*
*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer audioMixer;  // �ν����Ϳ��� ������ AudioMixer

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource noteSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public List<AudioClip> bgmClips;

    [Header("Note Hit Clips")]
    public List<AudioClip> noteClips;

    [Header("SFX Clips")]
    public List<AudioClip> sfxClips;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.Instance.onNoteDestroyed.AddListener((_, judge) =>
        {
            PlayNoteHit((int)judge);
        });
    }

    // ------------------------
    // BGM
    // ------------------------

    public void PlayBGM(int index, bool loop = true)
    {
        if (index < 0 || index >= bgmClips.Count) return;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PauseBGM() => bgmSource.Pause();

    public void ResumeBGM() => bgmSource.UnPause();

    // ------------------------
    // Note Hit
    // ------------------------

    public void PlayNoteHit(int index = 0)
    {
        if (index < 0 || index >= noteClips.Count) return;
        noteSource.PlayOneShot(noteClips[index]);
    }

    // ------------------------
    // SFX (UI, etc.)
    // ------------------------

    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Count) return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    /*
     * 
     * // UI �����̴����� ȣ��
        public void OnBGMVolumeChanged(float value)
        {
            SoundManager.Instance.SetBGMVolume(value);  // value = 0.0 ~ 1.0
        }
    */

    // 0.0001 ~ 1.0 ���� �� �� -80 ~ 0 dB�� ��ȯ
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void SetNoteVolume(float volume)
    {
        audioMixer.SetFloat("NoteVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

}
