using Godot;
using System;

namespace PlatformerExample
{
	public partial class KillZone : Area2D
	{
		public override void _Ready()
		{
			Connect(SignalName.BodyEntered,
				new Callable(this, nameof(OnBodyEntered)));
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is MainCharacter playerCharacter)
			{
				playerCharacter.Die();
			}
		}
	}
}