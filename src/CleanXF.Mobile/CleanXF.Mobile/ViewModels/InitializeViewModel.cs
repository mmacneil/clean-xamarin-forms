﻿using CleanXF.Core.Interfaces.Data.Repositories;
using CleanXF.Mobile.Services;
using CleanXF.Mobile.ViewModels;
using System.Threading.Tasks;

public class InitializeViewModel : BaseViewModel
{
    private readonly ISessionRepository _sessionRepository;
    private readonly INavigator _navigator;
    public InitializeViewModel(ISessionRepository sessionRepository, INavigator navigator)
    {
        _sessionRepository = sessionRepository;
        _navigator = navigator;
    }

    public async void Initialize()
    {
        await Task.Delay(700);

        // If we have an access token we're considered logged in so proceed to shell, otherwise route to login
        if (await _sessionRepository.HasAccessToken())
        {
            await _navigator.GoTo("///main");
        }
        else
        {
            await _navigator.GoTo("///login");
        }
    }
}