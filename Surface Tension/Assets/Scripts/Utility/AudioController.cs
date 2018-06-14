using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
	public struct Audio {
		public SoundEffectType type;
		public AudioClip audioClip;
		public AudioSource audioSource;
		public bool looping;
		public bool playonAwake;
	}

	public enum SoundEffectType {
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
	/// Mapping of sound effect types onto sound effects
	/// </summary>
	public Dictionary<SoundEffectType, AudioClip> soundEffects;

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
				type = SoundEffectType.MUSIC,
				audioClip = music,
				audioSource = null,
				looping = true,
				playonAwake = true
			},
			new Audio {
				type = SoundEffectType.JUMP,
				audioClip = jumpEffect,
				audioSource = null,
				looping = false,
				playonAwake = false
			},
			new Audio {
				type = SoundEffectType.MATERIAL_CHANGE,
				audioClip = materialChange,
				audioSource = null,
				looping = false,
				playonAwake = false
			}
		};

		// Add the bounce sound effects
		foreach(AudioClip clip in bounceEffects) {
			audioList.Add(new Audio {
				type = SoundEffectType.BOUNCE,
				audioClip = clip,
				audioSource = null,
				looping = false,
				playonAwake = false
			});
		}

		// Trim unassigned audio clips from audio list
		audioList.FindAll(a => a.audioClip == null).ForEach(a => audioList.Remove(a));

		// Create audio source for associated audio clip
		foreach(Audio audio in audioList) {
			InitializeAudioSource(audio);
			Debug.Log(audio.type + " audio source is null: " + (audio.audioSource == null).ToString());
		}

		PlaySoundEffect(SoundEffectType.MUSIC);
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
	/// <param name="soundEffectType">Desired sound effect</param>
	/// <param name="looping">AudioClip loops if true</param>
	public void PlaySoundEffect(SoundEffectType soundEffectType)
	{
		// Get audioclip from dictionary
		// AudioClip audioClip = soundEffects[soundEffectType];

		// Get audio object with matching type
		List<Audio> audios = audioList.FindAll(a => a.type == soundEffectType);
		Audio audio;

		// If more than one matching audio comes back,gi choose one at random
		if(audios.Count > 1) {
			int rand = UnityEngine.Random.Range(0, audios.Count);
			audio = audios[rand];
		}
		else {
			audio = audios[0];
		}
		// Debug.Log(audio.type + " audio source is null: " + (audio.audioSource == null).ToString());

		// audio.audioSource.Play();

		AudioSource audioSource = transform.Find(audio.audioClip.name).GetComponent<AudioSource>();
		Debug.Log(audio.type + " audio source is null: " + (audioSource == null).ToString());

		audioSource.Play();
	}
}
