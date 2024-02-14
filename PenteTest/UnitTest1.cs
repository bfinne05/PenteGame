using Pente;
using System;

namespace PenteTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod_Player1NotValidMove()
		{
			Thread thread = new Thread(() =>
			{
				string player1name = "Brett";
				string player2name = "BrOtt";
				int num = 9;

				var game = new MainWindow(false, player1name, player2name, num + 1, false, 0, 0, null);
				bool result = game.Player1Set(-1, -1);

				Assert.IsFalse(result);
			});

			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		[TestMethod]
		public void TestMethod_CheckSpace_Unoccupied()
		{
			Thread thread2 = new Thread(() =>
			{
				string player1name = "Brett";
				string player2name = "BrOtt";
				int num = 9;

				var game = new MainWindow(false, player1name, player2name, num + 1, false, 0, 0, null);
				bool result = game.CheckSpace(1, 1);

				Assert.IsTrue(result);
			});

			thread2.SetApartmentState(ApartmentState.STA);
			thread2.Start();
			thread2.Join();
		}

		[TestMethod]
		public void TestMethod_CheckSpace_Occupied()
		{
			Thread thread2 = new Thread(() =>
			{
				string player1name = "Brett";
				string player2name = "BrOtt";
				int num = 11;

				var game = new MainWindow(false, player1name, player2name, num + 1, false, 0, 0, null);
				
				bool result = game.CheckSpace(6, 6);

				Assert.IsFalse(result);
			});

			thread2.SetApartmentState(ApartmentState.STA);
			thread2.Start();
			thread2.Join();
		}

		[TestMethod]
		public void TestMethod_GameContinue()
		{
			Thread thread2 = new Thread(() =>
			{
				string player1name = "Brett";
				string player2name = "BrOtt";
				int num = 11;

				var game = new MainWindow(false, player1name, player2name, num + 1, false, 0, 0, null);

				bool result = game.CheckPlayer1Win(6, 6);

				Assert.IsFalse(result);
			});

			thread2.SetApartmentState(ApartmentState.STA);
			thread2.Start();
			thread2.Join();
		}
	}
}