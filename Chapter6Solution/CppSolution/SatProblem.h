#pragma once
#include <vector>
#include <memory>
#include <tuple>

class VariableAssignment : public std::vector<bool>
{
public:

};

typedef std::shared_ptr<VariableAssignment> VariableAssignmentPtr;


class Clause : public std::vector<std::tuple<unsigned, bool>>
{
public:
	bool operator() (VariableAssignmentPtr assignment) const;
};

typedef std::shared_ptr<Clause> ClausePtr;


class SatProblem : public std::vector<ClausePtr>
{
public:
	unsigned VariableCount;

	unsigned operator() (VariableAssignmentPtr assignment) const;
};

typedef std::shared_ptr<SatProblem> SatProblemPtr;