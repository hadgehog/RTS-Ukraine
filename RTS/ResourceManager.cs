﻿using UnityEngine;
using System.Collections;


namespace RTS
{
	public static class ResourceManager
	{
		public static float ScrollSpeed {get{return 40;}}
		public static float RotateSpeed {get{return 100;}}
		public static int ScrollWidth {get{return 15;}}
		public static float MinCameraHeight {get{return 5;}}
		public static float MaxCameraHeight {get{return 70;}}
		public static float RotateAmount {get{return 30;}}
		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 InvalidPosition { get { return invalidPosition; } }
		private static GUISkin selectBoxSkin;
		public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }
		private static Bounds invalidBounds = new Bounds(new Vector3(-99999, -99999, -99999), new Vector3(0, 0, 0));
		public static Bounds InvalidBounds { get { return invalidBounds; } }
		public static int BuildSpeed { get { return 2; } }
		private static GameObjectList gameObjectList;
		
		public static void StoreSelectBoxItems(GUISkin skin) 
		{
			selectBoxSkin = skin;
		}
		
		public static void SetGameObject(GameObjectList objectList)
		{
			gameObjectList = objectList;
		}
		
		public static GameObject GetBuilding(string name) 
		{
		    return gameObjectList.GetBuilding(name);
		}
	 
		public static GameObject GetUnit(string name) 
		{
		    return gameObjectList.GetUnit(name);
		}
	 
		public static GameObject GetWorldObject(string name) 
		{
		    return gameObjectList.GetWorldObject(name);
		}
	 
		public static GameObject GetPlayerObject() 
		{
		    return gameObjectList.GetPlayerObject();
		}
	 
		public static Texture2D GetBuildImage(string name) 
		{
	    	return gameObjectList.GetBuildImage(name);
		}
		
		public static void SetGameObjectList(GameObjectList objectList) 
		{
    		gameObjectList = objectList;
		}
	}
}
