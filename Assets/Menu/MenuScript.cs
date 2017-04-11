using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MenuScript : MonoBehaviour {
    public Button playButton;
    public Button shopButton;

    private Text lastDistance;
    private Text bestDistance;
    private Text diamound;

    GameDataLoaded gameDataLoaded;

    public AudioClip menuMusic;
    public AudioClip clickSound;

    bool isLoaded = false;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();

        playButton = playButton.GetComponent<Button>();
        lastDistance = GameObject.Find("LastScoreText").GetComponent<Text>();
        bestDistance = GameObject.Find("BestScoreText").GetComponent<Text>();
        diamound = GameObject.Find("DiamoundText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if(gameDataLoaded == null)
        {
            gameDataLoaded = (GameObject.Find("Save Manager").GetComponent<SaveGameManager>().gameDataLoaded) as GameDataLoaded;
        }
        else if(gameDataLoaded != null && !isLoaded)
        {
            bestDistance.text = Math.Floor(gameDataLoaded.bestDistance) + " M";
            lastDistance.text = Math.Floor(gameDataLoaded.lastDistance) + " M";
            diamound.text = gameDataLoaded.diamound.ToString();
        }

        CheckAndPlayAudioClip(menuMusic);
	}

    /// <summary>
    /// This method opened game scene
    /// and play click sound.
    /// </summary>
    public void StartGame()
    {
        CheckAndPlayAudioClip(clickSound);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// This method opened shop scene
    /// and play click sound.
    /// </summary>
    public void OpenShop()
    {
        CheckAndPlayAudioClip(clickSound);
        SceneManager.LoadScene(2);
    }



    #region Audio Helphers Method
    /// <summary>
    /// This method checked if current AudioClip component is
    /// not nullable.
    /// </summary>
    /// <param name="audio">AudioClip for checked.</param>
    /// <returns>True, if AudioClip is not nullable.
    ///          False, if AudioClip is nullable state.</returns>
    private bool CheckIfAudioClipIsNotNull(AudioClip audio)
    {
        if (audio == null)
            return false;
        else
            return true;
    }
    /// <summary>
    /// This methd checked if current AudioClip component is not played.
    /// </summary>
    /// <param name="audio">AudioClip for check.</param>
    /// <returns>True, if AudiClip is not playing state.
    ///          False, if AudioClip is playing state.</returns>
    private bool CheckIfAudioClipIsNotPlayed(AudioClip audio)
    {
        if (GetComponent<AudioSource>().isPlaying)
            return false;
        else
            return true;
    }

    /// <summary>
    /// This method using others audio helphers method for playing AudioClip.
    /// </summary>
    /// <param name="audio">AudioClip for playing.</param>
    private void CheckAndPlayAudioClip(AudioClip audio)
    {
        if (CheckIfAudioClipIsNotNull(audio))
        {
            if (CheckIfAudioClipIsNotPlayed(audio))
            {
                GetComponent<AudioSource>().PlayOneShot(audio);
            }
        }
    }
    #endregion

}
