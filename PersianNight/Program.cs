using System;
using System.IO;
using GameFramework;
using System.Diagnostics;

namespace PersianNight
{
	static class Program
	{
		static private GamePersianNight newgamemain = null;
		[STAThread]
		static void Main()
		{
			newgamemain = new GamePersianNight();
			newgamemain.Initialize();
			newgamemain.MainLoop();
		}
	}
}
