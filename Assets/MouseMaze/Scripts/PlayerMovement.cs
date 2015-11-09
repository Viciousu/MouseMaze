using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	private float shortScanDistance = 1f;
	private float longScanDistance = 999f;
	public float speed = 3f;
	private Vector3 currentPosition;
	private Vector3 lastPosition;
	
	private System.Random rnd = new System.Random();
	
	private bool moving = false;

	Vector3 directionToRotate;
	bool started = false;

	private void Start()
	{
		ScanSurroundingsAndMove();
	}
	
	void Update () {
		Debug.DrawRay(transform.position, transform.forward*longScanDistance, Color.green);
		
		currentPosition = transform.position;
		if ( currentPosition == lastPosition )
		{
			ScanSurroundingsAndMove();
			started = true;
		}
			
		lastPosition = currentPosition;

		if(started)
		transform.rotation= Quaternion.Slerp(transform.rotation, Quaternion.LookRotation( directionToRotate ), .5f); 
		
	}
	
	public void ScanSurroundingsAndMove()
	{
		Vector3 forward = transform.forward;
		Vector3 backward = -transform.forward;
		Vector3 left = -transform.right;
		Vector3 right = transform.right;
		var freeDirections = new List<Vector3>();
		
		RaycastHit hitFwd;
		RaycastHit hitLeft;
		RaycastHit hitRight;
		
		bool shootFwd = Physics.Raycast( transform.position, forward, out hitFwd, shortScanDistance );
		bool shootLeft = Physics.Raycast( transform.position, left, out hitLeft, shortScanDistance );
		bool shootRight = Physics.Raycast( transform.position, right, out hitRight, shortScanDistance );
		
		if ( !( shootFwd && IsWall( hitFwd ) ) )
			freeDirections.Add( forward );
		if ( !( shootLeft && IsWall( hitLeft ) ) )
			freeDirections.Add( left );
		if ( !( shootRight && IsWall( hitRight ) ) )
			freeDirections.Add( right );
		
		if ( freeDirections.Count == 0 )
		{
			transform.rotation = Quaternion.LookRotation( backward );
			return;
		}
		
		int index = rnd.Next( freeDirections.Count );
		var newDirection = freeDirections[index];

		directionToRotate = newDirection;

		//transform.rotation = Quaternion.LookRotation( newDirection );
		//Quaternion newRotation = Quaternion.AngleAxis(90, transform.up);
		//Quaternion.LookRotation(newDirection, transform.up)
		//transform.rotation= Quaternion.Slerp(transform.rotation, Quaternion.LookRotation( newDirection ), 1f); 
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime);
		StartCoroutine( MoveFromTo( transform, newDirection, 1 / speed ) );
	}
	
	private bool IsObstacle( RaycastHit hit )
	{
		return hit.collider.tag == "Wall" || hit.collider.tag == "Enemy" || hit.collider.tag == "Exit";
	}

	private bool IsWall( RaycastHit hit )
	{
		return hit.collider.tag == "Wall";
	}


	void SmoothLook(Vector3 newDirection){
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), Time.deltaTime);
	}

	private IEnumerator MoveFromTo( Transform theTransform, Vector3 d, float time )
	{
		if ( !moving )
		{
			moving = true;
			float t = 0f;
			Vector3 startPosition = theTransform.position;
			Vector3 endPosition = startPosition + d;
			while ( t < 1f )
			{
				t += Time.deltaTime / time;
				transform.position = Vector3.Lerp( startPosition, endPosition, t );
				yield return 0;
			}
			theTransform.position = endPosition;
			moving = false;
		}
	}
}
