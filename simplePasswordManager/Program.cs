using System;
using System.Data;
using Npgsql;
using simplePasswordManager.ClassLibrary;

namespace simplePasswordManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Simple Password Manager";
            Console.WriteLine("-------------- Simple Password Manager -------------------");
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=manjan;Database=passwordManager";

            MethodClass methodClass = new MethodClass(connString);
            methodClass.PsswrdManager();
        }        
    }
}
