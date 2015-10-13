using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles UI for the mapping
/// </summary>
public class MapperUIScript : MonoBehaviour
{
  #region Constants

  private static readonly string INSTRUCTIONS_1 = "Press any button of a device to start.";
  private static readonly string INSTRUCTIONS_2 = "Use ESC to skip and R to restart.\nPress the buttons in the following order:";

  #endregion

  public Text devicesLabel;
  public Text instructionsLabel;

  [Header("Bindings")]
  public Text[] bindingsHandles;
  public Text[] bindings;

  private Text currentActiveBindingHandle, currentActiveBinding;

  void Start()
  {
    instructionsLabel.text = INSTRUCTIONS_1;

    foreach (var b in bindingsHandles)
    {
      b.gameObject.SetActive(false);
    }
    foreach (var b in bindings)
    {
      b.gameObject.SetActive(false);
    }
  }

  /// <summary>
  /// Display all detected controllers
  /// </summary>
  /// <param name="devices"></param>
  public void SetDetectedDevices(string[] devices)
  {
    devicesLabel.text = "Detected: ";

    for (int i = 0; i < devices.Length; i++)
    {
      devicesLabel.text += devices[i] + (i < devices.Length - 1 ? ", " : "");
    }
  }

  public void ShowMappings(string deviceId)
  {
    devicesLabel.text = deviceId;
    instructionsLabel.text = INSTRUCTIONS_2;
  }

  public void SetActiveBinding(int bindingIndex, string bindingHandle)
  {
    if (currentActiveBindingHandle != null)
    {
      currentActiveBindingHandle.color = Color.white;
      currentActiveBindingHandle = null;
    }
    if (currentActiveBinding != null)
    {
      currentActiveBinding.color = Color.white;
      currentActiveBinding = null;
    }

    if (bindingIndex < bindingsHandles.Length)
    {
      currentActiveBindingHandle = bindingsHandles[bindingIndex];
      currentActiveBindingHandle.gameObject.SetActive(true);
      currentActiveBindingHandle.color = Color.green;

      currentActiveBinding = bindings[bindingIndex];
      currentActiveBinding.gameObject.SetActive(true);
      currentActiveBinding.color = Color.green;

      bindingsHandles[bindingIndex].text = bindingHandle;
    }
  }

  public void SetBindingValue(int i, string key)
  {
    if (i < bindings.Length)
    {
      bindings[i].text = key;
    }
  }
}
