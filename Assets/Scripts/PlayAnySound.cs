using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnySound : MonoBehaviour
{
    public string soundName;

    public void PlaySound()
    {
        SoundManager.PlaySFX(soundName);
    }
}
