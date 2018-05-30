using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FunnyV1.Models;

namespace FunnyV1.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                using (var db = new UserContext())
                {
                    Role alienRole = new Role { Name = "alien" };
                    Role humanRole = new Role { Name = "human" };
                    db.Roles.Add(alienRole);
                    db.Roles.Add(humanRole);
                    Human user = db.Humen.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);
                    Alien alien = db.Aliens.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);
                    
                    if (user != null || alien != null)
                    {
                        if (alien != null)
                        {
                            //var connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\FunnyV1.mdf';Integrated Security=True";
                            var connString = @"workstation id=FunnyV11.mssql.somee.com;packet size=4096;user id=Aldun_SQLLogin_1;pwd=6yy6wjrql5;data source=FunnyV11.mssql.somee.com;persist security info=False;initial catalog=FunnyV11
";
                            using (var conn = new SqlConnection(connString))
                            {
                                conn.Open();
                                using (var cmd = new SqlCommand("SELECT m.HumanId FROM Aliens a FULL OUTER JOIN Murders m ON m.AlienId = a.Id  WHERE a.Email = '"+ alien.Email.ToString() +"';", conn))//+ "'"+ alien.Email.ToString() + "';", conn))
                                using (var reader = cmd.ExecuteReader())
                                    while (reader.Read())
                                    {
                                        reader.IsDBNull(reader.GetOrdinal("HumanId"));
                                        int? p = reader.IsDBNull(reader.GetOrdinal("HumanId")) ? -1 : (int?)reader[0];
                                        if (p != -1)
                                            ModelState.AddModelError("", "Your dead, moron/Ти мертвий, дурню");
                                        else
                                        {
                                            FormsAuthentication.SetAuthCookie(model.Name, true);
                                            return RedirectToAction("Index", "Home");
                                        }
                                    }
                            }
                        }
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Name, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Uncorrect input");
                    }
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult Info()
        {
            var emailOfCurrent = User.Identity.Name;
            using (var db = new UserContext())
            {
                Human human = db.Humen.FirstOrDefault(d => d.Email == emailOfCurrent);
                Alien alien = db.Aliens.Where(d => d.Email == emailOfCurrent).FirstOrDefault();

                if (human != null)
                {
                    if (human.SpaceShipId != null)
                    {
                        ViewBag.Description = human.SpaceShip.ShipType.Description;
                        ViewBag.Type = human.SpaceShip.ShipType.Type;
                    }
                    return View("InfoHuman", human);
                }
                else if (alien != null)
                    return View("InfoAlien", alien);
            }
            return View();
        }
        [HttpGet]
        public ActionResult RegFirstStep()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegFirstStep(string type)
        {
            if (type == "alien")
            {
                return RedirectToAction("IsAlien");//View("IsRealAlien");
            }
            else if (type == "human")
            {
                return View("RegisterHuman");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult IsAlien()
        {
            string[] questions = new string[6];
            string[][] answers = new string[6][];
            for (int i = 0; i < 6; ++i)
                answers[i] = new string[4];
            answers[0][0] = "Хоча б раз в день"; answers[0][1] = "Раз в декілька днів"; answers[0][2] = "Раз в тиждень"; answers[0][3] = "Вмиватись не потрібно. Одяг всмоктує бруд. Потрібно лиш прати одяг.";
            questions[0] = "Як часто потрібно приймати душ?";
            answers[1][0] = "Я погано ладнаю з дітьми, але колись напевне заведу."; answers[1][1] = "Люблю дітей, в майбутньому планую завести сім'ю."; answers[1][2] = "Чайлд фрі! Личинки не потрібні."; answers[1][3] = "Вже їх маю. Дуже їх люблю.";
            questions[1] = "Яке ваше ставлення до дітей?";
            answers[2][0] = "Хочу зайнятись точними науками"; answers[2][1] = "Планую занятись творчістю, можливо напишу книгу."; answers[2][2] = "Буду сидіти на шиї у батьків, а потім жити на виплати по безробіттю."; answers[2][3] = "Попробую обманом дістати квартиру якоїсь бабусі.";
            questions[2] = "Які у вас плани на майбутнє?";
            answers[3][0] = "Жінка? Це що таке? Це нею пиво відкривають?"; answers[3][1] = "Матріархат - наше майбутнє. Чоловіки бридкі й брудні тварини"; answers[3][2] = "Чоловіки і жінки рівні."; answers[3][3] = "Існує велика різноманістність гендерів і всі вони рівні.";
            questions[3] = "Яке, на вашу думку, повинна займати місце жінка в сучасному суспільстві?";
            answers[4][0] = "Олег Винник"; answers[4][1] = "Михайло Поплавський"; answers[4][2] = "Вітас"; answers[4][3] = "Андрій Мацевко";
            questions[4] = "Яку музику ви слухаєте?";
            answers[5][0] = "Слава Україні"; answers[5][1] = "Дивітьця"; answers[5][2] = "Дзень добри"; answers[5][3] = "Ой вей";
            questions[5] = "Як ви вітаєтесь із своїм оточенням?";
            ViewBag.ra = "Чайлд фрі! Личинки не потрібні.";
            Random r = new Random();
            int rInt = r.Next(0, 6);
            switch (rInt)
            {
                case 0:
                    ViewBag.questions = questions[0];
                    ViewBag.answers = answers[0];
                    ViewBag.ra = answers[0][3];
                    break;
                case 1:
                    ViewBag.questions = questions[1];
                    ViewBag.answers = answers[1];
                    ViewBag.ra = answers[1][2];
                    break;
                case 2:
                    ViewBag.questions = questions[2];
                    ViewBag.answers = answers[2];
                    ViewBag.ra = answers[2][2];
                    break;
                case 3:
                    ViewBag.questions = questions[3];
                    ViewBag.answers = answers[3];
                    ViewBag.ra = answers[3][0];
                    break;
                case 4:
                    ViewBag.questions = questions[4];
                    ViewBag.answers = answers[4];
                    ViewBag.ra = answers[4][3];
                    break;
                case 5:
                    ViewBag.questions = questions[5];
                    ViewBag.answers = answers[5];
                    ViewBag.ra = answers[5][1];
                    break;
            }
            //ViewBag.questions = questions;
            //ViewBag.ra = answers;
            return View("IsRealAlien"); 
        }

        [HttpPost]
        public ActionResult IsAlien(FormCollection formCollection)
        {
            string ra = formCollection["rightanswer"];
            string a = formCollection["questions"];
            ViewBag.m = "You are just miserable human. Don't make me laugh.";
            
            if (ra == a)
                return View("RegisterAlien");
            else
                return View("NotReal");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAlien(RegisterAlienModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UserContext())
                {
                    Human user = db.Humen.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);
                    Human human = db.Humen.FirstOrDefault(u => u.Email == model.Name);
                    Alien alien = db.Aliens.FirstOrDefault(u => u.Email == model.Name);
                    if (user == null && human == null && alien == null)
                    {
                        db.Aliens.Add(new Alien
                        {
                            Email = model.Name,
                            Password = model.Password,
                            RoleId = 1,
                            FirstName = model.FirstName,
                            SecondName = model.SecondName,
                            ThirdName = model.ThirdName,
                            FourthName = model.FourthName,
                            Age = model.Age
                        });
                        db.SaveChanges();
                        alien = db.Aliens.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                        if (alien != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Name, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    else
                        ModelState.AddModelError("", "Alien with the same email is already exist");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterHuman(RegisterHumanModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UserContext())
                {
                    Human user = db.Humen.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);
                    Human human = db.Humen.FirstOrDefault(u => u.Email == model.Name);
                    Alien alien = db.Aliens.FirstOrDefault(u => u.Email == model.Name);
                    if (user == null && human == null && alien == null )
                    {

                        db.Humen.Add(new Human
                        {
                            Email = model.Name,
                            Password = model.Password,
                            RoleId = 2,
                            FirstName = model.FirstName,
                            SecondName = model.SecondName,
                            Age = model.Age

                        });
                        db.SaveChanges();
                        human = db.Humen.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                        if (human != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Name, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        ModelState.AddModelError("", "Human with the same email is already exist");
                }
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}