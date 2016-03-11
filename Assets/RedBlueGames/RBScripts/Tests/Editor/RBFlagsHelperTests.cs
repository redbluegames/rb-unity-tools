using NUnit.Framework;
using System;
using RedBlueGames;

namespace RedBlueGames.Tools.Tests
{
    [TestFixture]
    public class RBFlagsHelperTests
    {
        [Flags]
        enum TestFlags
        {
            First = 1 << 0,
            Second = 1 << 1,
            Third = 1 << 2,
            Fourth = 1 << 3
        }

        [Test]
        public void IsSet_FlaggedBit_IsSet()
        {
            // Arrange
            TestFlags secondSet = TestFlags.Second;

            // Act and Assert
            Assert.True(RBFlagsHelper.IsSet(secondSet, TestFlags.Second), 
                "IsSet failed. Bit is not set, expected set");
        }

        [Test]
        public void IsSet_UnflaggedBit_IsNotSet()
        {
            // Arrange
            TestFlags secondAndThirdSet = TestFlags.Second | TestFlags.Third;

            // Act and Assert
            Assert.False(RBFlagsHelper.IsSet(secondAndThirdSet, TestFlags.Fourth), 
                "IsSet failed. Bit is set, expected unset");
        }

        [Test]
        public void Set_UnflaggedBit_IsSet()
        {
            // Arrange
            TestFlags secondAndThirdSet = TestFlags.Second | TestFlags.Third;
            TestFlags secondThirdAndFourth = TestFlags.Second | TestFlags.Third | TestFlags.Fourth;

            // Act
            TestFlags setFourth = secondAndThirdSet;
            RBFlagsHelper.Set(ref setFourth, TestFlags.Fourth);

            // Assert
            Assert.AreEqual(secondThirdAndFourth, setFourth);
        }

        [Test]
        public void Set_FlaggedBit_IsUnaffected()
        {
            // Arrange
            TestFlags secondAndThirdSet = TestFlags.Second | TestFlags.Third;

            // Act
            TestFlags flagThird = secondAndThirdSet;
            RBFlagsHelper.Set(ref flagThird, TestFlags.Third);

            // Assert
            Assert.AreEqual(secondAndThirdSet, flagThird);
        }

        [Test]
        public void UnSet_FlaggedBit_IsUnset()
        {
            // Arrange
            TestFlags secondAndThird = TestFlags.Second | TestFlags.Third;
            TestFlags secondWithoutThird = TestFlags.Second;

            // Act
            TestFlags unflagThird = secondAndThird;
            RBFlagsHelper.Unset(ref unflagThird, TestFlags.Third);

            // Assert
            Assert.AreEqual(secondWithoutThird, unflagThird);
        }

        [Test]
        public void UnSet_UnFlaggedBit_IsUnaffected()
        {
            // Arrange
            TestFlags secondAndThird = TestFlags.Second | TestFlags.Third;

            // Act
            TestFlags unflagFourth = secondAndThird;
            RBFlagsHelper.Unset(ref unflagFourth, TestFlags.Fourth);

            // Assert
            Assert.AreEqual(secondAndThird, unflagFourth);
        }

        [Test]
        public void ShiftLeftCircular_NoWrap_IsExpected()
        {
            // Arrange
            TestFlags firstAndSecond = TestFlags.First | TestFlags.Second;
            TestFlags shiftedLeftTwice = TestFlags.Third | TestFlags.Fourth;

            // Act
            TestFlags shiftedFlags = firstAndSecond;
            RBFlagsHelper.ShiftLeftCircular(ref shiftedFlags, 2);

            // Assert
            Assert.AreEqual(shiftedLeftTwice, shiftedFlags);
        }

        [Test]
        public void ShiftLeftCircular_WrapsHighToLow_AsExpected()
        {
            // Arrange
            TestFlags firstAndLast = TestFlags.First | TestFlags.Fourth;
            TestFlags shiftedLeftOnce = TestFlags.Second | TestFlags.First;

            // Act
            TestFlags shiftedFlags = firstAndLast;
            RBFlagsHelper.ShiftLeftCircular(ref shiftedFlags, 1);

            // Assert
            Assert.AreEqual(shiftedLeftOnce, shiftedFlags);
        }

