using Microsoft.AspNetCore.Mvc;

namespace AssetTrackingSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly List<(string Username, string Password)> _validUsers = new()
        {
            ("admin1", "sifre1"),
            ("admin2", "sifre2"),
            ("admin3", "sifre3"),
        };

        [HttpGet]
        [Route("Auth/Login")]
        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Route("Auth/Login")]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            if (_validUsers.Any(u => u.Username == username && u.Password == password))
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        [HttpGet]
        [Route("Auth/Logout")]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}