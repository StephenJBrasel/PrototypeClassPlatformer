using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesToggle : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particlesToToggle;
    private PlayerControllerCC playerControllerCC;

    // Start is called before the first frame update
    void Awake()
    {
        playerControllerCC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCC>();
        playerControllerCC.OnTimeSlowEvent.AddListener(ToggleParticles);
        playerControllerCC.OnTimeResumeEvent.AddListener(ToggleParticles);
    }

    public void ToggleParticles()
    {
        for (int i = 0; i < particlesToToggle.Count; i++)
        {
            particlesToToggle[i].gameObject.SetActive(!particlesToToggle[i].gameObject.activeSelf);
            //var emission = particlesToToggle[i].emission;
            //emission.enabled = !particlesToToggle[i].emission.enabled;
        }
    }
}
