﻿using CleanXF.Core.Domain.Entities;
using System.Threading.Tasks;

namespace CleanXF.Core.Interfaces.Data.Repositories
{
    public interface IProfileRepository
    {
        Task<bool> Insert(GitHubUser user);
        Task<GitHubUser> Get();
    }
}