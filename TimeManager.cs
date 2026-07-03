using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    Material skyMat;
    public float OneDay_of_GamingTime = 600f; 
    public float GameTime_10 = 6f;

    public void Start()
    {
        skyMat = RenderSettings.skybox;
    }
    void Update()
    {
        GameTime_10 += (24f / OneDay_of_GamingTime) * Time.deltaTime;
        if (GameTime_10 >= 24f) GameTime_10 -= 24f;
        OnTimeChanged(GameTime_10);
        if(GameTime_10 <= 12)
        {
            skyMat.SetFloat("_CubemapTransition", (-GameTime_10 / 12) + 1);
        }
        else
        {
            skyMat.SetFloat("_CubemapTransition", (GameTime_10 / 12) - 1);
        }
        
    }

    public delegate void TimeChangeAction(float time);
    public static event TimeChangeAction OnTimeChanged;
}
