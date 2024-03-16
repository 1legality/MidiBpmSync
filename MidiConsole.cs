using MidiClock.Services;
using NAudio.Midi;
using System;
using System.Collections.Generic;

namespace MidiClock;
public class MidiConsole
{
    protected MidiService _midiService;

    public MidiConsole()
    {
        _midiService = new MidiService();
    }

    public void StartConsole()
    {
        SelectMasterDevice();
        Console.Clear();
        SelectSlaveDevices();
        Console.Clear();
        PrintSelectedDevices();

        MidiSyncListener midiSyncListener = new(_midiService.GetMasterIndex(), _midiService.GetSelectedSlavesDevicesIndexes());

        do
        {

        }
        while (true);
    }

    protected void SelectMasterDevice()
    {
        var midiInCapabilities = _midiService.GetMidiInCapabilities();

        for (int deviceId = 0; deviceId < midiInCapabilities.Count; deviceId++)
        {
            Console.WriteLine($"{deviceId}: Input - {midiInCapabilities[deviceId].ProductName}");
        }

        int selectedDeviceId = -1;
        do
        {
            Console.WriteLine($"Select the master device by entering its number:");
            if (int.TryParse(Console.ReadLine(), out selectedDeviceId))
            {
                if (selectedDeviceId >= 0 && selectedDeviceId < midiInCapabilities.Count)
                {
                    _midiService.SelectMasterDevice(selectedDeviceId);
                    break; // Valid selection
                }
            }
            Console.WriteLine("Invalid selection. Please try again.");
        }
        while (true);
    }

    protected void SelectSlaveDevices()
    {
        var midiOutputCapabilities = _midiService.GetMidiOutputDevices();

        for (int deviceId = 0; deviceId < midiOutputCapabilities.Count; deviceId++)
        {
            Console.WriteLine($"{deviceId}: Input - {midiOutputCapabilities[deviceId].ProductName}");
        }

        List<int> selectedDeviceIds = new List<int>();
        Console.WriteLine($"Enter output device numbers separated by commas (e.g., 0,1,2):");

        string[] inputs = Console.ReadLine().Split(',');
        foreach (var input in inputs)
        {
            if (int.TryParse(input.Trim(), out int deviceId))
            {
                if (deviceId >= 0 && deviceId < midiOutputCapabilities.Count && !selectedDeviceIds.Contains(deviceId))
                {
                    selectedDeviceIds.Add(deviceId);
                }
                else
                {
                    Console.WriteLine($"Invalid selection: {deviceId}. Ignoring.");
                }
            }
        }

        _midiService.SelectSlaveDevices(selectedDeviceIds);
    }

    protected void PrintSelectedDevices()
    {
        Console.WriteLine($"Master : {_midiService.GetMaster().ProductName}");
        Console.WriteLine("Slaves");
        foreach (var slave in _midiService.GetSelectedSlavesDevices())
        {
            Console.WriteLine($"- {slave.ProductName}");
        }
    }
}
