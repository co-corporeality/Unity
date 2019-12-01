using UnityEngine;
using System.Collections;

public class ReceivePosition : MonoBehaviour
{
    public int idx1;
    public int idx2;
    public OSC osc;


    // Use this for initialization
    void Start()
    {
        osc.SetAddressHandler("/pose", newPose);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void newPose(OscMessage message)
    {
        if (idx1 < 0)
        {
            return;
        }
            
        else
        {
            float x = message.GetFloat(idx1) * -1;
            float y = message.GetFloat(idx2) *-1;
            transform.position = new Vector3(x, y);
        }
        

        
    }
}
  
