using Godot;
using System;

namespace PlatformerExample
{
	public abstract partial class Collectable : Area2D
	{
		public override void _Ready()
		{
			Error connectionError = Connect(SignalName.BodyEntered,
				new Callable(this, nameof(OnBodyEntered)));

			if (connectionError != Error.Ok)
			{
				GD.PrintErr($"Error connecting BodyEntered signal: {connectionError}");
			}
		}

		private void OnBodyEntered(Node2D body)
		{
			// node on jokin toinen objekti, joka saapui tämän Area2D:n
			// alueelle.
			if (body is MainCharacter playerCharacter)
			{
				// Jos alueelle saapunut olio on MainCharacter,
				// kerättävä esine kerätään.
				Collect(playerCharacter);
				// Poista kerättävä esine.
				QueueFree();
			}
		}

		protected abstract void Collect(MainCharacter playerCharacter);
	}
}