using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private MVCNetEntities db = new MVCNetEntities();

        // Number of categories shown per page on the Index view.
        private const int PageSize = 2;

        // GET: Categories
        public ActionResult Index(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            int totalItems = db.Categories.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var categories = db.Categories
                .OrderBy(c => c.CategoryName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(categories);
        }

        // Base address of the WebAPI Categories endpoint.
        private const string ApiCategoriesUrl = "https://localhost:44362/api/Categories";

        // GET: Categories/ApiIndex
        // Shows categories fetched from the WebAPI project instead of the local database.
        public async Task<ActionResult> ApiIndex()
        {
            // Allow the self-signed IIS Express dev certificate on localhost.
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };

            try
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(10);

                    var response = await client.GetAsync(ApiCategoriesUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.ApiError = string.Format(
                            "The API returned {0} ({1}). Make sure the WebAPI project is running.",
                            (int)response.StatusCode, response.ReasonPhrase);
                        return View(new List<Category>());
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<Category>>(json)
                                     ?? new List<Category>();
                    return View(categories);
                }
            }
            catch (TaskCanceledException)
            {
                ViewBag.ApiError = "The request to the API timed out. Make sure the WebAPI project is running.";
                return View(new List<Category>());
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ApiError = "Could not reach the API: " + ex.Message
                    + " Make sure the WebAPI project is running.";
                return View(new List<Category>());
            }
            catch (JsonException ex)
            {
                ViewBag.ApiError = "The API response could not be parsed: " + ex.Message;
                return View(new List<Category>());
            }
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description,Picture")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description,Picture")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
