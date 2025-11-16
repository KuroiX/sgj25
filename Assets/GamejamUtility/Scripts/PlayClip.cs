using System;
using UnityEngine;

public class PlayClip : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        AudioManager.Instance.PlaySoundEffect(audioClip);
    }
}