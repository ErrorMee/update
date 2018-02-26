using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioModel : Singleton<AudioModel>
{

    private string crtMusic = "";
    private AudioSource music;
    private AudioSource sound;

    private List<AudioSource> actorSounds = new List<AudioSource>();

	private GameObject game;

    public void InitAudio()
    {
		game = GameObject.Find ("Game");
		AudioSource[] audioSources = game.GetComponents<AudioSource>();
        if (audioSources.Length > 0)
        {
            music = audioSources[0];
            music.loop = true;

            int music_mute = PlayerPrefsUtil.GetInt(PlayerPrefsUtil.MUSIC_MUTE);
            music.mute = music_mute == 1 ? true : false;
        }
        if (audioSources.Length > 1)
        {
            sound = audioSources[1];
            sound.loop = false;
            
            int sound_mute = PlayerPrefsUtil.GetInt(PlayerPrefsUtil.SOUND_MUTE);
            sound.mute = sound_mute == 1 ? true : false;
        }
    }

    public bool GetIsMute(bool isSound = true)
    {
        if (isSound)
        {
            return sound.mute;
        }
        else
        {
            return music.mute;
        }
    }

    public void SetMute(bool isSound,bool isMute)
    {
        if (isSound)
        {
            sound.mute = isMute;
            PlayerPrefsUtil.SetInt(PlayerPrefsUtil.SOUND_MUTE, isMute ? 1 : 0);
        }
        else
        {
            music.mute = isMute;
            PlayerPrefsUtil.SetInt(PlayerPrefsUtil.MUSIC_MUTE, isMute ? 1 : 0);
        }
    }

    public void PlayMusic(string audioName)
    {
        if (crtMusic != audioName)
        {
            crtMusic = audioName;
            music.clip = ResModel.Instance.GetAudioClip("audio/music/" + "music_" + audioName);
            if (music.clip != null)
            {
                music.Play();
            }
            else
            {
                Debug.Log("music_" + audioName + " is null");
            }
        }
    }

    public void PlayeSound(string audioName)
    {
        if (sound.clip == null || sound.clip.name != audioName)
        {
			sound.clip = ResModel.Instance.GetAudioClip("audio/sound/" + audioName);
        }
        if (sound.clip != null)
        {
            sound.Play();
        }
        else
        {
            Debug.Log("sound_" + audioName + " is null");
        }
    }

    public void PlayeActorSound(string audioName)
    {
        if (!sound.mute)
        {
			AudioSource actorSound = game.AddComponent<AudioSource>();

            actorSounds.Add(actorSound);

			actorSound.clip = ResModel.Instance.GetAudioClip("audio/sound/" + audioName);

            if (actorSound.clip != null)
            {
                actorSound.Play();
            }
            else
            {
                Debug.Log("sound_" + audioName + " is null");
            }

        }
    }

    public void ClearAllActorSound()
    {
        LeanTween.delayedCall(0.5f, DelayClearAllActorSound);
    }

    private void DelayClearAllActorSound()
    {
        while (actorSounds.Count > 0)
        {
            AudioSource actorSound = actorSounds[0];
            actorSounds.RemoveAt(0);
            GameObject.Destroy(actorSound);
        }
    }
}
