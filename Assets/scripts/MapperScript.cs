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

  private static int count = 0;

  [Header("UI")]
  public MapperUIScript ui;

  private int joystickId;
  private string joystickName;

  private int bindingIndex;
  private string bindingHandle;
  private bool lookForAnalogs;
  private string result;

  private Dictionary<string, float> initialAnalogValues;
  private float delayBetweenTwoAnalogs;

  private List<string> bindingsUsed = new List<string>();
  private bool allowDoublon;

  void Start()
  {
    joystickId = -1;
    bindingIndex = -1;
    initialAnalogValues = new Dictionary<string, float>();

    result = string.Empty;
    result += Application.platform + "\r\n";
    result += System.DateTime.Now + "\r\n";
  }


  void Update()
  {
    if (joystickId < 0)
    {
      UpdateNoDevice();
    }
    else
    {
      delayBetweenTwoAnalogs -= Time.deltaTime;
      if (delayBetweenTwoAnalogs <= 0f)
      {
        UpdateDeviceBinding();
      }
    }

    if (lookForAnalogs == false || delayBetweenTwoAnalogs > 1f)
    {
      UpdateAnalogValues();
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

        result += "Joystick" + joystickId + "\r\n";
        result += joystickName + "\r\n";

        // Next step
        bindingsUsed.Clear();
        delayBetweenTwoAnalogs = 2f;
        NextBinding();
      }
      catch (System.Exception)
      {

      }
    }
  }

  private void UpdateDeviceBinding()
  {
    var binding = string.Empty;

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
      var analogs = CheckForAnalogs(true, joystickId);
      if (analogs.Count > 0)
      {
        binding = string.Empty;

        foreach (var axes in analogs)
        {
          binding += "Joystick" + axes.Key + "Analog" + axes.Value + " ";
        }

        delayBetweenTwoAnalogs = 0.75f;
      }
    }

    // Key? Even for analog
	if (string.IsNullOrEmpty(binding))
    {
      var key = CheckForKey(joystickId.ToString());
      if (key != KeyCode.None)
      {
        binding = key.ToString();
      }
    }

    if (string.IsNullOrEmpty(binding) == false)
    {
      if (bindingsUsed.Contains(binding) == false || allowDoublon)
      {
        ui.SetBindingValue(bindingIndex, binding);

        bindingsUsed.Add(binding);

        result += string.Format("{0} = {1}\r\n", bindingHandle, binding);

        NextBinding();
      }
    }
  }

  private void UpdateAnalogValues()
  {
    initialAnalogValues.Clear();

    // A joystick is moving?
    for (var i = 1; i <= JOYSTICK_COUNT; i++)
    {
      if (joystickId >= 0 && joystickId != i) continue;

      for (var a = 0; a <= JOYSTICK_ANALOG_COUNT; a++)
      {
        var s = string.Format("joystick {0} analog {1}", i, a);

        // Get current value
        var f = Input.GetAxis(s);

        initialAnalogValues.Add(s, f);
      }
    }
  }

  private void NextBinding()
  {
    bindingIndex++;
    lookForAnalogs = false;

    switch (bindingIndex)
    {
      case 0:
        bindingHandle = "Left Stick";
        lookForAnalogs = true;
        break;
      case 1:
        bindingHandle = "A / Cross";
        break;
      case 2:
        bindingHandle = "B / Circle";
        break;
      case 3:
        bindingHandle = "X / Square";
        break;
      case 4:
        bindingHandle = "Y / Triangle";
        break;
      case 5:
        bindingHandle = "Start";
        break;
      case 6:
        bindingHandle = "Select / Back";
        break;
      case 7:
        bindingHandle = "Right Trigger / R2";
        lookForAnalogs = true;
        break;
      case 8:
        bindingHandle = "Left Trigger / L2";
        lookForAnalogs = true;
        break;
      case 9:
        bindingHandle = "Right Bumper / R1";
        break;
      case 10:
        bindingHandle = "Left Bumper / L1";
        break;
      case 11:
        bindingHandle = "DPad Up";
        lookForAnalogs = true;
        allowDoublon = true;
        break;
      case 12:
        bindingHandle = "DPad Down";
        lookForAnalogs = true;
        allowDoublon = true;
        break;
      case 13:
        bindingHandle = "DPad Left";
        lookForAnalogs = true;
        allowDoublon = true;
        break;
      case 14:
        bindingHandle = "DPad Right";
        lookForAnalogs = true;
        allowDoublon = true;
        break;

      default:
        // Over
        if (Export())
        {
          ui.ShowResult();
        }
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

  private List<KeyValuePair<int, int>> CheckForAnalogs(bool log = false, int joystickId = -1)
  {
    var analogs = new List<KeyValuePair<int, int>>();

    // A joystick is moving?
    for (var i = 1; i <= JOYSTICK_COUNT; i++)
    {
      if (joystickId >= 0 && joystickId != i) continue;

      for (var a = 0; a <= JOYSTICK_ANALOG_COUNT; a++)
      {
        var s = string.Format("joystick {0} analog {1}", i, a);

        // Get current value
        var f = Input.GetAxis(s);

        // Get previous
        var previousValue = 0f;
        initialAnalogValues.TryGetValue(s, out previousValue);

        // Value changed & is valid?
        const float DEAD_ZONE = 0.2f;

        if (Mathf.Abs(f) > DEAD_ZONE && previousValue != f)
        {
          if (log)
          {
            Debug.Log("Analog input detected: " + s + " = " + f);
          }

          analogs.Add(new KeyValuePair<int, int>(i, a));
        }
      }
    }

    return analogs;
  }

  private bool Export()
  {
    count++;

    var filename = "./" + joystickName + "_" + count + ".txt";

    Debug.Log("EXPORT at " + filename);

    System.IO.File.WriteAllText(filename, result);

    return true;
  }
}
