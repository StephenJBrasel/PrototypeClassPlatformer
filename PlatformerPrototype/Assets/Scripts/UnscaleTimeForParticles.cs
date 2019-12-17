using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnscaleTimeForParticles : MonoBehaviour
{
    private ParticleSystem pSystem;
    private ParticleSystem.MainModule pMain;

    private void Awake()
    {
        pSystem = GetComponent<ParticleSystem>();
        pMain = pSystem.main;
    }

    private void Update()
    {
        //pMain.useUnscaledTime = true;
        pSystem.Simulate(Time.unscaledDeltaTime, true, false);
    }
}
