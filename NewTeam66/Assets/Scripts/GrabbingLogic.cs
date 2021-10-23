using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using OculusSampleFramework;

public class GrabbingLogic : OVRGrabber
{

    private OVRHand m_hand;
    [SerializeField]
    float pinchThreshold = 0.3f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = m_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

        if(!m_grabbedObj && pinchStrength > pinchThreshold && m_grabCandidates.Count > 0)
        {
            GrabBegin();
            PlayerController.Instance.isHoldingPen = true;
        }
        else if(m_grabbedObj && !(pinchStrength > pinchThreshold))
        {
            GrabEnd();
            PlayerController.Instance.isHoldingPen = false;
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.tag == "Pen")
    //     {
            
    //     }
    // }

    // void OnTrggerExit(Collider other)
    // {
    //     if(other.tag == "Pen")
    //     {
    //         PlayerController.Instance.isHoldingPen = false;
    //     }
    // }
}
