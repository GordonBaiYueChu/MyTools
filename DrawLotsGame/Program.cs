using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DrawLotsGame
{
    internal class Program
    {
        /// <summary>
        /// 总扑克
        /// </summary>
        private static List<int> _allPokers = new List<int>();

        /// <summary>
        /// 是否是玩家一
        /// </summary>
        private static bool _isFirstMan = true;

        static void Main(string[] args)
        {
            Console.WriteLine("每人可以选择同行的一张或多张扑克牌，拿最后一个扑克牌的人失败");
            Console.WriteLine("选择第一行第一个扑克牌可输入: 11");
            Console.WriteLine("选择第二行第一个和第二个扑克牌可输入（逗号获取空格隔开）: 21,22 或者 21 22");
            while (true)
            {
                RefreshData();

                while (_allPokers.Count > 0)
                {
                    ShowCanSelectPokers();
                    List<int> selectPokers = GetManInput();
                    foreach (int select in selectPokers)
                    {
                        _allPokers.Remove(select);
                    }
                    if (IsEnd())
                    {
                        break;
                    }
                    _isFirstMan = !_isFirstMan;
                }

                Console.WriteLine("输入c再来一局，输入其他退出");
                if (!Console.ReadLine().ToLower().Equals("c"))
                {
                    return;
                }

            }
        }

        /// <summary>
        /// 显示目前可选poker
        /// </summary>
        public static void ShowCanSelectPokers()
        {
            Console.WriteLine("当前可选扑克编号:*==================================*");
            foreach (int poker in _allPokers)
            {
                Console.Write(poker + " ");
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// 刷新数据源
        /// </summary>
        public static void RefreshData()
        {
            _allPokers = new List<int>();
            for (int i = 1; i < 4; i++)
            {
                _allPokers.Add(i + 10);
            }
            for (int i = 1; i < 6; i++)
            {
                _allPokers.Add(i + 20);
            }
            for (int i = 1; i < 8; i++)
            {
                _allPokers.Add(i + 30);
            }
        }

        /// <summary>
        /// 获取玩家选择
        /// </summary>
        public static List<int> GetManInput()
        {
            List<int> selectList = new List<int>();
            if (_isFirstMan)
            {
                Console.WriteLine("请玩家一选择：");
            }
            else
            {
                Console.WriteLine("请玩家二选择：");
            }
            List<string> inputs = new List<string>();
            do
            {
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("输入为空或空格请重新输入：");
                    continue;
                }
                inputs = input.Split(new char[3] { '，', ',',' ' }).ToList();
                if (!IsWholeNumber(inputs, ref selectList) || !IsInputSameRow(inputs) || !IsWithinScope(selectList))
                {
                    selectList = new List<int>();
                    continue;
                }
                else
                {

                    break;
                }
            }
            while (true);
            return selectList;
        }

        /// <summary>
        /// 输入是否在范围内
        /// </summary>
        /// <returns></returns>
        private static bool IsWithinScope(List<int> selectList)
        {
            foreach (int select in selectList)
            {
                if (!_allPokers.Contains(select))
                {
                    Console.WriteLine("输入数字围在可用范围内，请重新输入：");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 数字匹配
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static bool IsWholeNumber(List<string> inputs, ref List<int> selectList)
        {
            Regex g = new Regex(@"^[0-9]\d*$");
            foreach (string strNumber in inputs)
            {
                if (string.IsNullOrWhiteSpace(strNumber))
                {
                    continue;
                }
                else if (!g.IsMatch(strNumber))
                {
                    Console.WriteLine("输入非数字字符，请重新输入：");
                    return false;
                }
                selectList.Add(Convert.ToInt32(strNumber));
            }
            return true;
        }

        /// <summary>
        /// 是否是同一行的数据
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static bool IsInputSameRow(List<string> inputs)
        {
            string firstNumber = string.Empty;
            foreach (string input in inputs)
            {
                if (string.IsNullOrEmpty(firstNumber))
                {
                    firstNumber = input.Substring(0, 1);
                }
                else if (!input.StartsWith(firstNumber))
                {
                    Console.WriteLine("输入非同行数字，请重新输入：");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <returns></returns>
        private static bool IsEnd()
        {
            if (_allPokers.Count == 1)
            {
                if (_isFirstMan)
                {
                    Console.WriteLine("玩家一胜利，玩家二失败");
                }
                else
                {
                    Console.WriteLine("玩家二胜利，玩家一失败");
                }
                return true;
            }
            else if (_allPokers.Count == 0)
            {
                if (_isFirstMan)
                {
                    Console.WriteLine("玩家二胜利，玩家一失败");
                }
                else
                {
                    Console.WriteLine("玩家一胜利，玩家二失败");
                }
                return true;
            }
            return false;
        }
    }
}
