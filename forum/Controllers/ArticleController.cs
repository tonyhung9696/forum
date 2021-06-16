using forum.Data;
using forum.Models;
using forum.ViewModels;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly SQLArticle _SQLArticle;
        private readonly SQLArticleReply _SQLArticleReply;
        private readonly SQLArticleLike _SQLArticleLike;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ArticleController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, SQLArticle SQLArticle, SQLArticleReply SQLArticleReply, SQLArticleLike SQLArticleLike, IHostingEnvironment hostingEnvironmen)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _SQLArticle = SQLArticle;
            _SQLArticleReply = SQLArticleReply;
            _SQLArticleLike = SQLArticleLike;
            _hostingEnvironment = hostingEnvironmen;
        }
        [HttpGet]
        [Route("Article/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var model = _SQLArticle.GetAlltem().Where(w=>w.id==id).FirstOrDefault();
            if (model == null)
            {
                Response.StatusCode = 404;
                return View("ArticleNotFound", id);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Article model)
        {
            var user = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                Article article = _SQLArticle.GetDetail(model.id);
                article.title = model.title;
                article.articleType = model.articleType;
                article.tag = model.tag;
                article.content = model.content;
                _SQLArticle.Update(article);
                return RedirectToAction("index");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article model)
        {
            var user = await userManager.GetUserAsync(User);
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                Article article = new Article
                { 
                    userID=user.Id,
                    name=user.UserName,
                    articleType=model.articleType,
                    title=model.title,
                    content=model.content,
                    tag=model.tag,
                    like=0,
                    clickRateDaily=model.clickRateDaily,
                    clickRateWeekly=model.clickRateWeekly,
                    top=0,
                    datetime=DateTime.Now,
                    ReplyCount=model.ReplyCount,
                    Replydatetime=DateTime.Now,
                    view=0
                };

                _SQLArticle.Add(article);
                return RedirectToAction("index");
            }

            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            var model = _SQLArticle.GetAlltem().AsQueryable();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.title.Contains(searchString)
                                       || s.name.Contains(searchString));
            }
            switch (sortOrder)
            {
                default:
                    model = model.OrderByDescending(s => s.id);
                    break;
            }


            return View(await PaginatedList<Article>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, 10));
        }
        [HttpGet]
        public ViewResult Edit(string id)
        {
            Article detail = _SQLArticle.GetDetail(Convert.ToInt32(id));

            if (detail == null)
            {
                Response.StatusCode = 404;
                return View("ArticleNotFound", id);
            }
            return View(detail);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await userManager.GetUserAsync(User);
            Article detail = _SQLArticle.GetDetail(Convert.ToInt32(id));

            if (detail == null)
            {
                Response.StatusCode = 404;
                return View("ArticleNotFound", id);
            }
            var model = _SQLArticleReply.GetAlltem().Where(e => e.articleID == id);
            int count = (_SQLArticleLike.GetAlltem().Where(e => e.ArticleID == id && e.userID == user.Id)).Count();
            bool like = false;
            if (count>=1)
            {
                like = true;
            }
            ArticleContentViewModel articleContentViewModel = new ArticleContentViewModel()
            {
                //Employee = _employeeRepository.GetEmplyee(id??1),
                Article = detail,
                ArticleReplyList = model,
                like = like
            };
            return View(articleContentViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> DetailGood(string id)
        {
            var user = await userManager.GetUserAsync(User);
            int count = (_SQLArticleLike.GetAlltem().Where(w => w.ArticleID == id && w.userID == user.Id)).Count();
            Article article = _SQLArticle.GetDetail(Convert.ToInt32(id));
            if (count<=0)
            {
                article.like = article.like + 1;
                _SQLArticle.Update(article);
                ArticleLike ArticleLike = new ArticleLike
                {
                    ArticleID=id,
                    userID=user.Id
                };
                _SQLArticleLike.Add(ArticleLike);
                return RedirectToAction("detail", new { id = article.id });
            }
            else
            {
                article.like = article.like - 1;
                _SQLArticle.Update(article);
                var articleLikeModel = (_SQLArticleLike.GetAlltem().Where(w => w.ArticleID == id && w.userID == user.Id)).FirstOrDefault();
                int articleId = articleLikeModel.id;
                _SQLArticleLike.Delete(articleId);
                return RedirectToAction("detail", new { id = article.id });
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Detail(ArticleContentViewModel ARmodel)
        {
            var user = await userManager.GetUserAsync(User);

            ArticleReply ArticleReply = new ArticleReply
            {
                userID = user.Id,
                articleID = ARmodel.ArticleReply.articleID,
                name = user.UserName,
                content = ARmodel.ArticleReply.content,
                datetime = DateTime.Now
            };
            _SQLArticleReply.Add(ArticleReply);

            Article article = _SQLArticle.GetDetail(Convert.ToInt32(ARmodel.ArticleReply.articleID));
            article.Replydatetime = DateTime.Now;
            article.ReplyCount = article.ReplyCount + 1;
            _SQLArticle.Update(article);
            return RedirectToAction("detail", new { id = ARmodel.ArticleReply.articleID });
        }
        [AllowAnonymous]
        public ViewResult Search(string text)
        {

            var model = _SQLArticle.GetAlltem().Where(e => e.title.Contains(text));
            if (model == null)
            {
                Response.StatusCode = 404;
                return View("ArticleNotFound", 0);
            }
            return View(model);
        }
        [AllowAnonymous]
        [Route("SearchTag/{tag}/{pageNumber?}")]
        public async Task<IActionResult> SearchTag(string tag, int? pageNumber)
        {
            ViewData["tag"] = tag;
            var model = _SQLArticle.GetAlltem().AsQueryable().Where(e => e.tag.Contains(tag)).OrderByDescending(s => s.id);
            return View(await PaginatedList<Article>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, 10));
        }
        [AllowAnonymous]
        [Route("SearchType/{name}/{type}/{pageNumber?}")]
        public async Task<IActionResult> SearchType(string name, string type, int? pageNumber)
        {
            ViewData["name"] = name;
            ViewData["type"] = type;
            var model = _SQLArticle.GetAlltem().AsQueryable().Where(e => e.articleType.Contains(type) && e.name==name).OrderByDescending(s => s.id);
            return View(await PaginatedList<Article>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, 10));
        }
        [AllowAnonymous]
        [Route("SearchUser/{name}/{pageNumber?}")]
        public async Task<IActionResult> SearchUser(string name, int? pageNumber)
        {
            ViewData["name"] = name;
            var model = _SQLArticle.GetAlltem().AsQueryable().Where(e => e.name == name).OrderByDescending(s => s.id);
            return View(await PaginatedList<Article>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, 10));
        }
        private async Task<string> ProcessUploadedFile(Upload model)
        {
            string uniqueFileName = null;

            if (model.PhotoPath != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoPath.CopyTo(fileStream);
                }
                var client = new ImgurClient("afc6b28ae1ae66c", "0c1d20c3749bc3c9820082db350e42622e9ce952");
                var endpoint = new ImageEndpoint(client);
                var ms = new MemoryStream();
                model.PhotoPath.OpenReadStream().CopyTo(ms);
                byte[] Value = ms.ToArray();
                uniqueFileName = (await endpoint.UploadImageBinaryAsync(Value)).Link;
            }

            return uniqueFileName;
        }
        public async Task<IActionResult> UploadFilesWihtLocation()
        {
            string hoststr = _hostingEnvironment.WebRootPath;

            string[] strFileNames;
            ///string url = "";
            try
            {
                long size = 0;
                var files = Request.Form.Files;
                strFileNames = new string[files.Count];
                int i = 0;
                foreach (var f in files)
                {
                    if (IsValidExtension(f))
                    {
                        var client = new ImgurClient("afc6b28ae1ae66c", "0c1d20c3749bc3c9820082db350e42622e9ce952");
                        var endpoint = new ImageEndpoint(client);
                        var ms = new MemoryStream();
                        f.CopyTo(ms);
                        byte[] Value = ms.ToArray();
                        strFileNames[0] = (await endpoint.UploadImageBinaryAsync(Value)).Link;
                    }
                    else
                    {
                        strFileNames = new string[1];
                        strFileNames[0] = "Invalid File";
                    }
                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                strFileNames = new string[1];
                strFileNames[0] = ex.Message;
            }

            return Json(strFileNames[0]);
        }
        private bool IsValidExtension(IFormFile filename)
        {
            bool isValid = false;
            Char delimiter = '.';
            string fileExtension;
            string[] imgTypes = new string[] { "png", "jpg", "gif", "jpeg" };
            string[] documentsTypes = new string[] { "doc", "docx", "xls", "xlsx", "pdf", "ppt", "pptx" };
            string[] collTypes = new string[] { "zip", "rar", "tar" };
            string[] VideoTypes = new string[] { "mp4", "flv", "mkv", "3gp" };
            fileExtension = filename.FileName.Split(delimiter).Last();
            // fileExtension = substrings[substrings.Length - 1].ToString();
            int fileType = 0;
            if (imgTypes.Contains(fileExtension.ToLower()))
            {
                fileType = 1;
            }
            else if (documentsTypes.Contains(fileExtension.ToLower()))
            {
                fileType = 2;
            }
            else if (collTypes.Contains(fileExtension.ToLower()))
            {
                fileType = 3;
            }
            else if (VideoTypes.Contains(fileExtension.ToLower()))
            {
                fileType = 4;
            }
            switch (fileType)
            {
                case 1:
                    if (imgTypes.Contains(fileExtension.ToLower()))
                    {
                        isValid = true;
                    }
                    break;
                case 2:
                    if (documentsTypes.Contains(fileExtension.ToLower()))
                    {
                        isValid = false;
                    }
                    break;
                case 3:
                    if (collTypes.Contains(fileExtension.ToLower()))
                    {
                        isValid = false;
                    }
                    break;
                case 4:
                    if (VideoTypes.Contains(fileExtension.ToLower()))
                    {
                        isValid = true;
                    }
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
        private string GetNewFileName(string filenamestart, IFormFile filename)
        {
            Char delimiter = '.';
            string fileExtension;
            string strFileName = string.Empty;
            strFileName = DateTime.Now.ToString().
                Replace(" ", string.Empty).
                Replace("/", "-").Replace(":", "-");
            fileExtension = filename.FileName.Split(delimiter).Last();
            Random ran = new Random();
            strFileName = $"{ filenamestart}_{ran.Next(0, 100)}_{strFileName}.{fileExtension}";
            return strFileName;
        }
    }
}
