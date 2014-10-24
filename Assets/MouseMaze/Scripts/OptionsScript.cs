using UnityEngine;
using System.Collections;

public class OptionsScript : MonoBehaviour
{
  public int MazeSize = 20;
  public int CheeseCount = 15;
  public int MiceCount = 10;

  void Awake()
  {
    DontDestroyOnLoad( this );

    if ( FindObjectsOfType( GetType() ).Length > 1 )
    {
      Destroy( gameObject );
    }
  }
}
