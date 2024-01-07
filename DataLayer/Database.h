#ifndef DATABASE_H
#define DATABASE_H
#endif // !DATABASE_H

using namespace std;

void executeQuery(string, bool);
void deleteTable(const char*);
void createTable(const char*);
static int callBack(void*, int, char**, char**);

extern "C"
{
	__declspec(dllexport) void insertPlayer(const char*, const char*, int, double);
	__declspec(dllexport) char* showLeaderboard(const char*);
	__declspec(dllexport) char* showTop3(const char*);
	__declspec(dllexport) char* showLeaderboardUser(const char*, const char*, double);
}
