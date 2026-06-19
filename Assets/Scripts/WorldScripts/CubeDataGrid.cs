namespace kube.map
{
	public class CubeDataGrid
	{
		public CubeGrid _grid;

		public byte this[int x, int y, int z]
		{
			get
			{
				return _grid.getdata(x, y, z);
			}
			set
			{
				_grid.setdata(x, y, z, value);
			}
		}

		public CubeDataGrid(CubeGrid grid)
		{
			_grid = grid;
		}
	}
}
