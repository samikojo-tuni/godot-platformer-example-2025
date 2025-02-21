namespace PlatformerExample
{
	public partial class Banana : Collectable
	{
		protected override void Collect(MainCharacter playerCharacter)
		{
			playerCharacter.Die();
		}
	}
}