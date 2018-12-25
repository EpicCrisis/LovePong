using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour 
{
	public Sound[] sounds;
	public Sound[] soundFX;

	public static AudioManager instance;

    private void Start()
    {
        Play("HeartBeat");
    }

    void Awake()
	{
		if (instance == null) {
			instance = this;
		}
		else
		{
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.playOnAwake = s.awake;
		}

		foreach (Sound fx in soundFX) {
			fx.source = gameObject.AddComponent<AudioSource> ();
			fx.source.clip = fx.clip;

			fx.source.volume = fx.volume;
			fx.source.pitch = fx.pitch;
			fx.source.loop = fx.loop;
			fx.source.playOnAwake = fx.awake;
		}
	}

	public void Play(string name)
	{
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null) 
		{
			Sound fx = Array.Find (soundFX, sound => sound.name == name);
			if (fx == null) {
				Debug.LogWarning ("Sound: " + name + " not found!");
				return;
			}
			fx.source.Play ();
		} else {
			s.source.Play ();
		}
	}

	public void Stop(string name)
	{
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null) {
			Sound fx = Array.Find (soundFX, sound => sound.name == name);
			if (fx == null) {
				Debug.LogWarning ("Sound: " + name + " not found!");
				return;
			} else {
				fx.source.Stop ();
			}
		} else {
			s.source.Stop ();
		}
	}

	public void SFXvolume(float volume)
	{
		foreach (Sound sfx in soundFX) {
			sfx.source.volume = volume;
		}
	}

	public void BGMvolume(float volume)
	{
		foreach (Sound s in sounds) {
			s.source.volume = volume;
		}
	}
		
	private void SFXLoader()
	{
		if (PlayerPrefs.HasKey ("SFXLevel")) {
			instance.SFXvolume(PlayerPrefs.GetFloat ("SFXLevel"));
		}
	}

	private void BGMLoader()
	{
		if (PlayerPrefs.HasKey ("BGMLevel")) {
			instance.BGMvolume(PlayerPrefs.GetFloat ("BGMLevel"));
		}
	}
}
