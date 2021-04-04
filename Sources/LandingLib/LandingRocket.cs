using LandingLib.Interfaces;
using LandingLib.Models;
using System;
using System.Collections.Generic;

namespace LandingLib
{
    /// <summary>
    /// Landing rocket class.
    /// </summary>
    public class LandingRocket : ILandingRocket
    {
        private readonly LandingArea landingArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingRocket"/> class.
        /// </summary>
        /// <param name="areaSizeX">Landing area size in x axis.</param>
        /// <param name="areaSizeY">>Landing area size in y axis.</param>
        public LandingRocket(int areaSizeX, int areaSizeY)
        {
            this.landingArea = new LandingArea(areaSizeX, areaSizeY);
        }

        ///<inheritdoc/>
        public string CheckForLanding(string rocket, int x, int y)
        {
            var result = this.landingArea.CheckForLanding(rocket, x, y);

            return result switch
            {
                UnitStatus.OkForLanding => "ok for landing",
                UnitStatus.Clash => "clash",
                _ => "out of platform",
            };

        }

        ///<inheritdoc/>
        public IList<Platform> GetLocatedPlatforms()
        {
            return this.landingArea.LandingPlatforms;
        }

        ///<inheritdoc/>
        public bool SetupLandingPlatform(Platform landingPlatform, out string error)
        {
            error = string.Empty;

            try
            {
                this.landingArea.SetupLandingPlatform(landingPlatform);
            }
            catch (ArgumentOutOfRangeException e)
            {
                error = e.Message;
                return false;
            }
            
            return true;
        }

        ///<inheritdoc/>
        public bool SetupLandingPlatforms(IList<Platform> landingPlatforms, out string error)
        {
            error = string.Empty;
            foreach (var lp in landingPlatforms)
            {
                if (!this.SetupLandingPlatform(lp, out error))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
