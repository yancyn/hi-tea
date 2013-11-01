using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class TableManager
    {
        private const int MAX = 100; // TODO: Max ball number store at config?

        public ObservableCollection<TableBall> Balls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayBalls">Zero based numbering of ball. Position 13 display A13.</param>
        public TableManager(Dictionary<int, string> displayBalls)
        {
            this.Balls = new ObservableCollection<TableBall>();
            Random random = new Random();
            for (int i = 0; i < MAX; i++)
            {
                TableBall ball = new TableBall();
                ball.Visible = displayBalls.ContainsKey(i) ? true : false;
                if (displayBalls.ContainsKey(i))
                    ball.No = displayBalls[i];
                ball.Size = 100;
                this.Balls.Add(ball);
            }
        }
    }
}
