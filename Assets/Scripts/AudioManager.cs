using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yade.Runtime;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {
	private AudioSource[] m_audioSources;
	[SerializeField]
	private AudioSource bgmSource;

	public static string CRISIS_AUDIO_PATH = "Music/pulse_voice";
	public static string MODIFY_WORKER_AUDIO_PATH = "Music/task_number_change";
	public static string REMOVE_BUILDING_AUDIO_PATH = "Music/destroy_build";
	public static string CLICK_WOOD_AUDIO_PATH = "Music/click_wood";

	protected override void OnInit() {
		DontDestroyOnLoad(gameObject);
		m_audioSources = GetComponentsInChildren<AudioSource>();
	}

	protected override void OnDuplicated() {
		Destroy(gameObject);
	}

	public void PlayAudioClip(string clipPath,float volume = 1f) {
		var audioClip = Resources.Load<AudioClip>(clipPath);
		if (audioClip == null) {
			return;
		}
		for (int i = 0; i < m_audioSources.Length; i++) {
			if (!m_audioSources[i].isPlaying) {
				m_audioSources[i].gameObject.SetActive(true);
				m_audioSources[i].clip = audioClip;
				m_audioSources[i].volume = volume;
				m_audioSources[i].Play();
				m_audioSources[i].gameObject.name = audioClip.name;
				StartCoroutine(WaitForAudioFinished(m_audioSources[i]));
				return;
			}
		}
	}

	IEnumerator WaitForAudioFinished(AudioSource audioSource) {
		yield return new WaitUntil(() => !audioSource.isPlaying);
		audioSource.gameObject.SetActive(false);
		audioSource.gameObject.name = "audio_source";
	}
}
