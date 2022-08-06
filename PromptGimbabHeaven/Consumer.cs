using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    internal class Consumer
    {
        #region Feild
        public int consumerOfWaiting { get; set; }      // Wating 변수
        int limitConsumer;                              // Wating 손님 제한 변수
        #endregion
         
        public void Start(int limitConsumer)
        {
            // 초기화
            consumerOfWaiting = 0;
            this.limitConsumer = limitConsumer;
        }

        public void Update()
        {
            // consumerOfWaiting 가 limitConsumer 보다 작다면 consumerOfWaiting 증가.
            if (consumerOfWaiting < limitConsumer) consumerOfWaiting++;
        }

    }
}
