using UnityEngine;
using System.Collections;
using System;
using Translation;

public class AudioTranslation : MonoBehaviour, ITranslatable {

	public float volume = 1;

	[Serializable]
	public class Translatable
	{
		public string language = "en-US";
		public AudioClip translatable = null;
	}
	
	public Translatable[] items;
	
	void Start () {
		if(volume < 0)
			volume = 0;
		if(volume > 1)
			volume = 1;
	}
	
	public AudioClip GetAudioClip()
	{ 
		string language = TranslationEngine.Instance.Language;
		foreach(Translatable item in items)
		{
			if(item.language.Equals (language))
			{
				return item.translatable;	
			}
		}
		return null;
	}
	
	public void Play()
	{
		AudioClip clip = GetAudioClip();
		if(clip != null)
		{
			this.GetComponent<AudioSource>().PlayOneShot (clip, volume);
		}
	}
		
	/// <summary>
	/// Translates all "ObjectTranslation" in the scene. This is needed 
	/// </summary>
	public void Translate ()
	{
		/*var objects = GameObject.FindObjectsOfType(typeof(ObjectTranslation));
		
		foreach(var obj in objects)
		{
			var translatable = (ITranslatable)obj;
			translatable.Translate();
		}*/
	}
}
