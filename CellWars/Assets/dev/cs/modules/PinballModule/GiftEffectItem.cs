using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GiftEffectItem : MonoBehaviour {

	private int wealthId;
	private int addCount;

	public void Play(int id,int count)
	{
		wealthId = id;
		addCount = count;

		Vector3 topos = new Vector3(-342,667,0);

		Image bottleIcon = transform.FindChild ("icon").GetComponent<Image> ();
		if(wealthId <= 10105 && wealthId >= 10101)
		{
			topos = new Vector3(-175*(10103 - id),485,0);
			bottleIcon.gameObject.SetActive(true);
			bottleIcon.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + wealthId);	
		}else
		{
			bottleIcon.gameObject.SetActive(false);
		}
		RectTransform itemRect = (RectTransform)transform;
		float len = Mathf.Sqrt(Mathf.Pow(itemRect.anchoredPosition.x - topos.x, 2) + Mathf.Pow(itemRect.anchoredPosition.y - topos.y, 2));
		float tweenTime = len / 1200.00f;

		LeanTween.move((RectTransform)transform, topos, tweenTime).onComplete = MoveCompleteHander;

		Text addText = transform.FindChild("count").GetComponent<Text>();
		addText.text = "";
	}

	private void MoveCompleteHander()
	{
		Text addText = transform.FindChild("count").GetComponent<Text>();
		addText.text = "+" + addCount;

		WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(wealthId);
		bottleInfo.count += addCount;

		PlayerModel.Instance.SaveWealths(wealthId);

		LeanTween.delayedCall(0.4f,DestroyItem);
	}

	private void DestroyItem()
	{
		if(this != null && this.gameObject != null)
		{
			Destroy(this.gameObject);
		}
	}

}
