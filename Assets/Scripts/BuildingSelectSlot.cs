using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingSelectSlot : MonoBehaviour
{
	[HideInInspector]
	public BuildingUIInfo slotInfo;


	public Image sprite;
	public Text  text;

	public static event System.Action<BuildingUIInfo> SetCurrentSelectedSlotInfoEvent;
	// Use this for initialization
	void Start ()
	{
		sprite.color = slotInfo.UIColor;
		text.text     = slotInfo.name;
	}

	public void SetSelectedSlot ()
	{
		SetSelectedSlot(slotInfo);
	//	BuildingSelectionManager.GetInstance ().SetSelectedUISlot (slotInfo);
		/*GameObject obj = Instantiate (slotInfo.prefab) as GameObject;
		//obj.transform.parent = InputManager.GetInstance().PoolContainer;
		Building3DInfo currentselectedBuilding = obj.GetComponent<Building3DInfo> ();
		currentselectedBuilding.PoolResetTransform ();*/

	}
	void SetSelectedSlot(BuildingUIInfo slotinfo)
	{
		if(SetCurrentSelectedSlotInfoEvent!=null)
			SetCurrentSelectedSlotInfoEvent(slotinfo);
	}

}
