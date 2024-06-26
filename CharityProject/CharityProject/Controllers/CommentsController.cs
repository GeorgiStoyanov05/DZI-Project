﻿using Charities.Data.Models;
using CharityProject.Contracts;
using CharityProject.Models;
using CharityProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CharityProject.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly ICaseService caseService;
        private readonly UserManager<User> userManager;

        public CommentsController(ICommentService _commentService, ICaseService _caseService, UserManager<User> _userManager)
        {
            this.caseService = _caseService;
            this.commentService = _commentService;
            this.userManager = _userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(DetailsCaseViewModel model)
        {
            Comment comment = new Comment()
            {
                Text = model.Comment.Text,
                CreatedDate = DateTime.Now,
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                CharityId = model.Charity.Id
            };
            try
            {
                Charity charity = await caseService.GetCharity(model.Charity.Id);
                await commentService.AddCommentToCharity(charity, comment);
            }
            catch (Exception err)
            {
                return RedirectToAction("ErrorPage", "Home", new { message = err.Message });
            }
            return RedirectToAction("DetailsCase", "Cases", new { id = model.Charity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(Guid commentId, Guid charityId)
        {
            try
            {
                Charity charity = await commentService.DeleteComment(commentId, charityId);
            }
            catch (Exception err)
            {
                return RedirectToAction("ErrorPage", "Home", new { message = err.Message });
            }
            return RedirectToAction("DetailsCase", "Cases", new { id = charityId });
        }
    }
}
