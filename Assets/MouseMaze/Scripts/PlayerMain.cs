using UnityEngine;
using System.Collections;

public class PlayerMain : MonoBehaviour
{
  private bool enableFPSCamera = true;
  private GameObject fpsCameraGameObject;
  private Camera fpsCamera;
  private Camera topDownCamera;
  private MouseLook mouseLookX;
  private MouseLook mouseLookY;
  public float speed = 6.0f;
  public float detectEnemyDistance = 10f;
  private Vector3 ExitOutOfMazePosition;
  private float metersToExit;
  private CharacterController controller;
  private GunShoot gunShoot;
  public GUIStyle CheeseCountStyle;
  public GUIStyle DefaultStyle;
  public AudioClip Nom;
  public bool ToggleMinimap = false;
  public MazeGen MazeGen;
  public int CurrentHP = 100;
  public PlayerMovement Movement;

  // Use this for initialization
  void Start()
  {
    controller = GetComponent<CharacterController>();

    fpsCameraGameObject = GameObject.Find("FPSCamera" );
    fpsCamera = fpsCameraGameObject.camera;
    topDownCamera = GameObject.Find( "TopDownCamera" ).camera;
    topDownCamera.pixelRect = new Rect( Screen.width - 200f, Screen.height - 150, 200, 150 );
    topDownCamera.clearFlags = CameraClearFlags.Depth;
    topDownCamera.GetComponent<TopDownCamera>().SetTarget( gameObject );
    mouseLookX = GetComponent( "MouseLook" ) as MouseLook;
    gunShoot = GetComponentInChildren<GunShoot>();
  }

  // Update is called once per frame
  void Update()
  {
    if ( Input.GetKeyDown( KeyCode.Escape ) )
      Application.LoadLevel( "MenuScene" );

    if ( Input.GetKeyDown( KeyCode.F ) )
    {
      enableFPSCamera = !enableFPSCamera;
      mouseLookX.enabled = enableFPSCamera;
      transform.rotation = Quaternion.identity;
      fpsCamera.enabled = enableFPSCamera;
      if ( enableFPSCamera )
      {
        topDownCamera.pixelRect = new Rect( Screen.width - 200f, Screen.height - 150, 200, 150 );
        topDownCamera.clearFlags = CameraClearFlags.Depth;
      }
      else
      {
        topDownCamera.clearFlags = CameraClearFlags.Skybox;
        topDownCamera.rect = new Rect( 0f, 0f, Screen.width, Screen.height );
      }

    }

    ReserveMove();
    Move( enableFPSCamera );
  }


  void OnTriggerEnter( Collider col )
  {
    if ( col.gameObject.tag == "Cheese" )
    {
      audio.PlayOneShot( Nom );
      if ( MazeGen != null )
      {
        var cheesesCounter = col.gameObject.GetComponent<CheeseCounter>();
        if ( !cheesesCounter.IsHit )
        {
          Debug.Log( "Player Cheese Collision=" + cheesesCounter.Count );
          cheesesCounter.IsHit = true;
          MazeGen.CurrentCheesesCount -= cheesesCounter.Count;
        }
      }
      Destroy( col.gameObject );
    }

    if ( col.gameObject.tag == "EnemyBullet" )
    {
      if ( CurrentHP > 0 )
        CurrentHP -= 10;
      if ( CurrentHP <= 0 )
        Application.LoadLevel( "LoseScene" );
    }

    if ( col.gameObject.tag == "Enemy" )
    {
      CurrentHP = 0;
      Application.LoadLevel( "LoseScene" );
    }

    if ( col.gameObject.tag == "Exit" && MazeGen != null && MazeGen.CurrentCheesesCount <= 0 )
    {
      Application.LoadLevel( "WinScene" );
      Debug.Log( "Game over! You won." );
    }
  }

  private void ReserveMove()
  {
	if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
      Movement.ReservedDirection = Directions.Left;

	if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
      Movement.ReservedDirection = Directions.Right;

	if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
      Movement.ReservedDirection = Directions.Backward;
  }

  private void Move( bool isFPSView )
  {
    var velocity = new Vector3();
    var movement=new Vector3();

//    if ( !isFPSView )
//    {
//      if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.D ) )
//      {
//        var z = Input.GetKey( KeyCode.W ) ? 1.0f : 0;
//        z = Input.GetKey( KeyCode.S ) ? -1.0f : z;
//
//        var x = Input.GetKey( KeyCode.D ) ? 1.0f : 0;
//        x = Input.GetKey( KeyCode.A ) ? -1.0f : x;
//
//        movement.z = speed * z;
//        movement.x = speed * x;
//
//        //face movement dir
//        Vector3 keyboardPosition = new Vector3( x, 0, z );
//        transform.LookAt( transform.position + keyboardPosition );
//      }
//    }
//    else
//    {
//      movement.x = Input.GetAxis( "Horizontal" ) * speed;
//      movement.z = Input.GetAxis( "Vertical" ) * speed;
//      movement = transform.TransformDirection( movement );
//    }

	movement = transform.TransformDirection( movement );
    velocity.y += Physics.gravity.y * Time.deltaTime;
    movement += velocity;
    movement += Physics.gravity;
    movement *= Time.deltaTime;
    controller.Move( movement );
  }

  private void OnGUI()
  {
    if ( MazeGen != null && MazeGen.CurrentCheesesCount > 0 )
    {
      CheeseCountStyle.fontSize = 52;
      GUI.Label( new Rect( Screen.width - 130, Screen.height - 150, 120, 120 ), MazeGen.CurrentCheesesCount.ToString(),
                CheeseCountStyle );
    }
    else
    {
      CheeseCountStyle.fontSize = 30;
      GUI.Label( new Rect( Screen.width - 130, Screen.height - 150, 120, 120 ), "find\nexit",
                CheeseCountStyle );
    }
    GUI.Label( new Rect( 0, Screen.height - 135, 230, 40 ), "Health " + CurrentHP, DefaultStyle );
    GUI.Label( new Rect( 0, Screen.height - 85, 230, 40 ), "Ammo   " + gunShoot.CurrentAmmo, DefaultStyle );
  }
}
