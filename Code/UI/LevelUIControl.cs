using Godot;
using System;

namespace PlatformerExample
{
	public partial class LevelUIControl : Control
	{
		[Export] private Label _scoreLabel = null;
		[Export] private PauseDialog _pauseDialog = null;
		[Export] private BaseButton _pauseButton = null;

		public override void _Ready()
		{
			base._Ready();

			if (_scoreLabel == null)
			{
				_scoreLabel = GetNode<Label>("ScoreLabel");
			}

			GameManager.Instance.Connect(GameManager.SignalName.ScoreChanged,
				new Callable(this, nameof(OnScoreChanged)));

			OnScoreChanged(GameManager.Instance.Score);

			if (_pauseButton != null)
			{
				_pauseButton.Connect(BaseButton.SignalName.Pressed,
					new Callable(this, nameof(OnPauseButtonPressed)));
			}
		}

		private void OnPauseButtonPressed()
		{
			if (_pauseDialog != null)
			{
				_pauseDialog.Open();
			}
		}

		private void OnScoreChanged(int currentScore)
		{
			if (_scoreLabel != null)
			{
				_scoreLabel.Text = $"Score: {currentScore}";
			}
		}
	}
}