        [Test]
        public void ShiftRightCircular_NoWrap_IsExpected()
        {
            // Arrange
            TestFlags firstAndSecond = TestFlags.Third | TestFlags.Second;
            TestFlags shiftedRightOnce = TestFlags.Second | TestFlags.First;

            // Act
            TestFlags shiftedFlags = firstAndSecond;
            RBFlagsHelper.ShiftRightCircular(ref shiftedFlags, 1);

            // Assert
            Assert.AreEqual(shiftedRightOnce, shiftedFlags);
        }

        [Test]
        public void ShiftRightCircular_WrapsLowToHigh_AsExpected()
        {
            // Arrange
            TestFlags firstAndLast = TestFlags.First | TestFlags.Fourth;
            TestFlags shiftedRightOnce = TestFlags.Fourth | TestFlags.Third;

            // Act
            TestFlags shiftedFlags = firstAndLast;
            RBFlagsHelper.ShiftRightCircular(ref shiftedFlags, 1);

            // Assert
            Assert.AreEqual(shiftedRightOnce, shiftedFlags);
        }

        [Test]
        public void FlagAll_Unflagged_AllBecomeFlagged()
        {
            // Arrange
            TestFlags none = 0;
            TestFlags all = TestFlags.Fourth | TestFlags.Third | TestFlags.Second | TestFlags.First;

            // Act
            TestFlags flagged = none;
            RBFlagsHelper.FlagAll(ref flagged);

            // Assert
            Assert.AreEqual(all, flagged);
        }

        [Test]
        public void FlagAll_SomeFlagged_AllBecomeFlagged()
        {
            // Arrange
            TestFlags one = TestFlags.Third;
            TestFlags all = TestFlags.Fourth | TestFlags.Third | TestFlags.Second | TestFlags.First;

            // Act
            TestFlags flagged = one;
            RBFlagsHelper.FlagAll(ref flagged);

            // Assert
            Assert.AreEqual(all, flagged);
        }

        [Test]
        public void FlagAll_AllFlagged_IsUnchanged()
        {
            // Arrange
            TestFlags all = TestFlags.Fourth | TestFlags.Third | TestFlags.Second | TestFlags.First;

            // Act
            TestFlags flagged = all;
            RBFlagsHelper.FlagAll(ref flagged);

            // Assert
            Assert.AreEqual(all, flagged);
        }

        [Test]
        public void SwapBits_NeitherIsFlagged_BitsAreSwapped()
        {
            // Arrange
            TestFlags firstAndFourth = TestFlags.First | TestFlags.Fourth;

            // Act
            TestFlags swapped = firstAndFourth;
            RBFlagsHelper.SwapBits(ref swapped, TestFlags.Second, TestFlags.Third);

            // Assert
            Assert.AreEqual(firstAndFourth, swapped);
        }

        [Test]
        public void SwapBits_FirstBitIsFlagged_BitsAreSwapped()
        {
            // Arrange
            TestFlags firstAndFourth = TestFlags.First | TestFlags.Fourth;
            TestFlags swappedFirstAndThird = TestFlags.Third | TestFlags.Fourth;

            // Act
            TestFlags swapped = firstAndFourth;
            RBFlagsHelper.SwapBits(ref swapped, TestFlags.First, TestFlags.Third);

            // Assert
            Assert.AreEqual(swappedFirstAndThird, swapped);
        }

        [Test]
        public void SwapBits_SecondBitIsFlagged_BitsAreSwapped()
        {
            // Arrange
            TestFlags firstAndFourth = TestFlags.First | TestFlags.Fourth;
            TestFlags swappedThirdAndFourth = TestFlags.First | TestFlags.Third;

            // Act
            TestFlags swapped = firstAndFourth;
            RBFlagsHelper.SwapBits(ref swapped, TestFlags.Third, TestFlags.Fourth);

            // Assert
            Assert.AreEqual(swappedThirdAndFourth, swapped);
        }

        [Test]
        public void SwapBits_BothFlagged_IsUnchanged()
        {
            // Arrange
            TestFlags firstAndThird = TestFlags.First | TestFlags.Third;

            // Act
            TestFlags swapped = firstAndThird;
            RBFlagsHelper.SwapBits(ref swapped, TestFlags.First, TestFlags.Third);

            // Assert
            Assert.AreEqual(firstAndThird, swapped);
        }
    }
}
