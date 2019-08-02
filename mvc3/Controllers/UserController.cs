using mvc3.Areas.AdminPanel.Models;
using mvc3.Areas.AdminPanel.Models.Repository;
using mvc3.Areas.AdminPanel.Models.Security;
using mvc3.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mvc3.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        UserRepository repo = new UserRepository(new kitapProjesiEntities());
        public ActionResult Index()
        {
            return View();
        }
        [NonAction]
        public bool isExistUser(string username)
        {
            var result = repo.Listele().Where(a => a.email == username).FirstOrDefault();
            return result == null ? false : true;
        }
        
        // Mail Doğrulaması(aktivasyon kodu için)
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;

            //db.Configuration.ValidateOnSaveEnabled = false;
            var result = repo.Listele().Where(a => a.activationCode == new Guid(id).ToString()).FirstOrDefault();
            if (result != null)
            {
                result.isMailVerified = true;
                repo.Guncelle(result);
                status = true;
            }
            else
            {
                ViewBag.Message = "Geçersiz istek";
            }

            ViewBag.status = status;
            return View("Index");
        }

        // yeni bir kullanıcı kaydetmek için
        [HttpPost]
        public ActionResult Register(user _user)
        {
            string message = "";
            bool status = false;
            if (isExistUser(_user.email))
            {
                message = "Böyle bir kullanıcı Mevcut";
                ViewBag.message = message;
                ViewBag.status = status;
                
                return View("Index");
            }
            _user.activationCode = Guid.NewGuid().ToString();
            //password şifreleniyor
            _user.password = Crypto.Hash(_user.password);
            _user.rePassword = Crypto.Hash(_user.rePassword);
            _user.isMailVerified = false;
            _user.createdDate = DateTime.Now;
            _user.roleId = 2;
            repo.Kaydet(_user);
            //email gönderecek.
            SendVerificationLinkEmail(_user.email, _user.activationCode);
            message= "Kaydınız başarılı şekilde gerçekleştirildi. Hesap aktivasyon linki "
                 + _user.email + " adresinize gönderilmiştir.Doğrulamayı unutmayınız.";
            ViewBag.message = message;
            status = true;
            ViewBag.status = status;
            return View("Index");
        }

        // kullanıcının login view'ine gönder
        public ActionResult Login()
        {
            return View();
        }

        // login yapıldığında
        [HttpPost]
        public ActionResult Login(Login login, string ReturnUrl)
        {
            string message = "";
            bool status=false;
            int sayac = 0;

            var user = repo.Listele().Where(x => x.email == login.email).FirstOrDefault();
            if (user != null)
            {
                // mail aktivasyon yapılmışmı?
                bool verified = user.isMailVerified ?? false;
                // yapılmamışsa
                if (!verified)
                {
                    ViewBag.Message = "Aktivasyon için mail hesabınızı kontrol edin.";
                    sayac++;
                    // login giriş sayısını güncelle
                    user.loginAttempt = sayac;
                    repo.Guncelle(user);
                    return View();
                }
                // girdiğiniz şifre şifreleniyor.
                login.password = Crypto.Hash(login.password);

                // veritabanındaki şifreyle girdiğin şifre kıyaslanıyor.şifreler tutuyorsa
                if (string.Compare(login.password, user.password) == 0)
                {
                    user.loginTime = DateTime.Now;
                    user.loginAttempt = sayac;
                    repo.Guncelle(user);
                    Session["username"] = login.email;
                    // cookie
                    // cookie kalıcılığı için süre
                    // beni hatırla işeretlendiğinde 60 dk, işaretlenmediğinde 10 dk;
                    int timeout = login.rememberMe ? 60 : 10;
                    // yeni bir ticket oluşturuluyor.
                    var ticket = new FormsAuthenticationTicket(login.email, login.rememberMe, timeout);
                    // ticket şifreleniyor.
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    // yeni cookie oluşturuluyor
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;

                    FormsAuthentication.SetAuthCookie("userName", login.rememberMe);
                    Response.Cookies.Add(cookie);
                    // giren kullanıcı admin ise
                    if (user.roleId == 1)
                    {
                        string url = "~/AdminPanel/Kategori";
                        return Redirect(url);
                    }
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Shop");
                    }
                   
                }
                // şifre tutmadıysa
                else
                {
                    sayac++;
                    user.loginAttempt = sayac;
                    repo.Guncelle(user);
                    message = "Şifreniz eşleşmiyor!";
                }
            }
            // yanlış bir email/kullanıcı adı girilmişse
            else
            {
                message = "Email/kullanıcı yanlış!";
            }
            ViewBag.status = status;
            ViewBag.message = message;
            return View();
        }

        // forgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(forgotPassword fp)
        {
            string message = "";
            bool status = false;
            if (fp!=null)
            {
                var user = repo.Listele().Where(x => x.email == fp.email).FirstOrDefault();
                user.resetCode = Guid.NewGuid().ToString();
                repo.Guncelle(user);
                // mail göndereceğiz
                SendResetPassword(fp.email, user.resetCode);
                message = "Parola Sıfırlama işlemi başarılı şekilde gerçekleştirildi. Parola sifirlama linki "
                          + fp.email + " adresinize gönderilmiştir.";
                status = true;
            }
            else
            {
                message = "Böyle bir Email mevcut değil";
                status = false;
            }
            ViewBag.status = status;
            ViewBag.message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            resetPassword model = new resetPassword()
            {
                resetCode = id
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(resetPassword rp)
        {
            bool status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                var user = repo.Listele().Where(x => x.resetCode == rp.resetCode).FirstOrDefault();
                user.password = Crypto.Hash(rp.newPassword);
                user.rePassword = Crypto.Hash(rp.comfirmPassword);
                repo.Guncelle(user);
                status = true;
                message = "Parolanız değiştirilmiştir.";
            }
            else
            {
                status = false;
                message = "Hata";
            }
            ViewBag.status = status;
            ViewBag.message = message;
            return View();
        }
        // logout
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // kullanıcıya reset kodu göndermek için mail
        [NonAction]
        public void SendResetPassword(string emailID, string resetCode)
        {
            SmtpSection network = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                var verifyUrl = "/User/ResetPassword/" + resetCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var fromEmail = new MailAddress(network.Network.UserName, "Bookstore Parola Sıfırlama");
                var toEmail = new MailAddress(emailID);

                string subject = "Bookstore parolası sıfırlanacaktır!";
                string body = "<br/><br/>Bookstore üyeliğinizin parolası sıfırlanacaktır. " +
                              "Parolanızı sıfırlamak için aşağıdaki linke tıklayınız" +
                      " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                var smtp = new SmtpClient
                {
                    Host = network.Network.Host,
                    Port = network.Network.Port,
                    EnableSsl = network.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = network.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(network.Network.UserName, network.Network.Password)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // kullanıcıya aktivasyon kodu göndermek için mail
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            SmtpSection network = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                var verifyUrl = "/User/VerifyAccount/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var fromEmail = new MailAddress(network.Network.UserName, "BookStore Kitap Satış Üyeliği");
                var toEmail = new MailAddress(emailID);

                string subject = "BookStore sitesine Hoşgeldiniz!";
                string body = "<br/><br/>BookStore hesabnız başarıyla oluşturulmuştur. Hesabınız aktive etmek için aşağıdaki linke tıklayınız" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                var smtp = new SmtpClient
                {
                    Host = network.Network.Host,
                    Port = network.Network.Port,
                    EnableSsl = network.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = network.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(network.Network.UserName, network.Network.Password)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}