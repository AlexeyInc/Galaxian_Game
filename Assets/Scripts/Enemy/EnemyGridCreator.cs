using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGrid
{ 
    public enum EnemyType
    {
        None, Standard, Boss
    }

    public struct Enemy
    {
        public float Row;
        public float Col;

        public EnemyType EnemyType;

        public Enemy(float row, float col, EnemyType enemyType)
        {
            Row = row;
            Col = col;
            EnemyType = enemyType;
        }
    }

    public class EnemyGridCreator
    {
        delegate void SetupLevels();

        SetupLevels[] _enemyPosOnLevels;

        private int _rowCount;
        private int _colCount;

        public int GridRows { get { return _rowCount; } }
        public int GridColumns { get { return _colCount; } }

        private Enemy[,] _enemyGrid;

        public EnemyGridCreator(int rowsCount, int columsCount)
        {
            _rowCount = rowsCount;
            _colCount = columsCount;

            _enemyGrid = new Enemy[_rowCount, _colCount];

            _enemyPosOnLevels = new SetupLevels[2];
            _enemyPosOnLevels[0] = SetupEnemyLvl_One;
            _enemyPosOnLevels[1] = SetupEnemyLvl_Two;
        }

        public void InitLevel(int numLevel)
        {
            SetupDefaultCells();

            if (numLevel <= _enemyPosOnLevels.Length)
                _enemyPosOnLevels[numLevel - 1]();

            SetupBoss(2);
        }

        void SetupDefaultCells()
        {
            float row = 0, col = 0;
            for (int r = 0; r < _rowCount; r++, row+=2.3f)
            {
                for (int c = 0; c < _colCount; c++, col+= 2.3f)
                {
                    _enemyGrid[r, c] = new Enemy(row, col, EnemyType.None);
                }
                col = 0;
            }
        }

        private void SetupEnemyLvl_One()
        {
            int value = 0;

            for (int row = 0; row < _rowCount - 1; row++)
            {
                for (int col = (_colCount - 1) - value; col >= 0 + value; col--)
                {
                    _enemyGrid[row, col].EnemyType = EnemyType.Standard;
                }
                value++;
            }
        }

        private void SetupEnemyLvl_Two()
        {
            for (int row = 2; row < _rowCount - 1; row++)
            {
                for (int col = 0; col < _colCount; col ++)
                {
                    _enemyGrid[row, col].EnemyType = EnemyType.Standard;
                }
            }
        }

        private void SetupBoss(int bossCount)
        {
            int bossColPos = _colCount / (bossCount + 1);

            for (int count = 1; count <= bossCount; count++)
            {
                _enemyGrid[_rowCount - 1, bossColPos].EnemyType = EnemyType.Boss;
                bossColPos += bossColPos;
            }
        }

        public Enemy this[int row, int col]
        {
            get
            {
                try
                {
                    return _enemyGrid[row, col];
                }
                catch (System.IndexOutOfRangeException)
                {
                    Debug.Log("Выход за границы массива");
                    return new Enemy(0,0,EnemyType.None);
                }
            }
        }
    }

}
