#include "StdAfx.h"
#include "RgcSocket.h"

RgcSocket::RgcSocket()
{
}

void RgcSocket::OnReceive(int nErrorCode)
{
	char buffer[BUFSIZE];
	int nRead = Receive(buffer, BUFSIZE);
	
	switch (nRead)
	{
	case 0:
		Close();
		break;
	case SOCKET_ERROR:
		if (GetLastError() != WSAEWOULDBLOCK)
		{
			TRACE("An error occured");
			Close();
		}
		break;
	default:
		buffer[nRead] = 0;
	}

	__super::OnReceive(nErrorCode);
}

void RgcSocket::OnSend(int nErrorCode)
{
	__super::OnSend(nErrorCode);
}