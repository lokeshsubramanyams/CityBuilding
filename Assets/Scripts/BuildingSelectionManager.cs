using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum BUILDING_DIMENSION
{
	NONE 		 = 0,
	BUILDING_1_1 ,
	BUILDING_2_2 ,
	BUILDING_2_3 ,
	BUILDING_2_4 ,
	BUILDING_2_5 ,
	BUILDING_3_2 ,
	BUILDING_3_3 ,
	BUILDING_3_4 ,
	BUILDING_3_5 ,
	BUILDING_4_2 ,
	BUILDING_4_3 ,
	BUILDING_4_4 ,
	BUILDING_4_5 ,
	BUILDING_5_2 ,
	BUILDING_5_3 ,
	BUILDING_5_4 ,
	BUILDING_5_5 ,
	BUILDING_6_2 ,
	BUILDING_6_3 ,
	BUILDING_6_4 ,
	BUILDING_6_5 ,
	BUILDING_6_6 ,
	BUILDING_7_2 ,
	BUILDING_7_3 ,
	BUILDING_7_4 ,
	BUILDING_7_5 

}

public enum MOUSECLICKTYPE
{
	NONE,
	MOUSEDOWN,
	MOUSEDRAG,
	MOUSEUP
}

public class BuildingSelectionManager : SingletonMono<BuildingSelectionManager>
{

	public GridSystemManager gridSystemInstance;
	public BuildingSelectUIManager buildingSlotSelectionInstance;
	public PoolSystemManager  poolsystemInstance;
	public LayerMask groundMask;
	public LayerMask buildigMask;


	private Building3DInfo CurrentSelectedBuilding = null;
	private BuildingUIInfo CurrentSelectedSlot = null;

	public override void Awake ()
	{
		base.Awake ();
	}
	IEnumerator Start ()
	{
		yield return new WaitForSeconds(1.0f);
		gridSystemInstance.SetRandomObstacles(poolsystemInstance.inititalObstaclescontainer);
	}
	void OnEnable ()
	{
		InputManager.InputHandlerMouseDownEvent += OnMouseDown;
		InputManager.InputHandlerMouseDragEvent += OnMouseDrag;
		InputManager.InputHandlerMouseUpEvent += OnMouseUp;
	}
	void OnDisable ()
	{
		InputManager.InputHandlerMouseDownEvent -= OnMouseDown;
		InputManager.InputHandlerMouseDragEvent -= OnMouseDrag;
		InputManager.InputHandlerMouseUpEvent -= OnMouseUp;
	}
	public void SetSelectedBuilding (Building3DInfo currentSelected)
	{
		CurrentSelectedBuilding = currentSelected;
	}
	public void GetSelectedUIBuilding3D (Vector3 position)
	{
		CurrentSelectedBuilding = buildingSlotSelectionInstance.GetSelectedSlotByInstantiate(poolsystemInstance,position);
		if(CurrentSelectedBuilding!=null)CurrentSelectedBuilding.SetSelectedBuilding(true);

	}


	Vector3 rayCasthitposition ;
	void OnMouseDown (RaycastHit hit)
	{
		if(hit.transform!=null  )
		{
			if((1<<hit.transform.gameObject.layer & groundMask) >0)
			{
				if(CurrentSelectedBuilding==null )
				{
					if( buildingSlotSelectionInstance.IsAnySlotSelected)
					{
						Vector3 gridPos = gridSystemInstance.GetGroundHitPosition(hit.point);
						Debug.Log(gridPos);
						GetSelectedUIBuilding3D(gridPos);
						CheckIsValidPosition(gridPos,MOUSECLICKTYPE.MOUSEDOWN);
					}
				}
				else 
				{
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}

			}
			else if((1<<hit.transform.gameObject.layer & buildigMask) >0)
			{
				if(CurrentSelectedBuilding == null)
				{
					CurrentSelectedBuilding = GetComponentByTransform<Building3DInfo>(hit.transform);
					CheckIsValidPosition(hit.point,MOUSECLICKTYPE.MOUSEDOWN);
				}
				else 
				{
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}
			}
			else 
			{
				if(CurrentSelectedBuilding != null)
				{
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}
			}
		}
		else 
		{
			if(CurrentSelectedBuilding != null)
			{
				CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
				CurrentSelectedBuilding = null;
			}
			else 
			{
				GetSelectedUIBuilding3D(Input.mousePosition);
			}
		}
	}

	void OnMouseDrag (RaycastHit hit)
	{

		if(hit.transform!=null &&((1<<hit.transform.gameObject.layer & groundMask) >0 || (1<<hit.transform.gameObject.layer & buildigMask) >0))
			CheckIsValidPosition (hit.point,MOUSECLICKTYPE.MOUSEDRAG);
		/*else 
		{
			if(CurrentSelectedBuilding != null)
			{
				CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
				CurrentSelectedBuilding = null;
			}
		}*/ 
	}
	void OnMouseUp (RaycastHit hit)
	{
		if(hit.transform!=null  )
		{
			if((1<<hit.transform.gameObject.layer & groundMask) >0)
			{
				if(CurrentSelectedBuilding!=null )
				{
					CheckIsValidPosition(hit.point,MOUSECLICKTYPE.MOUSEUP);
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}
				
			}
			else if((1<<hit.transform.gameObject.layer & buildigMask) >0)
			{
				if(CurrentSelectedBuilding!=null )
				{
					CheckIsValidPosition(hit.point,MOUSECLICKTYPE.MOUSEUP);
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}
			}
			else 
			{
				if(CurrentSelectedBuilding != null)
				{
					CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
					CurrentSelectedBuilding = null;
				}
			}
		}
		else 
		{
			if(CurrentSelectedBuilding != null)
			{
				CurrentSelectedBuilding.SetBuildingDeSelected(poolsystemInstance);
				CurrentSelectedBuilding = null;
			}
		}
	}

	void CheckIsValidPosition (Vector3 hitposition,MOUSECLICKTYPE type)
	{
		List<GridNode> selectingNodeList = null;
		if (CurrentSelectedBuilding != null )
		{ 
			Vector3 postion = gridSystemInstance.CheckValidPosition (hitposition, CurrentSelectedBuilding.info.gridSize, out selectingNodeList);

			CurrentSelectedBuilding.SetBuildingStateSelected (postion, selectingNodeList,type);
		}


	}

	T GetComponentByTransform<T>(Transform hitbuilding)where T : Component
	{
		T current3DInfo = null;
		if(hitbuilding)current3DInfo = hitbuilding.GetComponentInParent<T>();
		return current3DInfo;
	}

}
