using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyBot : MonoBehaviour
{
  public List<GameObject> CarriedCheeses;

  public GameObject cheese;
	
  private void Start()
  {
    CarriedCheeses = new List<GameObject>();
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
