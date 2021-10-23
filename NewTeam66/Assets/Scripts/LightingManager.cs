using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //references
    [SerializeField]
    private Light SunLight;

    [SerializeField]
    private LightingPreset Preset;
    //variables
    [SerializeField, Range(0,24)]
    private float timeOfDay;

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(SunLight != null)
        {
            SunLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            SunLight.transform.localRotation = Quaternion.Euler(new Vector3(((timePercent * 360f) - 90f), 170f, 0f)); 
        }
    }
    private void OnValidate()
    {
        if(SunLight != null)
        {
            return;
        }

        if(RenderSettings.sun != null)
        {
            SunLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
                if(light.type == LightType.Directional)
                {
                    SunLight = light;
                    return;
                }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Preset == null)
        {
            return;
        }
        if(Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }
}
