using NAudio.Midi;
using System.Diagnostics.Metrics;

namespace MidiClock;
internal class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        MidiConsole midiConsole = new();
        midiConsole.StartConsole();
    }
}
