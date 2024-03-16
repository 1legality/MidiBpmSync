using NAudio.Midi;
namespace MidiClock.Services;

public class MidiSyncListener
{
    private MidiIn midiIn;
    private List<MidiOut> midiOuts; // List of slave device outputs

    public MidiSyncListener(int masterDeviceId, List<int> slaveDeviceIds)
    {
        midiIn = new MidiIn(masterDeviceId);
        midiIn.MessageReceived += MidiIn_MessageReceived;
        midiIn.Start();

        midiOuts = slaveDeviceIds.Select(id => new MidiOut(id)).ToList();
    }

    private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
    {
        var message = e.MidiEvent.CommandCode;
        //ForwardToSlaves(e.RawMessage);
        switch (message)
        {
            case MidiCommandCode.TimingClock: // MIDI clock tick
                                              // Forward clock messages to slaves
                ForwardToSlaves(e.RawMessage);
                break;
            case MidiCommandCode.StartSequence: // 0xFA
            case MidiCommandCode.ContinueSequence: // 0xFB
            case MidiCommandCode.StopSequence: // 0xFC
                ForwardToSlaves(e.RawMessage);
                break;
        }
    }

    private void ForwardToSlaves(int message)
    {
        foreach (var midiOut in midiOuts)
        {
            midiOut.Send(message);
        }
    }
}
