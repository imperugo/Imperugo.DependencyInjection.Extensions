using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Host.Models;
using Sample.Host.Services;
using Sample.Package1.Services;
using Sample.Package2.Services;

namespace Sample.Host.Controllers
{
    public class HomeController : Controller
    {
	    private readonly IService1 service1;
	    private readonly IService2 service2;
	    private readonly IService3 service3;

	    public HomeController(IService1 service1, IService2 service2, IService3 service3)
	    {
		    this.service1 = service1;
		    this.service2 = service2;
		    this.service3 = service3;
	    }

	    public IActionResult Index()
	    {
		    this.ViewData["Message1"] = service1.SaySomething();
		    this.ViewData["Message2"] = service2.SaySomething();
		    this.ViewData["Message3"] = service2.SaySomething();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
