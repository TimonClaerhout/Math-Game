#include "pch.h"
#include "CppUnitTest.h"
#include "../DataLayer/Database.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTest_DataLayer
{
	TEST_CLASS(UnitTest_DataLayer)
	{
	public:
		
		TEST_METHOD(TestInsertPlayer)
		{
			createDatabase();
			createTable("Test");
			insertPlayer("Test", "Timon", 10, 20.25);
			// Don't forget the ranking function at the beginning
			const char* result = "1 Timon 10 / 10 with time -> 20.25";
			Assert::AreEqual(result, showLeaderboard("Test"));
		}
	};
}
