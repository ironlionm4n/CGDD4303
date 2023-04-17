using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [Header("Skyboxes")]
    [SerializeField] private Material daytime;
    [SerializeField] private Material sunset;
    [SerializeField] private Material dusk;

    [Header("Directional Light")]
    [SerializeField] private Light lt;

    void Start()
    {
        RenderSettings.skybox = daytime;
        lt.color = new Vector4(255.0f / 255.0f, 244.0f / 255.0f, 214.0f / 255.0f, 1);
    }

    public void changeSkybox(int newSkybox)
    {
        if (newSkybox == 0)
        {
            RenderSettings.skybox = daytime;
            lt.color = new Vector4(255.0f / 255.0f, 244.0f / 255.0f, 214.0f / 255.0f, 1);
        } else if (newSkybox == 1)
        {
            RenderSettings.skybox = sunset;
            lt.color = new Vector4(255.0f / 255.0f, 176.0f / 255.0f, 90.0f / 255.0f, 1);
        } else
        {
            RenderSettings.skybox = dusk;
            lt.color = new Vector4(212.0f / 255.0f, 131.0f / 255.0f, 137.0f / 255.0f, 1);
        }
    }
}
