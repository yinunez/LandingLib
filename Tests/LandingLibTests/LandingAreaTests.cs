using LandingLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LandingLibTests
{
    [TestClass]
    public class LandingAreaTests
    {
        [TestMethod]
        public void InitLandingArea_Success()
        {
            var landingArea = new LandingArea(100, 100);
            Assert.IsNotNull(landingArea);
            Assert.IsNotNull(landingArea.UnitArea);
            Assert.IsTrue(landingArea.LandingPlatforms.Count == 0);
            Assert.IsTrue(landingArea.UnitArea[0, 0].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[99, 99].Status == UnitStatus.OutOfPlatform);
        }

        [TestMethod]
        public void SetupPlatformInLandingArea_Success()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            // Act
            landingArea.SetupLandingPlatform(platform);

            // Assert
            Assert.IsTrue(landingArea.LandingPlatforms.Count == 1);
            Assert.IsTrue(landingArea.UnitArea[5, 4].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[5, 5].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[15, 15].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[15, 16].Status == UnitStatus.OutOfPlatform);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupPlatformWrongParameterValues_Fail()
        {
            // Arrange
            var landingArea = new LandingArea(0, 0);
            var platform = new Platform { SizeX = 10, SizeY = 15, InitialPositionX = 5, InitialPositionY = 5, };

            // Act
            landingArea.SetupLandingPlatform(platform);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupPlatformBiggerThanLandingAreaValues_Fail()
        {
            // Arrange
            var landingArea = new LandingArea(20, 20);
            var platform = new Platform { SizeX = 20, SizeY = 15, InitialPositionX = 5, InitialPositionY = 5, };

            // Act
            landingArea.SetupLandingPlatform(platform);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupPlatformInitialPositionOutOfLandingAreaValues_Fail()
        {
            // Arrange
            var landingArea = new LandingArea(20, 20);
            var platform = new Platform { SizeX = 5, SizeY = 5, InitialPositionX = 21, InitialPositionY = 5, };

            // Act
            landingArea.SetupLandingPlatform(platform);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetupPlatformOutOfLandingArea_Fail()
        {
            // Arrange
            var landingArea = new LandingArea(20, 20);
            var platform = new Platform { SizeX = 10, SizeY = 15, InitialPositionX = 5, InitialPositionY = 5, };

            // Act
            landingArea.SetupLandingPlatform(platform);

            // Assert
        }

        [TestMethod]
        public void SetupTwoPlatformsInLandingArea_Success()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);

            var platforms = new List<Platform>
            {
                new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5 },
                new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 30, InitialPositionY= 30 },
            };

            // Act
            landingArea.SetupLandingPlatforms(platforms);

            // Assert
            Assert.IsTrue(landingArea.LandingPlatforms.Count == 2);

            Assert.IsTrue(landingArea.UnitArea[5, 4].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[5, 5].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[15, 15].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[15, 16].Status == UnitStatus.OutOfPlatform);

            Assert.IsTrue(landingArea.UnitArea[30, 29].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[30, 30].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[40, 40].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[40, 41].Status == UnitStatus.OutOfPlatform);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The platform in position [10,15] could not be placed. Overlaps another platform.")]
        public void SetupTwoOverlapedPlatformsInLandingArea_Fail()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);

            var platforms = new List<Platform>
            {
                new Platform { SizeX = 10, SizeY = 10,InitialPositionX = 5, InitialPositionY = 5 },
                new Platform { SizeX = 10, SizeY = 10,InitialPositionX = 10, InitialPositionY = 10 },
            };

            // Act
            landingArea.SetupLandingPlatforms(platforms);

            // Assert
        }

        [TestMethod]
        public void CheckForLandingEdgePlatform_Success_OkForLanding()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);

            // Act
            var result = landingArea.CheckForLanding("Rocket1", 5, 5);

            // Assert
            Assert.IsTrue(result == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[4, 4].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[4, 5].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[4, 5].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[5, 4].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[5, 5].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[5, 6].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[6, 4].Status == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[6, 5].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[6, 6].Status == UnitStatus.Clash);
        }

        [TestMethod]
        public void CheckForLandingCenterPlatform_SuccessOkForLanding()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);

            // Act
            var result = landingArea.CheckForLanding("Rocket1", 10, 10);

            // Assert
            Assert.IsTrue(result == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[9, 9].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[9, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[9, 11].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[10, 9].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[10, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[10, 11].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 9].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 11].Status == UnitStatus.Clash);
        }

        [TestMethod]
        public void CheckForLandingTwiceSameRocketOverlapsPositions_SuccessOkForLanding()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);
            landingArea.CheckForLanding("Rocket1", 10, 10);

            // Act
            var result = landingArea.CheckForLanding("Rocket1", 11, 11);

            // Assert
            Assert.IsTrue(result == UnitStatus.OkForLanding);

            Assert.IsTrue(landingArea.UnitArea[9, 9].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[9, 10].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[9, 11].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[10, 9].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[11, 9].Status == UnitStatus.OkForLanding);


            Assert.IsTrue(landingArea.UnitArea[10, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[10, 11].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[10, 12].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 11].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 12].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[12, 10].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[12, 11].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[12, 12].Status == UnitStatus.Clash);
        }

        [TestMethod]
        public void CheckForLandingPlatform_SuccessOutOfPlatform()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);

            // Act
            var result = landingArea.CheckForLanding("Rocket1", 20, 25);

            // Assert
            Assert.IsTrue(result == UnitStatus.OutOfPlatform);
            Assert.IsTrue(landingArea.UnitArea[20, 25].Status == UnitStatus.OutOfPlatform);
        }

        [TestMethod]
        public void CheckForLandingSecondRockedIncorrectPosition_SuccessClash()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };


            landingArea.SetupLandingPlatform(platform);
            landingArea.CheckForLanding("Rocket1", 10, 10);

            // Act
            var result = landingArea.CheckForLanding("Rocket2", 11, 11);

            // Assert
            Assert.IsTrue(result == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 11].Status == UnitStatus.Clash);
        }

        [TestMethod]
        public void CheckForLandingSecondRockedInACorrectPositionButClashAreaOverlapsPreviousRocket_SuccessClash()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);
            landingArea.CheckForLanding("Rocket1", 10, 10);

            // Act
            var result = landingArea.CheckForLanding("Rocket2", 11, 12);

            // Assert
            Assert.IsTrue(result == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[11, 12].Status == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[11, 11].Status == UnitStatus.Clash);
        }

        [TestMethod]
        public void CheckForLandingThirdRockedCheckSamePostionAsFirstRocket_SuccessOkForLanding()
        {
            // Arrange
            var landingArea = new LandingArea(100, 100);
            var platform = new Platform { SizeX = 10, SizeY = 10, InitialPositionX = 5, InitialPositionY = 5, };

            landingArea.SetupLandingPlatform(platform);
            landingArea.CheckForLanding("Rocket1", 7, 7);
            Assert.IsTrue(landingArea.UnitArea[7, 7].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[7, 7].Rocket == "Rocket1");

            landingArea.CheckForLanding("Rocket2", 10, 10);
            // Act
            var result = landingArea.CheckForLanding("Rocket3", 7, 7);

            // Assert
            Assert.IsTrue(result == UnitStatus.OkForLanding);
            Assert.IsTrue(landingArea.UnitArea[7, 7].Status == UnitStatus.Clash);
            Assert.IsTrue(landingArea.UnitArea[7, 7].Rocket == "Rocket3");
        }
    }
}
