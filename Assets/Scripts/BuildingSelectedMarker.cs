using UnityEngine;
using System.Collections;

public class BuildingSelectedMarker : MonoBehaviour 
{

	public float speed;
	public Renderer renderer;
	public BoxCollider collider;
	Color color;
	void Start()
	{
		color = renderer.material.color;
		//collider = renderer.gameObject.GetComponent<BoxCollider>();
	}
	public void SetAnimation(bool activate,Color color)
	{
		if(activate)
		{
			//StopAllCoroutines();
			renderer.material.color = Color.blue;
			//collider.enabled = false;
			//StartCoroutine(Animate());
		}
		else 
		{
			renderer.material.color = color;
			//collider.enabled = true;
			//StopAllCoroutines();
		}
	}

	IEnumerator Animate()
	{
		//int  alpha = -1;
		while(true)
		{
			renderer.material.color = Color.Lerp(Color.green,Color.blue,speed);

			/*Color c = renderer.material.color;
			c.a +=0.02f* alpha;
			renderer.material.color = c;
			if(c.a>0.5f)alpha = -1;
			if(c.a<=0)alpha = 1;*/
			yield return null;
		}//new WaitForSeconds(speed);

		
		
		Debug.Log("Animating");
	}

}
