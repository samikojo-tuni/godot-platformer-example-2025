using Godot;
using System;

namespace PlatformerExample
{
	public partial class PauseDialog : DialogWindow
	{
		public override void Open()
		{
			base.Open();
			GameManager.Instance.PauseGame();
		}

		public override void Close()
		{
			base.Close();
			GameManager.Instance.ResumeGame();
		}
	}
}
