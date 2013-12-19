using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public partial class User : IDataErrorInfo
    {
        private string error;
        public string Error
        {
            get { return null; }
        }

        private bool checkLogin;
        private bool isValid;
        public bool IsValid { get { return this.isValid; } }
        public bool Login(string password)
        {
            checkLogin = true;
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db2 = new Main(connectionString);
            try
            {
                User user = db2.Users.Where(u => u.Username == this.Username).FirstOrDefault();
                if (user == null) return this.isValid;

                isValid = (user.Password == password) ? true : false;
            }
            finally { db2.Dispose(); }

            return this.isValid;
        }

        private bool isAdmin;
        public bool IsAdmin { get { return this.isAdmin; } }

        public string this[string columnName]
        {
            get
            {
                this.error = null;

                // TODO: Not checking all the time. Only check after execute login method
                //if (checkLogin)
                //{
                if(!String.IsNullOrEmpty(this.Username))
                {
                    switch (columnName)
                    {
                        case "Password":
                            Login(this.Password);
                            if (!isValid) this.error = "Invalid password or user not exist!";
                            break;

                    }
                }
                //}

                return this.error;
            }
        }
    }
}