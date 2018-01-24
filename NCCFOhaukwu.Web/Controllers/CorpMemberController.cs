using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MySql.BLL.Service;
using NCCFOhaukwu.Web.Models;
using PagedList;

namespace NCCFOhaukwu.Web.Controllers
{
    public class CorpMemberController : BaseController
    {
        private ApplicationUserManager _userManager;

        public CorpMemberController(IService service)
            : base(service)
        {
        }

        public CorpMemberController(IService service, ApplicationUserManager userManager)
            : base(service)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Access(Resources.CorpMember, Operations.Read)]
        public async Task<ActionResult> Index(string search, int? page)
        {
            var corpmembers = await Service.CorpMember.GetAsync();
            if (string.IsNullOrEmpty(search)) return View(corpmembers.ToPagedList(page ?? 1, 10));

            corpmembers = corpmembers.Where(x => x.SearchTag.Contains(search.ToLower())).ToList();
            
            ViewBag.search = search;

            return View(corpmembers.ToPagedList(page ?? 1, 10));
        }

        [Access(Resources.CorpMember, Operations.Read)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var corpMember = await Service.CorpMember.GetAsync(id);
            if (corpMember == null)
            {
                return HttpNotFound();
            }
            return View(corpMember);
        }

        [Access(Resources.CorpMember, Operations.Create)]
        public ActionResult Create()
        {
            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
            ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");
            return View(new CorpMembers());
        }

