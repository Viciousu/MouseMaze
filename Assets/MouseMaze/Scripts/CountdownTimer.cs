using UnityEngine;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
  private float currentTime;
  public Texture TimerIcon;
  public GUIStyle style;

  public void StartTimer( float time )
  {
    currentTime = time;
    StartCoroutine( countdown() );
  }

  IEnumerator countdown()
  {
    while ( currentTime > 0 )
    {
      yield return new WaitForSeconds( 1 );

      currentTime -= 1;
    }
    Application.LoadLevel( "LoseScene" );
  }

  void OnGUI()
  {
//    string minutes = Mathf.Floor( currentTime / 60 ).ToString( "00" );
//    string seconds = Mathf.Floor( currentTime % 60 ).ToString( "00" );

    //GUI.DrawTexture( new Rect( 20, 20, 100, 100 ), TimerIcon );
    //GUI.Label( new Rect( 115, 50, 100, 100 ), minutes + ":" + seconds, style );
  }
}
