using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
	
	public static event System.Action<RaycastHit> InputHandlerMouseDownEvent;
	public static event System.Action<RaycastHit> InputHandlerMouseDragEvent;
	public static event System.Action<RaycastHit> InputHandlerMouseUpEvent;
	
	public Camera mainCamera;
	
	public float ray_distance;

   // public Building3DInfo currentselectedBuilding;

	RayCasting _raycasting;
	Vector3 currenthitposition;
	List<GridNode> fillingNodes = null;
	bool isTouchDown = false;

	// Use this for initialization
	void Start () 
	{
		_raycasting = new RayCasting(mainCamera,ray_distance);

	}

	// Update is called once per frame
	void Update () 
	{

		if(Input.GetMouseButtonDown(0))
		{
			isTouchDown = true;
			if(_raycasting!=null)MouseDown(_raycasting.castRayHit());
		}
		else if(!Input.GetMouseButtonUp(0) && isTouchDown)
		{

			if(_raycasting!=null)MouseDrag(_raycasting.castRayHit());


		}
		else  if (Input.GetMouseButtonUp(0))
		{
			isTouchDown = false;
			if(_raycasting!=null)MouseUp(_raycasting.castRayHit());
		}
			

	}

	public static void MouseDown(RaycastHit hit)
	{
		if(InputHandlerMouseDownEvent!=null)
		{
			InputHandlerMouseDownEvent(hit);
		}
	}
	public static void MouseDrag(RaycastHit hit)
	{
		if(InputHandlerMouseDragEvent!=null)
		{
			InputHandlerMouseDragEvent(hit);
		}
	}
	public static void MouseUp(RaycastHit hit)
	{
		if(InputHandlerMouseUpEvent!=null)
		{
			InputHandlerMouseUpEvent(hit);
		}
	}

	
	
}
