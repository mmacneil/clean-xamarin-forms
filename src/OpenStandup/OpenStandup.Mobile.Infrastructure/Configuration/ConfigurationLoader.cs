﻿using System.Threading.Tasks;
using OpenStandup.Core.Interfaces.Apis;
using OpenStandup.Core.Interfaces.Data.Repositories;
using OpenStandup.Mobile.Infrastructure.Data;
using OpenStandup.Mobile.Infrastructure.Data.Model;
using OpenStandup.Mobile.Infrastructure.Interfaces;


namespace OpenStandup.Mobile.Infrastructure.Configuration
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private readonly IOpenStandupApi _openStandupApi;
        private readonly AppDb _appDb;
        private readonly ISecureDataRepository _secureDataRepository;

        public ConfigurationLoader(AppDb appDb, IOpenStandupApi openStandupApi, ISecureDataRepository secureDataRepository)
        {
            _appDb = appDb;
            _openStandupApi = openStandupApi;
            _secureDataRepository = secureDataRepository;
        }

        public async Task<bool> TryLoad()
        {
            if (await _appDb.AsyncDb.ExecuteScalarAsync<int>("SELECT EXISTS(SELECT 1 FROM configuration)")
                .ConfigureAwait(false) == 1)
            {
                return true;
            }

            var configurationResponse = await _openStandupApi.GetConfiguration();

            if (!configurationResponse.Succeeded) return false;

            await _secureDataRepository.SetGitHubClientId(configurationResponse.Payload.GitHubClientId);
            await _secureDataRepository.SetGitHubClientSecret(configurationResponse.Payload.GitHubClientSecret);

            return await _appDb.AsyncDb.InsertOrReplaceAsync(new ConfigurationData
            {
                Version = configurationResponse.Payload.Version,
                Created = configurationResponse.Payload.Created
            }) == 1;
        }
    }
}

