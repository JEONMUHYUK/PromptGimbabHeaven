using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    internal class CalculatorDay
    {
        #region Feild
        public int dayCount { get; private set; }       // 60초 카운트 변수
        public int day { get; private set; }            // 날짜 변수
        #endregion

        public void Start()
        {
            // 초기화
            dayCount = 0;
            day = 1;
        }
        public void Update()
        {
            // dayCount 59를 넘어가면 0으로 초기화 day += 1
            if (dayCount >= 59)
            {
                dayCount = 0;
                Console.Clear();
                day++;
            }
            else dayCount++;

        }

    }
}
