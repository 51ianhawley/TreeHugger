﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using TreeHugger.Models;

namespace TreeHugger;

public static class MauiProgram
{
    public static BusinessLogic BusinessLogic = new BusinessLogic();
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
//            .ConfigureLifecycleEvents(events =>
//            {
//#if ANDROID		// Makes the staus bars translucent
//                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

//                static void MakeStatusBarTranslucent(Android.App.Activity activity)
//                {
//                    activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

//                    activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

//                    activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
//                }
//#endif
//            })
            .UseMauiCommunityToolkit();
            builder.UseMauiMaps();
            

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

