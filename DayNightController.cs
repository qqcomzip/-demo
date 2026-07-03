using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateLighting;
    }

    void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateLighting;
    }

    void UpdateLighting(float time)
    {
        float t = time / 24f;

        if(time >= 5 && time <= 19)
        sunLight.transform.rotation = Quaternion.Euler((time * 15) - 90f, 0, 0);

        sunLight.color = sunColor.Evaluate(t);
        sunLight.intensity = sunIntensity.Evaluate(t);

        moonLight.enabled = (time < 6f || time > 18f);
    }
}
