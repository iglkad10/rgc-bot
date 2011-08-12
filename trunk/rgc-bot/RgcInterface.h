#pragma once

#include "rgc-bot.h"
#include "RgcSocket.h"

class RgcInterface : public IRgcInterface
{
protected:
	RgcSocket socket;

public:
	RgcInterface();
	virtual ~RgcInterface();

	virtual bool Init();
	virtual void Uninit();
	virtual bool Connect(const char* username, const char* password);
};
