using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TraversalDirection
{
	LEFT,
	RIGHT,
	TOP,
	BOTTOM
}
public class GridSystemManager : MonoBehaviour
{
	public static GridSystemManager instance = null;
	public int rows;
	public int columns;
	public  Renderer groundMaterial;
	public GameObject Obstractle;

	private   List<GridNode> GridNodes = new List<GridNode> ();
	Vector3 initialpoisition ;
	Vector3 offsetVector;

	int gridCount;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else 
			Destroy (gameObject);
	}


	// Use this for initialization
	void Start ()
	{

		groundMaterial.material.mainTextureScale = new Vector2 (rows, columns);
		//groundMaterial.material.mainTextureOffset = new Vector2(1/rows,1/columns);

		Debug.Log ("groundMaterial.bounds.size: " + groundMaterial.bounds.size);
		Debug.Log ("groundMaterial.bounds.min: " + groundMaterial.bounds.min);
		Debug.Log ("groundMaterial.bounds.max: " + groundMaterial.bounds.max);

		initialpoisition = groundMaterial.bounds.min;
		offsetVector = new Vector3 ((groundMaterial.bounds.size.x / rows), 0.0f, (groundMaterial.bounds.size.z / columns));
		Debug.Log ("offsetVector" + offsetVector);
		gridCount = rows * columns;
		Vector3 position = initialpoisition + offsetVector * 0.5f;
		for (int r = 0; r<rows; r++) {
			for (int c=0; c<columns; c++) {
				Vector3 pos = position + new Vector3 (offsetVector.x * r, 0, offsetVector.z * c);
				GridNode node = new GridNode (false, pos, r, c);
				GridNodes.Add (node);// GridNode(false,pos);
				//Instantiate(temp,pos,Quaternion.identity);
			}

		}



		for (int i=0; i<gridCount; i++) {
			GridNodes [i].left = (GridNodes [i].Column - 1 < 0) ? null : GridNodes [GetListIndex (GridNodes [i].row, GridNodes [i].Column - 1)];
			GridNodes [i].right = (GridNodes [i].Column + 1 >= columns) ? null : GridNodes [GetListIndex (GridNodes [i].row, GridNodes [i].Column + 1)];
			GridNodes [i].bottom = (GridNodes [i].row - 1 < 0) ? null : GridNodes [GetListIndex (GridNodes [i].row - 1, GridNodes [i].Column)];
			GridNodes [i].top = (GridNodes [i].row + 1 >= rows) ? null : GridNodes [GetListIndex (GridNodes [i].row + 1, GridNodes [i].Column)];

		}


	}

	public void SetRandomObstacles(Transform container)
	{
		for (int i=0; i<10; i++) 
		{
			GridNode n = GridNodes [Random.Range (0, gridCount)];
			n.isFilled = true;
			//Vector3 pos = n.position+Vector3.up*0.5f;
			GameObject obj =  Instantiate (Obstractle, n.position, Quaternion.identity) as GameObject;
			obj.transform.parent = container;
		}
	}

	int GetListIndex (int r, int c)
	{

		return r * columns + c;
	}
	int GetListIndex (Vector2 pos)
	{
		
		return (int)(pos.x * columns + pos.y);
	}


	public Vector3   CheckValidPosition (Vector3 raycasethitposition, Vector2 checkGridSize, out List<GridNode> fillingNodes)
	{

		int index = GetIndexByHitPosition (raycasethitposition, checkGridSize, out fillingNodes);
		if (index < 0 && index <gridCount)
			return Vector3.zero * -1.0f;

		return  GridNodes [index].position;
	}

	public int GetIndexByHitPosition (Vector3 hitposition, Vector2 size, out List<GridNode> fillingNodes)
	{
		Vector3 position = hitposition - initialpoisition;

		int r = Mathf.Clamp (Mathf.RoundToInt (position.x / offsetVector.x), 0, rows - 1);
		int c = Mathf.Clamp (Mathf.RoundToInt (position.z / offsetVector.z), 0, columns - 1);
	
		int startRow = r - Mathf.FloorToInt (((size.x - 1) * 0.5f));//-((size.x%2==0)?0.5f:0));
		int startColumn = c - Mathf.FloorToInt ((size.y - 1) * 0.5f);//-((size.y%2==0)?0.5f:0));

		int index = GetListIndex (r, c);
		if (index >= 0 || index < gridCount) {
			Vector2 submatrixstart = new Vector2 (startRow, startColumn);
			fillingNodes = IsValidPosition (submatrixstart, size);
			//GridNode startMatrixNode = GridNodes[GetListIndex(startRow,startColumn)];
			//isvalid = Traversal(startMatrixNode,TraversalDirection.TOP,(int)size.x,(int)size.y,0);
			return index;
		}
		fillingNodes = null;
		return -1;

	}

	public Vector3 GetGroundHitPosition (Vector3 hitposition)
	{
		Vector3 position = hitposition - initialpoisition;
		
		int r = Mathf.Clamp (Mathf.RoundToInt (position.x / offsetVector.x), 0, rows - 1);
		int c = Mathf.Clamp (Mathf.RoundToInt (position.z / offsetVector.z), 0, columns - 1);

		int index = GetListIndex (r, c);

		if (index >= 0 || index < gridCount)
		{

			return GridNodes[index].position;
		}

		return Vector3.zero;
		
	}


	public List<GridNode> IsValidPosition (Vector2 submatrix, Vector2 size)
	{
		List<GridNode> filledNodes = null;

		if (submatrix.x < 0 || submatrix.y < 0 || submatrix.x > rows || submatrix.y > columns)
			return null;

		int rcount = Mathf.RoundToInt (submatrix.x + size.x);
		int ccount = Mathf.RoundToInt (submatrix.y + size.y);

		if (rcount > rows || ccount > columns)
			return null;

		filledNodes = new List<GridNode> ();
		for (int r = (int)submatrix.x; r<rcount; r++) {
			for (int c = (int)submatrix.y; c<ccount; c++) {
				int index = GetListIndex (r, c);
				filledNodes.Add (GridNodes [index]);
				if (index >= 0 && index < gridCount) {
					if (GridNodes [index].isFilled)
						return null;
				} else
					return null;
			}
		}
		return filledNodes;
	}

