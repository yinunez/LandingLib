using LandingLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("LandingLibTests")]

namespace LandingLib.Models
{
    internal class LandingArea
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public Unit[,] UnitArea { get; private set; }

        public IList<Platform> LandingPlatforms { get; private set; } = new List<Platform>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingArea"/> class.
        /// </summary>
        /// <param name="sizeX">Landing area size in x axis.</param>
        /// <param name="sizeY">Landing area size in y axis.</param>
        public LandingArea(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            UnitArea = new Unit[SizeX, SizeY];
            IntiUnits();
        }

        /// <summary>
        /// Allows a rocket check if the position is available to land.
        /// </summary>
        /// <param name="rocket">Name of the rocket.</param>
        /// <param name="x">Position in x axis.</param>
        /// <param name="y">Position in y axis.</param>
        /// <returns>Whether is possible land or not.</returns>
        public UnitStatus CheckForLanding(string rocket, int x, int y)
        {
            var status = UnitArea[x, y].Status;

            if (status == UnitStatus.OkForLanding || rocket == UnitArea[x, y].Rocket)
            {
                status = CheckAndUpdateSurroundings(rocket, x, y);
            }

            return status;
        }

        /// <summary>
        /// Setup different platforms inside the landing Area.
        /// </summary>
        /// <param name="landingPlatforms">List of the platforms definitions to locate.</param>
        public void SetupLandingPlatforms(IList<Platform> landingPlatforms)
        {
            foreach (var lp in landingPlatforms)
            {
                SetupLandingPlatform(lp);
            }
        }

        /// <summary>
        /// Setup a platform inside the landing Area.
        /// </summary>
        /// <param name="landingPlatforms">A platform definition to locate.</param>
        public void SetupLandingPlatform(Platform landingPlatform)
        { 
            CheckInputParams(landingPlatform);

            CheckAllUnitsAreFree(landingPlatform);

            landingPlatform.Id = LandingPlatforms.Count() + 1;

            LandingPlatforms.Add(landingPlatform);

            LocatePlatform(landingPlatform);
        }

        /// <summary>
        /// Init all units into the landing area.
        /// </summary>
        private void IntiUnits()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    UnitArea[x, y] = new Unit();
                }
            }
        }

        /// <summary>
        /// Check all units arround the select point to land and update its status if they are ok for landing.
        /// </summary>
        /// <param name="rocket">Name of the rocket.</param>
        /// <param name="rocketX">Position in x axis.</param>
        /// <param name="rocketY">Position in y axis.</param>
        /// <returns></returns>
        private UnitStatus CheckAndUpdateSurroundings(string rocket, int rocketX, int rocketY)
        {
            for (int x = rocketX - 1; x < rocketX + 1; x++)
            {
                for (int y = rocketY - 1; y < rocketY + 1; y++)
                {
                    if (UnitArea[x, y].Rocket != rocket
                        && UnitArea[x, y].Status == UnitStatus.Clash)
                    {
                        return UnitArea[x, y].Status;
                    }
                }
            }

            ClearPlatformPreviousCheck(rocket, rocketX, rocketY);
            UpdateUnits(rocket, rocketX, rocketY);

            return UnitStatus.OkForLanding;
        }

        /// <summary>
        /// Clear units from previous check. 
        /// </summary>
        /// <param name="rocket">Name of the rocket.</param>
        /// <param name="x">Position in x axis.</param>
        /// <param name="y">Position in y axis.</param>
        private void ClearPlatformPreviousCheck(string rocket, int x, int y)
        {
            var landingPlatform = LandingPlatforms.FirstOrDefault(lp => lp.Id == UnitArea[x, y].LandingPlarformId);
            LocatePlatform(landingPlatform);
        }

        /// <summary>
        /// Update untis with the current rocket position. 
        /// </summary>
        /// <param name="rocket">Name of the rocket.</param>
        /// <param name="rocketX">Position in x axis.</param>
        /// <param name="rocketY">Position in y axis.</param>
        private void UpdateUnits(string rocket, int rocketX, int rocketY)
        {
            for (int x = rocketX - 1; x <= rocketX + 1; x++)
            {
                for (int y = rocketY - 1; y <= rocketY + 1; y++)
                {
                    if (UnitArea[x, y].Status != UnitStatus.OutOfPlatform)
                    {
                        UnitArea[x, y].Status = UnitStatus.Clash;
                        UnitArea[x, y].Rocket = rocket;
                    }
                }
            }
        }

        /// <summary>
        /// Check if the platform could be located.
        /// </summary>
        /// <param name="landingPlatform">Platform to locate into the landing area</param>
        private void CheckInputParams(Platform landingPlatform)
        {
            if (landingPlatform.SizeX < 0 || landingPlatform.SizeY < 0 || landingPlatform.InitialPositionX < 0 || landingPlatform.InitialPositionY < 0)
            {
                throw new ArgumentOutOfRangeException("SizeX, SizeY, InitialPositionX or InitialPositionY", "The platform could not be placed. The parametres should have a value upper than 0.");
            }

            if (landingPlatform.SizeX > SizeX || landingPlatform.SizeY > SizeY)
            {
                throw new ArgumentOutOfRangeException("SizeX or SizeY","The platform could not be placed. The size is bigger than the Landing Area");
            }

            if (landingPlatform.InitialPositionX > SizeX || landingPlatform.InitialPositionY > SizeY) 
            {
                throw new ArgumentOutOfRangeException("InitialPositionX or InitialPositionY","The platform could not be placed. The initial position is out of the Landing Area");
            }

            if (landingPlatform.InitialPositionX + landingPlatform.SizeX >= SizeX || landingPlatform.InitialPositionY + landingPlatform.SizeY >= SizeY)
            {
                throw new ArgumentOutOfRangeException("SizeX, SizeY, InitialPositionX or InitialPositionY","The platform could not be placed. Part of the platflorm out of the Landing Area");
            }
        }

        /// <summary>
        /// Check if the units are out of plarform to locate one.
        /// </summary>
        /// <param name="landingPlatform">Platform to locate into the landing area</param>
        private void CheckAllUnitsAreFree(Platform landingPlatform)
        {
            for (int x = landingPlatform.InitialPositionX; x < landingPlatform.InitialPositionX + landingPlatform.SizeX; x++)
            {
                for (int y = landingPlatform.InitialPositionY; y < landingPlatform.InitialPositionY + landingPlatform.SizeY; y++)
                {
                    if (UnitArea[x, y].Status != UnitStatus.OutOfPlatform)
                    {
                        throw new ArgumentOutOfRangeException($"The platform in position [{landingPlatform.InitialPositionX},{landingPlatform.InitialPositionY}] could not be placed. Overlaps another platform.");
                    }
                }
            }
        }

        /// <summary>
        /// Locate the platform into the landing area.
        /// </summary>
        /// <param name="landingPlatform">Platform to locate into the landing area</param>
        private void LocatePlatform(Platform landingPlatform)
        {
            for (int x = landingPlatform.InitialPositionX; x <= landingPlatform.InitialPositionX + landingPlatform.SizeX; x++)
            {
                for (int y = landingPlatform.InitialPositionY; y <= landingPlatform.InitialPositionY + landingPlatform.SizeY; y++)
                {
                    UnitArea[x, y].LandingPlarformId = landingPlatform.Id;
                    UnitArea[x, y].Status = UnitStatus.OkForLanding;
                    UnitArea[x, y].Rocket = string.Empty;
                }
            }
        }
    }
}
