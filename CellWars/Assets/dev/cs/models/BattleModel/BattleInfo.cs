using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfo
{
    public int mapId;
    
    //view rect
    public short start_x;
    public short start_y;
    public short end_x;
    public short end_y;
    //all size
    public short battle_width;
    public short battle_height;
	//战斗格子
    public List<List<BattleCellInfo>> allCells = new List<List<BattleCellInfo>>();
	//前景
    public List<int> fgIds = new List<int>();

    public bool need_open_fill = false;

    public bool clearCaching = false;

	public void FillNew(short set_start_x,short set_start_y,short set_end_x,short set_end_y,short set_battle_width,short set_battle_height)
    {
		allCells = new List<List<BattleCellInfo>> ();
		fgIds = new List<int> ();

		start_x = set_start_x;
		start_y = set_start_y;
		end_x = set_end_x;
		end_y = set_end_y;
		battle_width = set_battle_width;
		battle_height = set_battle_height;

        for (int i = 0; i < battle_height; i++)
        {
            List<BattleCellInfo> xcells = new List<BattleCellInfo>();
            allCells.Add(xcells);
            for (int j = 0; j < battle_width; j++)
            {
                BattleCellInfo battleCellInfo = new BattleCellInfo();
                xcells.Add(battleCellInfo);

                battleCellInfo.bg_id = 10202;
				battleCellInfo.monster_id = 0;
                battleCellInfo.floor_id = 0;
				battleCellInfo.cell_id = UnityEngine.Random.Range(10101, 10101 + 5);
                battleCellInfo.cover_id = 0;

                battleCellInfo.walls.Add(0);
                battleCellInfo.walls.Add(0);
                battleCellInfo.walls.Add(0);
            }
        }

        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
                if (i <= start_y && i >= end_y && j >= start_x && j <= end_x)
				{
					fgIds.Add(0);
				}else
				{
					fgIds.Add(10302);
				}
			}
		}
    }

    public void FillByte(byte[] bytes)
    {
        readIndex = 0;
        allCells = new List<List<BattleCellInfo>>();
        fgIds = new List<int>();

        start_x = ReadShort(bytes);
        start_y = ReadShort(bytes);
        end_x = ReadShort(bytes);
        end_y = ReadShort(bytes);
        battle_width = ReadShort(bytes);
        battle_height = ReadShort(bytes);

        if (HideHeight() > 0)
        {
            clearCaching = true;
        }

        for (int i = 0; i < battle_height; i++)
        {
            List<BattleCellInfo> xcells = new List<BattleCellInfo>();
            allCells.Add(xcells);
            for (int j = 0; j < battle_width; j++)
            {
                BattleCellInfo battleCellInfo = new BattleCellInfo();
                xcells.Add(battleCellInfo);

                battleCellInfo.bg_id = ReadInt(bytes);
                battleCellInfo.monster_id = ReadInt(bytes);
                battleCellInfo.floor_id = ReadInt(bytes);
                battleCellInfo.cell_id = ReadInt(bytes);
                battleCellInfo.cover_id = ReadInt(bytes);
                battleCellInfo.walls.Add(ReadInt(bytes));
                battleCellInfo.walls.Add(ReadInt(bytes));
                battleCellInfo.walls.Add(ReadInt(bytes));
            }
        }

        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
        {
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
            {
                fgIds.Add(ReadInt(bytes));
            }
        }
    }

	public void SaveBattleInfo()
    {
        string filePath = FileUtil.Instance().FullName("dat/map/" + mapId);
        File.Delete(filePath + ".bytes");

        FileStream fs = new FileStream(filePath + ".bytes", FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(start_x);
        bw.Write(start_y);
        bw.Write(end_x);
        bw.Write(end_y);
        bw.Write(battle_width);
        bw.Write(battle_height);

        for (int i = 0; i < battle_height; i++)
        {
            List<BattleCellInfo> xcells = allCells[i];
            for (int j = 0; j < battle_width; j++)
            {
                BattleCellInfo battleCellInfo = xcells[j];

                bw.Write(battleCellInfo.bg_id);
				bw.Write(battleCellInfo.monster_id);
                bw.Write(battleCellInfo.floor_id);
                bw.Write(battleCellInfo.cell_id);
                bw.Write(battleCellInfo.cover_id);

                bw.Write(battleCellInfo.walls[0]);
                bw.Write(battleCellInfo.walls[1]);
                bw.Write(battleCellInfo.walls[2]);
            }
        }

		int index = 0;
        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
				bw.Write(fgIds[index]);
				index ++;
			}
		}

        bw.Flush();
        bw.Close();
        fs.Close();
		Debug.Log ( mapId + " save complete");
        
    }

    private int readIndex = 0;
    private int ReadInt(byte[] bytes)
    {
        int v = BitConverter.ToInt32(bytes, readIndex);
        readIndex += 4;
        return v;
    }

    private short ReadShort(byte[] bytes)
    {
        short v = BitConverter.ToInt16(bytes, readIndex);
        readIndex += 2;
        return v;
    }

	public bool InViewRect(int posx,int posy)
	{
        if (posy <= start_y && posy >= end_y && posx >= start_x && posx <= end_x)
		{
			return true;
		}else
		{
			return false;
		}
	}

	public int HideHeight()
	{
        return battle_height - (start_y - end_y + 1);
	}

    public int ShowHeight()
    {
        return start_y - end_y + 1;
    }

    public int ShowWidth()
    {
        return end_x - start_x + 1;
    }

	public int ShowOffsetHeight()
	{
		return start_y + end_y;
	}
	
	public int ShowOffsetWidth()
	{
		return start_x + end_x;
	}

    public void RollCut(int cutNum)
    {
        battle_height -= (short)cutNum;
        for (int i = 0; i < cutNum;i++ )
        {
            allCells.RemoveAt(i);
        }
    }

    public BattleCellInfo GetBattleCellByPos(int posX, int posY)
    {
        if (posX < battle_width && posX >= 0 && posY < battle_height && posY >= 0)
        {
            return allCells[posY][posX];
        }
        return null;
    }

    public List<BattleCellInfo> GetNeighbors(int posX, int posY)
    {
        List<BattleCellInfo> neighbors = new List<BattleCellInfo>();
        neighbors.Add(GetBattleCellByPos(posX - 1, posY));
        neighbors.Add(GetBattleCellByPos(posX + 1, posY));
        neighbors.Add(GetBattleCellByPos(posX, posY + 1));
        neighbors.Add(GetBattleCellByPos(posX, posY - 1));
        if (IsHump(posX))
        {
            neighbors.Add(GetBattleCellByPos(posX - 1, posY - 1));
            neighbors.Add(GetBattleCellByPos(posX + 1, posY - 1));
        }
        else
        {
            neighbors.Add(GetBattleCellByPos(posX - 1, posY + 1));
            neighbors.Add(GetBattleCellByPos(posX + 1, posY + 1));
        }
        return neighbors;
    }

    public bool IsHump(int pos)
    {
        return (pos - CellInfo.start_x) % 2 != 0;
    }

}