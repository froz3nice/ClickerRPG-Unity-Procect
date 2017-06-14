using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerScript : MonoBehaviour {

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
	public AudioClip clip4;
	public AudioClip clip5;
	public AudioClip clip6;
	public AudioClip clip7;
	public AudioClip clip8;
	public AudioClip clip9;

	public static AudioControllerScript instance;
    private AudioSource source;
	void Awake()
	{
		if (instance != null) {
			Debug.LogError ("More than one audioController in the scene");
		} else {
			instance = this;
		}
	}
    void Start() 
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = clip1;
        source.Play();
        DontDestroyOnLoad(gameObject);
	}
    public void PlayBattleMusic()
    {
        source.Stop();
        //source.PlayOneShot(clip3);
    }

    public void PlayClip2() 
    {
        source.PlayOneShot(clip2);
    }
	public void PlayClip3()
	{
		source.PlayOneShot (clip3);	
	}
	public void PlayClip4()
	{
		source.PlayOneShot (clip4);
	}
	public void PlayClip5()
	{
		source.PlayOneShot (clip5);
	}
	public void SwordHit()
	{
		source.PlayOneShot (clip6);
	}
	public void GameOver()
	{
		source.PlayOneShot (clip7);
	}
	public void GameWin()
	{
		source.PlayOneShot (clip8);
	}
	public void Ambient()
	{
		source.PlayOneShot (clip9);
	}
}
