using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCCoreApp.Models;

namespace MVCCoreApp.Controllers
{
    public class HomeController : Controller
    {

        private Repository repo;

        public HomeController(Repository rRepo)
        {
            repo = rRepo;
        }


        public IActionResult Index()
        {
            return View();
        }


        public ViewResult Things() => View(repo.Things.OrderBy(c => c.Name));


        public IActionResult AddThing()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddThing(Thing thing)
        {

            if (ModelState.IsValid)
            {
                var thingExists = repo.Things.Any(t => t.Name == thing.Name);
                if (!thingExists)
                {
                    repo.SaveThing(thing);
                    return RedirectToAction("Things");
                }

                ViewData["Message"] = "A thing with this name already exists.";

                return View("CustomError");
            }
            return View();
        }

        public IActionResult EditThing(int thingId)
        {
            if (ModelState.IsValid)
            {
                var selectedThing = repo.Things.SingleOrDefault(t => t.Id == thingId);
                if (selectedThing != null)
                {
                    return View("EditThing", selectedThing);
                }

                ViewData["Message"] = "A thing with this id cannot be found.";

                return View("CustomError");                
            }
            return View();
        }



        [HttpPost]
        public IActionResult EditThing(Thing thing)
        {

            if (ModelState.IsValid)
            {
                repo.SaveThing(thing);

                return RedirectToAction("Things");
            }

            return View();
        }



        public IActionResult ConfirmDeletion(int thingId)
        {
            return View(repo.Things.SingleOrDefault(t => t.Id == thingId));
        }

        public IActionResult DeleteThing(int thingId)
        {

            repo.DeleteThing(thingId);

            return RedirectToAction("Things");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
