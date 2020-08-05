using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct CombatLogEntry // Used for starting and Cataloging Log keywords
{
    public string LogMessage; 
    public string LogTimeString;
    public string LogEntry;
    public DateTime LogTime;
    

    public CombatLogMessageType CombatLogMessageType;

}

public enum CombatLogMessageType // Used for setting up for meassages according to certain parameters
{
    Standard, Important, System, Warning, Error,
}

public static class Combat 
{
    public static List<CombatLogEntry> LogEntries = new List<CombatLogEntry>();
    public static int MaxLogEntries = 300; // Maximum number of entires visible on the Log Window
    public static bool OrderDesc = true; // The order in which the messages are listed.
    public static bool UseDebugLog = true; // The choice to use a debug log for displaying messages.

    public static void log (string message) // Initializes a Standard log message 
    {
        Log (CombatLogMessageType.Standard, message);
    }
    public static void LogWarning (string message) // Initializes a Warning Log Message
    {
        Log (CombatLogMessageType.Warning, message); 
    }
    public static void LogError(string message) // Initializes a Error Log Message
    {
        Log (CombatLogMessageType.Error, message);
    }
    public static void LogImportant(string message) // Initializes an Important Message
    {
        Log (CombatLogMessageType.Important, message);
    }
    public static void LogSystemInfo(string message) // Initializes a System Log Message
    {
        Log (CombatLogMessageType.System, message);
    }

    public static void Log(CombatLogMessageType type,string message) // Establishes how a Log should behave in the Combat UI
    {
        var entry = new CombatLogEntry();
        entry.CombatLogMessageType = type;
        entry.LogTime = DateTime.Now;
        entry.LogTimeString = GetFormatedDate(entry.LogTime);
        entry.LogMessage = message;

        AddEntry (entry);

        if (UseDebugLog) // In case there is an error with broadcasting messages in the CombatLog
        {
            switch (type)
            {
                case CombatLogMessageType.Error: Debug.LogError(message); break;
                case CombatLogMessageType.Warning: Debug.LogWarning(message);break;
                default: Debug.LogWarning(message); break;
            }
        }

        if (OrderDesc)
            OrderDescending();
        else
            OrderAscending();
        if (LogEntries.Count > MaxLogEntries)
            RemoveEntry(MaxLogEntries);
    }

    public static void ClearCombatLog() // clears the CombatLog entirely
    {
        LogEntries.Clear();
    }

    public static void OrderDescending() // moves the Log downward
    {
        LogEntries = LogEntries.OrderByDescending(x => x.LogTime).ToList();
    }
    public static void OrderAscending() // moves the Log Upward
    {
        LogEntries = LogEntries.OrderBy(x => x.LogTime).ToList();
    }

    public static void AddEntry(CombatLogEntry entry) // Adds an entry to the Log
    {
        LogEntries.Add(entry);
    }

    public static void RemoveEntry(int id) // Removes an Entry from the Log
    {
        LogEntries.RemoveAt(id);
    }

    private static string GetFormatedDate(DateTime time) // States the date and time of when the entry was made
    {
        const string format = "t";
        return time.ToString(format);
    }
}


//UI Snippet
public class CombatLogUI
{
    public Vector2 scrollPosition = Vector2.zero;
    void OnGUI()
    {
        //CombatLog layout
        GUILayout.BeginArea(new Rect(10, Screen.height - 175, 1000, 1000));
        GUILayout.Box("", GUILayout.Width(375), GUILayout.Height(150));
        GUILayout.EndArea();
        

        GUILayout.BeginArea(new Rect(10, Screen.height - 175, 1000, 1000));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(375), GUILayout.Height(150));

        GUILayout.BeginVertical();
        foreach (var entry in Combat.LogEntries) // When a specific a Log Message is put into the Log, it is differentiated by color as well as name
        {
            switch (entry.CombatLogMessageType)
            {
                case CombatLogMessageType.Standard:
                    GUILayout.Label("<color = white>" + entry.LogTimeString + ":" + entry.LogEntry  + "</ color>", GUILayout.Width(350));
                    GUILayout.Space(-10);
                    break;
                case CombatLogMessageType.Warning:
                    GUILayout.Label("<color = yellow>" + entry.LogTimeString + ":" + entry.LogEntry + "</ color>", GUILayout.Width(350));
                    GUILayout.Space(-10);
                    break;
                case CombatLogMessageType.System:
                    GUILayout.Label("<color = green>" + entry.LogTimeString + "[system]:" + entry.LogEntry + "</ color>", GUILayout.Width(350));
                    GUILayout.Space(-10);
                    break;
                case CombatLogMessageType.Important:
                    GUILayout.Label("<color = orange>" + entry.LogTimeString + ":" + entry.LogEntry + "</ color>", GUILayout.Width(350));
                    GUILayout.Space(-10);
                    break;
                case CombatLogMessageType.Error:
                    GUILayout.Label("<color = red>" + entry.LogTimeString + ":" + entry.LogEntry + "</ color>", GUILayout.Width(350));
                    GUILayout.Space(-10);
                    break;

            }
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}
