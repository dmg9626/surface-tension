using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
	public struct Audio {
		public SoundType type;
		public AudioClip audioClip;
		public AudioSource audioSource;
		public bool looping;
		public bool playonAwake;
	}

	public enum SoundType {
		BOUNCE,
		JUMP,
		MATERIAL_CHANGE,
		MUSIC
	};

	/// <summary>
	/// Game music
	/// </summary>
	public AudioClip music;

	/// <summary>
	/// Bounce sound effect
	/// </summary>
	public List<AudioClip> bounceEffects;

	/// <summary>
	/// Jump sound effect
	/// </summary>
	public AudioClip jumpEffect;

	/// <summary>
	/// Material change sound effect (played when changing material on surface)
	/// </summary>
	public AudioClip materialChange;

	/// <summary>
	/// List of playable audio effects
	/// </summary>
	public List<Audio> audioList;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		// Make sure AudioController isn't destroyed on scene change
		DontDestroyOnLoad(gameObject);

		// Initialize list of Audio objects
		audioList = new List<Audio> {
			new Audio {
				type = SoundType.MUSIC,
				audioClip = music,
				audioSource = null,
				looping = true,
				playonAwake = true
			},
			new Audio {
				type = SoundType.JUMP,
				audioClip = jumpEffect,
				audioSource = null,
				looping = false,
				playonAwake = false
			},
			new Audio {
				type = SoundType.MATERIAL_CHANGE,
				audioClip = materialChange,
				audioSource = null,
				looping = false,
				playonAwake = false
			}, new Audio {
				type = SoundType.BOUNCE,
				audioClip = bounceEffects[0],
				audioSource = null,
				looping = false,
				playonAwake = false
			}
		};

		// Trim unassigned audio clips from audio list
		audioList.FindAll(a => a.audioClip == null).ForEach(a => audioList.Remove(a));

		// Create audio source for associated audio clip
		foreach(Audio audio in audioList) {
			InitializeAudioSource(audio);
			Debug.Log(audio.type + " audio source is null: " + (audio.audioSource == null).ToString());
		}

		// Play music at start
		PlaySoundEffect(SoundType.MUSIC);
	}

	/// <summary>
	/// Creates an AudioSource for the provided AudioClip
	/// </summary>
	/// <param name="audio">Audio object</param>
	private void InitializeAudioSource(Audio audio)
	{
		// TODO: check if AudioSource for given audio clip already exists

		// Create child gameobject
		GameObject child = new GameObject(audio.audioClip.name);
		child.transform.SetParent(transform);

		// Give it an AudioSource fitted with audio clip
		AudioSource audioSource = child.AddComponent<AudioSource>();
		audioSource.clip = audio.audioClip;
		audioSource.loop = audio.looping;
		audioSource.playOnAwake = audio.playonAwake;

		// Give audio a reference to its audiosource
		audio.audioSource = audioSource;

		Debug.Log(audio.type + " audio source is null: " + (audio.audioSource == null).ToString());
		Debug.Log("Created AudioSource " + audioSource.name + " for clip " + audio.audioClip.name);
	}


	/// <summary>
	/// Plays sound effect corresponding to given SoundEffectType
	/// </summary>
	/// <param name="soundType">Desired sound effect</param>
	/// <param name="looping">AudioClip loops if true</param>
	public void PlaySoundEffect(SoundType soundType)
	{
		// Get audio object with matching type
		Audio audio = audioList.Find(a => a.type == soundType);

		// Find audioSource associated with that type
		AudioSource audioSource = transform.Find(audio.audioClip.name).GetComponent<AudioSource>();
		// Debug.Log(audio.type + " audio source is null: " + (audioSource == null).ToString());

		// Get random sound effect (if others found with same soundType)
		AudioClip clip = GetRandomSoundEffect(soundType);

		audioSource.PlayOneShot(clip);
	}

	/// <summary>
	/// Returns random audioclip associated with given type.
	/// If only one audioclip is associated with given type, returns that audioclip
	/// </summary>
	/// <param name="type">SoundEffectType</param>
	private AudioClip GetRandomSoundEffect(SoundType type) 
	{
		AudioClip audioClip;
		switch(type) {
			case SoundType.BOUNCE:
				int rand = Random.Range(0, bounceEffects.Count);
				audioClip = bounceEffects[rand];
				break;
			// If type only matches one sound effect return that one
			default:
				Audio audio = audioList.Find(a => a.type == type);
				return audio.audioClip;
		}

		return audioClip;
	}
}
