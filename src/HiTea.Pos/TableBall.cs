using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    /// <summary>
    /// Represent table object in cafe layout.
    /// </summary>
    public class TableBall
    {
        /// <summary>
        /// True if visible.
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// Represent table number.
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// Size of ball.
        /// </summary>
        public int Size { get; set; }

        public TableBall()
        {
            this.Size = 30; // min size
            this.No = string.Empty;
            this.Visible = false;
        }
    }
}
