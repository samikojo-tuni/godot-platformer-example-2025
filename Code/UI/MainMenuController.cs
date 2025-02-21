using Godot;
using System;

namespace PlatformerExample
{
	public partial class MainMenuController : Control
	{
		// Viittaukset nappeihin, jotta niiden Pressed-signaalia 
		// voidaan kuunnella ja niihin reagoida.
		[Export] private Button _newGameButton = null;
		[Export] private Button _optionsButton = null;
		[Export] private Button _quitButton = null;
		[Export] private DialogWindow _optionsWindow = null;

		private SceneTree _mainMenuSceneTree = null;

		public override void _Ready()
		{
			base._Ready();

			_mainMenuSceneTree = GetTree();
			if (_mainMenuSceneTree == null)
			{
				GD.PrintErr("SceneTree not found!");
			}

			// Tämä rivi aloittaa painikkeen Pressed-signaalin kuuntelun.
			_newGameButton.Connect(Button.SignalName.Pressed,
				new Callable(this, nameof(OnNewGamePressed)));

			_optionsButton.Connect(Button.SignalName.Pressed,
				new Callable(this, nameof(OnOptionsPressed)));

			_quitButton.Connect(Button.SignalName.Pressed,
				new Callable(this, nameof(OnQuitPressed)));
		}

		private void OnNewGamePressed()
		{
			GD.Print("New game pressed");

			// Levelin polku taika-arvona ei ole kovin fiksu tapa toteuttaa tätä.
			// Voisi korjata esim. staattisella Config-luokalla tai resurssi-
			// oliolla, jossa viittaukset leveleihin.
			_mainMenuSceneTree.ChangeSceneToFile("res://Maps/Level1.tscn");
		}

		private void OnOptionsPressed()
		{
			if (_optionsWindow != null)
			{
				_optionsWindow.Open();
			}
		}

		private void OnQuitPressed()
		{
			_mainMenuSceneTree.Quit();
		}
	}
}