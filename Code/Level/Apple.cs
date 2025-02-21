using Godot;

namespace PlatformerExample
{
	public partial class Apple : Collectable
	{
		[Export] private int _score = 1;

		protected override void Collect(MainCharacter playerCharacter)
		{
			GameManager.Instance.Score += _score;
		}
	}
}