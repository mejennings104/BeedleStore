using BeedleStore.DATA.EF.Models;
using BeedleStore.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Diagnostics;
using System.Net.Mail;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace BeedleStore.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly BeedleStoreContext _context;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, BeedleStoreContext context)
        {
            _logger = logger;
            _config = config;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Where(p => !p.IsDiscontinued).Take(4).Include(p => p.Category);

            return View(products.ToList());
        }

        #region Contact Form

        //GET - Contact
        public IActionResult Contact()
        {
            return View();
        }

        //POST - Contact
        [HttpPost]
        //public IActionResult Contact(ContactViewModel cvm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(cvm);
        //    }

        //    string message = $"You have received a new email from your site's contact form! <br />" +
        //        $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />Message: {cvm.Message}";
        //    var mm = new MimeMessage();

        //    mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));
        //    mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

        //    mm.Subject = cvm.Subject;
        //    mm.Body = new TextPart("HTML") { Text = message };
        //    mm.Priority = MessagePriority.Urgent;
        //    mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect(_config.GetValue<string>("Credentials:Email:Client"));
        //        client.Authenticate(
        //            _config.GetValue<string>("Credentials:Email:User"),
        //            _config.GetValue<string>("Credentials:Email:Password")
        //            );
        //        try
        //        {
        //            client.Send(mm);
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = $"There was an error processing your request. Please " +
        //                $"try again later.<br />Error Messag: {ex.StackTrace}";

        //            return View(cvm);
        //        }
        //    }
        //    return View("EmailConfirmation", cvm);
        //}
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult NewProducts()
        {
            

            return View();
        }
    }
}