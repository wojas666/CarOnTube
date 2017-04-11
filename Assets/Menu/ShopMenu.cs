using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ShopMenu : MonoBehaviour {
    public GameObject[] shipsPrefabs;
    /// <summary>
    /// 0 - Buy.
    /// 1 - Used.
    /// 2 - In Used.
    /// </summary>
    public Sprite[] interactionsButton;
    public List<GameObject> instantiateShipsList;
    public Vector3 shipPosition;

    int actually;

    private Vector3 hideShipPosition;

    private bool dragActive;

    private float x_m;
    private float x_c;
    private float y_m;
    private float y_c;
    private float delta_x;
    private float delta_y;
    private Vector3 rotation;
    public float mouseSensivity;

    GameObject saveGameManager;
    Image interactions;

    private void Initialize(GameObject inst)
    {
        Destroy(inst.GetComponent<Player>());

        foreach (Transform child in inst.transform)
        {
            if (child.name == "Jetpack")
            {
                Destroy(child.GetComponent<Jetpack>());
            }
        }
    }

    // Use this for initialization
    void Start () {
        actually = 0;
        instantiateShipsList = new List<GameObject>();
        interactions = GameObject.Find("Interactions").GetComponent<Image>();
        hideShipPosition = new Vector3(-999f, 5f, 0f);
        saveGameManager = GameObject.Find("Save Maneger");

        GameObject inst = Instantiate(shipsPrefabs[0], shipPosition, Quaternion.Euler(new Vector3(0f,90f,0f))) as GameObject;
        Initialize(inst);
        instantiateShipsList.Add(inst);

        for(int i = 1; i < shipsPrefabs.Length; i++)
        {
            inst = Instantiate(shipsPrefabs[i], hideShipPosition, Quaternion.Euler(new Vector3(0f, 90f, 0f))) as GameObject;
            Initialize(inst);
            instantiateShipsList.Add(inst);
        }
        
        dragActive = false;
        x_m = 0f;
        x_c = 0f;
        y_m = 0f;
        y_c = 0f;
        delta_x = 0f;
        delta_y = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            x_m = Input.mousePosition.x;
            y_m = Input.mousePosition.y;
            dragActive = true;
        }

        if (dragActive)
        {
            x_c = Input.mousePosition.x;
            y_c = Input.mousePosition.y;
            delta_x = (x_c - x_m) / 2f;
            delta_y = (y_c - y_m) / 2f;

            x_m = Input.mousePosition.x;
            y_m = Input.mousePosition.y;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            // Reset all start and end mouse possition to stop 
            // scrollview position changed.
            dragActive = false;
            x_m = 0f;
            x_c = 0f;
            y_m = 0f;
            y_c = 0f;
            delta_x = 0f;
            delta_y = 0f;
        }

        rotation.y = rotation.x + (delta_x / mouseSensivity);
        rotation.x = rotation.y + (delta_y / mouseSensivity);

        for(int i = 0; i < instantiateShipsList.Count; i++) { 
            Quaternion rotations = Quaternion.Euler(rotation);
            instantiateShipsList[i].transform.rotation = rotations * instantiateShipsList[i].transform.rotation;
        }

        rotation = new Vector3(0f, 0f, 0f);
    }

    public void Next()
    {
        if (actually < instantiateShipsList.Count - 1)
        {
            instantiateShipsList[actually].transform.position = hideShipPosition;
            instantiateShipsList[actually + 1].transform.position = shipPosition;
            instantiateShipsList[actually + 1].transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            actually++;
            interactions.sprite = GetTextureForInteractionButton();
        }
        else
        {
            instantiateShipsList[actually].transform.position = hideShipPosition;
            instantiateShipsList[0].transform.position = shipPosition;
            instantiateShipsList[0].transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            actually = 0;
            interactions.sprite = GetTextureForInteractionButton();
        }
    }

    public void Preview()
    {
        if(actually > 0)
        {
            instantiateShipsList[actually].transform.position = hideShipPosition;
            instantiateShipsList[actually - 1].transform.position = shipPosition;
            instantiateShipsList[actually - 1].transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            actually--;
            interactions.sprite = GetTextureForInteractionButton();

        }
        else
        {
            instantiateShipsList[actually].transform.position = hideShipPosition;
            instantiateShipsList[instantiateShipsList.Count - 1].transform.position = shipPosition;
            instantiateShipsList[instantiateShipsList.Count - 1].transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            actually = instantiateShipsList.Count - 1;
            interactions.sprite = GetTextureForInteractionButton();
        }
    }

    public void Interactions()
    {
       
    }

    private Sprite GetTextureForInteractionButton()
    {
        Sprite sprite;

        Debug.Log("Actually: " + actually);
        Debug.Log("Size: " + saveGameManager.GetComponent<SaveGameManager>().gameDataLoaded.used.Length);
        Debug.Log(saveGameManager.GetComponent<SaveGameManager>().gameDataLoaded.used[actually]);

        if (saveGameManager.GetComponent<SaveGameManager>().gameDataLoaded.buyngShips[actually])
        {
            sprite = interactionsButton[1];

            if (saveGameManager.GetComponent<SaveGameManager>().gameDataLoaded.used[actually])
            {
                sprite = interactionsButton[2];
            }
        }
        else
        {
            sprite = interactionsButton[0];
        }

        return sprite;
    }
}
