using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building3DInfo : MonoBehaviour
{

	public BuildingInfo info;


	public GameObject validMarker ;
	public GameObject inValidmarker ;
	public BuildingSelectedMarker marker;

	List<GridNode> filledNodes;
	//private  GridNode[]
	Color buildingColor;
	Vector3 validPosition = Vector3.zero*-1000.0f;
	public void SetMarker (bool isValid)
	{
		validMarker.SetActive (isValid);
		inValidmarker.SetActive (!isValid);
	}
	public void SetColor(Color color)
	{
		buildingColor = color;
	}
	public void SetSelectedBuilding(bool isselected)
	{
		marker.SetAnimation(isselected,buildingColor);
	}
	public void SetBuildingStateSelected (Vector3 position, List<GridNode> filledValidNodes,MOUSECLICKTYPE type)
	{
		Debug.Log("position:"+position);
		if(position != (Vector3.zero*-1.0f))
			transform.position = position;
		if(filledNodes == null && filledValidNodes!=null && type == MOUSECLICKTYPE.MOUSEUP)
		{
			filledNodes = filledValidNodes;
			validPosition = position;
		}
		else if(filledNodes!=null)
		{
			if(type == MOUSECLICKTYPE.MOUSEUP)
			{
				if(filledValidNodes!=null)
				{
					SetFilledGrids(false);
					filledNodes = filledValidNodes;
					validPosition = position;

				}
			}
			else if(type == MOUSECLICKTYPE.MOUSEDOWN || type == MOUSECLICKTYPE.MOUSEDRAG)
				SetFilledGrids(false);
		}

		SetMarker (filledValidNodes != null);
		SetSelectedBuilding(type != MOUSECLICKTYPE.MOUSEUP);
		
	}
	public void SetBuildingDeSelected(PoolSystemManager pool)
	{

		SetSelectedBuilding(false);
		if(filledNodes!=null)
		{
			if(validPosition!=Vector3.zero*-1000.0f)
				transform.position = validPosition;
			SetFilledGrids(true);
			SetMarker(true);
		}
		else 
		{
			PoolResetTransform(pool);
		}
	}
	public void PoolResetTransform (PoolSystemManager pool)
	{
		pool.SetUnUsedBuildings(this);
		transform.position = Vector3.one * -1000.0f;
	}

	public bool IsBuildingisSame(this Building3DInfo info)
	{
		return this == info;
	}
	void SetFilledGrids(bool isfilled)
	{
		foreach(GridNode n in filledNodes)
			n.isFilled = isfilled;
	}
}
