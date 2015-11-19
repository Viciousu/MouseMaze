using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
{
  public bool ReStart;
  public bool isWin;
  public GameObject Logo;
  private OptionsScript optionsScript;
  private GUISkin skin;
  private bool IsInOptions;
  private Vector3 scale;
  private float originalWidth=654f;
  private float originalHeight = 409f;
  public GUIStyle style;
  public GUIStyle winLoseStyle;

  float miceCount = 5;
  float cheezeCount = 10;

  private void Start()
  {
    Screen.showCursor = true;
    skin = Resources.Load( "GUISkin" ) as GUISkin;

    if ( optionsScript == null )
      optionsScript = FindObjectOfType<OptionsScript>();

    if ( !ReStart && optionsScript != null )
    {
      cheezeCount = optionsScript.CheeseCount;
      miceCount = optionsScript.MiceCount;
    }
  }

  private void OnGUI()
  {
    GUI.skin = skin;

    scale.x = Screen.width / originalWidth;
    scale.y = Screen.height / originalHeight;
    scale.z = 1;
    var svMat = GUI.matrix;
    GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, scale );

    const int buttonWidth = 120;
    const int buttonHeight = 30;
    const int offset = 20;

    if ( IsInOptions )
    {

	
#if UNITY_IPHONE || UNITY_ANDROID
	GUI.Label( new Rect( ( originalWidth - buttonWidth ) / 2f - buttonWidth, originalHeight / 2f - 5 * buttonHeight - offset, buttonWidth * 3, 100 ),
	          "Controls:\n\n Swipe Left/Right - Turns\n\n Tap or Swipe Up - Shoot\n\n Swipe Down - U Turn", style );
#else
      GUI.Label( new Rect( ( originalWidth - buttonWidth ) / 2f - buttonWidth, originalHeight / 2f - 5 * buttonHeight - offset, buttonWidth * 3, 100 ),
          "Controls:\n\n ASD or Arrows - Turns\n\n W or Up - Shoot\n\n F - Change view\n\n ESC - Main Menu", style );
#endif

      cheezeCount = GUI.HorizontalSlider( new Rect( ( originalWidth - buttonWidth ) / 2f + buttonWidth,
                                         originalHeight / 2f + buttonHeight - offset + 5f,
                                         buttonWidth,
                                         buttonHeight ), cheezeCount, 3.0F, 20.0F );

      GUI.Label( new Rect( ( originalWidth - buttonWidth ) / 2f - buttonWidth, originalHeight / 2f + buttonHeight - offset, buttonWidth, 20 ),
                "Cheese: " + (int)cheezeCount, style );

      miceCount = GUI.HorizontalSlider( new Rect( ( originalWidth - buttonWidth ) / 2f + buttonWidth,
                                   originalHeight / 2f + 3 * buttonHeight - offset + 5f,
                                   buttonWidth,
                                   buttonHeight ), miceCount, 1.0F, 15.0F );

      GUI.Label( new Rect( ( originalWidth - buttonWidth ) / 2f - buttonWidth, originalHeight / 2f + 3 * buttonHeight - offset, buttonWidth, 20 ),
                "Mice: " + (int)miceCount, style );

      Rect buttonBack = new Rect(
        ( originalWidth - buttonWidth ) / 2f,
        originalHeight - buttonHeight - offset,
        buttonWidth,
        buttonHeight
        );

      if ( GUI.Button( buttonBack, "Back" ) )
      {
        if ( optionsScript != null )
        {
          optionsScript.CheeseCount = (int)cheezeCount;
          optionsScript.MiceCount = (int)miceCount;
        }

        OptionsMode( false );
      }
    }
    else
    {
      if ( ReStart )
        GUI.Label(
          new Rect( ( originalWidth - buttonWidth ) / 2f, originalHeight / 2f - 2 * buttonHeight - offset,
                   buttonWidth, 50 ),
          isWin ? "you\nwin" : "you\nlose", winLoseStyle );

      Rect buttonStart = new Rect(
        ( originalWidth - buttonWidth ) / 2f,
        originalHeight - buttonHeight * 2 - offset * 3,
        buttonWidth,
        buttonHeight
        );

      Rect buttonOptions = new Rect(
        ( originalWidth - buttonWidth ) / 2f,
        originalHeight - buttonHeight * 2 - offset * 1.5f,
        buttonWidth,
        buttonHeight
        );

      Rect buttonQuit = new Rect(
        ( originalWidth - buttonWidth ) / 2f,
        originalHeight - buttonHeight * 2,
        buttonWidth,
        buttonHeight
        );


      if ( GUI.Button( buttonStart, ReStart ? "Restart" : "Start" ) )
      {
        Application.LoadLevel( "MazeScene" );
      }

      if ( GUI.Button( buttonOptions, ReStart ? "Menu" : "Options" ) )
      {
        if ( ReStart )
          Application.LoadLevel( "MenuScene" );
        else
          OptionsMode( true );
      }

      if ( GUI.Button( buttonQuit, "Quit" ) )
      {
        Application.Quit();
      }
    }
    GUI.matrix = svMat;
  }

  private void OptionsMode( bool isInOptions )
  {
    IsInOptions = isInOptions;
    if ( Logo != null )
      Logo.SetActive( !isInOptions );
  }
}