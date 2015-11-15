using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGlobalVariables
{
	//*************************************************************//	
	public const string VERSION = "0.6.08";  
	//*************************************************************//
	public static bool RELEASE = true;
	//*************************************************************//
	public static bool CUT_DOWN_GAME = true;
	//*************************************************************//
	public static bool TEST_BUILD = true;
	//*************************************************************//
	public static bool SHOW_LEVEL_NUMBERS = false;
#if UNITY_EDITOR
	public static bool SHOW_CONSOLE = true;
#else
	public static bool SHOW_CONSOLE = false;
#endif
	//*************************************************************//
	public const int RESCUE = 0;
	public const int LABORATORY = 1;
	public const int MINING = 2;
	public const int TRAIN = 3;
	public const int INTRO = 4;
	public static int CURRENT_GAME_PART = RESCUE;
	//*************************************************************//
	public static bool UNLOCK_ALL_LEVELS = false;
	//*************************************************************//
	public static bool DUMMY = false;
	//*************************************************************//
	public static int LAB_ENTERED = 0;
	public static int MINE_ENTERED = 0;
	public static int NUMBER_OF_MINE_LEVELS = 0;
	public static int I_WAS_IN_MINES = 0;
	public static bool BLOCK_LAB_ENTERED = true;
	public static int INTRO_PLAYED = 0;
	public static bool BACK_FROM_LEVEL = false;
	public static int WORLD_I_WAS_IN = 0;
	public static GameObject mostRecentLevel;
	public static FLStorageContainerClass lastStorageContainerClass;
	//*************************************************************//
	public static class Stats
	{ 
		public static int RECHARGEOCORES_IN_CONSTRUCTION = 0;
		public static int PREMIUM_CURRENCY = 200;
		public static int METAL_IN_CONTAINERS = 12;
		public static int PLASTIC_IN_CONTAINERS = 18;
		public static int VINES_IN_CONTAINERS = 6;
		public static int RECHARGEOCORES_IN_CONTAINERS = 0;
		public static int REDIRECTORS_IN_CONTAINERS = 0;
		
		public static class NewResources
		{
			public static int PREMIUM_CURRENCY = 0;
			public static int METAL = 0;
			public static int PLASTIC = 0;
			public static int VINES = 0;
			public static int RECHARGEOCORES = 0;
			public static int REDIRECTORS = 0;
			
			public static void reset ()
			{
				PREMIUM_CURRENCY = 0;
				METAL = 0;
				PLASTIC = 0;
				VINES = 0;
				RECHARGEOCORES = 0;
				REDIRECTORS = 0;
			}
		}
		
		public static void reset ()
		{
			PREMIUM_CURRENCY = 200;
			METAL_IN_CONTAINERS = 12;
			PLASTIC_IN_CONTAINERS = 18;
			VINES_IN_CONTAINERS = 6;
			RECHARGEOCORES_IN_CONTAINERS = 0;
			REDIRECTORS_IN_CONTAINERS = 0;
		}
	}
	
	public static class AdditionalMoves 
	{
		public static int ADDITIONAL_MOVES = 0;
		
		public static void reset ()
		{
			ADDITIONAL_MOVES = 0;
		}
	}
	
	public static class FontMaterials
	{
		public static Material WHITE_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_white" );
		public static Material BLACK_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_black" );
		public static Material RED_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_red" );
		public static Material WHITE_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_white" );
		public static Material BLACK_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black" );
		public static Material BLACK_TEXT_02 = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black1" );
		public static Material RED_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_red" );
		public static Material RED_TEXT_02 = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_red1" );
		public static Material GREY_TEXT =  (Material ) Resources.Load ( "Fonts/AdLibBT Regular_grey" );
		
		public static Material BLACK_BIG_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_black1" );
		public static Material WHITE_BIG_TEXT = ( Material ) Resources.Load ( "Fonts/AdLibBT Regular_white1" );
		
		public static Material BLACK_BIG_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_black1" );
		public static Material WHITE_BIG_TITLE = ( Material ) Resources.Load ( "Fonts/KOMIKAX_white1" );
	}
	
	public static class Missions
	{
		//*************************************************************//
		public class LevelClass
		{
			//*************************************************************//
			public class RequiredElement
			{
				public int elementID;
				public int amountRequiered;
				
				public RequiredElement ( int elementIDValue, int amountRequieredValue )
				{
					elementID = elementIDValue;
					amountRequiered = amountRequieredValue;
				}
			}
			//*************************************************************//
			public string myName;
			public int type;
			public bool iAmFinished;
			public int starsEarned;
			public int star01;
			public int star02;
			public int star03;
			public int star04;

			public int star01Slot;
			public int star02Slot;
			public int star03Slot;
			public int star04Slot;

			public int numberOfStars;

			public List < RequiredElement > requiredElements;
			public LevelControl.SpecificLevelData mySpecificLevelData;
			
			public LevelClass ( string myNameValue, int typeValue = FLMissionScreenNodeManager.TYPE_REGULAR_NODE, int star01Value = 2, int star02Value = 2, int star03Value = 2, int star04Value = 0, int star01SlotValue = 0, int star02SlotValue = 1, int star03SlotValue = 2, int star04SlotValue = -1 )
			{ 
				myName = myNameValue;
				type = typeValue;
				star01 = star01Value;
				star02 = star02Value;
				star03 = star03Value;
				star04 = star04Value;

				star01Slot = star01SlotValue;
				star02Slot = star02SlotValue;
				star03Slot = star03SlotValue;
				star04Slot = star04SlotValue;

				int numberOfSlots = 0;
				if ( star01Slot != -1 ) numberOfSlots++;
				if ( star02Slot != -1 ) numberOfSlots++;
				if ( star03Slot != -1 ) numberOfSlots++;
				if ( star04Slot != -1 ) numberOfSlots++;

				numberOfStars = numberOfSlots;
			}
		}
		//*************************************************************//
		public class MiningLevelClass
		{
			public string myName;
			public float coolDownTimeLeft;
			public int starsEarned;
			public bool iAmFinished;
			public int type;
			public MNLevelControl.SpecificLevelData mySpecificLevelData;
			
			public MiningLevelClass ( string myNameValue, int typeValue = FLMissionScreenNodeManager.TYPE_MINING_NODE )
			{
				myName = myNameValue;
				type = typeValue;
			}
		}
		//*************************************************************//
		public class TrainLevelClass
		{
			public string myName;
			public int starsEarned;
			public bool iAmFinished;
			//==================================Daves Edit=====================================
			public int star04;
			public int type;
			
			public TrainLevelClass ( string myNameValue, int star04Value = 0, int typeValue = FLMissionScreenNodeManager.TYPE_TRAIN_NODE )
			{
				myName = myNameValue;
				star04 = star04Value;
				type = typeValue;
			}
			//==================================Daves Edit=====================================
			
		}
		//*************************************************************//
		public class WorldClass
		{
			public string name;
			public List < LevelClass > levels;
			public List < MiningLevelClass > miningLevels;
			public List < LevelClass > bonusLevels;
			public List < TrainLevelClass > trainLevels;
			
			public WorldClass ( string nameValue, List < LevelClass > levelsValue, List < LevelClass > bonusValue, List < MiningLevelClass > miningLevelsValue, List < TrainLevelClass > trainLevelsValue = null )
			{
				name = nameValue;
				levels = levelsValue;
				bonusLevels = bonusValue;
				miningLevels = miningLevelsValue;
				trainLevels = trainLevelsValue;
			}
		}
		//*************************************************************//
		public const int WORLD_DURACELLIUM_MINES = 0;
		public const int WORLD_OTHER = 1;
		//*************************************************************//
		public static List < WorldClass > WORLDS;
		//*************************************************************//
		public static void fillWorldsAndLeveles ()
		{
			WORLDS = new List < WorldClass > ();
			// Duracellium Mines
			List < LevelClass > leveles = new List < LevelClass > ();
			List < MiningLevelClass > miningLeveles = new List < MiningLevelClass > ();
			List < TrainLevelClass > trainLeveles = new List < TrainLevelClass > ();
			leveles.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 6, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 1, 0, -1, 0, 2, -1 ));
			leveles.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 2, 0, -1, 0, 2, -1 ));
			leveles.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 0, 45, 0, 1, -1, 2 ));
			leveles.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 1, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 5, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 4, 2, 60, -1, 0, 1, 2 ));
			leveles.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 0, 60, 0, 1, -1, 2 ));
			leveles.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 5, 2, 0, 0, 1, 2, -1 ));

			List < LevelClass > levelesBonus = new List < LevelClass > ();
			
			levelesBonus.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 0, 4, 1, 0, -1, 0, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 1, 1, 2, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 4, 2, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 3, 4, 3, 0, 0, 1, 2, -1 ));
			 
			miningLeveles.Add ( new MiningLevelClass ( "1" ));
			trainLeveles.Add ( new TrainLevelClass ( "1", 80));

			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map1", leveles, levelesBonus, miningLeveles, trainLeveles ));

			// Some Other World 
			leveles = new List < LevelClass > ();
			leveles.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 6, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 1, 0, -1, 0, 2, -1 ));
			leveles.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 3, 2, 0, -1, 0, 2, -1 ));
			leveles.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 0, 45, 0, 1, -1, 2 ));
			leveles.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 1, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 5, 0, 0, -1, 1, -1, -1 ));
			leveles.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 0, 4, 2, 60, -1, 0, 1, 2 ));
			leveles.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 0, 60, 0, 1, -1, 2 ));
			leveles.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 5, 2, 0, 0, 1, 2, -1 ));

			levelesBonus = new List < LevelClass > ();

			levelesBonus.Add ( new LevelClass ( "1", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 0, 4, 1, 0, -1, 0, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "2", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "3", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 1, 1, 2, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "4", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 4, 2, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "5", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "6", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 3, 4, 3, 0, 0, 1, 2, -1 ));

			miningLeveles = new List < MiningLevelClass > ();
			trainLeveles = new List < TrainLevelClass > ();

			miningLeveles.Add ( new MiningLevelClass ( "1" ));
			trainLeveles.Add ( new TrainLevelClass ( "1",80 ));
			leveles.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 1, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "13", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 3, 3, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "14", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 5, 3, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "15", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 6, 3, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "16", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 3, 3, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "17", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 4, 4, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "18", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 2, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "19", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "20", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 3, 5, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "21", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 7, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "22", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 7, 6, 2, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "23", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 3, 9, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "24", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 2, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "25", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 5, 3, 2, 0, 0, 1, 2, -1 ));

			levelesBonus.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 1, 1, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 7, 1, 1, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 1, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 1, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 4, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 4, 1, 0, 0, 1, 2, -1 ));

			miningLeveles.Add ( new MiningLevelClass ( "2" ));
			trainLeveles.Add ( new TrainLevelClass ( "2",110 ));
			trainLeveles.Add ( new TrainLevelClass ( "3",110 ));

			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map2", leveles, levelesBonus, miningLeveles, trainLeveles ));
			//**************************************************************************************************************//
			leveles.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 1, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "13", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 3, 3, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "14", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 5, 3, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "15", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 6, 3, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "16", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 3, 3, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "17", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 4, 4, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "18", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 2, 4, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "19", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "20", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 3, 3, 5, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "21", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 7, 2, 0, 0, 0, 2, -1, -1 ));
			leveles.Add ( new LevelClass ( "22", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 7, 6, 2, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "23", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 2, 3, 9, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "24", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 1, 3, 2, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "25", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 5, 3, 2, 0, 0, 1, 2, -1 ));
			leveles.Add ( new LevelClass ( "26", FLMissionScreenNodeManager.TYPE_REGULAR_NODE, 5, 3, 2, 0, 0, 1, 2, -1 ));
			
			levelesBonus.Add ( new LevelClass ( "7", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 1, 1, 3, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "8", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 7, 1, 1, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "9", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 1, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "10", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 1, 0, 0, 0, 2, -1, -1 ));
			levelesBonus.Add ( new LevelClass ( "11", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 3, 4, 0, 0, 1, 2, -1 ));
			levelesBonus.Add ( new LevelClass ( "12", FLMissionScreenNodeManager.TYPE_BONUS_NODE, 2, 4, 1, 0, 0, 1, 2, -1 ));
			
			miningLeveles.Add ( new MiningLevelClass ( "2" ));
			trainLeveles.Add ( new TrainLevelClass ( "2",110 ));
			trainLeveles.Add ( new TrainLevelClass ( "3",110 ));
			WORLDS.Add ( new WorldClass ( "ui_sign_worldname_map3", leveles, levelesBonus, miningLeveles, trainLeveles ));
		}
	}
}
