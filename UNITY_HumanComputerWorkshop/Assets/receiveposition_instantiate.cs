using UnityEngine;
using System.Collections;

public class receiveposition_instantiate : MonoBehaviour
{
    public OSC osc;
    public GameObject prefab;
    private int numberOfGameObjects = 18;
    private GameObject[] gameObjects;
    public int sizeOfList = 40;

    // Use this for initialization
    void Awake()
    {
        objects();
    }
    void Start()
    {
        osc.SetAddressHandler("/pose", newPose);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void objects()
    {
        gameObjects = new GameObject[numberOfGameObjects];

        for (int i = 0; i < numberOfGameObjects; i++)
        {
            gameObjects[i] = Instantiate(prefab);
            gameObjects[i].name = i.ToString();
            gameObjects[i].SetActive(false);
        }
    }

    void newPose(OscMessage message)
    {

        for (int i=0; i< 35; i++) // go through the string
        {

            if (i % 2 == 0) // if it is an even number
            {         
                int c = i / 2;
                float x = message.GetFloat(i);
                float y = message.GetFloat(i + 1);

                Debug.Log(i);

                if (x == -1)
                {
                    gameObjects[c].SetActive(false);
                }

                else
                {
                    gameObjects[c].SetActive(true);
                    gameObjects[c].transform.position = new Vector3(x, y * -1);
                }

            }
         
        }


    }

}

