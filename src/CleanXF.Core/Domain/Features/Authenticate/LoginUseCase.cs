﻿using CleanXF.Core.Domain.Features.Authenticate.Models;
using CleanXF.Core.Interfaces.Authentication;
using CleanXF.Core.Interfaces.Data.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanXF.Core.Domain.Features.Authenticate
{
    public class LoginUseCase : IRequestHandler<AuthenticationRequest, AuthenticationResponse>
    {
        private readonly IAuthenticator _authenticator;
        private readonly ISessionRepository _sessionRepository;

        public LoginUseCase(IAuthenticator authenticator, ISessionRepository sessionRepository)
        {
            _authenticator = authenticator;
            _sessionRepository = sessionRepository;
        }

        public async Task<AuthenticationResponse> Handle(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var accessToken = await _authenticator.Authenticate();

            // The presence of a token means the authentication step succeeded
            if (!string.IsNullOrEmpty(accessToken))
            {
                return new AuthenticationResponse(await _sessionRepository.Initialize(accessToken));
            }

            // Authentication failed
            return new AuthenticationResponse();
        }
    }
}
