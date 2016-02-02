using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundEffects
{
	Sacrifice,
	Cut,
	Splatter
}

public class AudioManager : MonoBehaviour 
{

	public static AudioManager Instance = null;
	private AudioSource[] effectSources;


	private bool splatterPlaying = false;
	private bool gameStarted = false;
	private float splatterTimer = 0f;
	private float splatterMinInterval = 0.2f;

	[SerializeField]
	private float sfxVolume = 0.8f;
	
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
		
		effectSources = new AudioSource[6];
		for (int i = 0; i < effectSources.Length; i++)
		{
			effectSources[i] = gameObject.AddComponent<AudioSource>();
		}

	}

	void Update()
	{
		if (splatterTimer <= 0)
		{
			splatterPlaying = false;
		}

		if (splatterPlaying)
		{
			splatterTimer -= Time.deltaTime;
		}

	}

	
	public void PlaySound(SoundEffects sound, float delay, float volumeMultiplier)
	{
		AudioClip clip = null;
		int randomClipIndex = 0;


		switch (sound)
		{
		case SoundEffects.Sacrifice:
			randomClipIndex = Random.Range(1, 3);
			clip = Resources.Load("sacrifice_" + randomClipIndex.ToString()) as AudioClip;
			break;
		case SoundEffects.Splatter:
			if (splatterPlaying || !gameStarted)
			{
				return;
			}
			randomClipIndex = Random.Range(1, 5);
			clip = Resources.Load("splatter_" + randomClipIndex.ToString()) as AudioClip;
			splatterPlaying = true;
			splatterTimer = splatterMinInterval;
			break;
		case SoundEffects.Cut:
			randomClipIndex = Random.Range(1, 1);
			clip = Resources.Load("cut_" + randomClipIndex.ToString()) as AudioClip;
			break;
		}
		
		if (clip == null)
		{
			return;
		}
		
		var source = effectSources[0];
		
		for (int i = 0; i < effectSources.Length && source.isPlaying; i++)
		{
			source = effectSources[i];
		}
		if (source.isPlaying && source.time < 0.15f)
		{
			return;
		}
		
		source.Stop();
		source.pitch = Random.Range(0.95f, 1.05f);
		source.clip = clip;
		source.volume = volumeMultiplier * sfxVolume;

		source.PlayDelayed(delay);
	}


	public void SetGameStarted()
	{
		StartCoroutine (DelayGameStarted());
	}

	protected IEnumerator DelayGameStarted()
	{
		yield return new WaitForSeconds(0.2f);
		gameStarted = true;
	}
}
