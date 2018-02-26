using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SkillModel : Singleton<SkillModel>
{
    public List<SkillSeedInfo> seeds;

    public List<SkillEntityInfo> fighting_entitys;

    public SkillEntityInfo crt_entity;

    private List<Vector2> poss = new List<Vector2>() { new Vector2(-2, 4), new Vector2(-1, 4), new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4) };

    public void InitSeeds()
    {
		SkillSeedInfo.LAST_RUN_ID = 0;
        seeds = new List<SkillSeedInfo>();

        int index = 0;
        for (int i = 0; i < SkillTempletModel.Instance.skillSelects.Count; i++)
        {
            int skillId = SkillTempletModel.Instance.skillSelects[i];
			SkillTempletInfo skillTempletInfo;
			skillTempletInfo = SkillTempletModel.Instance.GetTemplet(skillId);

			CreateSkillSeed(index, skillId,skillTempletInfo);

            index++;
        }
    }

	private void CreateSkillSeed(int index, int skillId, SkillTempletInfo skillTempletInfo)
    {
        SkillSeedInfo seed = new SkillSeedInfo();
		Vector2 pos = poss[index];
		seed.seed_x = (int)pos.x;
		seed.seed_y = (int)pos.y;
		seed.progressTemp = seed.progress = 0;
		seed.fobid = skillTempletInfo.fobid;

		seed.config_cell_item = (config_cell_item)ResModel.Instance.config_cell.GetItem(skillId);

		seeds.Add(seed);

		if(seed.fobid)
		{
			return;
		}
		seed.open_holes = new List<int>();
		seed.dir = skillTempletInfo.config.dir;
        for (int i = 0; i < skillTempletInfo.lv;i++ )
        {
			seed.open_holes.Add(i);
        }
    }

    public void SeedCollect(bool isTemp)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            SkillSeedInfo seed = seeds[i];
			if(seed.fobid)
			{
				continue;
			}
            int allCount = CollectModel.Instance.tempCollect.GetCount(seed.GetHideCellId());

            if (isTemp)
            {
                seed.progressTemp = seed.progress + allCount / GameModel.Instance.GetGameConfig(1002);
            }
            else
            {
                seed.progressTemp = seed.progress = seed.progress + allCount / GameModel.Instance.GetGameConfig(1002);
            }
        }
    }

	public void InitFightingEntitys()
	{
		fighting_entitys = new List<SkillEntityInfo>();
		
		List<List<CellInfo>> allCells = CellModel.Instance.allCells;
		
		for(int i = 0;i< allCells.Count;i++)
		{
			List<CellInfo> xCells = allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				if(cellInfo.isBlank == false)
				{
					if(cellInfo.config.cell_type == (int)CellType.radiate || cellInfo.config.cell_type == (int)CellType.bomb)
					{
						SkillEntityInfo entity = new SkillEntityInfo();
                        entity.seed = CreateTempSeed(cellInfo.config.id);
						ThrowSkillEntity(entity,cellInfo);
					}

					if(cellInfo.config.cell_type == (int)CellType.line_bomb || cellInfo.config.cell_type == (int)CellType.line_bomb_r
					   || cellInfo.config.cell_type == (int)CellType.three_bomb || cellInfo.config.cell_type == (int)CellType.three_bomb_r)
					{
						SkillEntityInfo entity = new SkillEntityInfo();
						entity.seed = CreateSpecialSeed(cellInfo.config.id);
						ThrowSkillEntity(entity,cellInfo);
					}
				}

			}
		}
	}

	private SkillSeedInfo CreateTempSeed(int id)
	{
		SkillSeedInfo seed = new SkillSeedInfo();
		seed.config_cell_item = (config_cell_item)ResModel.Instance.config_cell.GetItem(id);
		seed.open_holes = new List<int>();
		SkillTempletInfo skillTempletInfo = SkillTempletModel.Instance.GetTemplet(id);
		seed.dir = skillTempletInfo.config.dir;
		for (int i = 0; i < skillTempletInfo.lv; i++)
		{
			seed.open_holes.Add(i);
		}
		return seed;
	}

	private SkillSeedInfo CreateSpecialSeed(int id)
	{
		SkillSeedInfo seed = new SkillSeedInfo();
		seed.config_cell_item = (config_cell_item)ResModel.Instance.config_cell.GetItem(id);
		return seed;
	}

    //取出可投放的新技能
    public List<SkillEntityInfo> GetNewSkillEntitys()
    {
        List<SkillEntityInfo> newEntitys = new List<SkillEntityInfo>();
        for (int i = 0; i < seeds.Count; i++)
        {
            SkillSeedInfo seed = seeds[i];
			if(seed.fobid)
			{
				continue;
			}
            if (seed.progress >= 0.99f)
            {
                seed.progressTemp = seed.progress = 0;
                SkillEntityInfo entity = new SkillEntityInfo();
                entity.seed = seed;
                entity.fightingType = SkillFightingType.wait;
                newEntitys.Add(entity);
            }
        }
        return newEntitys;
    }

    //投放一个技能
    public void ThrowSkillEntity(SkillEntityInfo entity,CellInfo cell)
    {
        entity.fightingType = SkillFightingType.idel;
        entity.cell = cell;
        fighting_entitys.Add(entity);
    }

    private SkillEntityInfo GetSkillEntityByCell(CellInfo cellInfo)
    {
        for (int i = 0; i < fighting_entitys.Count; i++)
        {
            SkillEntityInfo skillEntityInfo = fighting_entitys[i];
            if (skillEntityInfo.cell.runId == cellInfo.runId)
            {
                return skillEntityInfo;
            }
        }
        return null;
    }

	public List<CellInfo> GetBombCells(CellInfo cellInfo)
	{
		List<CellInfo> bombCells = new List<CellInfo>();
		for (int i = 0; i < fighting_entitys.Count; i++)
		{
			SkillEntityInfo skillEntityInfo = fighting_entitys[i];
			int entitytype = skillEntityInfo.seed.config_cell_item.cell_type;
			if (skillEntityInfo.skill_x == cellInfo.posX && skillEntityInfo.skill_y == cellInfo.posY && 
				(entitytype == (int)CellType.bomb || entitytype == (int)CellType.line_bomb_r || entitytype == (int)CellType.three_bomb_r))
			{
				bombCells = skillEntityInfo.controlCells;
				if (bombCells.Count != 0)
				{
					break;
				}
			}
		}
		return bombCells;
	}

    private void UpdateControlCells(SkillEntityInfo skillEntityInfo,bool isDeduct = false)
    {
        skillEntityInfo.controlCells = new List<CellInfo>();
        if (skillEntityInfo.skill_x == skillEntityInfo.cell.posX && skillEntityInfo.skill_y == skillEntityInfo.cell.posY)
        {
            skillEntityInfo.controlCells.Add(skillEntityInfo.cell);
        }
        RecursiveControl(skillEntityInfo, skillEntityInfo.controlCells);

        if (isDeduct == false)
        {
            for (int n = (CellModel.Instance.lineCells.Count - 1); n >= 0; n--)
            {
                CellInfo checkCell = CellModel.Instance.lineCells[n];
                SkillEntityInfo checkEntityInfo = GetSkillEntityByCell(checkCell);
                if (checkEntityInfo != null && checkEntityInfo != skillEntityInfo)
                {
                    int checkEntityType = checkEntityInfo.seed.config_cell_item.cell_type;
                    if (checkEntityType == (int)CellType.bomb || checkEntityType == (int)CellType.line_bomb || checkEntityType == (int)CellType.line_bomb_r 
                        || checkEntityType == (int)CellType.three_bomb || checkEntityType == (int)CellType.three_bomb_r)
                    {
                        if (checkEntityType == (int)CellType.line_bomb || checkEntityType == (int)CellType.line_bomb_r
                        || checkEntityType == (int)CellType.three_bomb || checkEntityType == (int)CellType.three_bomb_r)
                        {
                            skillEntityInfo.controlCells.Add(checkEntityInfo.cell);
                        }
                        
                        RecursiveControl(checkEntityInfo, skillEntityInfo.controlCells);
                    }
                }
            }
        }
    }

    private void RecursiveControl(SkillEntityInfo skillEntityInfo, List<CellInfo> controlCells)
    {
        bool isHump = skillEntityInfo.cell.IsHump(skillEntityInfo.skill_x);

		if(skillEntityInfo.seed.open_holes == null)
		{
			if(skillEntityInfo.cell.config.cell_type == (int)CellType.line_bomb || skillEntityInfo.cell.config.cell_type == (int)CellType.line_bomb_r)
			{
				int rotate = skillEntityInfo.cell.config.rotate;
				if(skillEntityInfo.cell.rotate >= 0)
				{
					rotate = skillEntityInfo.cell.rotate;
				}

				List<CellInfo> dir1Cells = CellModel.Instance.GetDirCells(skillEntityInfo.cell,(CellDirType)PosUtil.GetDirWithRotate(CellDirType.up,rotate),3);
				List<CellInfo> dir2Cells = CellModel.Instance.GetDirCells(skillEntityInfo.cell,(CellDirType)PosUtil.GetDirWithRotate(CellDirType.down,rotate),3);
				dir1Cells.InsertRange(dir1Cells.Count,dir2Cells);
				for (int i = 0; i < dir1Cells.Count; i++)
				{
					CellInfo getCell = dir1Cells[i];
                    AddControl(getCell, skillEntityInfo, controlCells);
				}
			}

			if(skillEntityInfo.cell.config.cell_type == (int)CellType.three_bomb || skillEntityInfo.cell.config.cell_type == (int)CellType.three_bomb_r)
			{
				int rotate = skillEntityInfo.cell.config.rotate;
				if(skillEntityInfo.cell.rotate >= 0)
				{
					rotate = skillEntityInfo.cell.rotate;
				}
				List<CellInfo> dir1Cells = CellModel.Instance.GetDirCells(skillEntityInfo.cell,(CellDirType)PosUtil.GetDirWithRotate(CellDirType.left_up,rotate),2);
				List<CellInfo> dir2Cells = CellModel.Instance.GetDirCells(skillEntityInfo.cell,(CellDirType)PosUtil.GetDirWithRotate(CellDirType.down,rotate),2);
				List<CellInfo> dir3Cells = CellModel.Instance.GetDirCells(skillEntityInfo.cell,(CellDirType)PosUtil.GetDirWithRotate(CellDirType.right_up, rotate),2);
				dir1Cells.InsertRange(dir1Cells.Count,dir2Cells);
				dir1Cells.InsertRange(dir1Cells.Count,dir3Cells);
				for (int i = 0; i < dir1Cells.Count; i++)
				{
					CellInfo getCell = dir1Cells[i];
                    AddControl(getCell, skillEntityInfo, controlCells);
				}
			}

		}else
		{
			for (int i = 0; i < skillEntityInfo.seed.open_holes.Count; i++)
			{
				int holeIndex = skillEntityInfo.seed.open_holes[i];
				Vector2 holePos = SkillTempletModel.Instance.GetHolePos (holeIndex + 1, isHump, skillEntityInfo.seed.dir);
				int getx = (int)(skillEntityInfo.skill_x + holePos.x);
				int gety = (int)(skillEntityInfo.skill_y - holePos.y);
				CellInfo getCell = CellModel.Instance.GetCellByPos(getx, gety);
                AddControl(getCell, skillEntityInfo, controlCells);
			}
		}
    }

    private void AddControl(CellInfo getCell, SkillEntityInfo skillEntityInfo, List<CellInfo> controlCells)
	{
		if(getCell != null && controlCells.Contains(getCell) == false)
		{
            controlCells.Add(getCell);

            if (getCell.isBlank == false && 
                (skillEntityInfo.cell.config.cell_type == (int)CellType.bomb || skillEntityInfo.cell.config.cell_type == (int)CellType.line_bomb ||
                skillEntityInfo.cell.config.cell_type == (int)CellType.line_bomb_r || skillEntityInfo.cell.config.cell_type == (int)CellType.three_bomb ||
                skillEntityInfo.cell.config.cell_type == (int)CellType.three_bomb_r) &&
                (getCell.config.cell_type == (int)CellType.bomb || getCell.config.cell_type == (int)CellType.line_bomb ||
                getCell.config.cell_type == (int)CellType.line_bomb_r || getCell.config.cell_type == (int)CellType.three_bomb ||
                getCell.config.cell_type == (int)CellType.three_bomb_r))
            {
                if (!CoverModel.Instance.StopSkill(getCell.posX, getCell.posY) && !MonsterModel.Instance.StopSkill(getCell.posX, getCell.posY))
                {
                    SkillEntityInfo bombEntity = GetSkillEntityByCell(getCell);
                    if (bombEntity.runId != crt_entity.runId)
                    {
						if(bombEntity.cell.isLink == false)
						{
							bombEntity.skill_x = getCell.posX;
							bombEntity.skill_y = getCell.posY;
						}
                        
                        RecursiveControl(bombEntity, controlCells);
                    }
                }
            }
		}
	}

	private void SkillEffect(SkillEntityInfo skillEntityInfo)
	{
		bool isBomb = (skillEntityInfo.seed.config_cell_item.cell_type == (int)CellType.bomb 
			|| skillEntityInfo.seed.config_cell_item.cell_type == (int)CellType.line_bomb_r
			|| skillEntityInfo.seed.config_cell_item.cell_type == (int)CellType.three_bomb_r);

		bool isRadiate = (skillEntityInfo.seed.config_cell_item.cell_type == (int)CellType.radiate);

		for (int i = 0; i < skillEntityInfo.controlCells.Count; i++)
		{
			CellInfo controlCell = skillEntityInfo.controlCells[i];

			if (isBomb)
			{
                controlCell.bombMark = true;
			}

            if (isRadiate && !CoverModel.Instance.StopSkill(controlCell.posX, controlCell.posY) && !MonsterModel.Instance.StopSkill(controlCell.posX, controlCell.posY))
			{
				if (controlCell.isBlank == false && controlCell.config.cell_type == (int)CellType.five)
				{
					controlCell.SetConfig(skillEntityInfo.seed.config_cell_item.hide_id);
				}
			}
		}
	}

	public List<CellInfo> EnterCell(CellInfo cellInfo)
	{
		SkillEntityInfo skillEntityInfo = GetSkillEntityByCell(cellInfo);

		if (skillEntityInfo == null)
		{
			if (crt_entity != null)
			{
				if(crt_entity.seed.config_cell_item.cell_type == (int)CellType.bomb)
				{
					//改变炸弹中心
					crt_entity.skill_x = cellInfo.posX;
					crt_entity.skill_y = cellInfo.posY;
				}
			}
			else
			{
				for (int i = (CellModel.Instance.lineCells.Count - 1); i >= 0 ;i-- )
				{
					CellInfo checkCell = CellModel.Instance.lineCells[i];
					SkillEntityInfo checkEntityInfo = GetSkillEntityByCell(checkCell);
					if (checkEntityInfo != null)
					{
						int checkEntityType = checkEntityInfo.seed.config_cell_item.cell_type;
						if (checkEntityType == (int)CellType.bomb)
						{
							crt_entity = checkEntityInfo;
							crt_entity.skill_x = cellInfo.posX;
							crt_entity.skill_y = cellInfo.posY;
							break;
						}
						if (checkEntityType == (int)CellType.line_bomb_r || checkEntityType == (int)CellType.three_bomb_r)
						{
							crt_entity = checkEntityInfo;
							crt_entity.skill_x = checkCell.posX;
							crt_entity.skill_y = checkCell.posY;
							break;
						}
					}
				}
			}
		}
		else
		{
            if (crt_entity != null)
            {
                PromptModel.Instance.Pop("");
            }
            crt_entity = skillEntityInfo;
            crt_entity.fightingType = SkillFightingType.active;
            crt_entity.skill_x = cellInfo.posX;
            crt_entity.skill_y = cellInfo.posY;
        }

		if (crt_entity == null)
		{
			return new List<CellInfo>();
		}
		else
		{
			UpdateControlCells(crt_entity);
			SkillEffect(crt_entity);
			return crt_entity.controlCells;
		}
	}

    public List<CellInfo> QuitCell(CellInfo cellInfo)
    {
        List<CellInfo> controlCells = new List<CellInfo>();
        SkillEntityInfo skillEntityInfo = GetSkillEntityByCell(cellInfo);

        if (skillEntityInfo != null)
        {
            skillEntityInfo.fightingType = SkillFightingType.idel;
            for (int i = 0; i < skillEntityInfo.controlCells.Count; i++)
            {
                CellInfo controlCell = skillEntityInfo.controlCells[i];
                if (skillEntityInfo.seed.config_cell_item.cell_type == (int)CellType.radiate)
                {
					if (controlCell.isBlank == false && controlCell.config.cell_type == (int)CellType.five 
                        && !CoverModel.Instance.StopSkill(controlCell.posX, controlCell.posY) && !MonsterModel.Instance.StopSkill(controlCell.posX, controlCell.posY))
                    {
                        controlCell.SetConfig(controlCell.originalConfigId);
                    }
                }
                controlCell.bombMark = false;
                controlCells.Add(controlCell);
            }
        }
        
        return controlCells;
    }

	public List<CellInfo> OutCell(CellInfo cellInfo)
	{
		List<CellInfo> controlCells = new List<CellInfo>();

		if (cellInfo.isLink == false)//退出技能格
		{
			if (crt_entity != null)
			{
				int crtentitytype = crt_entity.seed.config_cell_item.cell_type;
				if (crt_entity.cell.runId == cellInfo.runId)
				{
					crt_entity.fightingType = SkillFightingType.idel;
					if (crtentitytype == (int)CellType.radiate)
					{
						for (int i = 0; i < crt_entity.controlCells.Count; i++)
						{
							CellInfo controlCell = crt_entity.controlCells[i];
							if(controlCell.isBlank == false)
							{
								bool occupy = false;
								for (int n = (CellModel.Instance.lineCells.Count - 1); n >= 0 ;n-- )
								{
									CellInfo checkCell = CellModel.Instance.lineCells[n];
									SkillEntityInfo checkEntityInfo = GetSkillEntityByCell(checkCell);
									if (checkEntityInfo != null)
									{
										int checkEntityType = checkEntityInfo.seed.config_cell_item.cell_type;
										if (checkEntityType == (int)CellType.radiate)
										{
											bool have = checkEntityInfo.controlCells.Contains(controlCell);
											if(have)
											{
												occupy = true;
												break;
											}
										}
									}
								}
								if(occupy == false)
								{
									controlCell.SetConfig(controlCell.originalConfigId);
								}
							}

							controlCells.Add(controlCell);
						}
						crt_entity = null;
					}
					else if (crtentitytype == (int)CellType.bomb || crtentitytype == (int)CellType.line_bomb_r || crtentitytype == (int)CellType.three_bomb_r)
					{
						for (int i = 0; i < crt_entity.controlCells.Count; i++)
						{
							CellInfo controlCell = crt_entity.controlCells[i];
							controlCell.bombMark = false;
							controlCells.Add(controlCell);
						}
						crt_entity = null;
					}
				}
			}
		}

		if (crt_entity != null)
		{
			int crtentitytype = crt_entity.seed.config_cell_item.cell_type;
			if (crtentitytype == (int)CellType.bomb || crtentitytype == (int)CellType.line_bomb_r || crtentitytype == (int)CellType.three_bomb_r)
			{
				for (int i = 0; i < crt_entity.controlCells.Count; i++)
				{
					CellInfo controlCell = crt_entity.controlCells[i];
					controlCell.bombMark = false;
                    
					CellInfo find = controlCells.Find(
                        //使用匿名函数
                        delegate (CellInfo cell)
						{
							return cell.runId == controlCell.runId;
						});

					if (find == null)
					{
						controlCells.Add(controlCell);
					}
				}
			}
		}

		return controlCells;
	}

    public void SkillEnd()
    {
        for (int i = fighting_entitys.Count - 1; i >= 0; i--)
        {
            SkillEntityInfo skill = fighting_entitys[i];
            if (skill.fightingType == SkillFightingType.active)
            {
                fighting_entitys.RemoveAt(i);
            }
            else {

                if (skill.cell.isBlank)
                {
                    fighting_entitys.RemoveAt(i);
                }
            }
        }
        crt_entity = null;
    }

    public List<CellInfo> DeductSkill()
    {
        SkillEnd();
        if (fighting_entitys.Count > 0)
        {
            fighting_entitys.Sort();
            SkillEntityInfo skill = fighting_entitys[0];
            fighting_entitys.RemoveAt(0);
            crt_entity = skill;
            skill.skill_x = skill.cell.posX;
            skill.skill_y = skill.cell.posY;
            UpdateControlCells(skill,true);
            return skill.controlCells;
        }
        return new List<CellInfo>();
    }

}