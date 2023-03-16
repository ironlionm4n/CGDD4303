using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildSFX : MonoBehaviour
{
    [SerializeField] private List<AudioSource> woodSfx;
    [SerializeField] private AudioSource woodSfx1;
    [SerializeField] private AudioSource woodSfx2;
    [SerializeField] private AudioSource tieSfx;
    public void PlayWoodSFX()
    {
        woodSfx[Random.Range(0, woodSfx.Count)].Play();
    }

    public void PlayTieSFX()
    {
        tieSfx.Play();
    }
}
