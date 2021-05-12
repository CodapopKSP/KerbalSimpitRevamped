﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalSimpit.SimpitGUI
{


	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class WindowFlight : Window
	{

	}

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class WindowSpaceCenter : Window
	{

	}
	
	public class Window : MonoBehaviour
	{
		static Rect windowpos;
		private static bool gui_enabled;
		private static bool hide_ui;
		private KSPit simpitInstance;

		static Window instance;


		public static void ToggleGUI()
		{
			gui_enabled = !gui_enabled;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		public static void HideGUI()
		{
			gui_enabled = false;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		public static void ShowGUI()
		{
			gui_enabled = true;
			if (instance != null)
			{
				instance.UpdateGUIState();
			}
		}

		void UpdateGUIState()
		{
			enabled = !hide_ui && gui_enabled;
		}

		void onHideUI()
		{
			hide_ui = true;
			UpdateGUIState();
		}

		void onShowUI()
		{
			hide_ui = false;
			UpdateGUIState();
		}

		public void Awake()
		{
			instance = this;
			GameEvents.onHideUI.Add(onHideUI);
			GameEvents.onShowUI.Add(onShowUI);
		}

		void OnDestroy()
		{
			instance = null;
			GameEvents.onHideUI.Remove(onHideUI);
			GameEvents.onShowUI.Remove(onShowUI);
		}

		void Start()
		{
			UpdateGUIState();
			simpitInstance = (KSPit) FindObjectOfType(typeof(KSPit));
			if(simpitInstance == null)
            {
				Debug.Log("Simpit : the GUI could not locate the KSPit instance. GUI will not work");
            }
		}

		void WindowGUI(int windowID)
		{
			GUILayout.BeginVertical();

			GUILayout.Label("Status : " + KSPit.SerialPorts.First().portStatus);
			GUILayout.Label("Port used : " + KSPit.SerialPorts.First().portName);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Start"))
			{
				if(simpitInstance != null)
                {
					simpitInstance.OpenPorts();
				}
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Close"))
			{
				if (simpitInstance != null)
				{
					simpitInstance.ClosePorts();
				}
			}
			GUILayout.EndHorizontal();


			GUILayout.EndVertical();
			UnityEngine.GUI.DragWindow(new Rect(0, 0, 1000, 20));
		}

		void OnGUI()
		{
			if (gui_enabled)
			{
				UnityEngine.GUI.skin = HighLogic.Skin;
				windowpos = GUILayout.Window(GetInstanceID(), windowpos, WindowGUI, "Kerbal Simpit", GUILayout.Width(200));
			}
		}
	}
}
