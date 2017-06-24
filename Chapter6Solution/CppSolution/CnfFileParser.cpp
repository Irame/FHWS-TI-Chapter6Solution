#include "stdafx.h"
#include "CnfFileParser.h"
#include <fstream>
#include <string>
#include <sstream>

using namespace std;

static vector<string> SplitString(string s, char delim)
{
	vector<string> result;
	stringstream stream(s);
	string part;
	
	while (getline(stream, part, delim))
	{
		result.push_back(part);
	}

	return result;
}

SatProblemPtr CnfFileParser::Parse(string fileName)
{
	SatProblemPtr problem = make_shared<SatProblem>();

	ClausePtr curClause = make_shared<Clause>();

	ifstream filseStream(fileName);

	string line;
	while (getline(filseStream, line))
	{
		if (line.front() == 'c')
			continue;
		if (line.front() == 'p') {
			auto lileElements = SplitString(line, ' ');
			problem->VariableCount = stoi(lileElements[2]);
			problem->reserve(stoi(lileElements[3]));
		}
		else
		{
			auto lileElements = SplitString(line, ' ');
			for (auto element : lileElements)
			{
				int value = stoi(element);
				if (value == 0) {
					problem->push_back(curClause);
					curClause = make_shared<Clause>();
				}
				else
				{
					curClause->emplace_back(abs(value), value < 0);
				}
			}
		}
	}

	if (curClause->size() > 0)
		problem->push_back(curClause);

	return problem;
}
