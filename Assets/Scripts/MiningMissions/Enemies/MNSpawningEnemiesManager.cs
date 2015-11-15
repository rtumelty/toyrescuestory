using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNSpawningEnemiesManager : MonoBehaviour 
{
	//*************************************************************//
	public static int MAXIMUM_NUMBER_OF_ENEMIES = 10;
	public static List < int[] > RESERVED_TILES;
	//*************************************************************//
	public bool startSpawn = true;
	//*************************************************************//
	private float _countTimeToNextSpawn = 5f;
	private float _countMinutes = 0f;
	private List < int[] > _holesTiles;
	private List < bool > _holesTilesOccupied;
	private GameObject _slugPrefab;
	//*************************************************************//
	private static MNSpawningEnemiesManager _meInstance;
	public static MNSpawningEnemiesManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_enemySpawningObject" ).GetComponent < MNSpawningEnemiesManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake ()
	{
		// to prevent from delay of loading slug in runtime
		_slugPrefab = ( GameObject ) Resources.Load ( "Spine/Slug/spine" );

		GameObject interactiveObjectInstant = ( GameObject ) Instantiate ( _slugPrefab, new Vector3 ( -100f, -100f, -100f ), _slugPrefab.transform.rotation );
		
		IComponent currentIComponent = interactiveObjectInstant.AddComponent < IComponent > ();
		currentIComponent.myID = GameElements.ENEM_SLUG_01;

		interactiveObjectInstant.AddComponent < SelectedComponenent > ();
		MNEnemyComponent currentEnemyComponent = interactiveObjectInstant.AddComponent < MNEnemyComponent > ();
		interactiveObjectInstant.AddComponent < EnemyAnimationComponent > ();
		
		interactiveObjectInstant.tag = GlobalVariables.Tags.INTERACTIVE;
	}

	public void updateHolesOnLevel ()
	{
		RESERVED_TILES = new List < int[] > ();
		_holesTiles = new List < int[] > ();
		_holesTilesOccupied = new List < bool > ();

		for ( int i = 0; i < MNLevelControl.LEVEL_WIDTH; i++ )
		{
			for ( int j = 0; j < MNLevelControl.LEVEL_HEIGHT; j++ )
			{
				if ( GameElements.isInBorders ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][i][j], GameElements.HOLES_BORDERS ))
				{
					_holesTiles.Add ( new int[2] { i, j });
					_holesTilesOccupied.Add ( false );

					RESERVED_TILES.Add ( new int[] { i - 1, j } );
					RESERVED_TILES.Add ( new int[] { i - 1, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i + 1, j + 1 } );
					RESERVED_TILES.Add ( new int[] { i + 1, j } );
					RESERVED_TILES.Add ( new int[] { i + 1, j - 1 } );
					RESERVED_TILES.Add ( new int[] { i, j - 1 } );
					RESERVED_TILES.Add ( new int[] { i - 1, j - 1 } );
				}
			}
		}
	}
	
	void Update () 
	{
		if ( ! startSpawn ) return; 

		_countMinutes += Time.deltaTime;
		_countTimeToNextSpawn -= Time.deltaTime;
		
		if ( _countTimeToNextSpawn <= 0f )
		{
			if ( getCountEnmiesOnLevel () < MAXIMUM_NUMBER_OF_ENEMIES )
			{
				spawnRandomEnemy ();
			}
			
			if ( _countMinutes < 60f )
			{
				_countTimeToNextSpawn = 5f;
			}
			else if (( _countMinutes >= 60f ) && ( _countMinutes < 120f ))
			{
				_countTimeToNextSpawn = 3.5f;
			}
			else if ( _countMinutes >= 120f )
			{
				_countTimeToNextSpawn = 1.5f;
			}
		}
	}
	
	private int getCountEnmiesOnLevel ()
	{
		int number = 0;
		foreach ( MNEnemyComponent enemy in MNLevelControl.getInstance ().enemiesOnLevel )
		{
			if ( enemy != null )
			{
				number++;
			}
		}
		
		return number;
	}
	
	private void spawnRandomEnemy ()
	{
		int randomEnemyID = GameElements.ENEMIES[UnityEngine.Random.Range ( 0, GameElements.ENEMIES.Length )];

		switch ( randomEnemyID )
		{
			case GameElements.ENEM_TENTACLEDRAINER_01:
			case GameElements.ENEM_TENTACLEDRAINER_02:
				List < int[] > freeTiles = new List < int[] > (); 
				for ( int i = 6; i < MNLevelControl.LEVEL_WIDTH; i++ )
				{
					for ( int j = 0; j < MNLevelControl.LEVEL_HEIGHT; j++ )
					{
						if ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][i][j] == GameElements.EMPTY )
						{
							bool freeTileRealy = true;
							foreach ( int[] tile in RESERVED_TILES )
							{
								if ( ToolsJerry.compareTiles ( tile, new int[2] { i, j }))
								{
									freeTileRealy = false;
									break;
								}
							}

							if ( freeTileRealy ) freeTiles.Add ( new int[2] { i, j });
						}
					}
				}
				
				int[] randomTile = freeTiles[UnityEngine.Random.Range ( 0, freeTiles.Count )];
				MNLevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTile );
				break;
			case GameElements.ENEM_SLUG_01:
				int randomHoleID = UnityEngine.Random.Range ( 0, _holesTiles.Count );
				if ( _holesTilesOccupied.Count > 0 && _holesTilesOccupied[randomHoleID] )
				{
					randomEnemyID = GameElements.ENEMIES[UnityEngine.Random.Range ( 0, GameElements.ENEMIES_TENTACLES.Length )];
					List < int[] > freeTilesAgain = new List < int[] > (); 
					for ( int i = 6; i < MNLevelControl.LEVEL_WIDTH; i++ )
					{
						for ( int j = 0; j < MNLevelControl.LEVEL_HEIGHT; j++ )
						{
							if ( MNLevelControl.getInstance ().levelGrid[MNLevelControl.GRID_LAYER_NORMAL][i][j] == GameElements.EMPTY )
							{
								bool freeTileRealy = true;
								foreach ( int[] tile in RESERVED_TILES )
								{
									if ( ToolsJerry.compareTiles ( tile, new int[2] { i, j }))
									{
										freeTileRealy = false;
										break;
									}
								}
								
								if ( freeTileRealy ) freeTilesAgain.Add ( new int[2] { i, j });
							}
						}
					}
					
					int[] randomTileAgain = freeTilesAgain[UnityEngine.Random.Range ( 0, freeTilesAgain.Count )];
					MNLevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTileAgain );
					return;
				}

				int[] randomTileHole = _holesTiles[randomHoleID];
				_holesTilesOccupied[randomHoleID] = true;
				GameObject slugObject = MNLevelControl.getInstance ().createObjectOnPosition ( randomEnemyID, randomTileHole );
				slugObject.GetComponent < IComponent > ().myEnemyData.myHoleID = randomHoleID;

				int[] freeTile = new int[2] { randomTileHole[0] - 1, randomTileHole[1] };
			
				if (( MNLevelControl.getInstance ().isTileInLevelBoudaries ( freeTile[0], freeTile[1] )) && ( MNGridReservationManager.getInstance ().fillTileWithMe ( randomEnemyID, freeTile[0], freeTile[1], slugObject, randomEnemyID, true )))
				{
					if ( ! MNGridReservationManager.getInstance ().fillTileWithMe ( randomEnemyID, freeTile[0], freeTile[1], slugObject, randomEnemyID, false ))
					{
						print ( "Cannot register slug!" );
					}
				
					slugObject.GetComponent < IComponent > ().position = freeTile;
				
					Vector3 positionToGo = new Vector3 ((float) freeTile[0], (float) ( MNLevelControl.LEVEL_HEIGHT - freeTile[1] ), (float) freeTile[1] - 0.5f );
					iTween.MoveTo ( slugObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutCirc, "position", positionToGo ));
					StartCoroutine ( "startMovingAfterTime", slugObject );
					
				}
				else
				{
					_holesTilesOccupied[randomHoleID] = false;
					Destroy ( slugObject );
				}
			
				break;
		}
	}

	public void unRegisterHole ( int holeID )
	{
		if ( holeID == -1 ) return;
		_holesTilesOccupied[holeID] = false;
	}
	
	private IEnumerator startMovingAfterTime ( GameObject slugObject )
	{
		yield return new WaitForSeconds ( 1f );
		IComponent currentIComponent = slugObject.GetComponent < IComponent > ();

		slugObject.GetComponent < MNSlugMovementComponent > ().position1 = ToolsJerry.cloneTile ( currentIComponent.position );
		slugObject.GetComponent < MNSlugMovementComponent > ().position2 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 1, currentIComponent.position[1] + 1 });
		slugObject.GetComponent < MNSlugMovementComponent > ().position3 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 2, currentIComponent.position[1] });
		slugObject.GetComponent < MNSlugMovementComponent > ().position4 = ToolsJerry.cloneTile ( new int[2] { currentIComponent.position[0] + 1, currentIComponent.position[1] - 1 });

		int[][] path = AStar.search ( currentIComponent.position, slugObject.GetComponent < MNSlugMovementComponent > ().position2, false, currentIComponent.myID, slugObject );
		slugObject.GetComponent < MNSlugMovementComponent > ().initMove ( path, slugObject.GetComponent < MNSlugMovementComponent > ().position2 );
	}
}
