using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;
	public GameObject oneShotAudio;
	AudioSource m_musicTrack = null;
	string old_clip_name = "none";
	public bool Paused = false;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		/*foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}*/
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
	public void PlayClipAtPos(AudioClip ac, Vector3 pos, float baseVolume = 0.5f, float volumeRange = 0f, float pitchRange = 0f, float destroyAfter = 1f) {
		GameObject go = Instantiate (oneShotAudio, pos, Quaternion.identity);

		AudioSource ausource = go.AddComponent<AudioSource>();
		ausource.minDistance = 1f;
		ausource.maxDistance = 5f;
		ausource.spatialBlend = 1f;
		ausource.dopplerLevel = 0f;
		ausource.clip = ac;
		ausource.volume = baseVolume * (1f + UnityEngine.Random.Range(-volumeRange / 2f, volumeRange / 2f));
		ausource.pitch = 1f * (1f + UnityEngine.Random.Range(-pitchRange / 2f, pitchRange / 2f));
		ausource.Play ();
		Destroy (go, destroyAfter);
	}

	public void PlayMusic(AudioClip ac) {
		if (ac.name == old_clip_name)
			return;
		if (m_musicTrack != null)
			StopMusic ();
		m_musicTrack = gameObject.AddComponent<AudioSource>();
		m_musicTrack.clip = ac;
		m_musicTrack.volume = 0.5f;
		m_musicTrack.loop = true;
		old_clip_name = ac.name;
		m_musicTrack.Play ();
		Paused = false;
	}
	public void StopMusic() {
		if (m_musicTrack != null)
			Destroy (m_musicTrack);
		old_clip_name = "none";
		m_musicTrack = null;
		Paused = false;
	}
	public void PauseMusic() {
		if (m_musicTrack != null)
			m_musicTrack.Pause ();
		Paused = true;
	}
	public void ResumeMusic() {
		if (Paused && m_musicTrack != null)
			m_musicTrack.Play ();
		Paused = false;
	}
}
