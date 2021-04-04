using LandingLib.Models;
using System.Collections.Generic;

namespace LandingLib.Interfaces
{
    public interface ILandingRocket
    {
        /// <summary>
        /// Allows a rocket check if the position is available to land.
        /// </summary>
        /// <param name="rocket">Name of the rocke.t</param>
        /// <param name="x">Position in x axis.</param>
        /// <param name="y">Position in y axis.</param>
        /// <returns>Whether is possible land or not.</returns>
        string CheckForLanding(string rocket, int x, int y);

        /// <summary>
        /// Setup different platforms inside the landing Area.
        /// </summary>
        /// <param name="landingPlatforms">List of the platforms definitions to locate.</param>
        /// <param name="error">Description of an error. if it produce one.</param>
        /// <returns>Whether the platform was located at the landing area or not. </returns>
        bool SetupLandingPlatforms(IList<Platform> landingPlatforms, out string error);

        /// <summary>
        /// Setup a platform inside the landing Area.
        /// </summary>
        /// <param name="landingPlatforms">A platform definition to locate.</param>
        /// <param name="error">Description of an error. if it produce one.</param>
        /// <returns>Whether the platform was located at the landing area or not. </returns>
        bool SetupLandingPlatform(Platform landingPlatform, out string error);

        /// <summary>
        /// Look for the located platforms at the landing area.
        /// </summary>
        /// <returns>The list of all platforms located at the landing area.</returns>
        IList<Platform> GetLocatedPlatforms();    
    }
}