        [Access(Resources.CorpMember, Operations.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CorpMembers corpMember, HttpPostedFileBase miniUpload,
            HttpPostedFileBase majorUpload)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed = userRoles.Any(userRole => userRole.Contains(corpMember.PpaTown));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var otherName = corpMember.OtherName == null ? null : corpMember.OtherName.ToLower();
                    var corpmember = new CorpMember
                    {
                        BatchId = corpMember.BatchId,
                        AcademicInfo = corpMember.AcademicInfo,
                        PhoneNumber = corpMember.PhoneNumber,
                        Day = int.Parse(corpMember.Day),
                        FirstName = corpMember.FirstName,
                        Gender = corpMember.Gender,
                        IsGeneral = corpMember.IsGeneral,
                        IsLeader = corpMember.IsLeader,
                        LastName = corpMember.LastName,
                        Lga = corpMember.Lga,
                        Year = int.Parse(corpMember.Year),
                        YearId = corpMember.YearId,
                        MaritalStatus = corpMember.MaritalStatus,
                        Month = corpMember.Month,
                        NccfInfo = corpMember.NccfInfo,
                        OtherName = corpMember.OtherName,
                        PersonalInfo = corpMember.PersonalInfo,
                        StateOfOrigin = corpMember.StateOfOrigin,
                        PpaTown = corpMember.PpaTown,
                        Positions = string.Join(", ", PositionIdToName(corpMember.SelectedPositions)),
                        PositionIds = string.Join(",", corpMember.SelectedPositions),
                        FullName = corpMember.LastName + " " + corpMember.FirstName + " " + corpMember.OtherName,
                        Username = corpMember.PhoneNumber.ToLower() + corpMember.FirstName.ToLower() +
                            corpMember.LastName.ToLower(),
                        SearchTag =
                            corpMember.PhoneNumber.ToLower() + corpMember.FirstName.ToLower() +
                            corpMember.LastName.ToLower() +
                            otherName + corpMember.Gender.ToLower() +
                            corpMember.Lga.ToLower() +
                            corpMember.MaritalStatus.ToLower() + corpMember.StateOfOrigin.ToLower() +
                            corpMember.PpaTown.ToLower() +
                            string.Join(", ", PositionIdToName(corpMember.SelectedPositions)) + Service.Year.Get(corpMember.YearId).Year.ToLower() +
                            Service.Batch.Get(corpMember.BatchId).BatchName.ToLower()
                    };

                    if (miniUpload != null && miniUpload.ContentLength > 0)
                    {
                        corpmember.MiniContentType = miniUpload.ContentType;
                        corpmember.MiniImage = MiniImageConpresser.CompressImage(miniUpload.InputStream);
                    }
                    if (majorUpload != null && majorUpload.ContentLength > 0)
                    {
                        corpmember.MajorContentType = majorUpload.ContentType;
                        corpmember.MajorImage = ImageConpresser.CompressImage(majorUpload.InputStream);
                    }

                    try
                    {
                        await Service.CorpMember.AddAsync(corpmember);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", "Corp member " + corpMember.FirstName + " " + corpMember.LastName + " already exist");
                        }

                        ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
                        ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == corpMember.YearId), "Id",
                            "BatchName");
                        ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");

                        return View(corpMember);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to add corp member other than those in your sub zone");
            }

            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
            ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");
            return View(corpMember);
        }

        [Access(Resources.CorpMember, Operations.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var corpMember = await Service.CorpMember.GetAsync(id);
            if (corpMember == null)
            {
                return HttpNotFound();
            }

            var corpmemberModel = new CorpMembers
            {
                Year = corpMember.Year.ToString(),
                AcademicInfo = corpMember.AcademicInfo,
                BatchId = corpMember.BatchId,
                Day = corpMember.Day.ToString(),
                FirstName = corpMember.FirstName,
                PhoneNumber = corpMember.PhoneNumber,
                Gender = corpMember.Gender,
                Id = corpMember.Id,
                IsGeneral = corpMember.IsGeneral,
                IsLeader = corpMember.IsLeader,
                PpaTown = corpMember.PpaTown,
                YearId = corpMember.YearId,
                OtherName = corpMember.OtherName,
                LastName = corpMember.LastName,
                PersonalInfo = corpMember.PersonalInfo,
                Lga = corpMember.Lga,
                NccfInfo = corpMember.NccfInfo,
                Month = corpMember.Month,
                MaritalStatus = corpMember.MaritalStatus,
                StateOfOrigin = corpMember.StateOfOrigin,
                MajorImage = corpMember.MajorImage,
                MajorContentType = corpMember.MajorContentType,
                MiniImage = corpMember.MiniImage,
                MiniContentType = corpMember.MiniContentType,
                Positions = corpMember.Positions,
                PositionIds = corpMember.PositionIds,
                SelectedPositions = corpMember.PositionIds.Split(','),
                FullName = corpMember.LastName + " " + corpMember.FirstName + " " + corpMember.OtherName
            };

            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year", corpMember.YearId);
            ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == corpMember.YearId), "Id",
                "BatchName", corpMember.BatchId);
            ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");

            return View(corpmemberModel);
        }

        [Access(Resources.CorpMember, Operations.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CorpMembers corpMember, HttpPostedFileBase miniUpload,
            HttpPostedFileBase majorUpload)
        {
            var userRoles = UserManager.GetRoles(User.Identity.GetUserId());
            var isAllowed = userRoles.Any(userRole => userRole.Contains(corpMember.PpaTown));
            if (ModelState.IsValid)
            {
                if (isAllowed || User.IsInRole("Administrator") || User.IsInRole("administrator"))
                {
                    var otherName = corpMember.OtherName == null ? null : corpMember.OtherName.ToLower();
                    var corpmember = new CorpMember
                    {
                        BatchId = corpMember.BatchId,
                        AcademicInfo = corpMember.AcademicInfo,
                        Day = int.Parse(corpMember.Day),
                        FirstName = corpMember.FirstName,
                        Gender = corpMember.Gender,
                        PhoneNumber = corpMember.PhoneNumber,
                        IsGeneral = corpMember.IsGeneral,
                        IsLeader = corpMember.IsLeader,
                        LastName = corpMember.LastName,
                        Lga = corpMember.Lga,
                        Year = int.Parse(corpMember.Year),
                        YearId = corpMember.YearId,
                        MaritalStatus = corpMember.MaritalStatus,
                        Month = corpMember.Month,
                        NccfInfo = corpMember.NccfInfo,
                        OtherName = corpMember.OtherName,
                        PersonalInfo = corpMember.PersonalInfo,
                        StateOfOrigin = corpMember.StateOfOrigin,
                        PpaTown = corpMember.PpaTown,
                        Id = corpMember.Id,
                        MajorImage = corpMember.MajorImage,
                        MajorContentType = corpMember.MajorContentType,
                        MiniImage = corpMember.MiniImage,
                        MiniContentType = corpMember.MiniContentType,
                        Positions = string.Join(", ", PositionIdToName(corpMember.SelectedPositions)),
                        PositionIds = string.Join(",", corpMember.SelectedPositions),
                        FullName = corpMember.LastName + " " + corpMember.FirstName + " " + corpMember.OtherName,
                        Username = corpMember.PhoneNumber.ToLower() + corpMember.FirstName.ToLower() +
                            corpMember.LastName.ToLower(),
                        SearchTag =
                            corpMember.PhoneNumber.ToLower() + corpMember.FirstName.ToLower() +
                            corpMember.LastName.ToLower() +
                            otherName + corpMember.Gender.ToLower() +
                            corpMember.Lga.ToLower() +
                            corpMember.MaritalStatus.ToLower() + corpMember.StateOfOrigin.ToLower() +
                            corpMember.PpaTown.ToLower() +
                            corpMember.Positions.ToLower() + Service.Year.Get(corpMember.YearId).Year.ToLower() +
                            Service.Batch.Get(corpMember.BatchId).BatchName.ToLower()
                    };

                    if (miniUpload != null && miniUpload.ContentLength > 0)
                    {
                        corpmember.MiniContentType = miniUpload.ContentType;
                        corpmember.MiniImage = MiniImageConpresser.CompressImage(miniUpload.InputStream);
                    }
                    if (majorUpload != null && majorUpload.ContentLength > 0)
                    {
                        corpmember.MajorContentType = majorUpload.ContentType;
                        corpmember.MajorImage = ImageConpresser.CompressImage(majorUpload.InputStream);
                    }

                    try
                    {
                        await Service.CorpMember.UpdateAsync(corpmember);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.InnerException.Message.Contains("Duplicate entry"))
                        {
                            ModelState.AddModelError("", "Corp member " + corpMember.FirstName + " " + corpMember.LastName + " already exist");
                        }

                        ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
                        ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == corpMember.YearId), "Id",
                            "BatchName");
                        ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");

                        return View(corpMember);
                    }

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("",
                    "You are not authorized to update corp member details other than those in your sub zone");
            }

            ViewBag.YearId = new SelectList(Service.Year.Get(), "Id", "Year");
            ViewBag.BatchId = new SelectList(Service.Batch.Get().Where(x => x.YearId == corpMember.YearId), "Id",
                "BatchName");
            ViewBag.PortfolioId = new SelectList(Service.Portfolio.Get(), "Id", "Position");

            return View(corpMember);
        }

        [Access(Resources.CorpMember, Operations.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var corpMember = await Service.CorpMember.GetAsync(id);
            if (corpMember == null)
            {
                return HttpNotFound();
            }
            return View(corpMember);
        }

        [Access(Resources.CorpMember, Operations.Delete)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var corpMember = await Service.CorpMember.GetAsync(id);
            await Service.CorpMember.DeleteAsync(corpMember);
            return RedirectToAction("Index");
        }

        [Access(Resources.CorpMember, Operations.Execute)]
        [HttpGet]
        public async Task<ActionResult> JesusCorpMembersToGenerals(string year, string batch)
        {
            var yr = await Service.Year.GetAsync();
            ViewBag.year = new SelectList(yr, "Year", "Year");

            var returnBatch = batch;

            if (year == "All" || year == "") year = null;
            if (batch == "All" || batch == "") batch = null;

            var corpmembers = await Service.CorpMember.GetAsync();
            var searchedCorpMembers = corpmembers.Where(
                x =>
                    (x.ServiceYear.Year == year || year == null) &&
                    (x.Batch.BatchName == batch || batch == null));


            ViewBag.batch = new SelectList(Service.Batch.Get().Where(x => x.Year.Year == year), "BatchName",
                "BatchName", returnBatch);

            return View(searchedCorpMembers.ToList());
        }

        [Access(Resources.CorpMember, Operations.Execute)]
        [HttpPost]
        public async Task<ActionResult> JesusCorpMembersToGenerals(IList<CorpMember> corpMembers)
        {
            foreach (var corpmember in corpMembers)
            {
                await Service.CorpMember.UpdateAsync(corpmember);
            }

            var yr = await Service.Year.GetAsync();
            ViewBag.year = new SelectList(yr, "Year", "Year");
            ViewBag.batch = new SelectList("", "BatchName",
                "BatchName", null);

            return View(corpMembers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.CorpMember.Dispose();
                Service.Year.Dispose();
                Service.Batch.Dispose();
            }
            base.Dispose(disposing);
        }

        private string[] PositionIdToName(string[] selectedPositions)
        {
            var positions = Service.Portfolio.Get();
            var position = new string[selectedPositions.Length];
            var i = 0;
            foreach (var pos in selectedPositions)
            {
                var posi = positions.FirstOrDefault(x => x.Id == Convert.ToInt32(pos));
                if (posi != null)
                    position[i] = posi.Position;
                i++;
            }

            return position;
        }
    }
}