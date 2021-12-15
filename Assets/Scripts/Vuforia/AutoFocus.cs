using UnityEngine;
using Vuforia;

public class AutoFocus : MonoBehaviour
{    void Start()
    {
        Focus();
    }

    void Focus()
    {
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);   //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        Invoke("Focus", 1);
    }
}
