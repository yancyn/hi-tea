using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    /// <summary>
    /// Abstract class for all database object to implement.
    /// </summary>
    public abstract class DatabaseObject
    {
        protected string TABLE_NAME;
        public DatabaseObject()
        {

        }
        public abstract void Save();
        public abstract void Delete();
    }
}
