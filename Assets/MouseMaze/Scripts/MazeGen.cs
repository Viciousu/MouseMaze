using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MazeGen : MonoBehaviour
{
  public int HeightOfMaze = 11;
  public int WidthOfMaze = 11;

  public int CheesesCount = 3;
  public int CurrentCheesesCount;

  public int MiceCount = 3;

  private int[,] maze;

  public GameObject wallObject;
  public GameObject playerObject;
  public GameObject floorObject;

  public GameObject exitObject;
  private GameObject exitWallInstance;

  public GameObject enemyObject;
  private GameObject enemyInstance;

  public GameObject cheese;

  public GameObject CheeseParent;
  public GameObject EnemyParent;


  public Vector3[] waypoints;

  public bool UsePlayerObject;

  private RaycastHit Hit;

  static readonly System.Random random = new System.Random();


  private void Awake()
  {
    if ( !Application.isEditor )
      Screen.showCursor = false;


    var optionsObject = GameObject.Find( "Options" );
    if ( optionsObject != null )
    {
      var options=optionsObject.GetComponent<OptionsScript>();
      MiceCount = options.MiceCount;
      CheesesCount = options.CheeseCount;
    }

    CurrentCheesesCount = CheesesCount;

    if ( floorObject != null )
    {
      floorObject.transform.position = new Vector3( HeightOfMaze / 2f, 0f, WidthOfMaze / 2f );
      floorObject.transform.localScale = new Vector3( ( HeightOfMaze - 1f ) / 10f, 1f, ( WidthOfMaze - 1f ) / 10f );
    }

    maze = GenerateMaze( HeightOfMaze, WidthOfMaze );

    var exitOutOfMazePosition = GetRandomPosition( 0, HeightOfMaze, WidthOfMaze - 2, WidthOfMaze );
    exitOutOfMazePosition.z += 1;

    for ( int i = 0; i < HeightOfMaze; i++ )
      for ( int j = 0; j < WidthOfMaze; j++ )
      {
        if ( maze[i, j] == 1 )
        {
          Vector3 pos = new Vector3( i, 0, j );

          var exitPosition = exitOutOfMazePosition;
          exitPosition.y = 0;

          if ( pos == exitPosition )
          {
            exitWallInstance = Instantiate( wallObject ) as GameObject;
            exitWallInstance.transform.position = pos;
			exitWallInstance.transform.parent = this.transform;

            pos.y += 2;
            var exitInstance = Instantiate( exitObject ) as GameObject;
            exitInstance.transform.position = pos;
			exitInstance.transform.parent = this.transform;
            continue;
          }

        	GameObject wall = Instantiate( wallObject ) as GameObject;
        	wall.transform.position = pos;
			wall.transform.parent = this.transform;
        }
      }

    if ( UsePlayerObject )
    {
      GameObject playerInstance = Instantiate( playerObject ) as GameObject;

      int offset = 3;
      if ( playerInstance != null )
      {
        var playerPosition = GetRandomPosition( 0, offset, 0, offset );
        playerInstance.transform.position = playerPosition;
        var playerMain = playerInstance.GetComponent<PlayerMain>();
        playerMain.MazeGen = this;

        int m = 0;
        while ( m < MiceCount )
        {
          enemyInstance = Instantiate( enemyObject ) as GameObject;
          if ( enemyInstance != null )
          {
            var enemyStartPosition = GetRandomPosition( 0, HeightOfMaze, 0, WidthOfMaze );
            enemyStartPosition.y = 0;
            enemyInstance.transform.position = enemyStartPosition;
			enemyInstance.transform.parent = EnemyParent.transform;
            m += 1;
          }
        }

        int c = 0;
        while ( c < CheesesCount )
        {
          var cheeseInstance = Instantiate( cheese ) as GameObject;
          if ( cheeseInstance == null )
            continue;
          var cheesePos = GetRandomPosition( 0, HeightOfMaze, 0, WidthOfMaze );
          cheesePos.y = 0;
          cheeseInstance.transform.position = cheesePos;
		  cheeseInstance.transform.parent = CheeseParent.transform;
          c += 1;
        }
      }
    }
  }

  void Update()
  {
    if ( exitWallInstance != null && CurrentCheesesCount <= 0 )
      Destroy( exitWallInstance );
  }

  List< Tuple<int,int>> usedTiles = new List<Tuple<int, int>>();

  private Vector3 GetRandomPosition( int startPosHeight, int endPosHeight, int startPosWidth, int endPosWidth )
  {
    var random = MazeGen.random;
    int i = 0;
    int j = 0;
    var tuple = new Tuple<int, int>( i, j );
    while ( maze[i, j] != 0 || usedTiles.Contains( tuple ) )
    {
      i = random.Next( startPosHeight, endPosHeight );
      j = random.Next( startPosWidth, endPosWidth );
      tuple = new Tuple<int, int>( i, j );
    }
    usedTiles.Add( tuple );
    return new Vector3( i, +1, j );
  }

  private int[,] GenerateMaze( int Height, int Width )
  {
    int[,] maze = new int[Height, Width];

    for ( int i =0; i < Height; i++ )
      for ( int j = 0; j < Width; j++ )
        maze[i, j] = 1;

    System.Random rand = new System.Random();

    int r = rand.Next( Height );
    while ( r % 2 == 0 )
      r = rand.Next( Height );
    int c = rand.Next( Width );
    while ( c % 2 == 0 )
      c = rand.Next( Width );

    maze[r, c] = 0;

    MazeDigger( maze, r, c );

    return maze;
  }

  private void MazeDigger( int[,] maze, int r, int c )
  {
    // Create digging directions
    // 1 - North 
    // 2 - South
    // 3 - East
    // 4 - West

    int[] directions = new int[] { 1, 2, 3, 4 };
    Shuffle( directions );
    // Look in a random direction 2 blocks ahead
    for ( int i = 0; i < directions.Length; i++ )
    {
      switch ( directions[i] )
      {
      case 1: //UP
        if ( r - 2 <= 0 )
          continue;
        if ( maze[r - 2, c] != 0 )
        {
          maze[r - 2, c] = 0;
          maze[r - 1, c] = 0;
          MazeDigger( maze, r - 2, c );
        }
        break;
      case 2: //Right
        if ( c + 2 >= WidthOfMaze - 1 )
          continue;
        if ( maze[r, c + 2] != 0 )
        {
          maze[r, c + 2] = 0;
          maze[r, c + 1] = 0;
          MazeDigger( maze, r, c + 2 );
        }
        break;
      case 3: // Down
        if ( r + 2 >= HeightOfMaze - 1 )
          continue;
        if ( maze[r + 2, c] != 0 )
        {
          maze[r + 2, c] = 0;
          maze[r + 1, c] = 0;
          MazeDigger( maze, r + 2, c );
        }
        break;
      case 4: //Left
        if ( c - 2 <= 0 )
          continue;
        if ( maze[r, c - 2] != 0 )
        {
          maze[r, c - 2] = 0;
          maze[r, c - 1] = 0;
          MazeDigger( maze, r, c - 2 );
        }
        break;


      }
    }
  }

  public static void Shuffle<T>( T[] array )
  {
    var random = MazeGen.random;
    for ( int i = array.Length; i > 1; i-- )
    {
      int j = random.Next( i );
      T tmp = array[j];
      array[j] = array[i - 1];
      array[i - 1] = tmp;
    }
  }
}

public class Tuple<T1, T2>
{
  public Tuple( T1 item1, T2 item2 )
  {
    this.Item1 = item1;
    this.Item2 = item2;
  }

  public T1 Item1 { get; private set; }

  public T2 Item2 { get; private set; }
}