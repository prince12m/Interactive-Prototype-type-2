using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic backgroundMusicInstance;

    private void Awake()
    {
        if(backgroundMusicInstance != null && backgroundMusicInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        backgroundMusicInstance = this;
        DontDestroyOnLoad(this);
    }
}
