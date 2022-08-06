using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    class PriceAndTime
    {
        // 테이블에 가격과 시간을 담을 클래스.
        public int price;
        public int time;

        public PriceAndTime(int price, int time) { this.price = price; this.time = time; }
        
    }
    internal class Table
    {
        #region Feild
        Consumer    consumer;           // 손님 객체

        int         numberoftables;     // 테이블 수
        int[]       time;               // 시간 배열
        string[]    states;             // 상태 배열

        public int todaySales { get; set; }         // 금일 판매금액

        public int todayConsumer { get; set; }      // 금일 고객 수

        

        // 메뉴의 가격과 시간을 담기위한 딕셔너리
        Dictionary<int, PriceAndTime> MenuList;

        // 랜덤한 메뉴를 담기위한 리스트
        List<int> randomMenuList;

        Random random;

        #endregion

        // 열거형 상태 타입
        public enum StateType
        { 
            Empty,
            Clean,
            WaitOrder,
            WaitFood,
            AtTable,
        }

        // 열거형 음식 타입
        public enum FoodType
        {
            Kimbab = 0,
            VegitableKimbab,
            TunaKimbab,
            BeefKimbab,
            Ramen,
            Rabokki,
            BeefBowl,
            SoybeanPasteStew,
            KimchiStew,
        }


        public void Awake()
        {
            // 메뉴 딕셔녀리 객체 생성및 초기화
            MenuList = new Dictionary<int, PriceAndTime> 
            {
                { 0, new PriceAndTime(1000, 3) },          // 앞에는 키값이 될 int형 value
                { 1, new PriceAndTime(2500, 3) },          // 뒤에는 value 값이 될 가격과 나오는 시간을 담은 객체
                { 2, new PriceAndTime(2000, 4) },
                { 3, new PriceAndTime(2500, 4) },
                { 4, new PriceAndTime(2000, 10) },
                { 5, new PriceAndTime(3000, 11) },
                { 6, new PriceAndTime(4000, 11) },
                { 7, new PriceAndTime(3500, 12) },
                { 8, new PriceAndTime(3500, 12) }
            };

            random = new Random();              
            randomMenuList = new List<int>();               // 랜덤한 메뉴를 담기위한 리스트
        }
        public void Start(int numberoftables, Consumer consumer)
        {
            this.numberoftables = numberoftables;
            this.consumer = consumer;


            states = new string[numberoftables];
            time = new int[numberoftables];

            // 테이블 상태 배열 초기화.
            for (int i = 0; i < this.numberoftables; i++) states[i] = TableState(StateType.Empty);

            // 랜덤메뉴 리스트 값 초기화
            for (int i = 0; i < numberoftables; i++)
            {
                randomMenuList.Add(random.Next(0, 9));
            }

            // 총 판매 고객수 초기화
            todaySales = 0;
            todayConsumer = 0;


        }
        public void Update()
        {
            for (int i = 0; i < numberoftables; i++)
            {
                // 랜덤메뉴리스트가 10인 곳은 비어있다는 의미, 그 부분은 다시 랜덤 값으로 채워 넣는다.
                if (randomMenuList[i] == 10)  randomMenuList[i] = random.Next(0, 9);

                #region StateUpdate
                // 타임 시간에 따른 테이블 상태 변화  값
                if (time[i] > 3 && time[i] < 6 && states[i] == TableState(StateType.Empty) )
                {
                    states[i] = TableState(StateType.WaitOrder);
                    // 웨이팅 손님이 0이거나 음수면 리턴 아니라면 웨이팅 손님 값을 -1 하고 todayConsumer값을 +1
                    if (consumer.consumerOfWaiting - 1 <= 0) return;
                    else
                    { 
                        consumer.consumerOfWaiting -= 1;
                        todayConsumer += 1;
                    }
                }
                if (time[i] > 6 && time[i] < MenuList[randomMenuList[i]].time + 6 && states[i] == TableState(StateType.WaitOrder))
                {
                    states[i] = TableState(StateType.WaitFood);
                    todaySales += MenuList[randomMenuList[i]].price;
                }
                if (time[i] > MenuList[randomMenuList[i]].time + 6 && states[i] == TableState(StateType.WaitFood))
                {
                    states[i] = TableState(StateType.AtTable);
                }
                if (time[i] > MenuList[randomMenuList[i]].time + 6 + 3 && states[i] == TableState(StateType.AtTable))
                {
                    states[i] = TableState(StateType.Empty);
                }
                if (time[i] > MenuList[randomMenuList[i]].time + 6 + 4 && states[i] == TableState(StateType.Empty))
                {
                    states[i] = TableState(StateType.Clean);
                }
                if (time[i] > MenuList[randomMenuList[i]].time + 6 + 5 && states[i] == TableState(StateType.Clean))
                {
                    time[i] = 0;
                    states[i] = TableState(StateType.Empty);
                    randomMenuList[i] = 10;
                }
                #endregion

            }


            // 테이블 수 만큼 시간계산을 따로 한다.
            for (int i = 0; i < numberoftables; i++)
            {
                time[i]++;
            }
            
        }

        public void Render()
        {

            for (int i = 0; i < numberoftables; i++)
            {


                Console.Write("         ");

                // 상태가 상태타입 AtTable과 같다면 테이블 상태와 메뉴를 출력
                if (states[i] == TableState(StateType.WaitOrder))
                    Console.WriteLine($"{i}번 테이블 = {states[i]} / 주문대기중");
                else if (states[i] == TableState(StateType.WaitFood))
                    Console.WriteLine($"{i}번 테이블 = {states[i]} / {(FoodType)randomMenuList[i]} 대기중");
                else if (states[i] == TableState(StateType.AtTable))
                    Console.WriteLine($"{i}번 테이블 = {states[i]} / {(FoodType)randomMenuList[i]}");
                else Console.WriteLine($"{i}번 테이블 = {states[i]}");

                Console.WriteLine();

            }
            Console.Write("김밥 : ");
            for (int i = 0; i < numberoftables; i++)
            {
                if ((randomMenuList[i] == (int)FoodType.Kimbab || randomMenuList[i] == (int)FoodType.TunaKimbab ||
                    randomMenuList[i] == (int)FoodType.VegitableKimbab || randomMenuList[i] == (int)FoodType.BeefKimbab)
                    && states[i] == TableState(StateType.WaitOrder))
                    Console.Write($" {(FoodType)randomMenuList[i]} 요리 중");
                else
                { 
                    if (i == 1) Console.Write("rest");
                }
            }

            Console.WriteLine();
            Console.Write("주방 : ");
            for (int i = 0; i < numberoftables; i++)
            {
                if ((randomMenuList[i] == (int)FoodType.Ramen || randomMenuList[i] == (int)FoodType.BeefBowl ||
                randomMenuList[i] == (int)FoodType.KimchiStew || randomMenuList[i] == (int)FoodType.Rabokki
                || randomMenuList[i] == (int)FoodType.SoybeanPasteStew) && states[i] == TableState(StateType.WaitOrder))
                    Console.Write($" {(FoodType)randomMenuList[i]} 요리 중");
                else
                {
                    if (i == 1) Console.Write("rest");
                }
            }
            Console.WriteLine();
            Console.Write("서빙 : ");
            for (int i = 0; i < numberoftables; i++)
            {
                if (states[i] == TableState(StateType.WaitFood))
                    Console.Write($" {(FoodType)randomMenuList[i]} 서빙중 ");
                else
                {
                    if (i == 1) Console.Write("rest");
                }
            }
            Console.WriteLine();
            Console.Write("점주 : ");
            for (int i = 0; i < numberoftables; i++)
            {
                if (states[i] == TableState(StateType.Clean))
                    Console.Write($" {i}번 테이블 청소중 ");
                else
                {
                    if (i == 1) Console.Write("rest");
                }
            }

        }

        // 테이블 상태에 따른 값 함수
        public string TableState(StateType state)
        {
            switch (state)
            {
                case StateType.Empty:
                    return "비어 있음";
                case StateType.Clean:
                    return "주인, 청소중";
                case StateType.WaitOrder:
                    return "주문 대기중";
                case StateType.WaitFood:
                    return "음식 기다리는 중";
                case StateType.AtTable:
                    return "식사 중";
                default:
                    return "비어있음";
            }
        }

    }
}
