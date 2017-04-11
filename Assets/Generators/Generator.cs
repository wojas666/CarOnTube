using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generuje rury.
/// </summary>
public class Generator : MonoBehaviour {
    [Header("Tube Generator Settings ( Object )")]
    /// Player for follow position.
    public GameObject Player;
    /// Tubes object for Instantiate.
    public GameObject[] Tubes;
    public GameObject Parent;
    public GameObject TubeParent;

    [Header("Tube Generator Settings ( Offset )")]
    public float PlayerDifferendFromCenterTube;
    public int minHoliesOffset;
    public bool isLevelChanged;

    [Header("Tube Generator Settings ( Count )")]
    public int TubesGeneratedForStartGame;
    public int DestroyAffterPassing;

    private List<GameObject> generatedTube;
    private static System.Random random;
    private Vector3 nextGeneratePosition;
    private Vector3 rotation;
    private const float tubeSizes = 79.67f;

    private bool right;
    private int lastAngle;
    public int level;

    GameObject tubeInstantiate;

    // Use this for initialization
    void Start () {
        random = new System.Random();
        lastAngle = 0;
        Parent = Instantiate(TubeParent);

        generatedTube = new List<GameObject>();
        GenerateTubesForStartGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if(Player.transform.position.x < generatedTube[DestroyAffterPassing].transform.position.x)
        {
            Destroy(generatedTube[0]);
            generatedTube.RemoveAt(0);
            DynamicGeneratedTubes();
        }
    }

    /// <summary>
    /// This method is responded for dynamic generated tubes.
    /// </summary>
    private void DynamicGeneratedTubes()
    {
        int randomIndex = 0;

        if (level >= 3)
        {
            randomIndex = random.Next(0, Tubes.Length - 1);
        }

        int _tempAngle = RandomAngle();
        lastAngle = _tempAngle;
        rotation = new Vector3(_tempAngle * 10, 0f, 0f);

        tubeInstantiate = Instantiate(Tubes[randomIndex], nextGeneratePosition, Quaternion.Euler(rotation)) as GameObject;

        if (isLevelChanged && (randomIndex == 0 || randomIndex == 2))
            tubeInstantiate.GetComponent<ObstaclesGenerator>().ObstaclesFrequency = level;

        tubeInstantiate.transform.parent = Parent.transform;
        tubeInstantiate.transform.rotation = Quaternion.Euler(rotation);
        generatedTube.Add(tubeInstantiate);

        nextGeneratePosition -= new Vector3(tubeInstantiate.GetComponent<BoxCollider>().bounds.size.x, 0, 0);
    }

    /// <summary>
    /// This method generated tubes for start games.
    /// </summary>
    private void GenerateTubesForStartGame()
    {
        //int randomFirstIndex = random.Next(0, Tubes.Length - 1);
        nextGeneratePosition = DeffineStartGeneratePosition(0);
        lastAngle = 0;

        for (int i = 0; i < TubesGeneratedForStartGame; i++)
        {
            int _tempAngle = RandomAngle();
            lastAngle = _tempAngle;
            rotation = new Vector3(float.Parse((_tempAngle * 10).ToString()), 0f, 0f);
            tubeInstantiate = Instantiate(Tubes[0], nextGeneratePosition,Quaternion.Euler(rotation)) as GameObject;
            tubeInstantiate.transform.parent = Parent.transform;
            tubeInstantiate.transform.rotation = Quaternion.Euler(rotation);
            generatedTube.Add(tubeInstantiate);

            //if (isLevelChanged && (randomFirstIndex == 0 || randomFirstIndex == 2))
            tubeInstantiate.GetComponent<ObstaclesGenerator>().ObstaclesFrequency = level;

            nextGeneratePosition -= new Vector3(tubeInstantiate.GetComponent<BoxCollider>().bounds.size.x, 0, 0);
            //randomFirstIndex = random.Next(0, Tubes.Length - 1);
        }
    }

    private int RandomAngle()
    {
        bool right = (UnityEngine.Random.value > 0.5f);

        int _temp = lastAngle;
        if (right && _temp < 36 - minHoliesOffset)
        {
            _temp = _temp + minHoliesOffset;
            return random.Next(_temp, 36);
        }
        else if (!right && _temp > 0 + minHoliesOffset)
        {
            _temp = _temp - minHoliesOffset;
            return random.Next(0, _temp);
        }
        else {
            return random.Next(10, 25);
        }
    }

    /// <summary>
    /// This method deffined start generated position in round.
    /// </summary>
    /// <param name="randomIndex">Index from 'Tubes' array.</param>
    /// <returns>Start generated position.</returns>
    private Vector3 DeffineStartGeneratePosition(int randomIndex)
    {
        Vector3 startGeneratedPosition;

        if (Player != null)
        {
            float xPosition = (Player.transform.position.x - PlayerDifferendFromCenterTube) - GetOneTubeSize(randomIndex).x;
            float yPosition = 6.38f;
            startGeneratedPosition = new Vector3(xPosition, yPosition, 1.14f);
        }
        else
        {
            startGeneratedPosition = Vector3.zero;
            Debug.Log("Start Generated position is not found! Player is not deffinded!");
        }

        return startGeneratedPosition;
    }

    /// <summary>
    /// This method return one tube size.
    /// </summary>
    /// <param name="tubeIndex">Index from 'Tubes' array.</param>
    /// <returns>Vector3 not zero variables if found tube size in 'Tubes' array.</returns>
    private Vector3 GetOneTubeSize(int tubeIndex)
    {
        Vector3 tubeSize;

        if (tubeIndex < Tubes.Length && tubeIndex > 0)
            tubeSize = Tubes[tubeIndex].GetComponent<BoxCollider>().bounds.size;
        else
        {
            Debug.Log("tubeIndex is out of Tubes array range!");
            return Vector3.zero;
        }

        return tubeSize;
    }
}
