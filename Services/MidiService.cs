using NAudio.Midi;
using System;
using System.Collections.Generic;

namespace MidiClock.Services;
public class MidiService
{
    protected readonly List<MidiInCapabilities> inputDevices = new();
    protected int master = 0;

    protected readonly List<MidiOutCapabilities> outputDevices = new();
    protected List<int> slaves = new();

    public MidiService()
    {
        // Enumerate MIDI input devices
        for (int deviceId = 0; deviceId < MidiIn.NumberOfDevices; deviceId++)
        {
            inputDevices.Add(MidiIn.DeviceInfo(deviceId));
            //Console.WriteLine($"{deviceId}: Input - {MidiIn.DeviceInfo(deviceId).ProductName}");
        }

        // Enumerate MIDI output devices
        for (int deviceId = 0; deviceId < MidiOut.NumberOfDevices; deviceId++)
        {
            outputDevices.Add(MidiOut.DeviceInfo(deviceId));
            //Console.WriteLine($"{deviceId}: Output - {MidiOut.DeviceInfo(deviceId).ProductName}");
        }
    }

    public List<MidiInCapabilities> GetMidiInCapabilities()
    {
        return inputDevices;
    }
    public MidiInCapabilities GetMaster()
    {
        return inputDevices[master];
    }
    
    public int GetMasterIndex()
    {
        return master;
    }

    public void SelectMasterDevice(int deviceCount)
    {
        master = deviceCount;
    }

    public List<MidiOutCapabilities> GetMidiOutputDevices()
    {
        return outputDevices;
    }

    public List<int> GetSelectedSlavesDevicesIndexes()
    {
        return slaves;
    }

    public List<MidiOutCapabilities> GetSelectedSlavesDevices()
    {
        List<MidiOutCapabilities> result = new List<MidiOutCapabilities>();
        foreach (int deviceId in slaves)
        {
            result.Add(MidiOut.DeviceInfo(deviceId));
        }
        return result;
    }

    public void SelectSlaveDevices(List<int> devices)
    {
        slaves = [.. devices];
    }
}
