using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class User: DatabaseObject
    {
        public User()
        {
            base.TABLE_NAME = "users";
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
