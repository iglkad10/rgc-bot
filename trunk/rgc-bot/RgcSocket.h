#pragma once

#include <afxsock.h>

#define BUFSIZE 4096

class RgcSocket : public CSocket
{
public:
	RgcSocket();

public:
	virtual void OnReceive(int nErrorCode);
	virtual void OnSend(int nErrorCode);
};
