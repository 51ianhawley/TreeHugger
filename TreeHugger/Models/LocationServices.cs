using System;
namespace TreeHugger.Models;

public class LocationServices
{
    public static async Task<Location> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return location;
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return null;
    }

    public static async Task<Location> GetCurrentLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLocationAsync();

            if (location != null)
                return location;
            else
                return await GetCachedLocation();
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
            return await GetCachedLocation();
        }
    }
}

