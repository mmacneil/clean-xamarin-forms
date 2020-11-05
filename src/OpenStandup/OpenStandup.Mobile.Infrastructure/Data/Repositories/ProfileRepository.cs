﻿using OpenStandup.Core.Domain.Entities;
using OpenStandup.Core.Domain.Values;
using OpenStandup.Core.Interfaces.Data.Repositories;
using OpenStandup.Mobile.Infrastructure.Data.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStandup.Mobile.Infrastructure.Data.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDb _appDb;

        public ProfileRepository(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<bool> InsertOrReplace(GitHubUser user)
        {
            return await _appDb.AsyncDb.InsertOrReplaceAsync(new Profile
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                AvatarUrl = user.AvatarUrl,
                BioHTML = user.BioHTML,
                WebsiteUrl = user.WebsiteUrl,
                Company = user.Company,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Location = user.Location,
                FollowerCount = user.Followers.TotalCount,
                FollowingCount = user.Following.TotalCount,
                RepositoryCount = user.Repositories.TotalCount,
                GistCount = user.Gists.TotalCount
            }).ConfigureAwait(false) == 1;
        }

        public async Task<GitHubUser> Get()
        {
            var model = (await _appDb.AsyncDb.QueryAsync<Profile>("select * from profile").ConfigureAwait(false)).FirstOrDefault();

            if (model != null)
            {
                return new GitHubUser(model.Id, model.Login, model.Name, model.AvatarUrl, model.BioHTML, model.WebsiteUrl, model.Company, model.Location, model.DatabaseId, model.Email, model.CreatedAt, new Followers(model.FollowerCount), new Following(model.FollowingCount), new Core.Domain.Values.Repositories(model.RepositoryCount), new Gists(model.GistCount), model.Latitude ?? 0, model.Longitude ?? 0);
            }

            throw new Exception("No profile exists.");
        }

        public async Task<bool> UpdateLocation(string id, double latitude, double longitude)
        {
            var user = await _appDb.AsyncDb.GetAsync<Profile>(id);
            user.Latitude = latitude;
            user.Longitude = longitude;
            return await _appDb.AsyncDb.UpdateAsync(user).ConfigureAwait(false) == 1;
        }
    }
}