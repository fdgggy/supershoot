using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAudio : MonoBehaviour
{
    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

    public void PlayAudio(string audioName, bool isLoop = false)
    {
        AudioClip clip = null;
        audioCache.TryGetValue(audioName, out clip);
        if (clip == null)
        {
            ResManager.Instance.LoadAudio(audioName, (string assetName, object go) => {
                clip = go as AudioClip;
                if (clip != null)
                {
                    audioCache.Add(assetName, clip);

                    AudioSource source = GetAudioSource();
                    if (source != null)
                    {
                        source.clip = clip;
                        source.loop = isLoop;
                        source.Play();
                    }
                }
            });
        }
        else
        {
            AudioSource source = GetAudioSource();
            if (source != null)
            {
                source.clip = clip;
                source.loop = isLoop;
                source.Play();
            }
        }
    }

    private AudioSource GetAudioSource()
    {
        AudioSource[] arrSource = gameObject.GetComponents<AudioSource>();
        if (null == arrSource || 0 == arrSource.Length)
        {
            return gameObject.AddComponent<AudioSource>();
        }

        for (uint i = 0; i < arrSource.Length; i++)
        {
            if (!arrSource[i].isPlaying)
            {
                return arrSource[i];
            }
        }

        if (arrSource.Length > 30)
        {
            return null;
        }

        return gameObject.AddComponent<AudioSource>();
    }

    public void Clear()
    {
        AudioSource[] arrSource = gameObject.GetComponents<AudioSource>();
        if (null != arrSource ||  arrSource.Length > 0)
        {
            for (uint i = 0; i < arrSource.Length; i++)
            {
                if (arrSource[i].isPlaying)
                {
                    arrSource[i].Stop();
                }
            }
        }

        audioCache.Clear();
        GameObject.Destroy(gameObject);
    }
}












