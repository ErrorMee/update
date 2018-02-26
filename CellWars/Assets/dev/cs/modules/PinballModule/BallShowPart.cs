using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BallShowPart : MonoBehaviour
{
    public BallBaseLayer ballFgLayer;

    public BallBaseLayer ballLayer;

    public BallBaseLayer barrierLayer;

	public BallBaseLayer giftLayer;

	public BallBaseLayer effectLayer;

	public Transform operatePart;


	public Transform coinButton;
	public Transform gemButton;
	public Transform bottle10101;
	public Transform bottle10102;
	public Transform bottle10103;
	public Transform bottle10104;
	public Transform bottle10105;

    public const int BALL_WIDTH = 5;
    public const int BALL_HEIGHT = 6;

    private OrderAction rootAction;

    void Awake()
    {
        BattleModel.Instance.crtBattle = new BattleInfo();
        BattleModel.Instance.crtBattle.start_x = -2;
        BattleModel.Instance.crtBattle.start_y = 2;
		BattleModel.Instance.crtBattle.end_x = 2;
		BattleModel.Instance.crtBattle.end_y = -3;

        CellInfo.start_x = BattleModel.Instance.crtBattle.start_x;
        ShowBg();
		PlayerModel.Instance.updateWealthEvent += OnUpdateWealthEvent;
    }

	void OnDestroy()
	{
		PlayerModel.Instance.updateWealthEvent -= OnUpdateWealthEvent;
	}

    public void Init()
    {
        ballFgLayer.list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/pinballmodule.ab", "BallFgItem");
        ballLayer.list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/pinballmodule.ab", "BallItem");
        barrierLayer.list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/pinballmodule.ab", "BarrierItem");
		giftLayer.list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/pinballmodule.ab", "GiftItem");
		effectLayer.list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/pinballmodule.ab", "GiftEffectItem");
        InitLayers();
    }

    private void ShowBg()
    {
		WealthInfo coinInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
		coinButton.GetComponentInChildren<NumberText>().text = coinInfo.count + "";
		gemButton.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem).count + "";

		bottle10101.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(10101).count + "";
		bottle10102.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(10102).count + "";
		bottle10103.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(10103).count + "";
		bottle10104.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(10104).count + "";
		bottle10105.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(10105).count + "";
    }

	private void OnUpdateWealthEvent(int id)
	{
		WealthInfo wealthInfo = PlayerModel.Instance.GetWealth(id);

		if(id == 11101)
		{
			NumberText numberText = gemButton.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
		if(id == 11102)
		{
			NumberText numberText = coinButton.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}

		if(id == 10101)
		{
			NumberText numberText = bottle10101.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
		if(id == 10102)
		{
			NumberText numberText = bottle10102.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
		if(id == 10103)
		{
			NumberText numberText = bottle10103.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
		if(id == 10104)
		{
			NumberText numberText = bottle10104.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
		if(id == 10105)
		{
			NumberText numberText = bottle10105.GetComponentInChildren<NumberText>();
			int startNum = Convert.ToInt32(numberText.text);
			numberText.RollNumberStart(wealthInfo.count,startNum);
		}
	}

    private void InitLayers()
    {
        ballFgLayer.ShowList();
        barrierLayer.ShowList();
        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
        {
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
            {
				if (j <= BattleModel.Instance.crtBattle.end_x && j >= BattleModel.Instance.crtBattle.start_x &&
					i < BattleModel.Instance.crtBattle.start_y && i >= BattleModel.Instance.crtBattle.end_y)
                {
                    GameObject item = barrierLayer.CreateBaseItem(j, i);
                    item.name = (j - BattleModel.Instance.crtBattle.start_x) + "_" + (-i + BattleModel.Instance.crtBattle.start_y);
                    PosMgr.SetFightWallPos(item.transform, j - BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y, 1);
				}
                else
                {
					ballFgLayer.CreateBaseItem(j - BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y);
                }
            }
        }
		InitGift();
    }

	private void InitGift()
	{
		ballLayer.ShowList();
		effectLayer.ShowList();
		giftLayer.ShowList();
		for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
			for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
				if (j <= BattleModel.Instance.crtBattle.end_x && j >= BattleModel.Instance.crtBattle.start_x &&
					i < BattleModel.Instance.crtBattle.start_y && i >= BattleModel.Instance.crtBattle.end_y)
				{
					GameObject itemGift = giftLayer.CreateBaseItem(j - BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y);
					itemGift.name = (j - BattleModel.Instance.crtBattle.start_x) + "_" + (-i + BattleModel.Instance.crtBattle.start_y);
					if(i == BattleModel.Instance.crtBattle.end_y)
					{
						itemGift.transform.Find("coin").gameObject.SetActive(false);
					}else
					{
						itemGift.transform.Find("box").gameObject.SetActive(false);
					}
				}
			}
		}
	}

    public void Play(List<int> newPos)
    {
        ballLayer.ShowList();

        BallMove.Init(BALL_WIDTH, BALL_HEIGHT);

        List<CellMoveInfo> moveAnims = BallMove.Move(newPos);

        Filling(moveAnims);
    }

    private void Filling(List<CellMoveInfo> moveAnims, FightStadus fightState = FightStadus.move, int waitmillisecond = 0)
    {
        rootAction = new OrderAction();
        ParallelAction paralle = new ParallelAction();
        
        for (int i = 0; i < moveAnims.Count; i++)
        {
            CellMoveInfo cellMoveInfo = moveAnims[i];

            OrderAction orderAction = new OrderAction();
            paralle.AddNode(orderAction);
            
            GameObject item = ballLayer.CreateBaseItem((int)cellMoveInfo.paths[0].x, -1);

            for (int j = 0; j < cellMoveInfo.paths.Count; j++)
            {
                ParallelAction paralleMove = new ParallelAction();
                orderAction.AddNode(paralleMove);

                Vector2 pathPoint = cellMoveInfo.paths[j];
                Vector2 toPos = PosMgr.GetFightCellPos((int)pathPoint.x, (int)pathPoint.y);
                paralleMove.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), 0,0.2f));

                if (j > 0)
                {
                    Vector2 fromPoint = cellMoveInfo.paths[j - 1];
                    Vector2 fromPos = PosMgr.GetFightCellPos((int)fromPoint.x, (int)fromPoint.y);
                    OrderAction orderRot = new OrderAction();
                    GameObject itembarrier = barrierLayer.GetItemByPos((int)fromPoint.x, (int)fromPoint.y);
                    
					if (itembarrier != null)
                    {
						
                        if (fromPos.x > toPos.x)
                        {
                            orderRot.AddNode(new RoatateActor((RectTransform)itembarrier.transform, new Vector3(0, 0, -60), 0.15f));
                            orderRot.AddNode(new RoatateActor((RectTransform)itembarrier.transform, new Vector3(0, 0, 0), 0.05f));
                            paralleMove.AddNode(orderRot);
							paralleMove.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, -30), 0.2f));
                        }
                        if (fromPos.x < toPos.x)
                        {
                            orderRot.AddNode(new RoatateActor((RectTransform)itembarrier.transform, new Vector3(0, 0, 60), 0.15f));
                            orderRot.AddNode(new RoatateActor((RectTransform)itembarrier.transform, new Vector3(0, 0, 0), 0.05f));
                            paralleMove.AddNode(orderRot);
							paralleMove.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, 30), 0.2f));

                        }
                    }
					List<TIVInfo> p = new List<TIVInfo>();
					TIVInfo px = new TIVInfo();
					px.value = pathPoint.x;
					TIVInfo py = new TIVInfo();
					py.value = pathPoint.y;
					p.Add(px);
					p.Add(py);
					if (fromPos.x > toPos.x)
					{
						paralleMove.AddNode(new FuncTIVActor(Harvest,p));
					}
					if (fromPos.x < toPos.x)
					{
						paralleMove.AddNode(new FuncTIVActor(Harvest,p));
					}
                }
            }
			BallBaseItem itemCtr = item.GetComponent<BallBaseItem>();
			orderAction.AddNode(new PlayBallMoveEndActor(itemCtr));
        }
        if (waitmillisecond > 0)
        {
            rootAction.AddNode(new WaitActor(waitmillisecond));
        }
        rootAction.AddNode(paralle);
        ExecuteAction(fightState);
    }

	private void Harvest(List<TIVInfo> p)
	{
		int px = (int)p[0].value;
		int py = (int)p[1].value;

		GameObject itemGift = giftLayer.GetItemByPos(px, py);

		if(itemGift != null)
		{
			OrderAction giftOrder = new OrderAction();
			giftOrder.AddNode(new WaitActor(40));
            giftOrder.AddNode(new SoundActor("Harvest"));
			giftOrder.AddNode(new ScaleActor((RectTransform)itemGift.transform,new Vector3(1.2f,1.2f,0),0.1f));
			giftOrder.AddNode(new ScaleActor((RectTransform)itemGift.transform,new Vector3(0,0,0),0.04f));
			giftOrder.AddNode(new DestroyActor(itemGift));

			if(py == -BattleModel.Instance.crtBattle.end_y + BattleModel.Instance.crtBattle.start_y)
			{
				int bottleId = UnityEngine.Random.Range(10101, 10101 + 5);

				giftOrder.AddNode(new HarvestActor(px,py,bottleId,50,effectLayer));
			}else
			{
				giftOrder.AddNode(new HarvestActor(px,py,11102,10,effectLayer));
			}

			giftOrder.OnExecute();
		}
	}

    private void ExecuteAction(FightStadus fightStadus)
    {
        FightModule.crtFightStadus = fightStadus;
        rootAction.EventHandler += OnEventHandler;
        rootAction.OnExecute();
    }

    private void OnEventHandler(ActionNode node, ActionEventType eventType)
    {
        if (eventType == ActionEventType.action_end)
        {
			LeanTween.delayedCall(1.3f,Restart);
        }
    }

	private void Restart()
	{
		operatePart.gameObject.SetActive(true);
		InitGift();
	}

}