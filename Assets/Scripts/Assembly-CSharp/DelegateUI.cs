namespace kube.ui
{
	public class DelegateUI : BaseUI
	{
		public DrawCall onClose;

		public DrawCall onOpen;

		public DrawCall drawCall
		{
			get
			{
				return _draw;
			}
		}

		public DelegateUI(DrawCall func)
		{
			if (func != null)
			{
				_draw = func;
			}
		}

		public override void show()
		{
			if (onOpen != null)
			{
				onOpen();
			}
		}

		public override void hide()
		{
			if (onClose != null)
			{
				onClose();
			}
		}
	}
}
