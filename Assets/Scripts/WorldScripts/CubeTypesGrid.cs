namespace kube.map
{
	public class CubeTypesGrid
	{
		public CubeGrid _grid;

		public int this[int x, int y, int z]
		{
			get
			{
				return _grid.get(x, y, z);
			}
			set
			{
				_grid.set(x, y, z, value);
			}
		}

		public CubeTypesGrid(CubeGrid grid)
		{
			_grid = grid;
		}
	}
}
