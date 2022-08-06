using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    internal class GameLoop
    {
        #region Field
        // 객체 선언
        // 맵 크기 상수 선언
        public const int mapSizeY = 30;
        public const int mapSizeX = 71;

        // 객체 선언
        CalculatorDay calculatorDay;
        Table table;
        Consumer consumer;
        GameOver gameOver;
        StartScene startScene;


        int limitDay;               // 마지막 날짜 변수
        int numberoftables;         // 테이블 수 변수
        int limitConsumer;          // 웨이팅 손님 제한 변수

        int totalSale;              // 총 판매금액
        int totalConsumer;          // 총 손님 수

        int lastTick;               // fps 관리 변수
        #endregion

        // isGameOver 체크 변수
        public bool isGameOver { get; private set; }


        public void Awake()
        {
            // 객체 생성
            calculatorDay = new CalculatorDay();
            table = new Table();
            consumer = new Consumer();
            gameOver = new GameOver();
            startScene = new StartScene();


            table.Awake();

        }
        public void Start()
        {
            // 콘솔창 세팅값
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = mapSizeX;
            Console.BufferHeight = Console.WindowHeight = mapSizeY;

            // 변수 초기화
            limitDay = 0;
            numberoftables = 0;
            limitConsumer = 0;
            totalSale = 0;
            totalConsumer = 0;

            // 총 일, 테이블 개수, 기다리는 손님 수를 인풋값으로 받아오는 함수.
            InputSettingValue();

            // 클래스 초기화
            table.Start(numberoftables, consumer);
            calculatorDay.Start();
            consumer.Start(limitConsumer);
        }
        public void Update()
        {
            int currentTick = Environment.TickCount;
            if (currentTick - lastTick > 1000)
            {
                // 객체 업데이트
                consumer.Update();
                calculatorDay.Update();
                Console.Clear();
                table.Update();

                // dayCount가 초기화 될 때 마다 total값에 합산한다.
                if (calculatorDay.dayCount == 0)
                { 
                    totalSale += table.todaySales;
                    totalConsumer += table.todayConsumer;
                    table.todayConsumer = 0;
                    table.todaySales = 0;
                }

                lastTick = currentTick;
            }

            // 게임 끝 조건
            if (calculatorDay.day > limitDay)
                isGameOver = true;
        }
        public void Rander()
        {
            Console.SetCursorPosition(0, 2);
            Console.Write("                         ");
            Console.WriteLine("Welcome to Kimbab Heaven");
            Console.SetCursorPosition(0, 5);
            Console.Write("                          ");
            Console.WriteLine($"날짜: {calculatorDay.day}일차   시간 :  {calculatorDay.dayCount}");
            Console.WriteLine();
            Console.Write("        ");
            Console.WriteLine($"매출(금 일 / 누적) : {table.todaySales} / {totalSale}   손님 수(금 일 / 누적) : {table.todayConsumer} / {totalConsumer}");
            Console.WriteLine();
            Console.Write("                           ");
            Console.WriteLine($"기다리는 손님 수 : {consumer.consumerOfWaiting}");
            Console.WriteLine();

            table.Render();
        }

        // 처음 받는 입력값
        public void InputSettingValue()
        {
            Console.WriteLine(" 총 일, 테이블 개수, 기다리는 손님 수를 입력하세요 ");
            Console.Write(">> ");
            string[] ask = Console.ReadLine().Split(' ');

            limitDay = int.Parse(ask[0]);
            numberoftables = int.Parse(ask[1]);
            limitConsumer = int.Parse(ask[2]);
            Console.Clear();

        }

        public void GameOver()
        {
            gameOver.Render(limitDay, totalConsumer, totalSale);
        }

        public void GameStart()
        {
            startScene.Start();
        }
    }
}
