using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgMusic1;
    [SerializeField] private AudioSource bgMusic2;
    [SerializeField] private AudioSource bgMusic3;
    [SerializeField] private AudioSource woodMusic1;
    [SerializeField] private AudioSource woodMusic2;
    [SerializeField] private AudioSource metalMusic1;

    
    private AudioSource _currentSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _currentSource = bgMusic1;
            _currentSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _currentSource = bgMusic2;
            _currentSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            _currentSource = bgMusic3;
            _currentSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            _currentSource = woodMusic1;
            _currentSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            _currentSource = woodMusic2;
            _currentSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            _currentSource = metalMusic1;
            _currentSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _currentSource.Stop();
        }
    }
}
