using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    internal class GameOver
    {
        public void Render(int totalDay, int totalConsumer, int totalSales)
        {
            Console.Clear();
            Console.Write($@"
            

                    Thank you for using Kimbap Heaven.
            
                    Total Days      :    {totalDay} Days  
                    Total Consumer  :    {totalConsumer} Consumer
                    Total Sales     :    {totalSales} \
                

");

            Console.ReadKey(true);
        
        }
    }
}
