using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio; // 추가


/*
 * 
 * // 배경음 실행
SoundManager.Instance.PlayBGM(0);

// 노트 히트음
SoundManager.Instance.PlayNoteHit(1);

// 버튼 클릭음
SoundManager.Instance.PlaySFX(2);
*
*
*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer audioMixer;  // 인스펙터에서 연결할 AudioMixer

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
     * // UI 슬라이더에서 호출
        public void OnBGMVolumeChanged(float value)
        {
            SoundManager.Instance.SetBGMVolume(value);  // value = 0.0 ~ 1.0
        }
    */

    // 0.0001 ~ 1.0 사이 값 → -80 ~ 0 dB로 변환
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
