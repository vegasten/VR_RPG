using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputReader : MonoBehaviour
{
    List<InputDevice> _inputDevices = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputReader();
    }

    private void InitializeInputReader()
    {
        InputDevices.GetDevices(_inputDevices);
        foreach (InputDevice device in _inputDevices)
        {
            Debug.Log(device.name + " " + device.characteristics);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputDevices.Count < 3)
            InitializeInputReader();
    }

    public List<InputDevice> GetAllInputDevices()
    {
        return _inputDevices;
    }
}
