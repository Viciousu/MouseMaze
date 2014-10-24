using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyBot : MonoBehaviour
{
  public List<GameObject> CarriedCheeses;
  private float shortScanDistance = 1f;
  private float longScanDistance = 999f;
  public float speed = 2f;
  private Vector3 currentPosition;
  private Vector3 lastPosition;
  public GameObject cheese;
  private GunShoot gunShoot;

  public bool moving = false;
  private System.Random rnd = new System.Random();
  // Use this for initialization
  private void Start()
  {
    CarriedCheeses = new List<GameObject>();
    gunShoot = GetComponentInChildren<GunShoot>();
  }

  // Update is called once per frame
  private void Update()
  {
    Debug.DrawRay(transform.position, transform.forward*longScanDistance, Color.green);

    currentPosition = transform.position;
    if ( currentPosition == lastPosition )
      ScanSurroundingsAndMove();
    lastPosition = currentPosition;
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

    if ( !( shootFwd && IsObstacle( hitFwd ) ) )
      freeDirections.Add( forward );
    if ( !( shootLeft && IsObstacle( hitLeft ) ) )
      freeDirections.Add( left );
    if ( !( shootRight && IsObstacle( hitRight ) ) )
      freeDirections.Add( right );

    if ( freeDirections.Count == 0 )
    {
      transform.rotation = Quaternion.LookRotation( backward );
      return;
    }

    int index = rnd.Next( freeDirections.Count );
    var newDirection = freeDirections[index];

    foreach ( var freeDirection in freeDirections )
    {
      RaycastHit hit;
      if ( Physics.Raycast( transform.position, freeDirection * longScanDistance, out hit, longScanDistance ) )
      {
        if ( IsPlayer( hit ) )

        //if ( IsTarget( hit ) )
          newDirection = freeDirection;
        if ( freeDirection == forward && IsPlayer( hit ) && gunShoot != null )
          gunShoot.Shooting();
      }
    }

    transform.rotation = Quaternion.LookRotation( newDirection );
    StartCoroutine( MoveFromTo( transform, transform.forward, 1 / speed ) );
  }

  private bool IsObstacle( RaycastHit hit )
  {
    return hit.collider.tag == "Wall" || hit.collider.tag == "Enemy" || hit.collider.tag == "Exit";
  }

  private bool IsTarget( RaycastHit hit )
  {
    return hit.collider.tag == "Cheese" || hit.collider.tag == "Player";
  }

  private bool IsPlayer( RaycastHit hit )
  {
    return hit.collider.tag == "Player";
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

  private int carriedCheesesCount;
  private bool deathLock;

  void OnTriggerEnter( Collider col )
  {
    if ( col.gameObject.tag == "Cheese" )
    {
      var cheesesCounter = col.gameObject.GetComponent<CheeseCounter>();
      if ( !cheesesCounter.IsHit )
      {
        cheesesCounter.IsHit = true;
        carriedCheesesCount += cheesesCounter.Count;
        Debug.Log( "Enemy with Cheese Collision=" + carriedCheesesCount );
      }
      Destroy( col.gameObject );
    }

    if ( col.gameObject.tag == "Bullet" )
    {
      if ( carriedCheesesCount > 0 && !deathLock )
      {
        deathLock = true;
        var cheeseInstance = Instantiate( cheese ) as GameObject;
        if ( cheeseInstance != null )
        {
          Debug.Log( "Enemy Death carriedCheesesCount=" + carriedCheesesCount );

          cheeseInstance.transform.position = transform.position;
          var cheeseCounter = cheeseInstance.GetComponent<CheeseCounter>();
          cheeseCounter.Count = carriedCheesesCount;
        }
      }

      Destroy( gameObject );
    }
  }
  IEnumerator WaitAndGameOver()
  {
    yield return new WaitForSeconds( 2 );
    Application.LoadLevel( "LoseScene" );
  }

  IEnumerator WaitAndDestroy()
  {
    yield return new WaitForSeconds( 2 );
    Destroy( gameObject );
  }
}
