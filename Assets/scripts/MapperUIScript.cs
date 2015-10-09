using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

  void Start()
  {
    startPanel.SetActive(true);
    mappingPanel.SetActive(false);
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
      detectedDevicesLabel.text = devices[i] + (i < devices.Length - 1 ? ", " : "");
    }
  }

  public void ShowMappings()
  {
    startPanel.SetActive(false);
    mappingPanel.SetActive(true);
  }
}
