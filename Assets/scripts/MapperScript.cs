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

  private int joystickId;
  private string joystickName;

  private int bindingIndex;
  private string bindingHandle;
  private bool lookForAnalogs;
  private string result;

  void Start()
  {
    joystickId = -1;
    bindingIndex = -1;

    result = string.Empty;
  }


  void Update()
  {
    if (joystickId < 0)
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
    ui.SetDetectedDevices(Input.GetJoystickNames());

    var key = CheckForKey();
    if (key != KeyCode.None)
    {
      // Convert Joystick3Button42 to 3
      try
      {
        joystickId = int.Parse(key.ToString().Replace("Joystick", "").Split(new string[] { "Button" }, System.StringSplitOptions.RemoveEmptyEntries)[0]);
        joystickName = Input.GetJoystickNames()[joystickId - 1];

        // Launch binding UI
        ui.ShowMappings("#" + joystickId + " " + joystickName);

        NextBinding();
      }
      catch (System.Exception)
      {

      }
    }
  }

  private void UpdateDeviceBinding()
  {
    string binding = string.Empty;

    // Skip or restart?
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      NextBinding();
      return;
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
      Application.LoadLevel(Application.loadedLevelName);
      return;
    }

    // Axes?
    if (lookForAnalogs)
    {
      var analogs = CheckForAnalogs();
      if (analogs.Count > 0)
      {
        binding = string.Empty;

        foreach (var axes in analogs)
        {
          binding += "Joystick" + axes.Key + "Analog" + axes.Value + " ";
        }
      }
    }
    // Key?
    else
    {
      var key = CheckForKey();
      if (key != KeyCode.None)
      {
        binding = key.ToString();
      }
    }

    if (string.IsNullOrEmpty(binding) == false)
    {
      ui.SetBindingValue(bindingIndex, binding);

      result += string.Format("{0}\t=\t{1}\r\n", bindingHandle, binding);

      NextBinding();
    }
  }

  private void NextBinding()
  {
    bindingIndex++;
    lookForAnalogs = false;

    switch (bindingIndex)
    {
      case 0:
        bindingHandle = "A / Cross";
        break;
      case 1:
        bindingHandle = "B / Circle";
        break;
      case 2:
        bindingHandle = "X / Square";
        break;
      case 3:
        bindingHandle = "Y / Triangle";
        break;
      case 4:
        bindingHandle = "Start";
        break;
      case 5:
        bindingHandle = "Back / Select";
        break;
      case 6:
        bindingHandle = "RT / R2";
        lookForAnalogs = true;
        break;
      case 7:
        bindingHandle = "LT / L2";
        lookForAnalogs = true;
        break;
      case 8:
        bindingHandle = "RB / R1";
        break;
      case 9:
        bindingHandle = "LB / L1";
        break;
      case 10:
        bindingHandle = "DPad Up";
        lookForAnalogs = true;
        break;
      case 11:
        bindingHandle = "DPad Down";
        lookForAnalogs = true;
        break;
      case 12:
        bindingHandle = "DPad Left";
        lookForAnalogs = true;
        break;
      case 13:
        bindingHandle = "DPad Right";
        lookForAnalogs = true;
        break;
      case 14:
        bindingHandle = "Left stick";
        lookForAnalogs = true;
        break;
    }

    ui.SetActiveBinding(bindingIndex, bindingHandle);
  }

  private KeyCode CheckForKey(string id = "")
  {
    foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
    {
      // Get "JoystickNButtonX" keys but not "JoystickButtonX" keys
      if (k.ToString().Contains("Joystick" + id) && !k.ToString().Contains("JoystickButton"))
      {
        if (Input.GetKeyDown(k))
        {
          Debug.Log("Button input detected: " + k);

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
        string s = string.Format("joystick {0} analog {1}", i, a);
        float f = Input.GetAxis(s);

        const float DEAD_ZONE = 0.2f;

        if (Mathf.Abs(f) > DEAD_ZONE)
        {
          Debug.Log("Analog input detected: " + s + " = " + f);

          analogs.Add(new KeyValuePair<int, int>(i, a));
        }
      }
    }

    return analogs;
  }
}
