using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private Sound[] sounds;
	[SerializeField] private readonly string SFXSlider = "SoundsSliderBar";
	[SerializeField] private readonly string MusicSlider = "MusicSliderBar";

	[Range(0.5f, 1.0f)]
	[SerializeField] private float pitchRangeMin;
	[Range(1.0f, 1.5f)]
	[SerializeField] private float pitchRangeMax;

	[SerializeField] private int maxVolume = 100;

    private Sound temp;
    Sound temp2;
    Sound s;
    float SFXsliderVolume;
    float MUSICsliderVolume;



    // Start is called before the first frame update
    void Awake()
    {
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.spatialBlend = s.spatialBlend;
		}

    }

    void Update()
    {
        if (temp.type == Sound.SoundType.SFX)
        {
            SFXsliderVolume = (float)PlayerPrefs.GetInt(SFXSlider, 100) / (float)maxVolume;
            temp.source.volume = temp.volume * SFXsliderVolume;
        }else if(temp.type == Sound.SoundType.Music)
        {
            MUSICsliderVolume = (float)PlayerPrefs.GetInt(MusicSlider, 100) / (float)maxVolume;
            temp.source.volume = temp.volume * MUSICsliderVolume;
        }

    }

    public void Play(string name, bool pitchModulate = true)
	{
        s = Array.Find(sounds, sound => sound.name == name);

        if(s != null)
		{

			switch (s.type)
			{
				case Sound.SoundType.SFX:
					s.source.pitch = UnityEngine.Random.Range(pitchRangeMin, pitchRangeMax);
					SFXsliderVolume = (float)PlayerPrefs.GetInt(SFXSlider, 100)/ (float)maxVolume;
                    s.source.volume = s.volume * SFXsliderVolume;
                    break;
				case Sound.SoundType.Music:
					MUSICsliderVolume = (float)PlayerPrefs.GetInt(MusicSlider, 100) / (float)maxVolume;
                    s.source.volume = s.volume * MUSICsliderVolume;
                    temp = s;
                    break;
				default:
                    SFXsliderVolume = 0;
                    MUSICsliderVolume = 0;
                    break;
			}
			s.source.Play();

		}
	}
    

    public void Stop()
    {
        if (temp == null)
        {
            Debug.Log("No music playing");
        }
        else
        {
            temp.source.Stop();
        }
    }
}
