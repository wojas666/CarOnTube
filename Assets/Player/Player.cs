using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour {
    private GameObject cameras;
    private float distance;
    private int diamound;
    private bool isMissionComplete;
    
    public GameObject Generator;
    public GameObject QuestManager;
    public GameObject SaveManager;
    public Text distanceBoard;
    public Text diamoundBoard;

    [Header("Speed Settings")]
    public Vector3 forwardSpeed;
    public float timeToSpeedChange;
    public float speedToChanged;
    public float timeToChangeLevel;

    [Header("Audio Settings")]
    public AudioClip shipExplosionSound;
    public AudioClip earnDiamoundSound;

    private float _tempTime;
    private float _tempTimeToLevelChanged;

    public bool isDeath;
    private bool isSaved;

    private float _templateTimeToBackMenu = 5.0f;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();
        _tempTime = timeToSpeedChange;
        _tempTimeToLevelChanged = timeToChangeLevel;
        cameras = GameObject.Find("Main Camera");
        distanceBoard = GameObject.Find("Distance").GetComponent<Text>();
        diamoundBoard = GameObject.Find("Diamound").GetComponent<Text>();
        isDeath = false;
        isSaved = false;
        distance = 0f;

        isMissionComplete = false;

        QuestManager = GameObject.Find("Quest Manager(Clone)");
	}
	
    void LateUpdate()
    {
        transform.position += forwardSpeed * Time.deltaTime;
    }

	// Update is called once per frame
	void Update () {
        timeToSpeedChange -= Time.deltaTime;
        timeToChangeLevel -= Time.deltaTime;

        if (timeToSpeedChange < 0f)
        {
            forwardSpeed.x -= speedToChanged;
            timeToSpeedChange = _tempTime;
        }

        if (timeToChangeLevel < 0f)
        {
            if (Generator.GetComponent<Generator>().isLevelChanged && Generator.GetComponent<Generator>().level < 3)
            {
                Generator.GetComponent<Generator>().level += 1;
                timeToChangeLevel = _tempTimeToLevelChanged;
            }
        }

        if (!isDeath)
        {
            distance += 1 * (Time.deltaTime*10f);
            distanceBoard.text = Math.Floor(distance) + " m";
        }

        if(isDeath && !isSaved)
        {
            //isMissionComplete = QuestManager.GetComponent<QuestManager>().CheckIfQuestComplete();

            int currQuestID = SaveManager.GetComponent<SaveGameManager>().gameDataLoaded.currentQuestID;

            if (isMissionComplete)
            {
                currQuestID += 1;
            }
            
            SaveManager.GetComponent<SaveGameManager>().gameDataLoaded.UpdateGameData(distance, diamound, currQuestID);
            SaveManager.GetComponent<SaveGameManager>().SaveState(SaveManager.GetComponent<SaveGameManager>().gameDataLoaded);
            isSaved = true;
        }

        // Tymczasowe rozwiazanie.
        if(isDeath && isSaved)
        {
            _templateTimeToBackMenu -= Time.deltaTime;

            if (_templateTimeToBackMenu < 0f)
            {
                SceneManager.LoadScene(0);
            }
        }
	}

    void OnTriggerEnter(Collider unit)
    {
        if(unit.tag.Equals("Rura"))
        {
            forwardSpeed.y = -9.81f;
            //gameObject.GetComponent<Rigidbody>().useGravity = true;
            forwardSpeed.x = 0.1f;
            cameras.GetComponent<MainCamera>().isDeadh = true;
            isDeath = true;
        }
        if (unit.tag.Equals("Obstacles"))
        {
            CheckAndPlayAudioClip(shipExplosionSound);
            Transform child = transform.Find("Explosion");
            child.gameObject.SetActive(true);
            cameras.GetComponent<MainCamera>().isDeadh = true;
            Generator.SetActive(false);
            isDeath = true;
        }
        if(unit.tag.Equals("Money"))
        {
            if (CheckIfAudioClipIsNotNull(earnDiamoundSound))
            {
                GetComponent<AudioSource>().PlayOneShot(earnDiamoundSound);
            }

            diamound += 1;
            diamoundBoard.text = diamound.ToString();
            Transform parent = unit.transform.parent;
            Destroy(parent.gameObject);
        }
        if(unit.tag.Equals("Quest Item"))
        {
            Transform parent = unit.transform.parent;
            Destroy(parent.gameObject);
        }
        if (unit.tag.Equals("Bomb"))
        {
            CheckAndPlayAudioClip(shipExplosionSound);
            Transform parent = unit.transform.parent;
            Destroy(parent.gameObject);
            Transform child = transform.Find("Explosion");
            child.gameObject.SetActive(true);
            cameras.GetComponent<MainCamera>().isDeadh = true;
            Generator.SetActive(false);
            isDeath = true;
        }
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
            //if (CheckIfAudioClipIsNotPlayed(audio))
            //{
                GetComponent<AudioSource>().PlayOneShot(audio);
            //}
        }
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private SerializedObject player;

    private SerializedProperty forwardSpeedProperty;
    private SerializedProperty timeToSpeedChangeProperty;
    private SerializedProperty speedToChangeProperty;
    private SerializedProperty timeToChangeLevelProperty;

    void OnEnable()
    {
        player = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif