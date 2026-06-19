using System;
using UnityEngine;

namespace kube.game
{
	// Token: 0x02000444 RID: 1092
	public class CachedMasterBehaviour : MonoBehaviour
	{
		// Token: 0x060020F2 RID: 8434 RVA: 0x00016D20 File Offset: 0x00014F20
		private void OnDestroy()
		{
			CachedObject.ClearCache();
		}
	}
}
