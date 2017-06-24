#pragma once
#include "SatProblem.h"

class AlgorithmA
{
public:
	static unsigned GenerateAssingment(SatProblemPtr problem, VariableAssignmentPtr &assignment);
	static unsigned GenerateAssingment(SatProblemPtr problem, unsigned trys, VariableAssignmentPtr &assignment);
};
