using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using kube;
using static BattleControllerScript;
using System.Text;

public class Console : MonoBehaviour
{
	private struct ConsoleMessage
	{
		public readonly string message;

		public readonly string stackTrace;

		public readonly LogType type;

		public ConsoleMessage(string message, string stackTrace, LogType type)
		{
			this.message = message;
			this.stackTrace = stackTrace;
			this.type = type;
		}
	}

	private const int margin = 20;

	public static readonly Version version = new Version(1, 0);

	public KeyCode toggleKey = KeyCode.BackQuote;

	private List<ConsoleMessage> entries = new List<ConsoleMessage>();

	private Vector2 scrollPos;

	private bool show;

	private bool collapse;

	private GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");

	private GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

	private string cmd = string.Empty;

	private static char[] _separators = new char[1] { ' ' };

	private void OnEnable()
	{
		Application.RegisterLogCallback(HandleLog);
	}

	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	private void Update()
	{
		
        if (Cub2Input.GetKeyDown(toggleKey))
		{
			show = !show;
			if (show)
			{
				ControlFreak2.CFScreen.lockCursor = false;
			}
			GUI.FocusControl("ConsoleInput");
			scrollPos = new Vector2(0f, entries.Count * 200);
		}
		if (entries.Count >=140){
            entries.Clear();
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUI.depth = -6;
			ConsoleWindow(0);
		}
	}

	private void Execute(string args)
	{
		string[] array = args.Split(_separators);
		string text = array[0];
		MethodInfo method = GetType().GetMethod("CMD_" + text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (method != null)
		{
			method.Invoke(this, new object[1] { array });
		}
		else
		{
			Kube.SendMonoMessage("CMD_" + text, new object[1] { array });
		}
		entries.Add(new ConsoleMessage("> " + text, string.Empty, LogType.Log));
	}

	private void CMD_clr(string[] argv)
	{
		entries.Clear();
	}

    [HideInInspector]
    public PlayerInfo[] TplayersInfo;

	private void ConsoleWindow(int windowID)
	{
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
		{
			Execute(cmd);
			cmd = string.Empty;
			return;
		}
		GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty);
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 100));
		for (int i = 0; i < entries.Count; i++)
		{
			ConsoleMessage consoleMessage = entries[i];
			if (!collapse || i <= 0 || !(consoleMessage.message == entries[i - 1].message))
			{
				switch (consoleMessage.type)
				{
					case LogType.Error:
					case LogType.Exception:
						GUI.contentColor = Color.red;
						GUILayout.Label(consoleMessage.stackTrace);
						break;
					case LogType.Warning:
						GUI.contentColor = Color.yellow;
						break;
					default:
						GUI.contentColor = Color.white;
						break;
				}
				GUILayout.Label(consoleMessage.message);
			}
		}
		GUI.contentColor = Color.white;
		GUILayout.EndScrollView();
		GUI.SetNextControlName("ConsoleInput");
		cmd = GUILayout.TextField(cmd);
		GUILayout.BeginHorizontal();
		collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

		if (GUILayout.Button("<b>Player Info</b>", new GUILayoutOption[0]))
		{
			CMD_plist();
        }
		GUILayout.EndHorizontal();
	}

    private void CMD_plist()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("=======================\r\n");
        for (int i = 0; i < Kube.BCS.playersInfo.Length; i++)
        {
            stringBuilder.Append("id: " + Kube.BCS.playersInfo[i].serverId + " ." + AuxFunc.DecodeRussianName(Kube.BCS.playersInfo[i].Name) + "\r\n");
        }
        string text = stringBuilder.ToString();
        TextEditor textEditor = new TextEditor();
        textEditor.content = new GUIContent(text);
        textEditor.SelectAll();
        textEditor.Copy();
        Debug.Log(text);
    }

    private void HandleLog(string message, string stackTrace, LogType type)
	{
		ConsoleMessage item = new ConsoleMessage(message, stackTrace, type);
		entries.Add(item);
	}
}
