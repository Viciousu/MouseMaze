using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum Directions
{
  Forward,
  Backward,
  Right,
  Left
}

public class PlayerMovement : MonoBehaviour
{
  private float shortScanDistance = 1f;
  private float longScanDistance = 999f;
  public float speed = 3f;
  private Vector3 currentPosition;
  private Vector3 lastPosition;

  private System.Random rnd = new System.Random();

  private bool moving;

  private Vector3 currentDirection;

  public Directions ReservedDirection = Directions.Forward;

  private void Start()
  {
    currentPosition = lastPosition = transform.position;
  }

  private void Update()
  {
    Debug.DrawRay( transform.position, transform.forward * longScanDistance, Color.green );

    currentPosition = transform.position;
    if ( currentPosition == lastPosition )
      ScanSurroundingsAndMove();
    lastPosition = currentPosition;

    transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( currentDirection ), .5f );
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

    bool canGoForward = !( shootFwd && IsWall( hitFwd ) );
    bool canGoLeft = !( shootLeft && IsWall( hitLeft ) );
    bool canGoRight = !( shootRight && IsWall( hitRight ) );

    if ( canGoForward )
      freeDirections.Add( forward );
    if ( canGoLeft )
      freeDirections.Add( left );
    if ( canGoRight )
      freeDirections.Add( right );

    if ( freeDirections.Count == 0 )
    {
      transform.rotation = Quaternion.LookRotation( backward );
      currentDirection = backward;
      return;
    }

    Debug.LogWarning(ReservedDirection);

    if (ReservedDirection == Directions.Right && canGoRight)
    {
      currentDirection = right;
      ReservedDirection = Directions.Forward;
    }
    else if (ReservedDirection == Directions.Left && canGoLeft)
    {
      currentDirection = left;
      ReservedDirection = Directions.Forward;
    }
    else if (ReservedDirection == Directions.Backward)
    {
      transform.rotation = Quaternion.LookRotation(backward);
      currentDirection = backward;
      ReservedDirection = Directions.Forward;
    }
    else
    {
      if (canGoForward)
        currentDirection = forward;
      else
      {
        int index = rnd.Next(freeDirections.Count);
        currentDirection = freeDirections[index];
        ReservedDirection = Directions.Forward;
      }
    }

    StartCoroutine( MoveFromTo( transform, currentDirection, 1 / speed ) );
  }

  private bool IsWall( RaycastHit hit )
  {
    return hit.collider.tag == "Wall";
  }

  private void SmoothLook( Vector3 newDirection )
  {
    transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.LookRotation( newDirection ), Time.deltaTime );
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
