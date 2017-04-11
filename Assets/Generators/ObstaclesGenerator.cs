using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObstaclesGenerator : MonoBehaviour {
    [Header("Obstacles Generator Base Settings")]
    public int MoneyFrequency;
    public int BonusesFrequency;
    public int ObstaclesFrequency;
    public int SpecjalFrequency;

    private bool[] moneyFrequency;
    private bool[] bonusesFrequency;
    private bool[] specjalFrequency;

    [Header("GameObjects to Generate")]
    public GameObject[] Obstacles;
    public GameObject[] SpecjalObstacles;
    public GameObject[] Money;
    public GameObject[] Bonuses;

    private static System.Random random = new System.Random();

    private int obstaclesInTube;
    private float Differend;
    private float XpositionNextObstacle = 0f;
    
    // Use this for initialization
    void Start()
    {
        moneyFrequency = new bool[MoneyFrequency * 2];
        bonusesFrequency = new bool[BonusesFrequency * 2];
        specjalFrequency = new bool[SpecjalFrequency * 2];

        for(int i = 0; i < specjalFrequency.Length; i++)
        {
            specjalFrequency[i] = (UnityEngine.Random.value > 0.5f);
        }

        if (!CheckIfGenerateBonusesOrMoney(specjalFrequency))
        {
            for (int i = 0; i < moneyFrequency.Length; i++)
            {
                moneyFrequency[i] = (UnityEngine.Random.value > 0.5f);
            }

            for (int i = 0; i < bonusesFrequency.Length; i++)
            {
                bonusesFrequency[i] = (UnityEngine.Random.value > 0.5f);
            }
            if (CheckIfGenerateBonusesOrMoney(moneyFrequency) || CheckIfGenerateBonusesOrMoney(bonusesFrequency) && !(CheckIfGenerateBonusesOrMoney(moneyFrequency) && CheckIfGenerateBonusesOrMoney(bonusesFrequency)))
            {
                if (CheckIfGenerateBonusesOrMoney(moneyFrequency))
                {
                    Vector3 rotation = new Vector3(float.Parse(random.Next(0, 180).ToString()), 0f, 0f);
                    GameObject instantiateMoney = Instantiate(Money[random.Next(0, Money.Length)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(rotation)) as GameObject;
                    instantiateMoney.transform.parent = transform;
                }
                else if (CheckIfGenerateBonusesOrMoney(bonusesFrequency))
                {
                    Vector3 rotation = new Vector3(float.Parse(random.Next(0, 180).ToString()), 90f, 0f);
                    GameObject instantiateBonuses = Instantiate(Bonuses[random.Next(0, Bonuses.Length)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(rotation)) as GameObject;
                    instantiateBonuses.transform.parent = transform;
                }
            }
            else {
                if (Obstacles.Length > 0)
                {
                    obstaclesInTube = random.Next(0, ObstaclesFrequency * 2);
                    Differend = GetDifferendFromObstacles();

                    for (int i = 0; i < obstaclesInTube; i++)
                    {
                        Vector3 rotation = new Vector3(float.Parse(random.Next(0, 180).ToString()), 0f, 0f);
                        GameObject instantiateObstacles = Instantiate(Obstacles[random.Next(0, Obstacles.Length)], new Vector3(transform.position.x - (GetComponent<BoxCollider>().bounds.size.x / 2) + XpositionNextObstacle, transform.position.y, transform.position.z), Quaternion.Euler(rotation)) as GameObject;
                        instantiateObstacles.transform.parent = transform;
                        XpositionNextObstacle += Differend;
                    }
                }
            }
        }
        else
        {
            Vector3 rotation = new Vector3(float.Parse(random.Next(0, 180).ToString()), 0f, 0f);
            GameObject instantiateObstacles = Instantiate(SpecjalObstacles[random.Next(0, SpecjalObstacles.Length)], new Vector3(transform.position.x - (GetComponent<BoxCollider>().bounds.size.x / 2), transform.position.y, transform.position.z), Quaternion.Euler(rotation)) as GameObject;
            instantiateObstacles.transform.parent = transform;
        }
    }

    private bool CheckIfGenerateBonusesOrMoney(bool[] objectFrequences)
    {
        foreach (bool i in objectFrequences)
        {
            if (!i)
            {
                return false;
            }
        }

        return true;
    }
	// Update is called once per frame
	void Update () {
	
	}
    float GetDifferendFromObstacles()
    {
        return GetComponent<BoxCollider>().bounds.size.x / obstaclesInTube;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ObstaclesGenerator))]
public class ObstaclesGeneratorEditor : Editor
{
    private SerializedObject ob;

    private SerializedProperty ob_ObstaclesProperty;
    private SerializedProperty ob_ObstaclesFrequencyProperty;
    private SerializedProperty ob_MoneyProperty;
    private SerializedProperty ob_MoneyFrequencyProperty;
    private SerializedProperty ob_BonusesProperty;
    private SerializedProperty ob_BonusesFrequencyProperty;

    private SerializedProperty ob_SpecialProperty;
    private SerializedProperty ob_SpecialFrequencyProperty;


    void OnEnable()
    {
        ob = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        GUIStyle style = new GUIStyle();

        #region Obstacles Property
        ob_ObstaclesFrequencyProperty = ob.FindProperty("ObstaclesFrequency");

        if (!ob_ObstaclesFrequencyProperty.intValue.Equals(0))
        {
            ob_ObstaclesProperty = ob.FindProperty("Obstacles");
            EditorGUILayout.PropertyField(ob_ObstaclesProperty, new GUIContent("Obstacles"), true);
        }

        EditorGUILayout.IntSlider(ob_ObstaclesFrequencyProperty, 0, 5);
        string obstaclesFrequencyText = "";

        if (ob_ObstaclesFrequencyProperty.intValue.Equals(0))
        {
            obstaclesFrequencyText = "Without Obstacles";
        }
        else if (ob_ObstaclesFrequencyProperty.intValue.Equals(1))
        {
            obstaclesFrequencyText = "Little";
        }
        else if (ob_ObstaclesFrequencyProperty.intValue.Equals(2))
        {
            obstaclesFrequencyText = "Avarange";
        }
        else if (ob_ObstaclesFrequencyProperty.intValue.Equals(3))
        {
            obstaclesFrequencyText = "Big";
        }
        else if (ob_ObstaclesFrequencyProperty.intValue.Equals(4))
        {
            obstaclesFrequencyText = "Very Big";
        }
        else if (ob_ObstaclesFrequencyProperty.intValue.Equals(5))
        {
            obstaclesFrequencyText = "Same Obstacles";
        }

        ProgressBar(ob_ObstaclesFrequencyProperty.intValue / 5.0f, obstaclesFrequencyText);
        #endregion

        #region SpecialObstacles
        ob_SpecialFrequencyProperty = ob.FindProperty("SpecjalFrequency");

        if (!ob_SpecialFrequencyProperty.intValue.Equals(0))
        {
            ob_SpecialProperty = ob.FindProperty("SpecjalObstacles");
            EditorGUILayout.PropertyField(ob_SpecialProperty, new GUIContent("Special Obstacles"), true);
        }

        EditorGUILayout.IntSlider(ob_SpecialFrequencyProperty, 0, 5);
        string specialFrequencyText = "";

        if (ob_SpecialFrequencyProperty.intValue.Equals(0))
        {
            specialFrequencyText = "Without Obstacles";
        }
        else if (ob_SpecialFrequencyProperty.intValue.Equals(1))
        {
            specialFrequencyText = "Little";
        }
        else if (ob_SpecialFrequencyProperty.intValue.Equals(2))
        {
            specialFrequencyText = "Avarange";
        }
        else if (ob_SpecialFrequencyProperty.intValue.Equals(3))
        {
            specialFrequencyText = "Big";
        }
        else if (ob_SpecialFrequencyProperty.intValue.Equals(4))
        {
            specialFrequencyText = "Very Big";
        }
        else if (ob_SpecialFrequencyProperty.intValue.Equals(5))
        {
            specialFrequencyText = "Same Obstacles";
        }

        ProgressBar(ob_SpecialFrequencyProperty.intValue / 5.0f, specialFrequencyText);
        #endregion

        #region Money Property
        ob_MoneyFrequencyProperty = ob.FindProperty("MoneyFrequency");
        if (!ob_MoneyFrequencyProperty.intValue.Equals(0))
        {
            ob_MoneyProperty = ob.FindProperty("Money");
            EditorGUILayout.PropertyField(ob_MoneyProperty, new GUIContent("Money"), true);
        }

        EditorGUILayout.IntSlider(ob_MoneyFrequencyProperty, 0, 5);
        string moneyFrequencyText = "";
        if (ob_MoneyFrequencyProperty.intValue.Equals(5))
        {
            moneyFrequencyText = "Without Money";
        }
        else if (ob_MoneyFrequencyProperty.intValue.Equals(4))
        {
            moneyFrequencyText = "Little";
        }
        else if (ob_MoneyFrequencyProperty.intValue.Equals(3))
        {
            moneyFrequencyText = "Avarange";
        }
        else if (ob_MoneyFrequencyProperty.intValue.Equals(2))
        {
            moneyFrequencyText = "Big";
        }
        else if (ob_MoneyFrequencyProperty.intValue.Equals(1))
        {
            moneyFrequencyText = "Very Big";
        }
        else
        {
            moneyFrequencyText = "Same Money";
        }
        ProgressBar(ob_MoneyFrequencyProperty.intValue / 5.0f, moneyFrequencyText);
        #endregion

        #region Bonuses Property
        ob_BonusesFrequencyProperty = ob.FindProperty("BonusesFrequency");
        if (!ob_BonusesFrequencyProperty.intValue.Equals(0))
        {
            ob_BonusesProperty = ob.FindProperty("Bonuses");
            EditorGUILayout.PropertyField(ob_BonusesProperty, new GUIContent("Bonuses"), true);
        }

        EditorGUILayout.IntSlider(ob_BonusesFrequencyProperty, 0, 5);
        string bonusesFrequencyText = "";

        if (ob_BonusesFrequencyProperty.intValue.Equals(5))
        {
            bonusesFrequencyText = "Without Bonuses";
        }
        else if (ob_BonusesFrequencyProperty.intValue.Equals(4))
        {
            bonusesFrequencyText = "Little";
        }
        else if (ob_BonusesFrequencyProperty.intValue.Equals(3))
        {
            bonusesFrequencyText = "Avarange";
        }
        else if (ob_BonusesFrequencyProperty.intValue.Equals(2))
        {
            bonusesFrequencyText = "Big";
        }
        else if (ob_BonusesFrequencyProperty.intValue.Equals(1))
        {
            bonusesFrequencyText = "Very Big";
        }
        else
        {
            bonusesFrequencyText = "Same Bonuses";
        }
        ProgressBar(ob_BonusesFrequencyProperty.intValue / 5.0f, bonusesFrequencyText);
        #endregion

        ob.ApplyModifiedProperties();
    }
    

    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}
#endif



