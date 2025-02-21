using Godot;

namespace PlatformerExample
{
	public partial class Timer : Node
	{
		[Signal] public delegate void TimerCompletedEventHandler();
		[Export] private double _time = 1.0f;
		[Export] private bool _autoStart = true;

		private double _timer = 0;
		private bool _isRunning = false;

		public override void _Ready()
		{
			base._Ready();

			Restart(_autoStart);
		}

		public override void _Process(double delta)
		{
			if (_isRunning)
			{
				_timer -= delta;

				if (_timer <= 0)
				{
					_timer = 0;
					_isRunning = false;

					EmitSignal(SignalName.TimerCompleted);
				}
			}
		}

		public void SetTime(double time)
		{
			_time = time;
			_timer = _time;
		}

		public void Start()
		{
			_isRunning = true;
		}

		public void Stop()
		{
			_isRunning = false;
		}

		public void Restart(bool autoStart)
		{
			SetTime(_time);

			if (autoStart)
			{
				Start();
			}
			else
			{
				Stop();
			}
		}
	}
}