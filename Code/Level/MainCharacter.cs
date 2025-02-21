using Godot;
using System;

namespace PlatformerExample
{
	public partial class MainCharacter : CharacterBody2D
	{
		public enum Direction
		{
			None = 0,
			Left,
			Right,
			Up,
			Down
		}

		[Export] private float _speed = 300.0f;
		[Export] private float _jumpVelocity = -400.0f;
		[Export] private float _swipeMoveTime = 0.5f;

		#region Swipe data
		private Vector2 _touchStartPosition = default(Vector2);
		private float _swipeThreshold = 50.0f; // Taikaluku
		private Direction _swipeDirection = Direction.None;
		private double _swipeMoveTimer = 0;
		#endregion

		public override void _Input(InputEvent @event)
		{
			base._Input(@event);

			if (@event is InputEventScreenTouch touchEvent)
			{
				// Input vastaanotettu ja se on tyyppiä InputEventScreenTouch
				if (touchEvent.Pressed && _swipeDirection == Direction.None)
				{
					// Kosketus alkoi
					// Tallenna kosketuksen aloitussijainti näytöllä
					_touchStartPosition = touchEvent.Position;
				}
				else
				{
					// Kun touchEvent.Pressed saa arvon false, kosketus päättyy
					// Nyt voimme laskea kesketuksen aikana muodostuneen vektorin
					Vector2 touchEndPosition = touchEvent.Position;
					DetectSwipe(_touchStartPosition, touchEndPosition);
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}

			// Vähennä swipe timerin aikaa
			_swipeMoveTimer -= delta;

			velocity = SwipeMove(velocity);

			Velocity = velocity;
			MoveAndSlide();
		}

		private Vector2 SwipeMove(Vector2 velocity)
		{
			if (_swipeMoveTimer <= 0)
			{
				// Swipe on loppu, enää ei liikuta mihinkään suuntaan
				_swipeDirection = Direction.None;
				// Hidasta vauhtia tasaisesti
				velocity.X = Mathf.MoveToward(velocity.X, 0, _speed);
			}
			else
			{
				// Swipea vastaava liike on edelleen käynnissä, liiku
				// swipen suuntaan
				switch (_swipeDirection)
				{
					case Direction.Left:
						velocity.X = -_speed;
						break;
					case Direction.Right:
						velocity.X = _speed;
						break;
					case Direction.Up:
						// Hyppy. Toisin kuin vaakasuuntainen liike, joka annetaan
						// hahmolle tasaisesti usean framen aikana, hyppyvoima
						// annetaan hahmolle kerralla yhtenä impulssina.
						velocity.Y = _jumpVelocity;
						_swipeMoveTimer = 0; // Estä voiman antaminen uudelleen seuraavalla framella.
						break;
					case Direction.Down:
						// Ei alas suuntautuvaa liikettä, nollaa liikeajastin
						_swipeMoveTimer = 0;
						break;
				}
			}

			return velocity;
		}

		private void DetectSwipe(Vector2 swipeStart, Vector2 swipeEnd)
		{
			Vector2 swipeVector = swipeEnd - swipeStart;
			if (swipeVector.Length() > _swipeThreshold)
			{
				// Swipe tulkittu suoritetuksi. Lyhyitä kosketuksia ei tulkita
				// swipeksi.
				float lengthX = Mathf.Abs(swipeVector.X);
				float lengthY = Mathf.Abs(swipeVector.Y);

				if (lengthX > lengthY)
				{
					// Horisontaalinen swipe
					if (swipeVector.X > 0)
					{
						// Swipe oikealle
						_swipeDirection = Direction.Right;
					}
					else
					{
						// Swipe vasemmalle
						_swipeDirection = Direction.Left;
					}
				}
				else
				{
					// Vertikaalinen swipe
					if (swipeVector.Y > 0)
					{
						// Swipe alas
						_swipeDirection = Direction.Down;
					}
					else
					{
						// Swipe ylös
						_swipeDirection = Direction.Up;
					}
				}
			}

			// Alusta Swipe move timer.
			_swipeMoveTimer = _swipeMoveTime;
		}

		public void Die()
		{
			// Pelaajahahmon kuollessa, nollaa pisteet ja lataa scene uudelleen.
			GameManager.Instance.RestartGame();

			// TODO: Tämä ei ehkä kuulu pelaajahahmon vastuualueelle. 
			// Refactoroi tämä.
			SceneTree sceneTree = GetTree();
			if (sceneTree != null)
			{
				sceneTree.ReloadCurrentScene();
			}
		}
	}
}