using System.Collections.Generic;

namespace kube.data
{
	public class IntHash : Dictionary<int, bool>
	{
		public new bool this[int key]
		{
			get
			{
				bool value = false;
				TryGetValue(key, out value);
				return value;
			}
			set
			{
				base[key] = value;
			}
		}

		public IntHash(int x)
			: base(x)
		{
		}

		public IntHash()
		{
		}

		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (int key in base.Keys)
			{
				list.Add(string.Concat("(", key.GetType(), ")", key, "=(", key.GetType(), ")", this[key]));
			}
			return string.Join(", ", list.ToArray());
		}
	}
}
