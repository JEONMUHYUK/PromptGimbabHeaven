using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromptGimbabHeaven
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameLoop gameLoop = new GameLoop();

            
            gameLoop.Awake();

            gameLoop.GameStart();

            gameLoop.Start();

            

            while (!gameLoop.isGameOver)
            { 
                gameLoop.Update();
                gameLoop.Rander();
            }

            gameLoop.GameOver();
        }
    }
}
