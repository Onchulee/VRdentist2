using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DentalPlayerController : MonoBehaviour
{
    public XRInputReceiver rightInputReceiver;
    public GameObject rightFinger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rightInputReceiver.GetKey(XRInputReceiver.KEY.Grip) == true
            && rightInputReceiver.GetKey(XRInputReceiver.KEY.Trigger) == false)
        {
            rightFinger.SetActive(true);
        }
        else {
            rightFinger.SetActive(false);
        }
    }
}
