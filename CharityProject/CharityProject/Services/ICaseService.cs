﻿using Charities.Data.Models;
using CharityProject.Models;

namespace CharityProject.Services
{
    public interface ICaseService
    {

        Task<Charity> CreateCharity(CreateCaseViewModel model, string userId);

        Task<List<Category>> GetAllCategories();

        Task<List<Charity>> GetAllCharities();

        Task<Charity> GetCharity(Guid id);

        Task<List<int>> ExtractCountData();

        Task<Charity> UpdateCharity(Charity charity);
    }
}
