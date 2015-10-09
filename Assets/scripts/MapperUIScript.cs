using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles UI for the mapping
/// </summary>
public class MapperUIScript : MonoBehaviour
{
  [Header("Panels")]
  public GameObject startPanel;
  public GameObject mappingPanel;

  [Header("Misc")]
  public Text detectedDevicesLabel;

  [Header("Bindings")]
  public Text currentDeviceLabel;
  public Text[] bindingsHandles;
  public Text[] bindings;

  private Text currentActiveBindingHandle, currentActiveBinding;

  void Start()
  {
    startPanel.SetActive(true);
    mappingPanel.SetActive(false);

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
    detectedDevicesLabel.text = string.Empty;
    for (int i = 0; i < devices.Length; i++)
    {
      detectedDevicesLabel.text += devices[i] + (i < devices.Length - 1 ? ", " : "");
    }
  }

  public void ShowMappings(string deviceId)
  {
    startPanel.SetActive(false);
    mappingPanel.SetActive(true);

    currentDeviceLabel.text = deviceId;
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
