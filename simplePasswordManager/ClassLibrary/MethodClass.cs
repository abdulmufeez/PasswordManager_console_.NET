using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace simplePasswordManager.ClassLibrary
{
    class MethodClass
    {
        protected NpgsqlConnection npgsqlConnection;
        protected string username { get; set; }
        protected string password { get; set; }

        public MethodClass(string connstring)
        {
            npgsqlConnection = new NpgsqlConnection(connstring);
        }
        public void LoginMethod()
        {
            bool flag = false;
            do
            {
                Console.Write("Enter username for superuser ");
                Console.Write(":");
                username = Console.ReadLine();
                Console.Write("Enter password ");
                Console.Write(":");
                password = Console.ReadLine().ToLower();
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        var psqlquerry = "SELECT user_name,password FROM superuser " +
                                                "WHERE user_name='" + username + "" +
                                                "' AND password=crypt('"+ password +"',password);";
                        NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(psqlquerry, npgsqlConnection);
                        using (npgsqlDataAdapter)
                        {
                            DataTable dataTable = new DataTable();
                            npgsqlDataAdapter.Fill(dataTable);
                            npgsqlDataAdapter.Dispose();
                            if (dataTable.Rows.Count > 0 && dataTable.Rows.Count <= 1)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                                Console.WriteLine("Enter correct username and password !");
                            }
                        }                        
                    }
                    catch (NpgsqlException error)
                    {
                        Console.WriteLine(error.ToString());
                        flag = false;
                        Console.WriteLine("Enter correct username and password !");
                    }
                }
                else
                {
                    Console.WriteLine("Enter correct username and password !");
                    flag = false;
                }
            } while (flag == false);
            
        }
        public void psswrdManager()
        {
            Console.WriteLine("1. Sign in! \n" +
                                "2. Sign up!");
            Console.Write(":");
            var choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        LoginMethod();
                        var option = menuMethod();
                        switch (option)
                        {
                            case "1":
                                {
                                    passwordstoreMethod();
                                    break;
                                }
                            case "2":
                                {
                                    findPassword();
                                    break;
                                }
                            case "3":
                                {
                                    findSites();
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }                 
                case 2:
                    {
                        signupMethod();
                        Console.WriteLine("Do You want to login in ? \n" +
                            "1. yes \n" +
                            "2. no");
                        Console.Write(":");
                        var count = Console.ReadLine();
                        if (count == "1")
                        {
                            LoginMethod();
                            var option = menuMethod();
                            switch (option)
                            {
                                case "1":
                                    {
                                        passwordstoreMethod();
                                        break;
                                    }
                                case "2":
                                    {
                                        findPassword();
                                        break;
                                    }
                                case "3":
                                    {
                                        findSites();
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid Input!");
                        break;
                    }                    
            }           
        }
        public string menuMethod()
        {
            string choice = null;
            Console.Clear();
            Console.WriteLine("you are now Signed In!");
            Console.WriteLine("User id: " + requestUsernameid());
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Press \n" +
                "1. If do you want to store a new password for any app or website \n" +
                "2. find a password for any specific app or website \n" +
                "3. find all sites related to this username or passowrd \n" +
                "Q. Quit.");
            Console.Write(":");
            choice = Console.ReadLine();
            if (choice == "Q" || choice == "q")
            {
                Environment.Exit(0);
            }            
            return choice;
        }
        public void signupMethod()
        {
            bool flag = false;
            do
            {
                Console.Write("Enter username");
                Console.Write(":");
                var username1 = Console.ReadLine();
                Console.Write("Enter password ");
                Console.Write(":");
                var password1 = Console.ReadLine().ToLower();
                Console.Write("Enter password ");
                Console.Write(":");
                var confirmPassword = Console.ReadLine().ToLower();
                Console.Write("Enter email ");
                Console.Write(":");
                var email = Console.ReadLine();

                if (!string.IsNullOrEmpty(username1) && !string.IsNullOrEmpty(password1) 
                    && !string.IsNullOrEmpty(confirmPassword) && !string.IsNullOrEmpty(email))
                {
                    if (password == confirmPassword)
                    {
                        try
                        {
                            var psqlquery = "INSERT INTO  superuser (user_name,password,email) " +
                                                    "Values('" + username + "',crypt('"+ password +"',gen_salt('bf')),'" + email + "')";
                            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(psqlquery, npgsqlConnection);
                            npgsqlConnection.Open();
                            npgsqlCommand.ExecuteScalar();
                            flag = true;
                            npgsqlConnection.Close();

                        }
                        catch (NpgsqlException error)
                        {
                            Console.WriteLine(error.ToString());
                            flag = false;
                            Console.WriteLine("Enter correctly everything!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Enter correct username, password and email !");
                    flag = false;
                }
            } while (flag == false);
        }
        public void passwordstoreMethod()
        {
            bool flag = false;
            do
            {
                Console.Write("Enter website name ");
                Console.Write(":");
                var website_name = Console.ReadLine();
                Console.Write("Enter username for the website ");
                Console.Write(":");
                var username2 = Console.ReadLine();
                Console.Write("Enter password for the website ");
                Console.Write(":");
                var password2 = Console.ReadLine().ToLower();
                Console.Write("Enter email for the website ");
                Console.Write(":");
                var email = Console.ReadLine();
                Console.Write("Enter URL for the website ");
                Console.Write(":");
                var url = Console.ReadLine();

                string psqlquery = null;
                if (!string.IsNullOrEmpty(website_name) && !string.IsNullOrEmpty(username2)
                    && !string.IsNullOrEmpty(password2))
                {                                       
                    try
                    {
                        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(url))
                        {
                            psqlquery = "INSERT INTO users_data(website,user_name,password,email,url,superuser_id)" +
                                "VALUES('" + website_name + "','" + username2 + "','" + password2 + "','" + email + "','" + url + "','" + requestUsernameid() + "');";                            
                        }
                        else
                        {
                            psqlquery = "INSERT INTO users_data(website,user_name,password,superuser_id)" +
                                "VALUES('" + website_name + "','" + username2 + "','" + password2 + "','" + requestUsernameid() + "');";                            
                        }
                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(psqlquery, npgsqlConnection);
                        npgsqlConnection.Open();
                        npgsqlCommand.ExecuteScalar();
                        flag = true;
                        npgsqlConnection.Close();

                    }
                    catch (NpgsqlException error)
                    {
                        Console.WriteLine(error.ToString());
                        flag = false;                        
                    }
                }
                else
                {
                    Console.WriteLine("Enter Correctly !");
                    flag = false;
                }
            } while (flag == false);
        }
        public int requestUsernameid()
        {
            var id = 0;
            try
            {
                var query = "SELECT id FROM superuser WHERE user_name = '" + username + "';";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
                using (npgsqlCommand)
                {
                    npgsqlConnection.Open();
                    using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())       //here mysqldatareader and executereader both work together such as 
                                                                                        //reader read the data from the mysql output and executer execute the datareader 
                    {
                        while (reader.Read())       //reading in lope that is one by one
                        {
                            id = Convert.ToInt32(reader[0]);        //saving in variable                     
                        }
                    }
                    npgsqlConnection.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return id;      //finally return user id 
        }
        public int requestingRowNumber(string webname)
        {
            var id = 0;
            try
            {
                var query = "SELECT count(*) FROM users_data " +
                            "WHERE website = '" + webname + "' AND superuser_id = " + requestUsernameid() + ";";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
                using (npgsqlCommand)
                {
                    npgsqlConnection.Open();
                    using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())       //here mysqldatareader and executereader both work together such as 
                                                                                          //reader read the data from the mysql output and executer execute the datareader 
                    {
                        while (reader.Read())       //reading in lope that is one by one
                        {
                            id = Convert.ToInt32(reader[0]);        //saving in variable                     
                        }
                    }
                    npgsqlConnection.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return id;      //finally return user id 
        }
        public void findPassword()
        {
            Console.WriteLine("Enter name of app or website ");
            Console.Write(": ");
            var webname = Console.ReadLine();
            var flag = false;            
            do
            {                
                if (!string.IsNullOrEmpty(webname))
                {
                    try
                    {
                        var psqlquery = "SELECT user_name ,password ,email FROM users_data " +
                            "WHERE website = '" + webname + "' AND superuser_id = " + requestUsernameid() + ";";
                        NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(psqlquery, npgsqlConnection);
                        using (npgsqlDataAdapter)        //running in like a loop
                        {
                            DataTable dataTable = new DataTable();          //Creating table same as in Mysql
                            npgsqlDataAdapter.Fill(dataTable);               //filling table with mysql data
                            npgsqlDataAdapter.Dispose();                     //disposig adapter means closing connection
                            flag = true;

                            /*for (int j = 0; j < dataTable.Rows.Count; j++)
                            {
                                for (int i = 0; i < dataTable.Columns.Count; i++)
                                {
                                    Console.Write(dataTable.Columns[i].ColumnName + " ");
                                    Console.WriteLine(dataTable.Rows[j].ItemArray[i]);
                                }
                            }*/

                            //==========================================

                            Console.WriteLine(DataTableToCsv(dataTable));

                            //=========================================

                            /*Console.WriteLine("Username         | Password      | Email");
                            Console.WriteLine("----------------------------------------");
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                {
                                    Console.Write(item + "          | ");
                                }
                                Console.WriteLine();
                            }*/

                            //===================================

                            /*NpgsqlCommand npgsqlCommand = new NpgsqlCommand(psqlquery, npgsqlConnection);
                            using (npgsqlCommand)
                            {
                                npgsqlConnection.Open();
                                using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())       //here mysqldatareader and executereader both work together such as 
                                                                                                      //reader read the data from the mysql output and executer execute the datareader 
                                {
                                    Console.WriteLine("Username         | Password          | Email");
                                    Console.WriteLine("--------------------------------------------");
                                    while (reader.Read())       //reading in lope that is one by one
                                    {
                                        string userName = reader["user_name"].ToString();
                                        string password = reader["password"].ToString();
                                        string email = reader["email"].ToString();

                                        // Print the data.
                                        Console.WriteLine(userName + "      |  " + password + "       |  " + email);
                                    }
                                }
                                npgsqlConnection.Close();
                            }*/
                        }
                    }
                    catch (NpgsqlException error)
                    {
                        Console.WriteLine(error.ToString());
                        flag = false;
                    }
                }
                else
                {
                    Console.WriteLine("Enter Correctly");
                    flag = false;
                }
            } while (flag == false);            
        }
        public void findSites()
        {
            try
            {
                var psqlquery = "SELECT website, user_name FROM users_data " +
                    "WHERE superuser_id = " + requestUsernameid() + ";";
                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(psqlquery, npgsqlConnection);
                using (npgsqlDataAdapter)        //running in like a loop
                {
                    DataTable dataTable = new DataTable();          //Creating table same as in Mysql
                    npgsqlDataAdapter.Fill(dataTable);               //filling table with mysql data
                    npgsqlDataAdapter.Dispose();                     //disposig adapter means closing connection

                    Console.WriteLine(DataTableToCsv(dataTable));
                }
            }
            catch (NpgsqlException error)
            {
                Console.WriteLine(error.ToString());                
            }
        }

        public static string DataTableToCsv(DataTable table)
        {
            string result = string.Empty;
            StringBuilder resultBuilder = new StringBuilder();

            if (table != null && table.Rows != null && table.Columns != null && table.Columns.Count > 0)
            {
                int lastItemIndex = table.Columns.Count - 1;
                int index = 0;

                foreach (DataColumn column in table.Columns)
                {
                    resultBuilder.Append(column.ColumnName);

                    if (index < lastItemIndex)       // if not the last element in the row
                        resultBuilder.Append(", ");  // add the separator
                    

                    index++;
                }               
                resultBuilder.AppendLine();  // add a CRLF after column names row
                

                foreach (DataRow dataRow in table.Rows)
                {
                    lastItemIndex = dataRow.ItemArray.Length - 1;
                    index = 0;

                    foreach (object item in dataRow.ItemArray)
                    {
                        resultBuilder.Append(item);

                        if (index < lastItemIndex)       // if not the last element in the row
                            resultBuilder.Append(", ");  // add the separator
                        

                        index++;
                    }
                    
                    resultBuilder.AppendLine();  // add a CRLF after each data row                    
                }

                result = resultBuilder.ToString();
            }

            return result;
        }
    }
}
