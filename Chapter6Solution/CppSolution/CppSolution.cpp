// CppSolution.cpp : Definiert den Einstiegspunkt für die Konsolenanwendung.
//

#include "stdafx.h"
#include "CnfFileParser.h"
#include "AlgorithmA.h"
#include <iostream>

using namespace std;

int main(int argc, char* argv[])
{
	if (argc < 2)
	{
		cout << "Bitte eine CNF Datei angeben." << endl;
		exit(1);
	}

	auto problem = CnfFileParser::Parse(argv[1]);
	VariableAssignmentPtr assignment;
	int satisfaction = AlgorithmA::GenerateAssingment(problem, 100, assignment);
    return 0;
}

