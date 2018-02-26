using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvadeModel : Singleton<InvadeModel>
{

	public List<InvadeInfo> invadeSources = new List<InvadeInfo>();

	public InvadeModel ()
	{
	}

	public void Destroy()
	{
		Clear ();
		invadeSources = new List<InvadeInfo> ();
	}
	
	public void Clear()
	{
	}

	public void AddInvade(CellInfo cellInfo)
	{
		for(int i = 0;i<invadeSources.Count;i++)
		{
			InvadeInfo invadeInfo = invadeSources[i];

			if(invadeInfo.invadeId == cellInfo.config.hide_id)
			{
				invadeInfo.sourcePos.Add(new Vector2(cellInfo.posX,cellInfo.posY));
				return;
			}
		}

		InvadeInfo newInvade = new InvadeInfo();
		newInvade.invadeId = cellInfo.config.hide_id;
		newInvade.sourcePos.Add(new Vector2(cellInfo.posX,cellInfo.posY));
		invadeSources.Add (newInvade);
	}

    public List<CellAnimInfo> EffectInvade()
	{
        List<CellAnimInfo> cellInvades = new List<CellAnimInfo>();

		for(int i = 0;i<invadeSources.Count;i++)
		{
			InvadeInfo invadeInfo = invadeSources[i];

			int count = 0;

			if(count == 0)
			{
                CellAnimInfo cellInvade = invadeInfo.Invade();

				if(cellInvade != null)
				{
					cellInvades.Add(cellInvade);
				}
			}
		}

		return cellInvades;
	}

	public void InvadeBlocked(CellInfo cellInfo)
	{
		for(int i = 0;i<invadeSources.Count;i++)
		{
			InvadeInfo invadeInfo = invadeSources[i];

			if(cellInfo.config.id == invadeInfo.invadeId)
			{
				invadeInfo.blocked = true;
			}
		}
	}
}

