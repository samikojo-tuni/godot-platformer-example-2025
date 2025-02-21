using Godot;

namespace PlatformerExample
{
	public partial class GameManager : Node
	{
		#region Singleton pattern
		// Singleton pattern: https://gameprogrammingpatterns.com/singleton.html
		public static GameManager Instance { get; private set; }

		public override void _Ready()
		{
			Instance = this;
		}
		#endregion

		[Signal] public delegate void ScoreChangedEventHandler(int score);

		private int _score = 0;

		public int Score
		{
			get { return _score; }
			set
			{
				_score = value;
				// Signaali välittää tiedon pisteiden muuttumisesta muille komponenteille.
				EmitSignal(SignalName.ScoreChanged, _score);
			}
		}

		public void RestartGame()
		{
			Score = 0;
		}

		public void PauseGame()
		{
			Engine.TimeScale = 0;
		}

		public void ResumeGame()
		{
			Engine.TimeScale = 1;
		}
	}
}