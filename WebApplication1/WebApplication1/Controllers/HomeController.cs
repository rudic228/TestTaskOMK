using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DocumentStorage storage = new DocumentStorage();
        List<string> listPost = new List<string>() { "Администратор", "Бухгалтер", "Архивист" };//для быстроты созданы локальные списки вместо перечислений
        List<string> listDocType = new List<string>() { "Сертификат качества", "Сертификат качества на покрытие",
            "Приказ на отгрузку", "Приложение"};
        List<string> listWorkShop = new List<string>() { "ТЭСЦ-1", "ТЭСЦ-3", "ТЭСЦ-5", "КПЦ" };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Post = listPost;
            return Redirect("~/Home/Enter");//быстрое решение проблемы с маршрутизацией
        }
        [HttpPost]
        public void Index(int post)
        {
            Program.UserRole = listPost[post];
            Response.Redirect("DocumentList");
            return;
        }
        public IActionResult Enter()
        {
            int i = 0;
            ViewBag.Post = listPost.Select(x=> new Tuple<int, string>(i++,x)).ToList();
            return View();
        }
        [HttpPost]
        public void Enter(int post)
        {
            Program.UserRole = listPost[post];
            Response.Redirect("DocumentList");
            return;
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DocumentCreate()
        {
            if (Program.UserRole == "Бухгалтер") return Redirect("~/Home/DocumentList");
            if (string.IsNullOrEmpty(Program.UserRole))
            {
                return Redirect("~/Home/Index");
            }
            int i = 0;
            ViewBag.DocType = listDocType.Select(x => new Tuple<int, string>(i++, x)).ToList();
            i = 0;
            ViewBag.WorkShop = listWorkShop.Select(x => new Tuple<int, string>(i++, x)).ToList();
            foreach (var parameter in Request.Query)
            {
                var key = parameter.Key;
                if (key == "id")
                {
                    ViewBag.Id = parameter.Value;
                }
            }
            return View();
        }
        [HttpPost]
        public void DocumentDelete()
        {
            if (Program.UserRole == "Бухгалтер") return;
            storage.Delete(Request.Form["id"]);
        }
        [HttpPost]
        public void DocumentSetReturn()
        {
            storage.SetStateReturn(Request.Form["id"]);
        }
        [HttpPost]
        public void DocumentSetAccept()
        {
            storage.SetStateAccept(Request.Form["id"]);
        }
        [HttpPost]
        public void DocumentCreate(string DocumentId, string DeliveryNumber, string Barcode, string Note, int DocType, int WorkShop)
        {
            if (Program.UserRole == "Бухгалтер") return;
            if (storage.GetElement(DocumentId) != null)
            {
                storage.Update(new Document()
                {
                    Id = DocumentId,
                    DeliveryNumber = DeliveryNumber,
                    Barcode = Barcode,
                    Note = Note,
                    DocumentType = listDocType[DocType],
                    Workshop = listWorkShop[WorkShop],
                    DateUpdate = DateTime.Now,
                    DateDocument = DateTime.Now,
                    RolePersonChange = Program.UserRole,
                    Stage = "Введен в архив"
                });
            }
            else
            {
                storage.Insert(new Document()
                {
                    Id = DocumentId,
                    DeliveryNumber = DeliveryNumber,
                    Barcode = Barcode,
                    Note = Note,
                    DocumentType = listDocType[DocType],
                    Workshop = listWorkShop[WorkShop],
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    DateDocument = DateTime.Now,
                    RolePersonChange = Program.UserRole,
                    Stage = "Введен в архив"// нужно также ввести enum
                });
            }
            Response.Redirect("DocumentList");
            return;
        }

        [HttpGet]
        public IActionResult DocumentList()
        {
            if (string.IsNullOrEmpty(Program.UserRole))
            {
                return Redirect("~/Home/Index");
            }
            foreach (var parameter in Request.Query)
            {
                var key = parameter.Key;
                if (key == "filter")
                {
                    return View(storage.GetFiltredList(parameter.Value));
                }
            }
            return View(storage.GetFullList());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
