using UnityEngine;

public class TouchTapHandler : MonoBehaviour
{
	public enum TouchTaps
	{
		Unknown,
		SingleTap,
		DoubleTap
	}

	public float   DoubleTapTimeout = 0.3f;
	public KeyCode KeyboardDoubleTapKey = KeyCode.Space;

	private float _lastTapTime = 0;

	public TouchTaps GetTouchTap()
	{
		var touch = Input.touchCount > 0 ? Input.GetTouch( 0 ) : new Touch();
		var result = TouchTaps.Unknown;

		if ( ( Input.touchCount > 0 && touch.phase == TouchPhase.Stationary ) || Input.GetKeyDown(KeyboardDoubleTapKey) || Input.GetMouseButtonDown(0))
		{
			result = TouchTaps.SingleTap;

			if ( Time.time < ( _lastTapTime + DoubleTapTimeout ) )
			{
				result = TouchTaps.DoubleTap;
				_lastTapTime = 0;
			}

			_lastTapTime = Time.time;
		}

		return result;
	}
	
}