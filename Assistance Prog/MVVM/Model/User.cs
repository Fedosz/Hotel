using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Assistance_Prog.MVVM.Model
{
    internal enum Permission
    {
        None,
        HouseMaid,
        Bartender,
        Administrator,
        Manager
    }
    internal class User {
        public bool isLogged {get; set;}
        private int id {get; set;}
        private Permission permission { get; set;}

        public User()
        {
            isLogged = false;
            id = 0;
            permission = Permission.None;
        }
        
        public void Log(int id, Permission permission)
        {
            this.id = id;
            this.permission = permission;
            this.isLogged = true;
        }

        public int getId() 
        {
            return id;
        }

        public Permission getPermission()
        {
            return permission;
        }
    }
}
