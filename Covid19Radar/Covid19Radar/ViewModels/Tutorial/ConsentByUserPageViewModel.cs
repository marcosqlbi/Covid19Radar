﻿using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Covid19Radar.Model;
using Covid19Radar.Renderers;
using Covid19Radar.Resources;
using Covid19Radar.Services;
using DryIoc;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Covid19Radar.ViewModels
{
    public class ConsentByUserPageViewModel : ViewModelBase
    {
        private UserDataService _userDataService;

        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        public ConsentByUserPageViewModel(INavigationService navigationService, IStatusBarPlatformSpecific statusBarPlatformSpecific)
            : base(navigationService, statusBarPlatformSpecific)
        {
            Title = AppResources.TitleConsentByUserPage;
            Url = Resources.AppResources.UrlPrivacyPolicy;

            _userDataService = App.Current.Container.Resolve<UserDataService>();
        }

        public Command OnClickNext => new Command(async () =>
        {
            UserDialogs.Instance.ShowLoading("Waiting for register");
            if (!_userDataService.IsExistUserData)
            {
                UserDataModel userData = await _userDataService.RegistUserAsync();
                if (userData == null)
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(Resources.AppResources.DialogNetworkConnectionError, "Connection error", Resources.AppResources.DialogButtonOk);
                    return;
                }
            }
            UserDialogs.Instance.HideLoading();
            await NavigationService.NavigateAsync("InitSettingPage");
        });

    }
}
