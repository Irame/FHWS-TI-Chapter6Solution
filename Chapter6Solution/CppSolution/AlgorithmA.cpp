#include "stdafx.h"
#include "AlgorithmA.h"

unsigned AlgorithmA::GenerateAssingment(SatProblemPtr problem, VariableAssignmentPtr& assignment)
{
	assignment = std::make_shared<VariableAssignment>();
	for (int i = 0; i < problem->VariableCount; ++i)
	{
		assignment->push_back(rand() % 2 == 0);
	}
	return (*problem)(assignment);
}

unsigned AlgorithmA::GenerateAssingment(SatProblemPtr problem, unsigned trys, VariableAssignmentPtr& assignment)
{
	unsigned bestSatisfaction = 0;
	assignment = std::make_shared<VariableAssignment>();
	for (int i = 0; i < trys; ++i)
	{
		unsigned newSatisfaction;
		VariableAssignmentPtr newAssignment;
		if ((newSatisfaction = GenerateAssingment(problem, newAssignment)) > bestSatisfaction)
		{
			bestSatisfaction = newSatisfaction;
			assignment = newAssignment;
		}
	}
	return bestSatisfaction;
}
