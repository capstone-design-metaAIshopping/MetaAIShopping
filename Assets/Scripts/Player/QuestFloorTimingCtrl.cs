using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class QuestFloorTimingCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeToFloorOriginMode());
    }

    IEnumerator ChangeToFloorOriginMode()
    {
        yield return null;

        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        Debug.Log("Found " + subsystems.Count + " input subsystems.");

        for (int i = 0; i < subsystems.Count; i++)
        {
            Debug.Log("Current Tracking Origin Mode for " + subsystems[i].SubsystemDescriptor.id
                + " is " + subsystems[i].GetTrackingOriginMode());
            Debug.Log("Available Tracking Origin Modes for " + subsystems[i].SubsystemDescriptor.id
                + " are " + subsystems[i].GetSupportedTrackingOriginModes());
            Debug.Log("Setting TrackingOriginMode to Floor");
            if (subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor))
                Debug.Log("Successfully set TrackingOriginMode to Floor");
            else
                Debug.Log("Failed to set TrackingOriginMode to Floor");
        }
    }
}
