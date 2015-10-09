using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Listen to controllers and extract buttons information
/// </summary>
public class MapperScript : MonoBehaviour
{
  private const int JOYSTICK_COUNT = 10;
  private const int JOYSTICK_ANALOG_COUNT = 19;

  [Header("UI")]
  public MapperUIScript ui;

  private string deviceName;

  void Start()
  {
    ui.SetDetectedDevices(Input.GetJoystickNames());
  }


  void Update()
  {
    if (string.IsNullOrEmpty(deviceName))
    {
      UpdateNoDevice();
    }
    else
    {
      UpdateDeviceBinding();
    }
  }

  private void UpdateNoDevice()
  {
    var key = CheckForKey();
    if (key != KeyCode.None)
    {
      Debug.Log(key);
    }
  }

  private void UpdateDeviceBinding()
  {

  }

  private KeyCode CheckForKey(string joystickId = "")
  {
    foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
    {
      // Get "JoystickNButtonX" keys but not "JoystickButtonX" keys
      if (k.ToString().Contains("Joystick" + joystickId) && !k.ToString().Contains("JoystickButton"))
      {
        if (Input.GetKeyDown(k))
        {
          return k;
        }
      }
    }
    return KeyCode.None;
  }

  private List<KeyValuePair<int, int>> CheckForAnalogs()
  {
    List<KeyValuePair<int, int>> analogs = new List<KeyValuePair<int, int>>();

    // A joystick is moving?
    for (int i = 1; i <= JOYSTICK_COUNT; i++)
    {
      for (int a = 0; a <= JOYSTICK_ANALOG_COUNT; a++)
      {
        float f = Input.GetAxis(string.Format("joystick {0} analog {1}", i, a));

        const float DEAD_ZONE = 0.2f;

        if (Mathf.Abs(f) > DEAD_ZONE)
        {
          analogs.Add(new KeyValuePair<int, int>(i, a));
        }
      }
    }

    return analogs;
  }
}
