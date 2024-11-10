using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]

public class S_SoundParameters

{
    public string _Name;

    public AudioClip[] _Clip;
    [Range(0f,1f)]
    public float _Volume;
    [Range(0.1f,2f)]
    public float _Pitch;
    [Range(0f,1f)]
    public float _SpatialBlend;
    public AudioRolloffMode _RolloffMode;
    [Min(1)]
    public float _MaxDistance;
    
    public float _MinDistance;
    public bool _Loop;
    public bool _PlayOnAwake;

    [HideInInspector]
    public AudioSource _Source;
    public AudioMixerGroup _AudioMixerGroup;

    

}
