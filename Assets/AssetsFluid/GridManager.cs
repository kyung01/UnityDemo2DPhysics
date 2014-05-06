using UnityEngine;
using System.Collections;
using ExtensionsTransform;
using ExtensionsUnityVectors;

public class GridManager : MonoBehaviour {
	/**
	 * Register particle and update their located grid.
	 * Do spatial partitioning.
	 * TRIVIAL TTM : 
	 *		Particle cannot be struct!
	 *		It has to be monobehavior class how are you goign to keep track of things?
	 *		well I could start with set number of particles
	 *		possible but it's going to dynamically register and update.
	 *		So have two parts, ok.
	 *		
	 *	PS you know, this could "not" work? I know...I am going to fail fanstastically.
	 *	Life is about taking risk, Triver said that! Learning takes burning through all you know.
	 *	Ok. Lets do this.
	 * **/
	public Grid PREFAB_GRID;
    public Vector2	gridCount;

	Vector3 posInit;
	Vector2 gridSize;
	Grid[,] gridArray;

	void register()
	{
		KParticle.MANAGER_GRID = this;
		Grid.MANAGER_GRID = this;
	}
    void Awake()
    {
		register();

		var p = transform.parent;
		var rot = transform.rotation;
		transform.parent = null;
		transform.rotation = Quaternion.identity;

		init();

		renderer.enabled = false;
		transform.parent = p;
		transform.rotation = rot;
    }
	void init()
	{
		posInit = transform.getPosBottomLeft();
		gridSize = transform.localScale.divide(new Vector3(gridCount.x, gridCount.y, 1));
		InitGrids(transform.getPosBottomLeft(),
			(int)gridCount.x, (int)gridCount.y, gridSize);
	}
	void InitGrids(Vector3 from, int w, int h, Vector3 scale)
	{
		gridArray = new Grid[w, h];
		from += new Vector3(scale.x * .5f, scale.y *.5f, 0);
		for (int i = 0; i < w; i++) for (int j = 0; j < h; j++)
			{
				var pos = from + new Vector3(i*scale.x,j* scale.y,0);
				var g = Instantiate(PREFAB_GRID, pos, Quaternion.identity) as Grid;
				g.transform.localScale = scale;
				g.name = "G " + i + " " + j;
				g.transform.parent = transform;
				g.init();
				gridArray[i, j] = g;
				
			}
	}
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	bool helperRegister(KParticle p)
	{
		var posRelative = p.transform.position - posInit;
		if (posRelative.x < 0 || posRelative.y < 0) return false;
		int		x = (int)(posRelative.x / gridSize.x),
				y = (int)(posRelative.y / gridSize.y);
		if (x >= gridCount.x || y >= gridCount.y) return false;
		gridArray[x, y].register(p);
		return true;
	}
	public void register(KParticle p)
	{
		if(!helperRegister(p)) GameObject.Destroy(p.gameObject);
	}
	public void updateGrid(KParticle p)
	{
	}
}
