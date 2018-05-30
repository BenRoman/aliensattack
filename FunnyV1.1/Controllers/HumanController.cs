using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunnyV1.Models;
using FunnyV1.Models.ViewModels;
using System.Web.SessionState;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;


namespace FunnyV1._1.Controllers
{
    public class HumanController : Controller
    {
        //static List<Alien> listAliens = new List<Alien>();

        // GET: Human
        [Authorize(Roles = "human")]
        public ActionResult Murdering()
        {
            List<Alien> listAliens = new List<Alien>();
            //listAliens.Clear();
            List<int> arrIndex = new List<int>();
//            var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Aliens", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        Alien alien = new Alien();
                        alien.Id = (int)reader[0];
                        alien.Email = reader[1].ToString();
                        alien.Password = reader[2].ToString();
                        alien.FirstName = reader[3].ToString();
                        alien.SecondName = reader[4].ToString();
                        alien.ThirdName = reader[5].ToString();
                        alien.FourthName = reader[6].ToString();
                        alien.Age = (int)reader[7];
                        alien.RoleId = 1;
                        listAliens.Add(alien);
                    }

                using (var cmd = new SqlCommand("SELECT a.Id, m.HumanId FROM Aliens a FULL OUTER JOIN Murders m ON m.AlienId = a.Id ;", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        reader.IsDBNull(reader.GetOrdinal("HumanId"));
                        int? p = reader.IsDBNull(reader.GetOrdinal("HumanId")) ? -1 : (int?)reader[1];  
                        if (p == -1)
                            arrIndex.Add((int)reader[0]);

                    }
            }
            ViewBag.arrIndex = arrIndex;
            return View(listAliens);
        }
        [Authorize(Roles = "human")]
        [HttpPost]
        public ActionResult Murdering(FormCollection formCollection)
        {
            var emailOfCurrent = User.Identity.Name;
            //var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11";
            string id = formCollection["alianoid"];
            string descript = formCollection["descrition"];
            int idH = 0;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Humen", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())

                        if (emailOfCurrent == reader[1].ToString())
                            idH = (int)reader[0];
                string sql1 = "INSERT INTO Murders( AlienId, DateTime, HumanId, Description) VALUES ( @alien, @date , @human, @descr)";
                SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                myCommand1.Parameters.AddWithValue("@alien", int.Parse(id));
                myCommand1.Parameters.AddWithValue("@date", DateTime.Now);
                myCommand1.Parameters.AddWithValue("@human", idH);
                //myCommand1.Parameters.AddWithValue("@descr", idH);
                myCommand1.Parameters.Add("@descr", SqlDbType.NVarChar).Value = descript;
                myCommand1.ExecuteNonQuery();
            }
            return RedirectToAction("Murdering");
        }

        //Escape
        [Authorize(Roles = "human")]
        public ActionResult Escape()
        {
            //var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11";
            string emailOfCurrent = User.Identity.Name;
            int idH = 0;
            Human human = new Human();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Humen", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())

                        if (emailOfCurrent == reader[1].ToString())
                        {
                            idH = (int)reader[0];

                            human.Id = (int)reader[0];
                            human.Email = reader[1].ToString();
                            human.Password = reader[2].ToString();
                            human.FirstName = reader[3].ToString();
                            human.SecondName = reader[4].ToString();
                            human.RoleId = 2;
                            human.Age = (int)reader[5];
                            reader.IsDBNull(reader.GetOrdinal("SpaceShipId"));
                            int? p = reader.IsDBNull(reader.GetOrdinal("SpaceShipId")) ? -1 : (int?)reader[7];
                            if (p != -1)
                                human.SpaceShipId = p;
                        }
            }
            ViewBag.ship = human.SpaceShipId;
            return View();
        }


        [Authorize(Roles = "human")]
        [HttpPost]
        public ActionResult Escape(FormCollection formCollection)
        {
            //var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";

            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11"; string emailOfCurrent = User.Identity.Name;
            int idH = 0;
            Human human = new Human();
            Random r = new Random();
            int rInt = r.Next(0, 100);
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Humen", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())
                        if (emailOfCurrent == reader[1].ToString())
                        {
                            idH = (int)reader[0];

                            human.Id = (int)reader[0];
                            human.Email = reader[1].ToString();
                            human.Password = reader[2].ToString();
                            human.FirstName = reader[3].ToString();
                            human.SecondName = reader[4].ToString();
                            human.RoleId = 2;
                            human.Age = (int)reader[5];
                            reader.IsDBNull(reader.GetOrdinal("SpaceShipId"));
                            int? p = reader.IsDBNull(reader.GetOrdinal("SpaceShipId")) ? -1 : (int?)reader[7];
                            if (p != -1)
                                human.SpaceShipId = p;
                        }
                if (rInt <= 40)
                {
                    string sql1 = "INSERT INTO Escapes( HumanId, ShipId, DateTime) VALUES ( @human, @ship, @date)";
                    SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                    myCommand1.Parameters.AddWithValue("@human", human.Id);
                    myCommand1.Parameters.AddWithValue("@ship", human.SpaceShipId);
                    myCommand1.Parameters.AddWithValue("@date", DateTime.Now);

                    string sql2 = "UPDATE Humen SET SpaceShipId = NULL WHERE id=@id";
                    SqlCommand myCommand2 = new SqlCommand(sql2, conn);
                    myCommand2.Parameters.AddWithValue("@id", human.Id);
                    myCommand2.ExecuteNonQuery();
                    myCommand1.ExecuteNonQuery();
                    ViewBag.txtrslt = "You are free... for now";
                }
                else
                    ViewBag.txtrslt = "You failed. Back to your cage";

            }
            return View("Result");

        }
    }
}