using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FunnyV1._1.Controllers
{
    public class StatisticController : Controller
    {
        //static string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
        static string connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11";
        public ActionResult Statistic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FirstQuery(FormCollection formCol)
        {
            string query = "SELECT h.Id, h.FirstName, h.SecondName" +
                            " FROM Aliens a" +
                            " INNER JOIN Kidnappings k ON k.AlienId = a.Id" +
                            " INNER JOIN KidnappingWhoms w ON w.KidnappingId = k.Id" +
                            " INNER JOIN Humen h ON h.Id = w.HumanId" +
                           $" WHERE(a.Id = {formCol["inputAlien"]} AND k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}')" +
                            " GROUP BY h.Id, h.FirstName, h.SecondName" +
                           $" HAVING COUNT(h.Id) >= {formCol["N"]};";
            
            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString() });
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult SecondQuery(FormCollection formCol)
        {
            string query = "SELECT DISTINCT s.Name" +
                            " FROM Humen h" +
                            " FULL OUTER JOIN KidnappingWhoms w ON w.HumanId = h.Id" +
                            " FULL OUTER JOIN Kidnappings k ON k.Id = w.KidnappingId" +
                            " FULL OUTER JOIN SpaceShips s ON s.Id = k.ShipId" +
                           $" WHERE(h.Id = {formCol["inputHuman"]} AND (k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}'))" +
                            " EXCEPT" +
                            " SELECT DISTINCT s.Name" +
                            " FROM Humen h" +
                            " FULL OUTER JOIN Transfers t ON t.HumanId = h.Id" +
                            " FULL OUTER JOIN SpaceShips s ON s.Id = t.SpaceShipToId" +
                           $" WHERE(h.Id = {formCol["inputHuman"]} AND" +
                           $" (t.date >= '{formCol["dateFrom"]}' AND t.date <= '{formCol["dateTo"]}'))" +
                            " UNION" +
                            " (SELECT DISTINCT s.Name" +
                            " FROM Humen h" +
                            " FULL OUTER JOIN Transfers t ON t.HumanId = h.Id" +
                            " FULL OUTER JOIN SpaceShips s ON s.ID = t.SpaceShipToId" +
                            $" WHERE(h.Id = {formCol["inputHuman"]} AND(t.date >= '{formCol["dateFrom"]}' AND t.date <= '{formCol["dateTo"]}'))" +
                            " EXCEPT" +
                            " SELECT DISTINCT s.Name" +
                            " FROM Humen h" +
                            " FULL OUTER JOIN KidnappingWhoms w ON w.HumanId = h.Id" +
                            " FULL OUTER JOIN Kidnappings k ON k.Id = w.KidnappingId" +
                            " FULL OUTER JOIN SpaceShips s ON s.Id = k.ShipId" +
                            $" WHERE(h.Id = {formCol["inputHuman"]} AND(k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}')));";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString() });
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult ThirdQuery(FormCollection formCol)
        {
            string query = "SELECT DISTINCT a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                            " FROM Aliens a" +
                            " INNER JOIN Kidnappings k ON k.AlienId = a.Id" +
                            " INNER JOIN KidnappingWhoms w ON w.KidnappingId = k.Id" +
                            $" WHERE(w.HumanId = {formCol["inputHuman"]} AND k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}')" +
                            " GROUP BY" +
                            " a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                            " HAVING" +
                            $" COUNT(a.Id) >= {formCol["N"]};";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                                               reader[3].ToString(), reader[4].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult FourthQuery(FormCollection formCol)
        {
            string query = "SELECT a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                            " FROM Aliens AS a" +
                            " INNER JOIN Murders m ON m.AlienId = a.Id" +
                           $" WHERE(m.HumanId = {formCol["inputHuman"]} AND m.DateTime >= '{formCol["dateFrom"]}' AND m.DateTime <= '{formCol["dateTo"]}');";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                                               reader[3].ToString(), reader[4].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult FifthQuery(FormCollection formCol)
        {
            string query = "SELECT DISTINCT a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                            " FROM Aliens a" +
                            " INNER JOIN Murders m ON m.AlienId = a.Id" +
                            " INNER JOIN Kidnappings k ON k.AlienId = m.AlienId" +
                            " INNER JOIN KidnappingWhoms w ON w.KidnappingId = k.Id" +
                            $" WHERE(w.HumanId = {formCol["inputHuman"]});";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                                               reader[3].ToString(), reader[4].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult SixthQuery(FormCollection formCol)
        {
            string query = "SELECT a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                            " FROM Aliens a" +
                            " INNER JOIN Kidnappings k ON k.AlienId = a.Id" +
                            " INNER JOIN KidnappingWhoms w ON w.KidnappingId = k.Id" +
                           $" WHERE(k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}')" +
                            " GROUP BY a.Id, a.FirstName, a.SecondName, a.ThirdName, a.FourthName" +
                           $" HAVING COUNT(DISTINCT w.HumanId) >= {formCol["N"]};";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                                               reader[3].ToString(), reader[4].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult SeventhQuery(FormCollection formCol)
        {
            string query = "SELECT h.Id, h.FirstName, h.SecondName" +
                            " FROM Humen h" +
                            " INNER JOIN KidnappingWhoms w ON w.HumanId = h.Id" +
                            " INNER JOIN Kidnappings k ON k.Id = w.KidnappingId" +
                           $" WHERE(k.DateTime >= '{formCol["dateFrom"]}' AND k.DateTime <= '{formCol["dateTo"]}')" +
                            " GROUP BY h.Id, h.FirstName, h.SecondName" +
                           $" HAVING COUNT(w.HumanId) >= {formCol["N"]};";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult EighthQuery(FormCollection formCol)
        {
            string query = "SELECT exc.Id, exp.Id" +
                            " FROM Excursions exc" +
                            " INNER JOIN Experiments exp ON exp.AlienId = exc.AlienId" +
                            " INNER JOIN ExOns exp_on ON exp_on.ExperimentId = exp.Id" +
                            " INNER JOIN ExcursionFors exc_for ON exc_for.ExcursionId = exc.Id" +
                           $" WHERE(exp_on.HumanId = {formCol["inputHuman"]} AND exc_for.HumanId = {formCol["inputHuman"]} AND" +
                                $" exc.AlienId = {formCol["inputAlien"]} AND exc.date >= '{formCol["dateFrom"]}'" +
                                $" AND exc.date <= '{formCol["dateTo"]}' AND exp.DateTime >= '{formCol["dateFrom"]}'" +
                                $" AND exp.DateTime <= '{formCol["dateTo"]}');";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult NinethQuery(FormCollection formCol)
        {
            string query = "SELECT COUNT(*)" +
                            " FROM (SELECT exc.Id FROM Excursions exc INNER JOIN ExcursionFors exc_for ON exc_for.ExcursionId = exc.Id" +
                            $" WHERE (exc.AlienId = {formCol["inputAlien"]} AND exc.date >= '{formCol["dateFrom"]}' AND exc.date <= '{formCol["dateTo"]}')" +
                            $" GROUP BY exc.Id HAVING COUNT(exc_for.HumanId) >= {formCol["N"]}) AS h;";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString()});
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult TenthQuery(FormCollection formCol)
        {

            string query = "SELECT COUNT(*)" +
                            " FROM (SELECT exp.Id FROM Experiments exp INNER JOIN ExOns exp_on ON exp_on.ExperimentId = exp.Id" +
                            $" WHERE (exp_on.HumanId = {formCol["inputHuman"]} AND exp.DateTime >= '{formCol["dateFrom"]}'" +
                            $" AND exp.DateTime <= '{formCol["dateTo"]}') GROUP BY exp.Id HAVING COUNT(exp.AlienId) >= {formCol["N"]}) AS c_exp;";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString() });
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult EleventhQuery(FormCollection formCol)
        {
            string query = "SELECT MONTH(DateTime), COUNT(Id)" +
                            " FROM Kidnappings" +
                            " GROUP BY MONTH(DateTime);";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString() });
                    }
            }

            return View("Query", res);
        }

        [HttpPost]
        public ActionResult TwelvethQuery(FormCollection formCol)
        {

            string query = "SELECT exp.ShipId, s.Name, COUNT(exp.Id)" +
                            " FROM Experiments exp" +
                            " INNER JOIN Aliens a ON exp.AlienId = a.Id" +
                            " INNER JOIN SpaceShips s ON exp.ShipId = s.Id" +
                            $" WHERE a.Id = {formCol["inputAlien"]} AND exp.DateTime >= '{formCol["dateFrom"]}' AND exp.DateTime <= '{formCol["dateTo"]}'" +
                            " GROUP BY" +
                            " exp.ShipId, s.Name" +
                            " ORDER BY" +
                            " COUNT(exp.Id) DESC;";

            List<string[]> res = new List<string[]>();
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        res.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString() });
                    }
            }

            return View("Query", res);
        }
    }
}