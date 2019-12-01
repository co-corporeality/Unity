using UnityEngine;
using System.Collections;

public class receiveposition_instantiate : MonoBehaviour
{
    public OSC osc;
    public GameObject avatar;
    public GameObject avatar2;
    public GameObject skeleton1;
    public GameObject skeleton2;
    private int numberOfGameObjects = 18; 
    private GameObject[] gameObjects;
    private GameObject[] gameObjects_2;

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
            gameObjects[i] = Instantiate(skeleton1);
            gameObjects[i].transform.parent = avatar.transform;
            gameObjects[i].name = i.ToString();
            gameObjects[i].SetActive(false);
        }

        gameObjects_2 = new GameObject[numberOfGameObjects];

        for (int i = 0; i < numberOfGameObjects; i++)
        {
            gameObjects_2[i] = Instantiate(skeleton2);
            gameObjects_2[i].transform.parent = avatar2.transform;
            gameObjects_2[i].name = i.ToString();
            gameObjects_2[i].SetActive(false);
        }

    }

    void newPose(OscMessage message)
    {
        if (message.GetFloat(0) == 0)
        {
            Debug.Log(message);
            for (int i = 1; i < 36; i++) // go through the string
            {

                if (i % 2 != 0) // if it is an uneven number
                {
                    int c = (i-1)/2;
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
        if (message.GetFloat(0) == 1)
        {
            Debug.Log(message);
            for (int i = 1; i < 36; i++) // go through the string
            {

                if (i % 2 != 0) // if it is an uneven number
                {
                    int c = ((i - 1) / 2);
                    float x = message.GetFloat(i);
                    float y = message.GetFloat(i + 1);


                    if (x == -1)
                    {
                        gameObjects_2[c].SetActive(false);
                    }

                    else
                    {
                        gameObjects_2[c].SetActive(true);
                        gameObjects_2[c].transform.position = new Vector3(x, y * -1);

                    }

                }
            }

        }
        //else
        //{
          //  for (int i = 0; i < skeleton.transform.childCount; i++)
            //{
              //  var child = skeleton.transform.GetChild(i).gameObject;
                //if (child != null)
                  //  child.SetActive(true);
           // }
        //}
            
    }

}

