using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _4_29ImageLikes.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Library.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace _4_29ImageLikes.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private IHostingEnvironment _environment;
        public HomeController(IConfiguration configuration,IHostingEnvironment environment)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            Manager mgr = new Manager(_connectionString);
               return View(mgr.GetImages());
        }

        public IActionResult Upload()
        {
            return View();
        }
        public IActionResult AddImage(IFormFile imageName,string title)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageName.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "My_Images", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                imageName.CopyTo(stream);
            }
            Image i = new Image { ImageName = fileName, Title = title, Likes = 0 ,DateAdded=DateTime.Now};
           // i.ImageName = fileName; //fileName

            Manager mgr = new Manager(_connectionString);
             mgr.InsertImage(i);
            return Redirect("/");
        }
        public IActionResult Image(int id)
        {
            Manager mgr = new Manager(_connectionString);
            var i=  mgr.GetImageById(id);
            return View(i);
        }
        [HttpPost]
        public IActionResult addLike(int Id)
        {
            Manager mgr = new Manager(_connectionString);
            mgr.SetLike(Id);
            return Json(Id);
        }
        [HttpPost]
        public IActionResult GetLikes(int id)
        {
            Manager mgr = new Manager(_connectionString);
            int likes=mgr.GetLike(id);
            return Json(likes);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
