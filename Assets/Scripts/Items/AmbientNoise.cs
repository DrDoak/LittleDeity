using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoise : MonoBehaviour {
	public List<AudioClip> startingClips = new List<AudioClip>();
	List<AudioClip> audioClips= new List<AudioClip> ();
	List<AudioSource> sources = new List<AudioSource> ();
	//Dictionary<AudioSource,float> timeSinceLastPlay

	void Start () {
		audioClips = new List<AudioClip> ();
		sources = new List<AudioSource> ();
		foreach (AudioClip cl in startingClips)
		{
			AddSound (cl);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddSound(AudioClip ac) {
		if (!audioClips.Contains (ac)) {
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = ac;
			source.volume = 0.2f;
			source.loop = true;
			source.minDistance = 0.5f;
			source.spatialBlend = 1f;
			source.maxDistance = 2.5f;
			source.Play ();
			sources.Add (source);
			audioClips.Add (ac);
		}
	}

	public void RemoveSound(AudioClip ac) {
		if (audioClips.Contains (ac)) {
			audioClips.Remove (ac);
			List<AudioSource> newList = new List<AudioSource> ();
			foreach (AudioSource asource in sources) {
				if (asource.clip == ac) {
					GameObject.Destroy (asource);

				} else {
					newList.Add (asource);
				}
			}
			sources = newList;
		}
	}
}
