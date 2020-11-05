﻿using OpenStandup.Core.Domain.Features.Authenticate.Models;
using OpenStandup.Core.Interfaces.Authentication;
using OpenStandup.Core.Interfaces.Data.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OpenStandup.Core.Interfaces;
using Vessel;

namespace OpenStandup.Core.Domain.Features.Authenticate
{
    public class LoginUseCase : IRequestHandler<AuthenticationRequest, Dto<string>>
    {
        private readonly IAuthenticator _authenticator;
        private readonly ISecureDataRepository _secureDataRepository;
        private readonly IOutputPort<Dto<string>> _outputPort;


        public LoginUseCase(IAuthenticator authenticator, ISecureDataRepository secureDataRepository, IOutputPort<Dto<string>> outputPort)
        {
            _authenticator = authenticator;
            _secureDataRepository = secureDataRepository;
            _outputPort = outputPort;
        }

        public async Task<Dto<string>> Handle(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var authenticationResponse = await _authenticator.Authenticate().ConfigureAwait(false);

            if (authenticationResponse.Succeeded)
            {
                await _secureDataRepository.SetPersonalAccessToken(authenticationResponse.Payload).ConfigureAwait(false);
            }
            else
            {
                await _secureDataRepository.SetPersonalAccessToken("").ConfigureAwait(false);
            }

            _outputPort.Handle(authenticationResponse);
            return authenticationResponse;
        }
    }
}