using UnityEngine;
using System.Collections;
using NUnit.Framework;
using RedBlueGames.Tools;

namespace RedBlueGames.Tools.Tests
{
	[TestFixture]
	public class UtilitiesTests
	{

		[Test]
		public void CopyText ()
		{
			var copyString = "TestStrrrring";
			var expectedString = copyString;
			Utilities.CopyStringToBuffer (copyString);

			var editor = new TextEditor ();
			editor.Paste ();
			Assert.AreEqual (expectedString, editor.text);
		}
	}
}