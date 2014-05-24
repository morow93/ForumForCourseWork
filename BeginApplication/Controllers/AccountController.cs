using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BeginApplication.Filters;
using BeginApplication.Models;
using Calabonga.Mvc.Extensions;
using System.Net.Mail;
using BeginApplication.Context;
using BeginApplication.Repository;
using System.Data;

namespace BeginApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        public IForumRepository repository;

        public AccountController(IForumRepository _repository)
        {
            repository = _repository;
        }

        #region Личный кабинет
        
        [Authorize]
        public ActionResult Index()
        {
            var model = repository.GetPrivateCabinet(WebSecurity.CurrentUserId);
            return View(model);
        }

        #endregion

        #region Вход

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            var user = new UserProfile();
            using(var context = new SimpleMembershipContext())
            {
                user = context.UserProfiles.FirstOrDefault(u => u.UserName == model.UserName || u.Email == model.UserName);
            }

            if (user != null && !user.IsDeleted && ModelState.IsValid)
            {
                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (WebSecurity.Login(user.UserName, model.Password, persistCookie: model.RememberMe))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Данные для входа указаны неверно.");
            return View(model);
        }

        #endregion
 
        #region Выход

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logoff(string returnUrl)
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Регистрация

        [AllowAnonymous]
        public ActionResult Captcha()
        {
            return new CaptchaResult();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [Captcher(MessageText = "Неверный код с картинки")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var context = new SimpleMembershipContext();
                    var user = context.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                    context.Dispose();

                    if (user == null)
                    {
                        WebSecurity.CreateUserAndAccount(
                            model.UserName, 
                            model.Password, 
                            new { 
                                Email = model.Email, 
                                Mobile = model.Mobile, 
                                RegistrationDate = DateTime.Now 
                            }
                        );
                        WebSecurity.Login(model.UserName, model.Password);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Электронный адрес уже существует. Введите другой электронный адрес.");
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        #endregion

        #region Смена пароля из личного кабинета

        public ActionResult ChangePassword()
        {
            ViewBag.HasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            bool hasLocAcc = ViewBag.HasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            if (hasLocAcc)
            {
                if (ModelState.IsValid)
                {
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        TempData["Message"] = "Пароль был успешно изменен";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный текущий пароль или недопустимый новый пароль.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "У пользователя нет локальной учетной записи.");
            }
            return View(model);
        }

        #endregion        

        #region Высылка пароля на почту

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            var user = new SimpleMembershipContext().UserProfiles.FirstOrDefault(u => u.Email.ToLower() == Email.ToLower());
            if (user == null)
            {
                TempData["Message"] = "Не существует пользователя с такой почтой.";
            }
            else
            {
                var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                
                var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { userName = user.UserName, resetToken = token }, "http") + "'>ссылке</a>";
                
                var subject = "Сброс пароля";
                var body = "<b>Для сброса пароля перейдите по </b>" + resetLink;

                bool flag = true;
                try {
                    SendEMail(Email, subject, body);                    
                }
                catch (Exception) {
                    flag = false;                    
                }

                if (flag)
                    TempData["Message"] = "Письмо для сброса пароля было отправлено на указанную почту";
                else
                    TempData["Message"] = "При отправке письме для сброса пароля произошла ошибка";
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userName, string resetToken)
        {
            return View(new RecoveryPasswordModel { Token = resetToken, UserName = userName });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(RecoveryPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                SimpleMembershipContext db = new SimpleMembershipContext();

                //ищем айди юзера по его имени
                var userid = (from i in db.UserProfiles
                              where i.UserName == model.UserName
                              select i.UserId).FirstOrDefault();

                //проверка соответствия айди юзера и токена 
                bool any = (from j in db.webpages_Memberships
                            where (j.UserId == userid)
                            && (j.PasswordVerificationToken == model.Token)
                            select j).Any();

                if (any == true)
                {
                    //сброс пароля
                    bool response = WebSecurity.ResetPassword(model.Token, model.NewPassword);

                    if (response == true)                    
                        TempData["Message"] = "Пароль успешно изменен. Теперь можете войти.";                    
                    else
                    {
                        TempData["Message"] = "Увы, пароль не был изменен.";
                    }
                }
                else
                {
                    TempData["Message"] = "Выявлено несоотвествие пользователя и токена сброса пароля.";
                }
            }
            else
            {
                ModelState.AddModelError("", "Невозможно выполнить сброс пароля.");
            }
            return View();
        }

        private void SendEMail(string email, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("derchepur@gmail.com", "1running1*");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("derchepur@gmail.com");
            msg.To.Add(new MailAddress(email));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

        #endregion

        #region Вспомогательные методы

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }

        #endregion

        #region Настройки приватности

        public ActionResult EditPrivateProperties()
        {
            var model = repository.GetPrivateProperties(WebSecurity.CurrentUserId);
            return View(model);            
        }

        [HttpPost]
        public ActionResult EditPrivateProperties(PrivatePropertiesModel model)
        {
            repository.SetPrivateProperties(model, WebSecurity.CurrentUserId);
            return RedirectToAction("Index");
        }

        #endregion

        #region Аватар

        [AllowAnonymous]
        public FileContentResult GetImage(int userId)
        {
            var user = repository.UserProfiles.FirstOrDefault(u => u.UserId == userId);
            if (user != null && user.ImageData != null)
            {
                return File(user.ImageData, user.ImageMimeType);
            }
            else
            {
                return null;
            }
        } 

        [HttpPost]
        public ActionResult ChangeAvatar(PrivateCabinetModel model, HttpPostedFileBase file)
        { 
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    using (var context = new SimpleMembershipContext())
                    {
                        var user = context.UserProfiles.FirstOrDefault(x => x.UserId == WebSecurity.CurrentUserId);
                        
                        user.ImageData = new byte[file.ContentLength];
                        file.InputStream.Read(user.ImageData, 0, file.ContentLength);
                        user.ImageMimeType = file.ContentType;

                        context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult GetThemesByUser(int? id)
        {
            int _id = id ?? WebSecurity.CurrentUserId;
            var model = repository.GetThemesByUser(_id);
            return PartialView(model);
        }

        #endregion
    }
}
