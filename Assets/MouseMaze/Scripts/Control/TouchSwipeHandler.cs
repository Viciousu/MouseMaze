using System;
using UnityEngine;

public class TouchSwipeHandler : MonoBehaviour
{
	public enum SwipeDirections
	{
		Unknown,
		Up,
		Down,
		Left,
		Right
	}

	public TouchSwipeHandler()
	{
		IsHandleLeftRightTouches = true;
	}

	public bool IsHandleLeftRightTouches { get; set; }
	
	private bool    _isOffsetChanged = false;
	private float   _minimalSwipeMagnitude = 20f;

	public event Action<SwipeDirections> OnSwipe;

	bool _isLeftDown = false;
	bool _isRightDown = false;
	bool _isUpDown = false;
	bool _isDownDown = false;
	
	Vector2 _touchStartPos = Vector2.zero;

	public SwipeDirections SwipeDirection;
	
	public SwipeDirections GetSwipeDirection ()
	{
		var swipeDirection = SwipeDirections.Unknown;

		if ( Input.touchCount > 0 )
		{
			Touch touch = Input.touches[0];

			if(touch.phase == TouchPhase.Began)
			{
				_touchStartPos = touch.position;
				_isOffsetChanged = false;
			}
			else if (touch.phase == TouchPhase.Moved && !_isOffsetChanged)
			{
				Vector2 deltaPos = touch.position - _touchStartPos;

				if (deltaPos.magnitude > _minimalSwipeMagnitude)
				{
					_isOffsetChanged = true;

					deltaPos.Normalize();

					float dir = Vector2.Dot(new Vector2(1, 0), deltaPos);

					if(deltaPos.y > 0 && dir > -0.7f && dir < 0.7f)
					{
						swipeDirection = SwipeDirections.Up;
					}
					if(deltaPos.y < 0 && dir > -0.7f && dir < 0.7f)
					{
						swipeDirection = SwipeDirections.Down;
					}
					if(dir >= 0.7f)
					{
						swipeDirection = SwipeDirections.Right;
					}
					if(dir <= -0.7f)
					{
						swipeDirection = SwipeDirections.Left;
					}
				}
			}
			else if (touch.phase == TouchPhase.Stationary)
			{
				_touchStartPos = touch.position;
				_isOffsetChanged = false;
			}

			//
			//
			//
		}
		else
		{
			_isOffsetChanged = false;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow) && !_isRightDown)
		{
			_isRightDown = true;
			swipeDirection = SwipeDirections.Right;
			if(OnSwipe!=null)
				OnSwipe(swipeDirection);
		}
			
		if (Input.GetKeyUp (KeyCode.RightArrow) && _isRightDown)
		{
			_isRightDown = false;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow) && !_isLeftDown && IsHandleLeftRightTouches) 
		{
			_isLeftDown = true;
			swipeDirection = SwipeDirections.Left;
			if(OnSwipe!=null)
				OnSwipe(swipeDirection);
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow) && _isLeftDown && IsHandleLeftRightTouches) 
		{
			_isLeftDown = false;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) && !_isUpDown) 
		{
			_isUpDown = true;
			swipeDirection = SwipeDirections.Up;
			if(OnSwipe!=null)
				OnSwipe(swipeDirection);
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) && _isUpDown) 
		{
			_isUpDown = false;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow) && !_isDownDown) 
		{
			_isDownDown = true;
			swipeDirection = SwipeDirections.Down;
			if(OnSwipe!=null)
				OnSwipe(swipeDirection);
		}

		if (Input.GetKeyDown (KeyCode.DownArrow) && _isDownDown) 
		{
			_isDownDown = false;
		}

		SwipeDirection = swipeDirection;
		return swipeDirection;
	}
	
			/*
			if(!_isOffsetChanged)
			{
				var deltaPos = touch.position - _prevTouchPos;
				if(touch.phase == TouchPhase.Moved && deltaPos.magnitude > _minimalSwipeMagnitude - (GameSettings.Instance.SettingsData.Sensitivity * _minimalSwipeMagnitude))
				{
					_isOffsetChanged = true;
					var v2up    = new Vector2(0f, 1f);
					var v2right = new Vector2(1f, 0f);

					var ay = Vector2.Angle(deltaPos, v2up); 
					var ax = Vector2.Angle(deltaPos, v2right);

					if (( Vector2dExtra.Cross( deltaPos, v2right ).y > 0 ) && ( ax >= 65 && ax <= 115 ))
					{
						swipeDirection = SwipeDirections.Up;
					}
					else if (( Vector2dExtra.Cross( deltaPos, v2right ).y < 0 ) && ( ax >= 65 && ax <= 115 ))
					{
						swipeDirection = SwipeDirections.Down;
					}
					else if (( IsHandleLeftRightTouches && Vector2dExtra.Cross( deltaPos, v2up ).x < 0 ) && ( ay > 45 && ay < 135))
					{
						swipeDirection = SwipeDirections.Left;
					}
					else if (( IsHandleLeftRightTouches && Vector2dExtra.Cross( deltaPos, v2up ).x > 0 ) && ( ay > 45 && ay < 135))
					{
						swipeDirection = SwipeDirections.Right;
					}

					if(OnSwipe!=null)
						OnSwipe(swipeDirection);
				}	
				else
					_isOffsetChanged = false;
			}
			else
			{
				if (touch.phase == TouchPhase.Stationary)
					_isOffsetChanged = false;
			}
			*/
}