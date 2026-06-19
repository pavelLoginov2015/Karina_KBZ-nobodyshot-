namespace kube.map
{
	public class CubeWaterGrid
	{
		public CubeGrid _grid;

		public byte this[int x, int y, int z]
		{
			get
			{
				int type = 0;
				int data = 0;
				_grid.get(x, y, z, ref type, ref data);
				if (type == 128 || type == 0)
				{
					return (byte)data;
				}
				return 0;
			}
			set
			{
				int num = _grid.get(x, y, z);
				if (num == 128 || num == 0)
				{
					_grid.setdata(x, y, z, value);
				}
			}
		}

		public CubeWaterGrid(CubeGrid grid)
		{
			_grid = grid;
		}
	}
}
