using UnityEngine;
using System.Collections;

public class receiveposition_instantiate2 : MonoBehaviour
{
    public OSC osc;
    public GameObject prefab;
    public GameObject prefab2;
    private int numberOfGameObjects = 18; //18
    private GameObject[] gameObjects;
    private GameObject[] gameObjects_skeleton2;

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

        gameObjects_skeleton2 = new GameObject[numberOfGameObjects];

        for (int i = 0; i < numberOfGameObjects; i++)
        {
            gameObjects_skeleton2[i] = Instantiate(prefab2);
            gameObjects_skeleton2[i].name = i.ToString();
            gameObjects_skeleton2[i].SetActive(false);
        }
    }

    void newPose(OscMessage message)
    {
        if (message.GetFloat(0) == 1)
        {
            Debug.Log(message);
            for (int i = 1; i < 36; i++) // go through the string
            {

                if (i % 2 != 0) // if it is an uneven number
                {
                    int c = i / 2;
                    float x = message.GetFloat(i);
                    float y = message.GetFloat(i + 1);


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

}

