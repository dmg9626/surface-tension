using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
	public enum SoundEffectType {
		BOUNCE,
		SLICK,
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
	/// Slick surface sound effect (triggered while moving on slick surface)
	/// </summary>
	public AudioClip slickEffect;

	/// <summary>
	/// Jump sound effect
	/// </summary>
	public AudioClip jumpEffect;

	/// <summary>
	/// Material change sound effect (played when changing material on surface)
	/// </summary>
	public AudioClip materialChange;

	/// <summary>
	/// Material sound effects
	/// </summary>
	public Dictionary<SoundEffectType, AudioClip> soundEffects;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		// Make sure AudioController isn't destroyed on scene change
		DontDestroyOnLoad(gameObject);
	}


	/// <summary>
	/// Plays sound effect corresponding to given SoundEffectType
	/// </summary>
	/// <param name="soundEffectType">Desired sound effect</param>
	/// <param name="looping">AudioClip loops if true</param>
	public void PlaySoundEffect(SoundEffectType soundEffectType, bool looping = false)
	{
		// Get audioclip from dictionary
		AudioClip audioClip = soundEffects[soundEffectType];

		// TODO: play audio clip
	}
}
