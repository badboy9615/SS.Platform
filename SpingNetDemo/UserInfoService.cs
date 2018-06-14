using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpingNetDemo
{
    public class UserInfoService:IUserInfoService
    {
        public UserInfoDal userInfoDal { get; set; }
        public UserInfoService(int age)
        {
            Age = age;
        }
        public int Age { get; set; }
        public string ServerName { get; set; }
        public void Show()
        {
            Console.WriteLine("ok2-------"+ServerName);
            Console.WriteLine("Age:"+Age);
            Console.WriteLine("UserInfoDal:"+userInfoDal.UName);
        }
    }
}
