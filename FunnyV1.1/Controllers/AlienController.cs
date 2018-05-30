using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunnyV1.Models.ViewModels;
using FunnyV1.Models;
using System.Data;
using System.Web.SessionState;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;


namespace FunnyV1._1.Controllers
{
    public class AlienController : Controller
    {
        // //Kidnapping started
        // 
        //
        public List<SpaceShip> fleet = new List<SpaceShip>();
        //public bool where = false;

        [Authorize(Roles = "alien")]
        public ActionResult Kidnap()
        {
            Session["where"] = false;
            Session["whereEx"] = false;
            Session["whereExp"] = false;
            return RedirectToAction("Kidnapping");
        }

        // GET: Alien
        [Authorize(Roles = "alien")]
        public ActionResult Kidnapping()
        {
            List<Human> listHuman = new List<Human>();
            List<Human> addedHumans = new List<Human>();

            bool where = false;

            if (Session["where"] != null)
                where = ((bool)(Session["where"]));
            if (where == true)
            {
                listHuman = new List<Human>(((List<Human>)(Session["listHuman"])));
                addedHumans = new List<Human>(((List<Human>)(Session["addedHumans"])));
                //addedHumans = ((List<Human>)(Session["addedHumans"]));
                fleet = ((List<SpaceShip>)(Session["fleet"]));
                //listHuman = ((List<Human>)(Session["listHuman"]));
                where = ((bool)(Session["where"]));
            }
            if (where == false)
            {

                //Session["fleet"] = new List<SpaceShip>();
                //Session["listHuman"] = new List<Human>();
                //Session["addedHumans"] = new List<Human>();
                Session["where"] = false;
                addedHumans.Clear();
                fleet.Clear();
                listHuman.Clear();

                where = false;
                var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand("SELECT * FROM Humen", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            Human human = new Human();
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
                            listHuman.Add(human);
                        }
                    //if (((List<SpaceShip>)(Session["fleet"])) != null)
                    //{
                    if (fleet.Count == 0)
                    {
                        using (var cmd = new SqlCommand("SELECT * FROM SpaceShips", conn))
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                SpaceShip spaceship = new SpaceShip();
                                spaceship.Id = (int)reader[0];
                                spaceship.Name = reader[1].ToString();
                                spaceship.ShipId = (int)reader[2];
                                spaceship.ShipTypeType = reader[3].ToString();
                                fleet.Add(spaceship);
                            }
                    }

                    conn.Close();
                }
            }
            ViewBag.addedHumans = addedHumans;//Session["addedHumans"];
            ViewBag.fleet = fleet;// Session["fleet"];
            ViewBag.where = where;
            ViewBag.listHuman = listHuman;
            Session["addedHumans"] = new List<Human>(addedHumans);
            Session["listHuman"] = new List<Human>(listHuman);
            Session["fleet"] = fleet;
            Session["where"] = where;
            return View(listHuman);
        }


        [Authorize(Roles = "alien")]
        [HttpPost]
        public ViewResult Kidnapping(FormCollection formCollection)
        {
            List<Human> listHuman = new List<Human>(((List<Human>)(Session["listHuman"])));
            List<Human> addedHumans = new List<Human>(((List<Human>)(Session["addedHumans"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool where = ((bool)(Session["where"]));
            Session["where"] = false;
            var emailOfCurrent = User.Identity.Name;
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
            List<Alien> listA = new List<Alien>();
            string id = formCollection["inputShip"];
            foreach (var ship in fleet)
                if (ship.Id == int.Parse(id))
                    ViewBag.ShipId = ship.Name;
            int idA = 0;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Aliens", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())
                        if (emailOfCurrent == reader[1].ToString())
                            idA = (int)reader[0];
                string sql1 = "INSERT INTO Kidnappings(DateTime, ShipId, AlienId) VALUES (@date , @ship, @alien)";
                SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                myCommand1.Parameters.AddWithValue("@date", DateTime.Now);
                myCommand1.Parameters.AddWithValue("@ship", id);
                myCommand1.Parameters.AddWithValue("@alien", idA);
                myCommand1.ExecuteNonQuery();
                int temp = 0;
                using (var cmdd = new SqlCommand("SELECT * FROM Kidnappings", conn))
                using (var reader = cmdd.ExecuteReader())
                    while (reader.Read())
                        temp = (int)reader[0];
                foreach (var a in addedHumans)
                {
                    var c = new SqlCommand("INSERT INTO KidnappingWhoms(KidnappingId, HumanId) VALUES (" + temp.ToString() + ", " + a.Id.ToString() + ")", conn);
                    c.ExecuteNonQuery();
                }
                foreach (var a in addedHumans)
                {
                    string sql2 = "UPDATE Humen SET SpaceShipId = @ship WHERE id=@id";
                    SqlCommand myCommand2 = new SqlCommand(sql2, conn);
                    myCommand2.Parameters.AddWithValue("@ship", id);
                    myCommand2.Parameters.AddWithValue("@id", a.Id);
                    myCommand2.ExecuteNonQuery();

                }
                addedHumans.Clear();
                conn.Close();
            }
            Session["addedHumans"] = new List<Human>(addedHumans);
            Session["listHuman"] = new List<Human>(listHuman);
            //Session["addedHumans"] = addedHumans;
            //Session["listHuman"] = listHuman;
            Session["fleet"] = fleet;
            Session["where"] = where;
            return View("KidnapComplet");
        }


        [Authorize(Roles = "alien")]
        public ActionResult AddToList(Human human)
        {
            List<Human> listHuman = new List<Human>(((List<Human>)(Session["listHuman"])));
            List<Human> addedHumans = new List<Human>(((List<Human>)(Session["addedHumans"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool where = ((bool)(Session["where"]));
            where = true;
            Session["where"] = true;
            for (int i = 0; i < listHuman.Count; ++i)
                if (listHuman[i].Id == human.Id)
                {
                    listHuman.RemoveAt(i);
                }
            bool containsItem = addedHumans.Any(item => item.Id == human.Id);
            if (!containsItem)
                addedHumans.Add(human);

            ViewBag.addedHumans = addedHumans;
            ViewBag.fleet = fleet;
            Session["addedHumans"] = new List<Human>(addedHumans);
            Session["listHuman"] = new List<Human>(listHuman);
            Session["fleet"] = fleet;
            Session["where"] = where;
            return RedirectToAction("Kidnapping");
        }

        [Authorize(Roles = "alien")]
        public ActionResult DeleteFromList(Human human)
        {
            List<Human> listHuman = new List<Human>(((List<Human>)(Session["listHuman"])));
            List<Human> addedHumans = new List<Human>(((List<Human>)(Session["addedHumans"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool where = ((bool)(Session["where"]));
            where = true;
            Session["where"] = true;
            for (int i = 0; i < addedHumans.Count; ++i)
                if (addedHumans[i].Id == human.Id)
                {
                    (addedHumans).RemoveAt(i);
                }
            bool containsItem = listHuman.Any(item => item.Id == human.Id);
            if (!containsItem)
                listHuman.Add(human);
            ViewBag.addedHumans = addedHumans;
            ViewBag.fleet = (fleet);
            Session["addedHumans"] = new List<Human>(addedHumans);
            Session["listHuman"] = new List<Human>(listHuman);

            Session["fleet"] = fleet;
            Session["where"] = where;
            return RedirectToAction("Kidnapping");
        }
        //Kidnapping finished




        //EXCRUSION STARTED



        //public bool whereEx = false;

        [Authorize(Roles = "alien")]
        public ActionResult Excursion()
        {
            List<Human> listExc = new List<Human>();
            List<Human> listExcAdded = new List<Human>();
            bool whereEx = false;
            if (Session["whereEx"] != null)
                whereEx = ((bool)(Session["whereEx"]));
            if (whereEx == true)
            {
                listExc = new List<Human>(((List<Human>)(Session["listExc"])));
                listExcAdded = new List<Human>(((List<Human>)(Session["listExcAdded"])));
                fleet = ((List<SpaceShip>)(Session["fleet"]));
                whereEx = ((bool)(Session["whereEx"]));
            }
            if (whereEx == false)
            {
                Session["whereEx"] = false;
                listExcAdded.Clear();
                fleet.Clear();
                listExc.Clear();

                whereEx = false;
                var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Humen", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            Human human = new Human();
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
                            listExc.Add(human);
                        }

                    using (var cmd = new SqlCommand("SELECT * FROM SpaceShips", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            SpaceShip spaceship = new SpaceShip();
                            spaceship.Id = (int)reader[0];
                            spaceship.Name = reader[1].ToString();
                            spaceship.ShipId = (int)reader[2];
                            spaceship.ShipTypeType = reader[3].ToString();
                            fleet.Add(spaceship);
                        }
                    conn.Close();
                }
            }

            ViewBag.where = whereEx;
            ViewBag.listHuman = listExc;
            Session["listExcAdded"] = new List<Human>(listExcAdded);
            Session["listExc"] = new List<Human>(listExc);
            Session["fleet"] = fleet;
            Session["whereEx"] = whereEx;
            ViewBag.addedHumans = listExcAdded;
            ViewBag.fleet = fleet;

            return View(listExc);
        }

        [Authorize(Roles = "alien")]
        [HttpPost]
        public ViewResult Excursion(FormCollection formCollection)
        {
            List<Human> listExc = new List<Human>(((List<Human>)(Session["listExc"])));
            List<Human> listExcAdded = new List<Human>(((List<Human>)(Session["listExcAdded"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereEx = ((bool)(Session["whereEx"]));
            whereEx = false;
            var emailOfCurrent = User.Identity.Name;
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
            string id = formCollection["inputShip"];
            foreach (var ship in fleet)
                if (ship.Id == int.Parse(id))
                    ViewBag.ShipId = ship.Name;
            int idA = 0;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Aliens", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())
                        if (emailOfCurrent == reader[1].ToString())
                            idA = (int)reader[0];
                string sql1 = "INSERT INTO Excursions( SpaceShipId, AlienId, date) VALUES (@ship, @alien, @date)";
                SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                myCommand1.Parameters.AddWithValue("@ship", id);
                myCommand1.Parameters.AddWithValue("@alien", idA);
                myCommand1.Parameters.AddWithValue("@date", DateTime.Now);
                myCommand1.ExecuteNonQuery();
                int temp = 0;
                using (var cmdd = new SqlCommand("SELECT * FROM Excursions", conn))
                using (var reader = cmdd.ExecuteReader())
                    while (reader.Read())
                        temp = (int)reader[0];
                foreach (var a in listExcAdded)
                {
                    var c = new SqlCommand("INSERT INTO ExcursionFors(HumanId, ExcursionId) VALUES (" + a.Id.ToString() + ", " + temp.ToString() + ")", conn);
                    c.ExecuteNonQuery();
                }
                listExcAdded.Clear();
                conn.Close();
            }
            ViewBag.where = whereEx;
            ViewBag.listHuman = listExc;
            Session["listExcAdded"] = new List<Human>(listExcAdded);
            Session["listExc"] = new List<Human>(listExc);
            Session["fleet"] = fleet;
            Session["whereEx"] = whereEx;
            ViewBag.addedHumans = listExcAdded;
            ViewBag.fleet = fleet;
            return View("ExCreated");
        }



        [Authorize(Roles = "alien")]
        public ActionResult AddToListEx(Human human)
        {
            List<Human> listExc = new List<Human>(((List<Human>)(Session["listExc"])));
            List<Human> listExcAdded = new List<Human>(((List<Human>)(Session["listExcAdded"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereEx = ((bool)(Session["whereEx"]));
            whereEx = true;
            for (int i = 0; i < listExc.Count; ++i)
                if (listExc[i].Id == human.Id)
                {
                    listExc.RemoveAt(i);
                }
            bool containsItem = listExcAdded.Any(item => item.Id == human.Id);
            if (!containsItem)
                listExcAdded.Add(human);

            ViewBag.addedHumans = listExcAdded;
            ViewBag.fleet = fleet;
            ViewBag.where = whereEx;
            ViewBag.listHuman = listExc;
            Session["listExcAdded"] = new List<Human>(listExcAdded);
            Session["listExc"] = new List<Human>(listExc);
            Session["fleet"] = fleet;
            Session["whereEx"] = whereEx;
            return RedirectToAction("Excursion");
        }

        [Authorize(Roles = "alien")]
        public ActionResult DeleteFromListEx(Human human)
        {
            List<Human> listExc = new List<Human>(((List<Human>)(Session["listExc"])));
            List<Human> listExcAdded = new List<Human>(((List<Human>)(Session["listExcAdded"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereEx = ((bool)(Session["whereEx"]));
            whereEx = true;
            for (int i = 0; i < listExcAdded.Count; ++i)
                if (listExcAdded[i].Id == human.Id)
                {
                    listExcAdded.RemoveAt(i);
                }
            bool containsItem = listExc.Any(item => item.Id == human.Id);
            if (!containsItem)
                listExc.Add(human);
            ViewBag.addedHumans = listExcAdded;
            ViewBag.fleet = fleet;
            ViewBag.where = whereEx;
            ViewBag.listHuman = listExc;
            Session["listExcAdded"] = new List<Human>(listExcAdded);
            Session["listExc"] = new List<Human>(listExc);
            Session["fleet"] = fleet;
            Session["whereEx"] = whereEx;
            return RedirectToAction("Excursion");
        }
        // End Excursion


        //Transfer

        [Authorize(Roles = "alien")]
        public ActionResult Transfer()
        {
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
            List<Human> huList = new List<Human>();
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Humen", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        Human human = new Human();
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
                        huList.Add(human);
                    }
                conn.Close();
            }

            if (fleet.Count == 0)
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();


                    using (var cmd = new SqlCommand("SELECT * FROM SpaceShips", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            SpaceShip spaceship = new SpaceShip();
                            spaceship.Id = (int)reader[0];
                            spaceship.Name = reader[1].ToString();
                            spaceship.ShipId = (int)reader[2];
                            spaceship.ShipTypeType = reader[3].ToString();
                            fleet.Add(spaceship);
                        }
                    conn.Close();
                }
            }


            ViewBag.fleet = fleet;
            Session["fleet"] = fleet;
            return View(huList);
        }

        [Authorize(Roles = "alien")]
        [HttpPost]
        public ActionResult Transfer(FormCollection formCollection)
        {
            var emailOfCurrent = User.Identity.Name;
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
            string idS = formCollection["outputShip"];
            string idH = formCollection["humanoid"];


            foreach (var ship in ((List<SpaceShip>)(Session["fleet"])))
                if (ship.Id == int.Parse(idS))
                    ViewBag.ShipId = ship.Name;
            int idA = 0;
            int? idSfrom = 0;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Humen", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        if ((int)reader[0] == int.Parse(idH))
                            idSfrom = (int?)reader[7];
                if (int.Parse(idS) != idSfrom)
                {
                    using (var cm = new SqlCommand("SELECT * FROM Aliens", conn))
                    using (var reader = cm.ExecuteReader())
                        while (reader.Read())
                            if (emailOfCurrent == reader[1].ToString())
                                idA = (int)reader[0];

                    string sql1 = "INSERT INTO Transfers( HumanId, date, SpaceShipFromId, SpaceShipToId, AlienId) VALUES (@human, @date, @shipfrom, @shipto, @alien)";
                    SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                    myCommand1.Parameters.AddWithValue("@human", int.Parse(idH));
                    myCommand1.Parameters.AddWithValue("@date", DateTime.Now);
                    myCommand1.Parameters.AddWithValue("@shipfrom", idSfrom);
                    myCommand1.Parameters.AddWithValue("@shipto", int.Parse(idS));
                    myCommand1.Parameters.AddWithValue("@alien", idA);
                    myCommand1.ExecuteNonQuery();
                    string sql2 = "UPDATE Humen SET SpaceShipId = @ship WHERE id=@id";
                    SqlCommand myCommand2 = new SqlCommand(sql2, conn);
                    myCommand2.Parameters.AddWithValue("@ship", int.Parse(idS));
                    myCommand2.Parameters.AddWithValue("@id", int.Parse(idH));
                    myCommand2.ExecuteNonQuery();
                }
                conn.Close();
            }

            return View("TransferCompleted");
        }
        //TRANSFER END



        //EXPEREMENT


        //static List<ExType> exTypes = new List<ExType>();
        //
        //static List<Human> listHumanExp = new List<Human>();
        //static List<Human> addedHumansExp = new List<Human>();
        //static bool whereExp = false;


        [Authorize(Roles = "alien")]
        public ActionResult Experiment()
        {
            List<ExType> exTypes = new List<ExType>();

            List<Human> listHumanExp = new List<Human>();
            List<Human> addedHumansExp = new List<Human>();
            bool whereExp = false;
            if (Session["whereExp"] != null)
                whereExp = ((bool)(Session["whereExp"]));
            if (whereExp == true)
            {
                listHumanExp = new List<Human>(((List<Human>)(Session["listHumanExp"])));
                addedHumansExp = new List<Human>(((List<Human>)(Session["addedHumansExp"])));
                exTypes = new List<ExType>(((List<ExType>)(Session["ExTypes"])));
                fleet = ((List<SpaceShip>)(Session["fleet"]));
                whereExp = ((bool)(Session["whereExp"]));
            }
            if (whereExp == false)
            {
                addedHumansExp.Clear();
                fleet.Clear();
                listHumanExp.Clear();
                exTypes.Clear();
                //Session["whereExp"] = false;
                //Session["fleet"] = new List<SpaceShip>();
                //Session["listHumanExp"] = new List<Human>();
                //Session["addedHumansExp"] = new List<Human>();
                //Session["exTypes"] = new List<ExType>();

                whereExp = false;
                var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT * FROM Humen", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            Human human = new Human();
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
                            listHumanExp.Add(human);
                        }
                    if (exTypes.Count == 0)
                    {
                        using (var cmd = new SqlCommand("SELECT * FROM ExTypes", conn))
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                ExType ext = new ExType();
                                ext.Type = reader[0].ToString();
                                ext.Description = reader[1].ToString();
                                exTypes.Add(ext);
                            }
                    }
                    if (fleet.Count == 0)
                    {
                        using (var cmd = new SqlCommand("SELECT * FROM SpaceShips", conn))
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                            {
                                SpaceShip spaceship = new SpaceShip();
                                spaceship.Id = (int)reader[0];
                                spaceship.Name = reader[1].ToString();
                                spaceship.ShipId = (int)reader[2];
                                spaceship.ShipTypeType = reader[3].ToString();
                                fleet.Add(spaceship);
                            }
                    }
                    conn.Close();
                }
            }
            ViewBag.fleet = fleet;// Session["fleet"];
            ViewBag.where = whereExp;
            ViewBag.listHuman = listHumanExp;
            Session["addedHumansExp"] = new List<Human>(addedHumansExp);
            Session["listHumanExp"] = new List<Human>(listHumanExp);
            Session["fleet"] = fleet;
            Session["whereExp"] = whereExp;
            Session["exTypes"] = new List<ExType>(exTypes);
            ViewBag.exTypes = exTypes;
            ViewBag.addedHumans = addedHumansExp;
            return View(listHumanExp);
        }

        [Authorize(Roles = "alien")]
        [HttpPost]
        public ViewResult Experiment(FormCollection formCollection)
        {
            List<Human> listHumanExp = new List<Human>(((List<Human>)(Session["listHumanExp"])));
            List<Human> addedHumansExp = new List<Human>(((List<Human>)(Session["addedHumansExp"])));
            List<ExType> exTypes = new List<ExType>(((List<ExType>)(Session["exTypes"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereExp = ((bool)(Session["whereExp"]));
            whereExp = false;
            var emailOfCurrent = User.Identity.Name;
            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11 ";
            List<Alien> listA = new List<Alien>();
            string id = formCollection["inputShip"];
            string exID = formCollection["inputEx"];
            foreach (var ship in fleet)
                if (ship.Id == int.Parse(id))
                    ViewBag.ShipId = ship.Name;
            foreach (var ex in exTypes)
                if (ex.Type == exID)
                    ViewBag.ex = ex.Type;
            int idA = 0;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cm = new SqlCommand("SELECT * FROM Aliens", conn))
                using (var reader = cm.ExecuteReader())
                    while (reader.Read())
                        if (emailOfCurrent == reader[1].ToString())
                            idA = (int)reader[0];
                string sql1 = "INSERT INTO Experiments(AlienId, ExTypeType, DateTime, ShipId) VALUES (@alien ,  @type, @date , @ship)";
                SqlCommand myCommand1 = new SqlCommand(sql1, conn);
                myCommand1.Parameters.AddWithValue("@alien", idA);
                myCommand1.Parameters.Add("@type", SqlDbType.NVarChar, 128).Value = exID;
                myCommand1.Parameters.AddWithValue("@date", DateTime.Now);
                myCommand1.Parameters.AddWithValue("@ship", int.Parse(id));
                myCommand1.ExecuteNonQuery();
                int temp = 0;
                using (var cmdd = new SqlCommand("SELECT * FROM Experiments", conn))
                using (var reader = cmdd.ExecuteReader())
                    while (reader.Read())
                        temp = (int)reader[0];
                foreach (var a in addedHumansExp)
                {
                    var c = new SqlCommand("INSERT INTO ExOns(ExperimentId, HumanId) VALUES (" + temp.ToString() + ", " + a.Id.ToString() + ")", conn);
                    c.ExecuteNonQuery();
                }
                addedHumansExp.Clear();
                conn.Close();
            }
            Session["addedHumansExp"] = new List<Human>(addedHumansExp);
            Session["listHumanExp"] = new List<Human>(listHumanExp);
            Session["fleet"] = fleet;
            Session["whereExp"] = whereExp;
            Session["exTypes"] = new List<ExType>(exTypes);
            return View("ExperimentCompleted");
        }


        [Authorize(Roles = "alien")]
        public ActionResult AddToListExp(Human human)
        {
            List<Human> listHumanExp = new List<Human>(((List<Human>)(Session["listHumanExp"])));
            List<Human> addedHumansExp = new List<Human>(((List<Human>)(Session["addedHumansExp"])));
            List<ExType> exTypes = new List<ExType>(((List<ExType>)(Session["exTypes"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereExp = ((bool)(Session["whereExp"]));
            whereExp = true;
            for (int i = 0; i < listHumanExp.Count; ++i)
                if (listHumanExp[i].Id == human.Id)
                {
                    listHumanExp.RemoveAt(i);
                }
            bool containsItem = addedHumansExp.Any(item => item.Id == human.Id);
            if (!containsItem)
                addedHumansExp.Add(human);

            ViewBag.addedHumans = addedHumansExp;
            ViewBag.fleet = fleet;
            Session["addedHumansExp"] = new List<Human>(addedHumansExp);
            Session["listHumanExp"] = new List<Human>(listHumanExp);
            Session["fleet"] = fleet;
            Session["whereExp"] = whereExp;
            Session["exTypes"] = new List<ExType>(exTypes);
            return RedirectToAction("Experiment");
        }

        [Authorize(Roles = "alien")]
        public ActionResult DeleteFromListExp(Human human)
        {
            List<Human> listHumanExp = new List<Human>(((List<Human>)(Session["listHumanExp"])));
            List<Human> addedHumansExp = new List<Human>(((List<Human>)(Session["addedHumansExp"])));
            List<ExType> exTypes = new List<ExType>(((List<ExType>)(Session["exTypes"])));
            fleet = ((List<SpaceShip>)(Session["fleet"]));
            bool whereExp = ((bool)(Session["whereExp"]));
            whereExp = true;
            for (int i = 0; i < addedHumansExp.Count; ++i)
                if (addedHumansExp[i].Id == human.Id)
                {
                    addedHumansExp.RemoveAt(i);
                }
            bool containsItem = listHumanExp.Any(item => item.Id == human.Id);
            if (!containsItem)
                listHumanExp.Add(human);
            ViewBag.addedHumans = addedHumansExp;
            ViewBag.fleet = fleet;
            Session["addedHumansExp"] = new List<Human>(addedHumansExp);
            Session["listHumanExp"] = new List<Human>(listHumanExp);
            Session["fleet"] = fleet;
            Session["whereExp"] = whereExp;
            Session["exTypes"] = new List<ExType>(exTypes);
            return RedirectToAction("Experiment");
        }


    } //
}