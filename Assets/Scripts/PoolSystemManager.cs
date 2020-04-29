using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolSystemManager : MonoBehaviour 
{
	public Transform inititalObstaclescontainer;
	List<Building3DInfo> unusedBuildingList = new List<Building3DInfo>();

	public void SetUnUsedBuildings(Building3DInfo building)
	{
		unusedBuildingList.Add(building);

	}
	public Building3DInfo GetUnusedBuilding(Vector2 size)
	{
		Building3DInfo building = null;
		foreach(Building3DInfo info in unusedBuildingList)
		{
			if(info.info.gridSize.x == size.x && info.info.gridSize.y == size.y)
			{
				building = info;
				break;
			}
				 
		}
		if(building!=null)unusedBuildingList.Remove(building);
		return building;
	}
	public Building3DInfo GetUnusedBuilding(BUILDING_DIMENSION dimension)
	{
		Building3DInfo building = null;
		foreach(Building3DInfo info in unusedBuildingList)
		{
			if(info.info.buildingType == dimension)
			{
				building = info;
				break;
			}
			
		}
		if(building!=null)unusedBuildingList.Remove(building);
		return building;
	}


}
