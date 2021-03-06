﻿using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Button = Xamarin.Forms.Button;
namespace OpenStandup.Mobile.Controls
{
    public class AppButton : Button
    {
        public AppButton()
        {
            //https://github.com/xamarin/Xamarin.Forms/pull/1935#issuecomment-375728802
            On<Android>().SetUseDefaultPadding(true).SetUseDefaultShadow(true);
        }
    }
}