/*	bool Traversal(GridNode node , TraversalDirection dir,Vector2 size,int leftcount,int topcount)
	{
		if(node == null)return false;
		
		if(node.isFilled)return false;
		
		if(leftcount>=size.x)return true;

		leftcount++;

		return Traversal(GetNode(node,TraversalDirection.LEFT),TraversalDirection.LEFT,size ,leftcount,topcount);
		
				
		if(topcount>=size.y-1)return true;

		topcount++;
		leftcount =0;

		return Traversal(GetNode(node,TraversalDirection.TOP),TraversalDirection.TOP,size ,leftcount,topcount);

	}*/
	bool Traversal (GridNode node, TraversalDirection dir, int toplimit, int limit, int traversalCount)
	{
		if (node == null)
			return false;
		Debug.Log ("***********************node" + node.row + "," + node.Column);
		if (node.isFilled)
			return false;
		
		bool leftcheck = SubTraversal (GetNode (node, TraversalDirection.RIGHT), TraversalDirection.RIGHT, limit, 1);

		if (!leftcheck)
			return false;
		
		if (traversalCount >= toplimit - 1)
			return true;

		traversalCount++;

		return Traversal (GetNode (node, TraversalDirection.TOP), TraversalDirection.TOP, toplimit, limit, traversalCount);

		
	}

	bool SubTraversal (GridNode node, TraversalDirection dir, int limit, int traversalCount)
	{
		if (node == null)
			return false;
		Debug.Log ("***********************node" + node.row + "," + node.Column);
		if (node.isFilled)
			return false;
		
		if (traversalCount >= limit - 1)
			return true;

		traversalCount++;

		return SubTraversal (GetNode (node, dir), dir, limit, traversalCount);
		

	}


	
	GridNode GetNode (GridNode node, TraversalDirection dir)
	{
		GridNode n = null;
		switch (dir) {
		case TraversalDirection.TOP:
			n = node.top;
			break;
		case TraversalDirection.BOTTOM:
			n = node.bottom;
			break;
		case TraversalDirection.LEFT:
			n = node.left;
			break;
		case TraversalDirection.RIGHT:
			n = node.right;
			break;
		}
		return n;
	}

	public void SetSelectedObjectPosition (int index, Transform spawnedObj)
	{
		if (index >= 0 && index < gridCount) {
			Vector3 pos = GridNodes [index].position;
			if (spawnedObj != null)
				spawnedObj.position = pos;
			Instantiate(Obstractle,pos,Quaternion.identity);
		}
	
	}

	Vector3 ReturnPosition (GridNode node)
	{
		Vector3 pos = node.position;
		pos.y += 0.002f;
		return pos;
	}

}
