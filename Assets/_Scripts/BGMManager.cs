using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{

    public AudioClip[] BGMS;

    private AudioSource _playingBGM;
    private int _bgmNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        _bgmNum = Random.Range(0, BGMS.Length);
        _playingBGM = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(!_playingBGM.isPlaying)
        {
            _bgmNum++;
            if (BGMS.Length <= _bgmNum)
            {
                _bgmNum = 0;
            }
            _playingBGM.clip = BGMS[_bgmNum];
            _playingBGM.Play();
        }
    }

}
