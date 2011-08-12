#include "StdAfx.h"
#include "RgcInterface.h"

RgcInterface::RgcInterface()
{
}

RgcInterface::~RgcInterface()
{
}

bool RgcInterface::Init()
{
	if (!AfxWinInit(GetModuleHandle(NULL), NULL, GetCommandLine(), 0))
	{
		TRACE("Cannot initialize MFC");
		return false;
	}
	if (!AfxSocketInit())
	{
		TRACE("Cannot initialize sockets");
		return false;
	}
	if (!socket.Create())
	{
		TRACE("Cannot create socket");
		return false;
	}

	return true;
}

void RgcInterface::Uninit()
{
	AfxSocketTerm();
}

bool RgcInterface::Connect(const char* username, const char* password)
{
	socket.AsyncSelect();

	if (!socket.Connect(RGC_HOSTNAME, RGC_PORT))
	{
		DWORD dw = GetLastError();
		TRACE("Cannot connect");
		socket.Close();
		return false;
	}

	return true;
}