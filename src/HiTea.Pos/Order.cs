using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class Order: DatabaseObject
    {
        public Order()
        {
        }

        public override void Save()
        {
            //--insert into orders(created,member,total) values(datetime(),'ali', 20);
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
