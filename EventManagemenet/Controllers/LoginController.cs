using EventManagemenet.PasswordHash;
using EventManagemenetApp.DataAccess.Interface;
using EventManagemenetApp.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace EventManagemenet.Controllers
{
    public class LoginController : Controller
    {
        ILogin _ILogin;

        public LoginController(ILogin ILogin)
        {
            _ILogin = ILogin;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginViewModel.Username) && !string.IsNullOrEmpty(loginViewModel.Password))
                {
                    var Username = loginViewModel.Username;
                    var password = EncryptionLibrary.EncryptText(loginViewModel.Password);

                    var result = _ILogin.Login(Username, password);

                    if (result != null)
                    {
                        if (result.ID == 0 || result.ID < 0)
                        {
                            ViewBag.errormessage = "Entered Invalid Username and Password";
                        }
                        else
                        {
                            var RoleID = result.RoleID;
                            ClearOldCookies(); //Remove Anonymous_Cookies

                            //HttpContext.Session.SetString("UserID", Convert.ToString(result.ID));
                            //HttpContext.Session.SetString("RoleID", Convert.ToString(result.RoleID));
                            //HttpContext.Session.SetString("Username", Convert.ToString(result.Username));
                            if (RoleID == 1)
                            {
                                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                            }
                        }
                    }
                    else
                    {
                        ViewBag.errormessage = "Entered Invalid Username and Password";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public void ClearOldCookies()
        {
            if (Request.Cookies["EventChannel"] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("EventChannel", "", option);
            }
        }
    }
}
