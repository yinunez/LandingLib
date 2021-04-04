using LandingLib;
using LandingLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LandingLibTests
{
    [TestClass]
    public class LandingRocketTests
    {
        [TestMethod]
        public void SetupPlatformInLandingArea_Success()
        {
            // Arrange
            var landingRocket = new LandingRocket(100, 100);

            var platform = new Platform
            {
                SizeX = 10,
                SizeY = 10,
                InitialPositionX = 5,
                InitialPositionY = 5,
            };

            // Act
            var result = landingRocket.SetupLandingPlatform(platform, out string error);
            var platforms = landingRocket.GetLocatedPlatforms();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(error));

            Assert.IsTrue(platforms.Count == 1);
            Assert.IsTrue(platforms[0].InitialPositionX == platform.InitialPositionX);
            Assert.IsTrue(platforms[0].InitialPositionY == platform.InitialPositionY);
        }

        [TestMethod]
        public void SetupPlatformWrongParameterValues_Fail()
        {
            // Arrange
            var landingRocket = new LandingRocket(20, 20);

            var platform = new Platform
            {
                SizeX = 10,
                SizeY = 15,
                InitialPositionX = 5,
                InitialPositionY = 5,
            };

            // Act
            var result = landingRocket.SetupLandingPlatform(platform, out string error);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(error.Contains("The platform could not be placed. Part of the platflorm out of the Landing Area"));
        }

        [TestMethod]
        public void SetupTwoPlatformsInLandingArea_Success()
        {
            // Arrange
            var landingRocket = new LandingRocket(100, 100);

            var platforms = new List<Platform>
            {
                new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5 },
                new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 30, InitialPositionY= 30 },
            };

            // Act
            var result = landingRocket.SetupLandingPlatforms(platforms, out string error);
            var locatedPlatforms = landingRocket.GetLocatedPlatforms();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(error));

            Assert.IsTrue(locatedPlatforms.Count == 2);
            Assert.IsTrue(locatedPlatforms[0].InitialPositionX == platforms[0].InitialPositionX);
            Assert.IsTrue(locatedPlatforms[0].InitialPositionY == platforms[0].InitialPositionY);
            Assert.IsTrue(locatedPlatforms[1].InitialPositionX == platforms[1].InitialPositionX);
            Assert.IsTrue(locatedPlatforms[1].InitialPositionY == platforms[1].InitialPositionY);
        }

        [TestMethod]
        public void CheckForLandingPlatform_SuccessOkForLanding()
        {
            // Arrange
            var landingRocket = new LandingRocket(100, 100);

            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingRocket.SetupLandingPlatform(platform, out string error);

            // Act
            var result = landingRocket.CheckForLanding("Rocket1", 10, 10);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(error));
            Assert.IsTrue(result == "ok for landing");
        }

        [TestMethod]
        public void CheckForLandingSecondRockedIncorrectPosition_SuccessClash()
        {
            // Arrange
            var landingRocket = new LandingRocket(100, 100);

            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingRocket.SetupLandingPlatform(platform, out string error);
            landingRocket.CheckForLanding("Rocket1", 10, 10);

            // Act
            var result = landingRocket.CheckForLanding("Rocket2", 11, 11);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(error));
            Assert.IsTrue(result == "clash");
        }

        [TestMethod]
        public void CheckForLandingPlatform_SuccessOutOfPlatform()
        {
            // Arrange
            var landingRocket = new LandingRocket(100, 100);

            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingRocket.SetupLandingPlatform(platform, out string error);

            // Act
            var result = landingRocket.CheckForLanding("Rocket1", 20, 25);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(error));
            Assert.IsTrue(result == "out of platform");
        }
    }
}
