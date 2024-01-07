#include <iostream>
#include <format>
#include <string>
#include <stdbool.h>

#include "sqlite3.h"
#include "Database.h"

string result;

/**
 * @brief Executes an query inside the "leaderboard.db" database.
 * @param Query that has to be executed
 * @param If true callback function is used, this way the string result will be filled with the result of that query.
 * If false the query is executed without the response loading in the result string.
 */
void executeQuery(string sql, bool callback)
{
	char* messaggeError;
	sqlite3* DB;
	// If leaderboard.db exists open it, otherwise create leaderboard.db and open it.
	int exit = sqlite3_open("leaderboard.db", &DB);
	if (callback) {
		exit = sqlite3_exec(DB, sql.c_str(), callBack, 0, &messaggeError);
	}
	else {
		exit = sqlite3_exec(DB, sql.c_str(), NULL, 0, &messaggeError);
	}

	// When SQLite query gives an error
	if (exit != SQLITE_OK) {
		cerr << "Error: " << messaggeError << endl;
		sqlite3_free(messaggeError);
	}
	
	sqlite3_close(DB);
}

/**
 * @brief Creates table inside the "leaderboard.db" database.
 * @param Name of the table
 */
void createTable(const char* table) {
	string sql = format("CREATE TABLE IF NOT EXISTS {} ("
		"NAME	TEXT	NOT NULL, "
		"SCORE	INT		NOT NULL,"
		"POINTS	TEXT	NOT NULL,"
		"TIME	REAL	NOT NULL);", table);

	executeQuery(sql, false);
}

/**
 * @brief Deletes table inside the "leaderboard.db" database.
 * @param Name of the table
 */
void deleteTable(const char* table) {
	string sql = format("DROP TABLE {}", table);

	executeQuery(sql, false);
}

/**
 * @brief Inserts a player inside the "leaderboard.db" database.
 * @param Name of the table inside "leaderboard.db"
 * @param Name of the user
 * @param Score of the user
 * @param Time that the user needed to solve the challenges
 */
void insertPlayer(const char* table, const char* username, int score, double time) {
	string sql = format("INSERT INTO {} (NAME, SCORE, POINTS, TIME) VALUES('{}', {}, '/ 10 with time ->', {});",
						table, username, to_string(score),to_string(time));

	executeQuery(sql, false);
}


/**
 * @brief Callback function of the sqlite3 database
 * @param unused
 * @param Number of columns
 * @param Row's data
 * @param Column names
 * @return Contents of the table inside the "leaderboard.db" database
 */
static int callBack(void* data, int argc, char** argv, char** azColName)
{
	for (int i = 0; i < argc; i++) {
		if (i == 0) {
			result += format("{}: ", argv[i]);
		}
		else {
			result += format("{} ", argv[i]);
		}
	}
	result += "\n";
	return 0;
}

/**
 * @param Name of the table
 * @return All players of the table inside the "leaderboard.db" database with the best score and fastest time first 
 */
char* showLeaderboard(const char* table) {
	string sql = format("SELECT rank() OVER( ORDER BY SCORE DESC, TIME ASC ) GLOBAL_RANK, * FROM {};",table);

	result = "";
	result += format("This is the global {}\n", table);
	executeQuery(sql, true);

	return const_cast<char*>(result.c_str());
}

/**
 * @param Name of the table
 * @return 3 players of the table inside the "leaderboard.db" database with the best score and fastest time first
 */
char* showTop3(const char* table) {
	string sql = format("SELECT rank() OVER( ORDER BY SCORE DESC, TIME ASC ) GLOBAL_RANK, * FROM {} LIMIT 3;", table);

	result = "";
	result += format("This is the global top 3 of {}\n",table);
	executeQuery(sql, true);

	return const_cast<char*>(result.c_str());
}

/**
 * @param Name of the table
 * @param Time that the user needed to solve the math challenges
 * @return Place that the player ended on the "leaderboard.db" database at specific table that is ordered with the best score and fastest time first
 * 
 * @info This is done by looking over all columns inside the table that has the username with his specific time needed to solve the challenges.
 * Since the time is stored with 0.001sec precision and the name of other players are different is this an accurate method.
 */
char* showLeaderboardUser(const char* table, const char* name, double time) {
	string sql = format("WITH cte AS (SELECT rank() OVER( ORDER BY SCORE DESC, TIME ASC ) GLOBAL_RANK, "
						"* FROM {}) SELECT * FROM cte WHERE NAME='{}' AND TIME = {};",table, name, to_string(time));

	result = "";
	result += format("This is the place you ended on {}:\n", table);
	executeQuery(sql, true);

	return const_cast<char*>(result.c_str());
}
