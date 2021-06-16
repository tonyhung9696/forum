using forum.Data;
using forum.Models;
using forum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class UserController : Controller
    {
        private readonly SQLArticle _SQLArticle;
        private readonly SQLArticleReply _SQLArticleReply;
        private readonly SQLArticleLike _SQLArticleLike;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UserController(UserManager<ApplicationUser> userManager,
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
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                website = user.website,
                introduction = user.introduction,
                ExistingPhotoPath = user.photopath
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditUserViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            user.website = model.website;
            user.introduction = model.introduction;
            if (model.PhotoPath != null)
            {
                if (model.ExistingPhotoPath != null)
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                        "images", model.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }
                user.photopath = ProcessUploadedFile(model);
            }

            await userManager.UpdateAsync(user);
            return RedirectToAction("Edit");
        }
        private string ProcessUploadedFile(UserEditUserViewModel model)
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
            }

            return uniqueFileName;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("User/SearchUser/{name}")]
        public async Task<IActionResult> SearchUser(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            if (user != null)
            {
                int article = (_SQLArticle.GetAlltem().Where(w => w.name == user.UserName)).Count();
                int like = (_SQLArticleLike.GetAlltem().Where(w => w.userID == user.Id)).Count();
                int reply = ((_SQLArticleReply.GetAlltem().Where(w => w.name == user.UserName)).GroupBy(g=>g.articleID)).Count();

                var model = new UserInfoViewModel
                {
                    ExistingPhotoPath=user.photopath,
                    UserName = user.UserName,
                    website = user.website,
                    introduction = user.introduction,
                    Article = article,
                    like = like,
                    reply = reply
                };
                return View(model);
            }
            return null;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("User/Name/{name}")]
        public async Task<IActionResult> Name(string name, int? pageNumber)
        {
            
            var user = await userManager.FindByNameAsync(name);
            ViewData["name"] = name;
            int article = (_SQLArticle.GetAlltem().Where(w => w.name == user.UserName)).Count();
            int like = (_SQLArticleLike.GetAlltem().Where(w => w.userID == user.Id)).Count();
            int reply = ((_SQLArticleReply.GetAlltem().Where(w => w.name == user.UserName)).GroupBy(g => g.articleID)).Count();

            var userdetail = new UserInfoViewModel
            {
                ExistingPhotoPath = user.photopath,
                UserName = user.UserName,
                website = user.website,
                introduction = user.introduction,
                Article = article,
                like = like,
                reply = reply
            };
            ViewData["userdetail"] = userdetail;
            var model = _SQLArticle.GetAlltem().AsQueryable().Where(e => e.name == name).OrderByDescending(s => s.id);
            return View(await PaginatedList<Article>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, 10));
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("User/Like/{name}")]
        public async Task<IActionResult> Like(string name, int? pageNumber)
        {
            var user = await userManager.FindByNameAsync(name);
            ViewData["name"] = name;
            int article = (_SQLArticle.GetAlltem().Where(w => w.name == user.UserName)).Count();
            int like = (_SQLArticleLike.GetAlltem().Where(w => w.userID == user.Id)).Count();
            int reply = ((_SQLArticleReply.GetAlltem().Where(w => w.name == user.UserName)).GroupBy(g => g.articleID)).Count();

            var userdetail = new UserInfoViewModel
            {
                ExistingPhotoPath = user.photopath,
                UserName = user.UserName,
                website = user.website,
                introduction = user.introduction,
                Article = article,
                like = like,
                reply = reply
            };
            ViewData["userdetail"] = userdetail;
            var model = (_SQLArticleLike.GetAlltem().Join(_SQLArticle.GetAlltem(), str1 => str1.userID, str2 => str2.userID, (str1, str2) => str2)).AsQueryable().Where(w=>w.userID==user.Id).OrderByDescending(str2 => str2.id);
            List<int> IDs = new List<int>();
            foreach (var item in model)
            {
                if (!IDs.Contains(item.id))
                {
                    IDs.Add(item.id);
                }
            }
            var IAmodel = _SQLArticle.GetAlltem().AsQueryable().Where(w => IDs.Contains(w.id)).OrderByDescending(str1 => str1.id);
            return View(await PaginatedList<Article>.CreateAsync(IAmodel.AsNoTracking(), pageNumber ?? 1, 10));
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("User/Reply/{name}")]
        public async Task<IActionResult> Reply(string name, int? pageNumber)
        {
            var user = await userManager.FindByNameAsync(name);
            ViewData["name"] = name;
            int article = (_SQLArticle.GetAlltem().Where(w => w.name == user.UserName)).Count();
            int like = (_SQLArticleLike.GetAlltem().Where(w => w.userID == user.Id)).Count();
            int reply = ((_SQLArticleReply.GetAlltem().Where(w => w.name == user.UserName)).GroupBy(g => g.articleID)).Count();

            var userdetail = new UserInfoViewModel
            {
                ExistingPhotoPath = user.photopath,
                UserName = user.UserName,
                website = user.website,
                introduction = user.introduction,
                Article = article,
                like = like,
                reply = reply
            };
            ViewData["userdetail"] = userdetail;
            List<int> IDs = new List<int>();
            var model = (_SQLArticle.GetAlltem().Join(_SQLArticleReply.GetAlltem(), str1 => str1.userID, str2 => str2.userID, (str1, str2) => str1)).AsQueryable().Where(w => w.userID == user.Id).OrderByDescending(str1 => str1.id);
            foreach (var item in model)
            {
                if (!IDs.Contains(item.id))
                {
                    IDs.Add(item.id);
                }
            }
            var IAmodel = _SQLArticle.GetAlltem().AsQueryable().Where(w => IDs.Contains(w.id)).OrderByDescending(str1 => str1.id);
            return View(await PaginatedList<Article>.CreateAsync(IAmodel.AsNoTracking(), pageNumber ?? 1, 10));
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
