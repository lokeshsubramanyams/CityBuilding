using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildingUIInfo
{
	public string name;
	public BUILDING_DIMENSION buildingDimention;
	public Color UIColor;
	public GameObject prefab;
	[System.NonSerialized]
	public Building3DInfo this3DBuildinginfo;
	public Building3DInfo InstantiatePrefab (PoolSystemManager pool,Vector3 position)
	{
		this3DBuildinginfo = pool.GetUnusedBuilding(buildingDimention);
		if(this3DBuildinginfo==null)
		{
			GameObject obj = GameObject.Instantiate (prefab) as GameObject;
			obj.transform.parent = pool.transform;
			this3DBuildinginfo = obj.GetComponent<Building3DInfo> ();
		}
		this3DBuildinginfo.SetBuildingStateSelected(position,null,MOUSECLICKTYPE.NONE);
		this3DBuildinginfo.SetColor(UIColor);
		return this3DBuildinginfo;
	}


}


public class BuildingSelectUIManager : MonoBehaviour
{
	public GameObject buildingUISlotPrefab;
	public Transform  ScrollContainer;
	public List<BuildingUIInfo> ConstructorDB;


	private BuildingUIInfo currentSelectedSlot = null;

	public BuildingUIInfo CurrentSelectedSlot {
		get {
			return currentSelectedSlot;
		}
	}

	void OnEnable()
	{
		BuildingSelectSlot.SetCurrentSelectedSlotInfoEvent +=SetSelectedBuildingInfo;
	}
	void OnDisable()
	{
		BuildingSelectSlot.SetCurrentSelectedSlotInfoEvent -=SetSelectedBuildingInfo;
	}
	// Use this for initialization
	void Start ()
	{
		foreach (BuildingUIInfo info in ConstructorDB) 
		{
			GameObject obj = Instantiate (buildingUISlotPrefab)as GameObject;
			obj.transform.parent = ScrollContainer;
			obj.GetComponent<BuildingSelectSlot> ().slotInfo = info;
		}
	}

	void SetSelectedBuildingInfo(BuildingUIInfo info)
	{
		currentSelectedSlot = info;
	}


	public bool IsAnySlotSelected {
		get {
			return currentSelectedSlot!=null;
		}
	}
	
	public Building3DInfo GetSelectedSlotByInstantiate(PoolSystemManager pool,Vector3 Position)
	{
		Building3DInfo info = null;
		if(currentSelectedSlot!=null)
		{
			info =  currentSelectedSlot.InstantiatePrefab(pool,Position);
			currentSelectedSlot = null;
		}
		return info;
	}

}
