#include "stdafx.h"
#include "SatProblem.h"

using namespace std;

bool Clause::operator()(VariableAssignmentPtr assignment) const
{
	for (auto& literal : *this)
	{
		if ((*assignment)[get<0>(literal)-1] != get<1>(literal))
			return true;
	}
	return false;
}

unsigned SatProblem::operator()(VariableAssignmentPtr assignment) const
{
	unsigned result = 0;
	for (auto& clause : *this)
	{
		if ((*clause)(assignment)) 
			result++;
	}
	return result;
}
