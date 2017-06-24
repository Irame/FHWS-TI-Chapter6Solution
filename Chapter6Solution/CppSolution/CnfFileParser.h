#pragma once
#include "SatProblem.h"

class CnfFileParser
{
public:
	static SatProblemPtr Parse(std::string fileName);
};
